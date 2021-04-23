using Discord;
using Discord.Commands;
using NadekoBot.Common;
using NadekoBot.Common.Attributes;
using NadekoBot.Common.Replacements;
using NadekoBot.Core.Common;
using NadekoBot.Core.Modules.Help.Common;
using NadekoBot.Core.Services;
using NadekoBot.Extensions;
using NadekoBot.Modules.Help.Services;
using NadekoBot.Modules.Permissions.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace NadekoBot.Modules.Help
{
    public class Help : NadekoTopLevelModule<HelpService>
    {
        public const string PatreonUrl = "NO_LINK_YET";
        public const string PaypalUrl = "NO_LINK_YET"; // gotta add this soon. Lol
        private readonly IBotCredentials _creds;
        private readonly CommandService _cmds;
        private readonly GlobalPermissionService _perms;
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _client;

        private readonly AsyncLazy<ulong> _lazyClientId;

        public Help(IBotCredentials creds, GlobalPermissionService perms, CommandService cmds,
            IServiceProvider services, DiscordSocketClient client)
        {
            _creds = creds;
            _cmds = cmds;
            _perms = perms;
            _services = services;
            _client = client;

            _lazyClientId = new AsyncLazy<ulong>(async () => (await _client.GetApplicationInfoAsync()).Id);
        }

        public async Task<(string plainText, EmbedBuilder embed)> GetHelpStringEmbed()
        {
            var clientId = await _lazyClientId.Value;
            var r = new ReplacementBuilder()
                .WithDefault(Context)
                .WithOverride("{0}", () => clientId.ToString())
                .WithOverride("{1}", () => Prefix)
                .Build();

            var app = await _client.GetApplicationInfoAsync();


            if (!CREmbed.TryParse(Bc.BotConfig.HelpString, out var embed))
                return ("", new EmbedBuilder().WithOkColor()
                    .WithDescription(String.Format(Bc.BotConfig.HelpString, clientId, Prefix)));

            r.Replace(embed);

            return (embed.PlainText, embed.ToEmbed());
        }

        [NadekoCommand, Usage, Description, Aliases]
        public async Task Modules()
        {
            var teacupText = "";
            var teacupCheck = false;
            if (ctx.Guild.Id == 706492309604401206) { teacupText = " - ₊˚๑ ꒰🍵꒱ Tea Cup"; teacupCheck = true; }


            var embed = new EmbedBuilder().WithOkColor()
                .WithFooter(efb => efb.WithText(GetText("modules_footer", Prefix)));

            Emote star = Emote.Parse("<a:tcustarry:809392809508470805>");
            Emote star2 = Emote.Parse("<a:tcustarry2:809400070566445056>");
            Emote cupcake = Emote.Parse("<:tcucupcake:809400108571033661>");

            if (teacupCheck == false)
            {
                embed.WithDescription(
                    $"₊˚๑ {star} Administration\n" +
                    $"₊˚๑ {star} CustomReactions\n" +
                    $"₊˚๑ {star} Gambling\n" +
                    $"₊˚๑ {star} Games\n" +
                    $"₊˚๑ {star} Help\n" +
                    $"ʚ﹕₊˚︶꒷꒦꒷︶︶꒷꒦︶‧˚₊⊹\n" +
                    $"₊˚๑ {star2} Permissions\n" +
                    $"₊˚๑ {star2} Searches\n" +
                    $"₊˚๑ {star2} Utilities\n" +
                    $"₊˚๑ {star2} Logging\n" +
                    $"₊˚๑ {star2} Xp\n" +
                    $"₊˚๑ {star2} NSFW")
                .WithTitle(GetText("list_of_modules", teacupText));
            }
            else if (teacupCheck == true)
            {
                embed.WithDescription(
                    $"—・ {cupcake} ┊ **Tea Cup**\n" +
                    $"₊˚๑ {star} Administration\n" +
                    $"₊˚๑ {star} CustomReactions\n" +
                    $"₊˚๑ {star} Gambling\n" +
                    $"₊˚๑ {star} Games\n" +
                    $"₊˚๑ {star} Help\n" +
                    $"ʚ﹕₊˚︶꒷꒦꒷︶︶꒷꒦︶‧˚₊⊹\n" +
                    $"₊˚๑ {star2} Permissions\n" +
                    $"₊˚๑ {star2} Searches\n" +
                    $"₊˚๑ {star2} Utilities\n" +
                    $"₊˚๑ {star2} Logging\n" +
                    $"₊˚๑ {star2} Xp\n" +
                    $"₊˚๑ {star2} NSFW")
                .WithTitle(GetText("list_of_modules", teacupText));
            }


            await ctx.Channel.EmbedAsync(embed).ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [NadekoOptions(typeof(CommandsOptions))]
        public async Task Commands(string module = null, params string[] args)
        {
            var channel = ctx.Channel;

            var (opts, _) = OptionsParser.ParseFrom(new CommandsOptions(), args);

            module = module?.Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(module))
                return;

            // Find commands for that module
            // don't show commands which are blocked
            // order by name
            var cmds = _cmds.Commands.Where(c => c.Module.GetTopLevelModule().Name.ToUpperInvariant().StartsWith(module, StringComparison.InvariantCulture))
                                                .Where(c => !_perms.BlockedCommands.Contains(c.Aliases[0].ToLowerInvariant()))
                                                  .OrderBy(c => c.Aliases[0])
                                                  .Distinct(new CommandTextEqualityComparer());


            // check preconditions for all commands, but only if it's not 'all'
            // because all will show all commands anyway, no need to check
            var succ = new HashSet<CommandInfo>();
            if (opts.View != CommandsOptions.ViewType.All)
            {
                succ = new HashSet<CommandInfo>((await Task.WhenAll(cmds.Select(async x =>
                {
                    var pre = (await x.CheckPreconditionsAsync(Context, _services).ConfigureAwait(false));
                    return (Cmd: x, Succ: pre.IsSuccess);
                })).ConfigureAwait(false))
                    .Where(x => x.Succ)
                    .Select(x => x.Cmd));

                if (opts.View == CommandsOptions.ViewType.Hide)
                {
                    // if hidden is specified, completely remove these commands from the list
                    cmds = cmds.Where(x => succ.Contains(x));
                }
            }

            var cmdsWithGroup = cmds.GroupBy(c => c.Module.Name.Replace("Commands", "", StringComparison.InvariantCulture))
                .OrderBy(x => x.Key == x.First().Module.Name ? int.MaxValue : x.Count());

            if (!cmds.Any())
            {
                if (opts.View != CommandsOptions.ViewType.Hide)
                    await ReplyErrorLocalizedAsync("module_not_found").ConfigureAwait(false);
                else
                    await ReplyErrorLocalizedAsync("module_not_found_or_cant_exec").ConfigureAwait(false);
                return;
            }
            var i = 0;
            var groups = cmdsWithGroup.GroupBy(x => i++ / 48).ToArray();
            var embed = new EmbedBuilder().WithOkColor();
            foreach (var g in groups)
            {
                var last = g.Count();
                for (i = 0; i < last; i++)
                {
                    var transformed = g.ElementAt(i).Select(x =>
                    {
                        //if cross is specified, and the command doesn't satisfy the requirements, cross it out
                        if (opts.View == CommandsOptions.ViewType.Cross)
                        {
                            return $"{(succ.Contains(x) ? "✅" : "❌")}{Prefix + x.Aliases.First(),-15} {"[" + x.Aliases.Skip(1).FirstOrDefault() + "]",-8}";
                        }
                        return $"{Prefix + x.Aliases.First(),-15} {"[" + x.Aliases.Skip(1).FirstOrDefault() + "]",-8}";
                    });

                    if (i == last - 1 && (i + 1) % 2 != 0)
                    {
                        var grp = 0;
                        var count = transformed.Count();
                        transformed = transformed
                            .GroupBy(x => grp++ % count / 2)
                            .Select(x =>
                            {
                                if (x.Count() == 1)
                                    return $"{x.First()}";
                                else
                                    return String.Concat(x);
                            });
                    }
                    embed.AddField(g.ElementAt(i).Key, "```css\n" + string.Join("\n", transformed) + "\n```", true);
                }
            }
            embed.WithFooter(GetText("commands_instr", Prefix));
            await ctx.Channel.EmbedAsync(embed).ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [Priority(0)]
        public async Task H([Leftover] string fail)
        {
            var prefixless = _cmds.Commands.FirstOrDefault(x => x.Aliases.Any(cmdName => cmdName.ToLowerInvariant() == fail));
            if (prefixless != null)
            {
                await H(prefixless).ConfigureAwait(false);
                return;
            }

            await ReplyErrorLocalizedAsync("command_not_found").ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [Priority(1)]
        public async Task H([Leftover] CommandInfo com = null)
        {
            var channel = ctx.Channel;

            if (com == null)
            {
                IMessageChannel ch = channel is ITextChannel
                    ? await ((IGuildUser)ctx.User).GetOrCreateDMChannelAsync().ConfigureAwait(false)
                    : channel;
                try
                {
                    var (plainText, helpEmbed) = await GetHelpStringEmbed();
                    await ch.EmbedAsync(helpEmbed, msg: plainText ?? "").ConfigureAwait(false);
                    await ctx.OkAsync();
                }
                catch (Exception)
                {
                    await ReplyErrorLocalizedAsync("cant_dm").ConfigureAwait(false);
                }
                return;
            }

            var embed = _service.GetCommandHelp(com, ctx.Guild);
            await channel.EmbedAsync(embed).ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [OwnerOnly]
        public async Task Hgit()
        {
            Dictionary<string, List<object>> cmdData = new Dictionary<string, List<object>>();
            foreach (var com in _cmds.Commands.OrderBy(com => com.Module.GetTopLevelModule().Name).GroupBy(c => c.Aliases.First()).Select(g => g.First()))
            {
                var module = com.Module.GetTopLevelModule();
                List<string> optHelpStr = null;
                var opt = ((NadekoOptionsAttribute)com.Attributes.FirstOrDefault(x => x is NadekoOptionsAttribute))?.OptionType;
                if (opt != null)
                {
                    optHelpStr = HelpService.GetCommandOptionHelpList(opt);
                }
                var obj = new
                {
                    Aliases = com.Aliases.Select(x => Prefix + x).ToArray(),
                    Description = string.Format(com.Summary, Prefix),
                    Usage = JsonConvert.DeserializeObject<string[]>(com.Remarks).Select(x => string.Format(x, Prefix)).ToArray(),
                    Submodule = com.Module.Name,
                    Module = com.Module.GetTopLevelModule().Name,
                    Options = optHelpStr,
                    Requirements = HelpService.GetCommandRequirements(com),
                };
                if (cmdData.TryGetValue(module.Name, out var cmds))
                    cmds.Add(obj);
                else
                    cmdData.Add(module.Name, new List<object>
                    {
                        obj
                    });
            }
            File.WriteAllText("../../docs/cmds_new.json", JsonConvert.SerializeObject(cmdData, Formatting.Indented));
            await ReplyConfirmLocalizedAsync("commandlist_regen").ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        public async Task Guide()
        {
            await ConfirmLocalizedAsync("guide",
                "https://gitlab.com/Dok4440/TCUBetaBot",
                "https://gitlab.com/Dok4440/TCUBetaBot/-/wikis/home").ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        public async Task Donate()
        {
            await ReplyConfirmLocalizedAsync("donate", PatreonUrl, PaypalUrl).ConfigureAwait(false);
        }

        private string GetRemarks(string[] arr)
        {
            return string.Join(" or ", arr.Select(x => Format.Code(x)));
        }
    }

    public class CommandTextEqualityComparer : IEqualityComparer<CommandInfo>
    {
        public bool Equals(CommandInfo x, CommandInfo y) => x.Aliases[0] == y.Aliases[0];

        public int GetHashCode(CommandInfo obj) => obj.Aliases[0].GetHashCode(StringComparison.InvariantCulture);

    }

    public class JsonCommandData
    {
        public string[] Aliases { get; set; }
        public string Description { get; set; }
        public string Usage { get; set; }
    }
}
