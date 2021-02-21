using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NadekoBot.Common;
using NadekoBot.Common.Attributes;
using NadekoBot.Core.Services;
using NadekoBot.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NadekoBot.Modules.TeaCup
{
    public partial class TeaCup : NadekoTopLevelModule
    {
        private readonly DiscordSocketClient _client;
        private readonly NadekoBot _bot;

        public TeaCup(DiscordSocketClient client, NadekoBot bot)
        {
            _client = client;
            _bot = bot;
        }


        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [UserPerm(GuildPerm.Administrator)]
        [BotPerm(GuildPerm.ManageRoles)]
        public async Task Demote(IGuildUser targetUser)
        {
            // THIS COMMAND DOES NOT WORK IN ANY OTHER SERVERS THAN TEA CUP
            // IF YOURE SELFHOSTING, YOU CAN COMPLETELY REMOVE THIS COMMAND FROM THE CODE
            // CHECK BEFORE COMMIT 122 TO SEE ORIGINAL SOURCE CODE COMMENTS & "EXTRAS"

            if (ctx.Guild.Id != 706492309604401206)
            {
                await ReplyErrorLocalizedAsync("server_error").ConfigureAwait(false);
                return;
            }

            var roleCheck = targetUser;
            var roleToAdd = Context.Guild.GetRole(707253174678847541); // idek what the fuck that role is KANKER and i can't find it so like
                                                                       // it'll break the code if i remove it, so ghost id in code :dance: :yeah: :fuck: :haha: :rofl:

            ulong roleManager = 706542855119962122;
            ulong roleSecretary = 795982486244556801;
            ulong roleCook = 706503570622775332;
            ulong roleAssistant = 706543395044327545;
            ulong roleTrialAssistant = 706543894288007209;
            ulong roleCustomer = 707253174678847541;

            if (roleCheck.RoleIds.Any(id => id == roleManager)) // manager to secretary
            {
                roleToAdd = Context.Guild.GetRole(roleSecretary);
            }
            else if (roleCheck.RoleIds.Any(id => id == roleSecretary)) // secretary to cook
            {
                roleToAdd = Context.Guild.GetRole(roleCook);
            }
            else if (roleCheck.RoleIds.Any(id => id == roleCook)) // cook to assistant
            {
                roleToAdd = Context.Guild.GetRole(roleAssistant);
            }
            else if (roleCheck.RoleIds.Any(id => id == roleAssistant)) // Assistant to trial assistant
            {
                roleToAdd = Context.Guild.GetRole(roleTrialAssistant);
            }
            else if (roleCheck.RoleIds.Any(id => id == roleTrialAssistant)) // trial assistant to customer
            {
                roleToAdd = Context.Guild.GetRole(roleCustomer);
            }
            else if (roleCheck.RoleIds.Any(id => id != roleManager || id != roleSecretary || id != roleCook || id != roleAssistant || id != roleTrialAssistant)) // Customer to customer
            {
                roleToAdd = Context.Guild.GetRole(roleCustomer);
            }
            else // if none of the above check out (which should technically never happen); it gives the regular customer role.)
            {
                await ReplyErrorLocalizedAsync("demote_remrole_error").ConfigureAwait(false);
            }

            // Remove old roles
            var runnerUser2 = (IGuildUser)ctx.User; // "runnerUser2 isn't the person you want to demote. It's the person doing the command."
            var roleToRemoveSecretary = Context.Guild.GetRole(795982486244556801);
            var roleToRemoveCook = Context.Guild.GetRole(706503570622775332);
            var roleToRemoveAssistant = Context.Guild.GetRole(706543395044327545);
            var roleToRemoveTrialAssistant = Context.Guild.GetRole(706543894288007209);

            try // removes all staff role from 1 user.
            {
                if (roleCheck.RoleIds.Any(id => id != roleManager))
                {
                    if (roleToAdd == Context.Guild.GetRole(roleCook))
                    {
                        await ReplyErrorLocalizedAsync("demote_hierarchy_error").ConfigureAwait(false);
                        return;
                    }
                }
                else if (roleCheck.RoleIds.Any(id => id != roleSecretary) || roleCheck.RoleIds.Any(id => id != roleManager))
                {
                    if (roleToAdd == Context.Guild.GetRole(roleAssistant))
                    {
                        await ReplyErrorLocalizedAsync("demote_hierarchy_error").ConfigureAwait(false);
                        return;
                    }
                }

                await targetUser.RemoveRoleAsync(roleToRemoveSecretary).ConfigureAwait(false);
                await targetUser.RemoveRoleAsync(roleToRemoveCook).ConfigureAwait(false);
                await targetUser.RemoveRoleAsync(roleToRemoveAssistant).ConfigureAwait(false);
                await targetUser.RemoveRoleAsync(roleToRemoveTrialAssistant).ConfigureAwait(false);
            }
            catch
            {
                await ReplyErrorLocalizedAsync("demote_remrole_error").ConfigureAwait(false);
            }

            // Add new role
            var runnerUser1 = (IGuildUser)ctx.User;
            var runner1MaxRolePosition = runnerUser1.GetRoles().Max(x => x.Position);
            // var roleToAdd = Context.Guild.GetRole(706543395044327545);

            if ((ctx.User.Id != ctx.Guild.OwnerId) && runner1MaxRolePosition <= roleToAdd.Position)
            {
                return;
            }
            try
            { // adds new role
                await targetUser.AddRoleAsync(roleToAdd).ConfigureAwait(false);
                // await ReplyConfirmLocalizedAsync("demote_addrole").ConfigureAwait(false); <- uncomment this for an extra confirmation the command is succeeding.
                await ReplyConfirmLocalizedAsync("demote_success").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ReplyErrorLocalizedAsync("demote_addrole_error").ConfigureAwait(false);
                _log.Info(ex);
            }
        }


        // Promote command
        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [UserPerm(GuildPerm.Administrator)]
        [BotPerm(GuildPerm.ManageRoles)]
        public async Task Promote(IGuildUser targetUser)
        {

            // THIS COMMAND DOES NOT WORK IN ANY OTHER SERVERS THAN TEA CUP
            // IF YOURE SELFHOSTING, YOU CAN COMPLETELY REMOVE THIS COMMAND FOR THE CODE
            // CHECK BEFORE COMMIT 122 TO SEE ORIGINAL SOURCE CODE COMMENTS & "EXTRAS"


            if (ctx.Guild.Id != 706492309604401206)  // server check (tea cup)
            {
                await ReplyErrorLocalizedAsync("server_error").ConfigureAwait(false);
                return;
            }

            var roleCheck = targetUser;
            var roleToAdd = Context.Guild.GetRole(706543395044327545);

            ulong roleManager = 706542855119962122;
            ulong roleSecretary = 795982486244556801;
            ulong roleCook = 706503570622775332;
            ulong roleAssistant = 706543395044327545;
            ulong roleTrialAssistant = 706543894288007209;
            ulong roleCustomer = 707253174678847541;

            if (roleCheck.RoleIds.Any(id => id == roleSecretary)) // secretary to manager
            {
                roleToAdd = Context.Guild.GetRole(roleManager);
            }
            else if (roleCheck.RoleIds.Any(id => id == roleCook)) // cook to secretary
            {
                roleToAdd = Context.Guild.GetRole(roleSecretary);
            }
            else if (roleCheck.RoleIds.Any(id => id == roleAssistant)) // assistant to cook
            {
                roleToAdd = Context.Guild.GetRole(roleCook);
            }
            else if (roleCheck.RoleIds.Any(id => id == roleTrialAssistant)) // trial assistant to assistant
            {
                roleToAdd = Context.Guild.GetRole(roleAssistant);
            }
            else if (roleCheck.RoleIds.Any(id => id == roleCustomer)) // customer to trial assistant
            {
                roleToAdd = Context.Guild.GetRole(roleTrialAssistant);
            }
            else
            {
                await ReplyErrorLocalizedAsync("promote_remrole_error").ConfigureAwait(false);
            }


            // Remove old role
            var runnerUser2 = (IGuildUser)ctx.User;
            var roleToRemoveSecretary = Context.Guild.GetRole(795982486244556801);
            var roleToRemoveCook = Context.Guild.GetRole(706503570622775332);
            var roleToRemoveAssistant = Context.Guild.GetRole(706543395044327545);
            var roleToRemoveTrialAssistant = Context.Guild.GetRole(706543894288007209);

            try
            {
                if (roleCheck.RoleIds.Any(id => id == roleCook))
                {
                    if (roleToAdd == Context.Guild.GetRole(roleSecretary))
                    {
                        await ReplyErrorLocalizedAsync("promote_hierarchy_error").ConfigureAwait(false);
                        return;
                    }
                    if (roleToAdd == Context.Guild.GetRole(roleCook))
                    {
                        await ReplyErrorLocalizedAsync("promote_hierarchy_error").ConfigureAwait(false);
                        return;
                    }
                }

                await targetUser.RemoveRoleAsync(roleToRemoveSecretary).ConfigureAwait(false);
                await targetUser.RemoveRoleAsync(roleToRemoveCook).ConfigureAwait(false);
                await targetUser.RemoveRoleAsync(roleToRemoveAssistant).ConfigureAwait(false);
                await targetUser.RemoveRoleAsync(roleToRemoveTrialAssistant).ConfigureAwait(false);
            }
            catch
            {
                await ReplyErrorLocalizedAsync("demote_remrole_error").ConfigureAwait(false);
            }

            // Add new role
            var runnerUser1 = (IGuildUser)ctx.User;
            var runner1MaxRolePosition = runnerUser1.GetRoles().Max(x => x.Position);

            if ((ctx.User.Id != ctx.Guild.OwnerId) && runner1MaxRolePosition <= roleToAdd.Position)
            {
                await ReplyErrorLocalizedAsync("promote_addrole_error").ConfigureAwait(false);
                return;
            }

            try
            {
                await targetUser.AddRoleAsync(roleToAdd).ConfigureAwait(false);
                // await ReplyConfirmLocalizedAsync("demote_addrole").ConfigureAwait(false); <- uncomment this for an extra confirmation the command is succeeding.
                await ReplyConfirmLocalizedAsync("promote_success").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ReplyErrorLocalizedAsync("promote_addrole_error").ConfigureAwait(false);
                _log.Info(ex);
            }
        }

        // confess command
        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.DM)]
        [Priority(1)]
        public async Task Confess([Leftover] string confession=null)
        {
            // THIS COMMAND DOES NOT WORK IN ANY OTHER SERVERS THAN TEA CUP
            // IF YOURE SELFHOSTING, YOU CAN COMPLETELY REMOVE THIS COMMAND FOR THE CODE

            var server = _client.Guilds.FirstOrDefault(s => s.Id == 706492309604401206);
            var uid = ctx.User.Id;
            var user = server.Users.FirstOrDefault(u => u.Id == uid);

            string[] icon = // will continue to update this list whenever i'm in the mood.
{
            "https://i.imgur.com/79XfsbS.png",
            "https://i.imgur.com/yldY7sh.png",
            "https://i.imgur.com/iKGgeKz.png",
            "https://i.imgur.com/wFsgSnr.png",
            "https://i.imgur.com/hSauh7K.png",
            "https://i.imgur.com/OzxRYsD.png"
          };
            Random rand = new Random();
            int index = rand.Next(icon.Length);

            if (user == null)
            {
                var embed1 = new EmbedBuilder().WithErrorColor()
                    .WithAuthor(eab => eab.WithName("Server Error")
                    .WithIconUrl(icon[index]))
                    .WithDescription(GetText("server_error"))
                    .WithFooter("You can't use this command :/");

                var DMCh1 = await Context.User.GetOrCreateDMChannelAsync().ConfigureAwait(false);
                await DMCh1.EmbedAsync(embed1).ConfigureAwait(false);
                return;
            }

            if (confession == null || confession == "")
            {
                await ErrorLocalizedAsync("confess_empty_message").ConfigureAwait(false);
                return;
            }
            if (confession.Contains("nigg") || confession.Contains("n i g")) // GitLab if you see this, please don't ban me i swear i'm just too lazy to get this from a database
            {
                await ErrorLocalizedAsync("confess_forbidden").ConfigureAwait(false);
                return;
            }

            var embed = new EmbedBuilder().WithOkColor()
                    .WithAuthor(eab => eab.WithName("Anonymous Confession")
                                .WithIconUrl(icon[index]))
                            .WithFooter("Type .confess <message> in my DMs to confess.");


            if (confession.StartsWith("\"") && confession.EndsWith("\""))
            {
                embed.WithDescription(confession.ToString());
            }
            else
            {
                embed.WithDescription("\"" + confession.ToString() + "\"");
            }

            var ch = server.TextChannels.FirstOrDefault(c => c.Id == 812023722856415248);
            await ch.EmbedAsync(embed).ConfigureAwait(false);

            var embed2 = new EmbedBuilder().WithOkColor()
                .WithAuthor(eab => eab.WithName("Confessions ~ ─・୨🍵੭・Tea Cup ꒷꒦")
                .WithIconUrl(icon[index]))
                .WithDescription(GetText("confess_sent"))
                .WithFooter("Pro privacy tip: delete your confession in this DM.");

            var DMCh2 = await Context.User.GetOrCreateDMChannelAsync().ConfigureAwait(false);
            await DMCh2.EmbedAsync(embed2).ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [Priority(0)]
        public async Task Confess(string param = null, [Leftover] string confession = null) // extra string (little workaround, works for now)
        { 

            if (ctx.Guild.Id != 706492309604401206)
            {
                await ReplyErrorLocalizedAsync("server_error").ConfigureAwait(false);
                return;
            }
            else if (ctx.Guild.Id == 706492309604401206)
            {
                await ctx.Message.DeleteAsync();
                await ErrorLocalizedAsync("confess_dm").ConfigureAwait(false);
                return;
            }
        }
    }
}

