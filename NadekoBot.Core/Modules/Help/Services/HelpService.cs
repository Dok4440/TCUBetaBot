using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System;
using Discord.Commands;
using NadekoBot.Extensions;
using System.Linq;
using NadekoBot.Common.Attributes;
using NadekoBot.Common.ModuleBehaviors;
using NadekoBot.Core.Services;
using NadekoBot.Core.Services.Impl;
using NadekoBot.Common;
using NLog;
using CommandLine;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace NadekoBot.Modules.Help.Services
{
    public class HelpService :/*ILateExecutor, */INService
    {
        private readonly IBotConfigProvider _bc;
        private readonly CommandHandler _ch;
        private readonly NadekoStrings _strings;
        private readonly Logger _log;

        public HelpService(IBotConfigProvider bc, CommandHandler ch, NadekoStrings strings)
        {
            _bc = bc;
            _ch = ch;
            _strings = strings;
            _log = LogManager.GetCurrentClassLogger();
        }


        // To enable "DMHelpString" in .bce & database -> uncomment this & the commented part on line 19 (TCU doesn't support fixes for this, so an errored DMHelpString is to fix yourself)

        // public Task LateExecute(DiscordSocketClient client, IGuild guild, IUserMessage msg)
        // {
        //     try
        //     {
        //         if (guild == null)
        //         {
        //             if (CREmbed.TryParse(_bc.BotConfig.DMHelpString, out var embed))
        //                 return msg.Channel.EmbedAsync(embed.ToEmbed(), embed.PlainText?.SanitizeMentions() ?? "");

        //             return msg.Channel.SendMessageAsync(_bc.BotConfig.DMHelpString);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _log.Warn(ex);
        //     }
        //     return Task.CompletedTask;
        //}

        public EmbedBuilder GetCommandHelp(CommandInfo com, IGuild guild)
        {
            var prefix = _ch.GetPrefix(guild);
            var str = string.Format("**`{0}`**", prefix + com.Aliases.First());
            var alias = com.Aliases.Skip(1).FirstOrDefault();
            if (alias != null)
                str += string.Format(" **/ `{0}`**", prefix + alias);
            var em = new EmbedBuilder()
                .AddField(fb => fb.WithName(str)
                    .WithValue($"{com.RealSummary(prefix)}")
                    .WithIsInline(true));

            var reqs = GetCommandRequirements(com);
            if(reqs.Any())
            {
                em.AddField(GetText("requires", guild),
                    string.Join("\n", reqs));
            }

            var json = new WebClient().DownloadString("https://gitlab.com/Dok4440/TCUBetaBot/-/raw/develop/TCURawJsons/HelpGifs.json");
            string[] helpGifs = JsonConvert.DeserializeObject<string[]>(json);

            Random rand = new Random();
            int index = rand.Next(helpGifs.Length);

             em.AddField(fb => fb.WithName(GetText("usage", guild))
                    .WithValue(com.RealRemarks(prefix))
                    .WithIsInline(false))
                .WithThumbnailUrl(helpGifs[index])
                .WithColor(NadekoBot.OkColor);

            int credit = rand.Next(1, 5);
            if (credit == 1)
            {
                em.WithFooter(efb => efb.WithText(GetText("module", guild, com.Module.GetTopLevelModule().Name) + " | Art by JuicyBblue :D"));
            }
            else
            {
                em.WithFooter(efb => efb.WithText(GetText("module", guild, com.Module.GetTopLevelModule().Name)));
            }

            var opt = ((NadekoOptionsAttribute)com.Attributes.FirstOrDefault(x => x is NadekoOptionsAttribute))?.OptionType;
            if (opt != null)
            {
                var hs = GetCommandOptionHelp(opt);
                if(!string.IsNullOrWhiteSpace(hs))
                    em.AddField(GetText("options", guild), hs, false);
            }

            return em;
        }

        public static string GetCommandOptionHelp(Type opt)
        {
            var strs = GetCommandOptionHelpList(opt);

            return string.Join("\n", strs);
        }

        public static List<string> GetCommandOptionHelpList(Type opt)
        {
            var strs = opt.GetProperties()
                   .Select(x => x.GetCustomAttributes(true).FirstOrDefault(a => a is OptionAttribute))
                   .Where(x => x != null)
                   .Cast<OptionAttribute>()
                   .Select(x =>
                   {
                       var toReturn = $"`--{x.LongName}`";

                       if (!string.IsNullOrWhiteSpace(x.ShortName))
                           toReturn += $" (`-{x.ShortName}`)";

                       toReturn += $"   {x.HelpText}  ";
                       return toReturn;
                   })
                   .ToList();

            return strs;
        }

        public static string[] GetCommandRequirements(CommandInfo cmd) =>
            cmd.Preconditions
                  .Where(ca => ca is OwnerOnlyAttribute || ca is RequireUserPermissionAttribute)
                  .Select(ca =>
                  {
                      if (ca is OwnerOnlyAttribute)
                      {
                          return "Bot Owner Only";
                      }

                      var cau = (RequireUserPermissionAttribute)ca;
                      if (cau.GuildPermission != null)
                      {
                          return (cau.GuildPermission.ToString() + " Server Permission")
                                       .Replace("Guild", "Server", StringComparison.InvariantCulture);
                      }

                      return (cau.ChannelPermission + " Channel Permission")
                                       .Replace("Guild", "Server", StringComparison.InvariantCulture);
                  })
                .ToArray();

        private string GetText(string text, IGuild guild, params object[] replacements) =>
            _strings.GetText(text, guild?.Id, "Help".ToLowerInvariant(), replacements);
    }
}
