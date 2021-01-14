using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NadekoBot.Common;
using NadekoBot.Common.Attributes;
using NadekoBot.Core.Services.Database.Models;
using NadekoBot.Extensions;
using NadekoBot.Modules.Administration.Services;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NadekoBot.Modules.Administration
{
    public partial class Administration
    {
        public class RoleCommands : NadekoSubmodule<RoleCommandsService>
        {
            public enum Exclude { Excl }

            public async Task InternalReactionRoles(bool exclusive, params string[] input)
            {
                var msgs = await ((SocketTextChannel)ctx.Channel).GetMessagesAsync().FlattenAsync().ConfigureAwait(false);
                var prev = (IUserMessage)msgs.FirstOrDefault(x => x is IUserMessage && x.Id != ctx.Message.Id);

                if (prev == null)
                    return;

                if (input.Length % 2 != 0)
                    return;

                var g = (SocketGuild)ctx.Guild;

                var grp = 0;
                var all = input
                    .GroupBy(x => grp++ / 2)
                    .Select(x =>
                    {
                        var inputRoleStr = x.First().ToLowerInvariant();
                        var role = g.Roles.FirstOrDefault(y => y.Name.ToLowerInvariant() == inputRoleStr);
                        if (role == null)
                        {
                            _log.Warn("Role {0} not found.", inputRoleStr);
                            return null;
                        }
                        //var emote = g.Emotes.FirstOrDefault(y => y.ToString() == x.Last());
                        //if (emote == null)
                        //{
                        //    _log.Warn("Emote {0} not found.", x.Last());
                        //    return null;
                        //}
                        //else

                        var emote = x.Last().ToIEmote();
                        return new { role, emote };
                    })
                    .Where(x => x != null);

                if (!all.Any())
                    return;

                foreach (var x in all)
                {
                    await prev.AddReactionAsync(x.emote, new RequestOptions()
                    {
                        RetryMode = RetryMode.Retry502 | RetryMode.RetryRatelimit
                    }).ConfigureAwait(false);
                    await Task.Delay(100).ConfigureAwait(false);
                }

                if (_service.Add(ctx.Guild.Id, new ReactionRoleMessage()
                {
                    Exclusive = exclusive,
                    MessageId = prev.Id,
                    ChannelId = prev.Channel.Id,
                    ReactionRoles = all.Select(x =>
                    {
                        return new ReactionRole()
                        {
                            EmoteName = x.emote.ToString(),
                            RoleId = x.role.Id,
                        };
                    }).ToList(),
                }))
                {
                    await ctx.Channel.SendConfirmAsync(":ok:")
                        .ConfigureAwait(false);
                }
                else
                {
                    await ReplyErrorLocalizedAsync("reaction_roles_full").ConfigureAwait(false);
                }
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [NoPublicBot]
            [UserPerm(GuildPerm.ManageRoles)]
            [BotPerm(GuildPerm.ManageRoles)]
            [Priority(0)]
            public Task ReactionRoles(params string[] input) =>
                InternalReactionRoles(false, input);

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [NoPublicBot]
            [UserPerm(GuildPerm.ManageRoles)]
            [BotPerm(GuildPerm.ManageRoles)]
            [Priority(1)]
            public Task ReactionRoles(Exclude _, params string[] input) =>
                InternalReactionRoles(true, input);

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [NoPublicBot]
            [UserPerm(GuildPerm.ManageRoles)]
            public async Task ReactionRolesList()
            {
                var embed = new EmbedBuilder()
                    .WithOkColor();
                if (!_service.Get(ctx.Guild.Id, out var rrs) ||
                    !rrs.Any())
                {
                    embed.WithDescription(GetText("no_reaction_roles"));
                }
                else
                {
                    var g = ((SocketGuild)ctx.Guild);
                    foreach (var rr in rrs)
                    {
                        var ch = g.GetTextChannel(rr.ChannelId);
                        var msg = (await (ch?.GetMessageAsync(rr.MessageId)).ConfigureAwait(false)) as IUserMessage;
                        var content = msg?.Content.TrimTo(30) ?? "DELETED!";
                        embed.AddField($"**{rr.Index + 1}.** {(ch?.Name ?? "DELETED!")}",
                            GetText("reaction_roles_message", rr.ReactionRoles?.Count ?? 0, content));
                    }
                }
                await ctx.Channel.EmbedAsync(embed).ConfigureAwait(false);
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [NoPublicBot]
            [UserPerm(GuildPerm.ManageRoles)]
            public async Task ReactionRolesRemove(int index)
            {
                if (index < 1 || index > 5 ||
                    !_service.Get(ctx.Guild.Id, out var rrs) ||
                    !rrs.Any() || rrs.Count < index)
                {
                    return;
                }
                index--;
                var rr = rrs[index];
                _service.Remove(ctx.Guild.Id, index);
                await ReplyConfirmLocalizedAsync("reaction_role_removed", index + 1).ConfigureAwait(false);
            }

	        // Demote Command
	        [NadekoCommand, Usage, Description, Aliases]
          [RequireContext(ContextType.Guild)]
          [UserPerm(GuildPerm.Administrator)]
          [BotPerm(GuildPerm.ManageRoles)]
          public async Task Demote(IGuildUser targetUser)
          {
            // THIS COMMAND DOES NOT WORK IN ANY OTHER SERVERS THAN TEA CUP
            // IF YOURE SELFHOSTING, YOU CAN COMPLETELY REMOVE THIS COMMAND FOR THE CODE

              if(ctx.Guild.Id != 706492309604401206)  // server check (tea cup)
                  {
                      await ReplyConfirmLocalizedAsync("teacup_error").ConfigureAwait(false);
                      return;
                  }

              var roleCheck = targetUser;
              var roleToAdd = Context.Guild.GetRole(707253174678847541); // Start off with customer. This way an errored `if` can't give an unwanted role

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
              else if(roleCheck.RoleIds.Any(id => id == roleSecretary)) // secretary to cook
              {
                roleToAdd = Context.Guild.GetRole(roleCook);
              }
              else if(roleCheck.RoleIds.Any(id => id == roleCook)) // cook to assistant
              {
                roleToAdd = Context.Guild.GetRole(roleAssistant);
              }
              else if(roleCheck.RoleIds.Any(id => id == roleAssistant)) // Assistant to trial assistant
              {
                roleToAdd = Context.Guild.GetRole(roleTrialAssistant);
              }
              else if(roleCheck.RoleIds.Any(id => id == roleTrialAssistant)) // trial assistant to customer
              {
                roleToAdd = Context.Guild.GetRole(roleCustomer);
              }
              else if(roleCheck.RoleIds.Any(id => id != roleManager || id != roleSecretary || id != roleCook || id != roleAssistant || id != roleTrialAssistant)) // Customer to customer
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

                // if (ctx.User.Id != runnerUser2.Guild.OwnerId && // checks if runneruser is high enough in role hierarchy
                // (runnerUser2.GetRoles().Max(x => x.Position) <= roleToRemoveManager.Position) ||
                // (runnerUser2.GetRoles().Max(x => x.Position) <= roleToRemoveSecretary.Position) ||
                // (runnerUser2.GetRoles().Max(x => x.Position) <= roleToRemoveCook.Position) ||
                // (runnerUser2.GetRoles().Max(x => x.Position) <= roleToRemoveAssistant.Position) ||
                // (runnerUser2.GetRoles().Max(x => x.Position) <= roleToRemoveTrialAssistant.Position))
                //        await ReplyErrorLocalizedAsync("demote_hierarchy_error").ConfigureAwait(false);
                //        return;
                   try
                    { // removes all staff role from 1 user.
                       if (roleCheck.RoleIds.Any(id => id != roleManager))
                       {
                         if (roleToAdd == Context.Guild.GetRole(roleCook))
                         {
                           await ReplyErrorLocalizedAsync("demote_hierarchy_error").ConfigureAwait(false); // error
                           return;
                         }
                       }
                       else if (roleCheck.RoleIds.Any(id => id != roleSecretary) || roleCheck.RoleIds.Any(id => id != roleManager))
                       {
                         if (roleToAdd == Context.Guild.GetRole(roleAssistant))
                         {
                           await ReplyErrorLocalizedAsync("demote_hierarchy_error").ConfigureAwait(false); // error
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

                if ((ctx.User.Id != ctx.Guild.OwnerId) && runner1MaxRolePosition <= roleToAdd.Position){
                return;}
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

                    if(ctx.Guild.Id != 706492309604401206)  // server check (tea cup)
                        {
                            await ReplyConfirmLocalizedAsync("version_error").ConfigureAwait(false);
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
                        else if(roleCheck.RoleIds.Any(id => id == roleCook)) // cook to secretary
                        {
                          roleToAdd = Context.Guild.GetRole(roleSecretary);
                        }
                        else if(roleCheck.RoleIds.Any(id => id == roleAssistant)) // assistant to cook
                        {
                          roleToAdd = Context.Guild.GetRole(roleCook);
                        }
                        else if(roleCheck.RoleIds.Any(id => id == roleTrialAssistant)) // trial assistant to assistant
                        {
                          roleToAdd = Context.Guild.GetRole(roleAssistant);
                        }
                        else if(roleCheck.RoleIds.Any(id => id == roleCustomer)) // customer to trial assistant
                        {
                          roleToAdd = Context.Guild.GetRole(roleTrialAssistant);
                        }
                        else // if none of the above check out (which should technically never happen); it gives the regular customer role.)
                        {
                          await ReplyErrorLocalizedAsync("promote_remrole_error").ConfigureAwait(false);
                        }


                      // Remove old role
                      var runnerUser2 = (IGuildUser)ctx.User;
                      var roleToRemoveSecretary = Context.Guild.GetRole(795982486244556801); // removes secretary
                      var roleToRemoveCook = Context.Guild.GetRole(706503570622775332); // removes cook
                      var roleToRemoveAssistant = Context.Guild.GetRole(706543395044327545); // removes assistant
                      var roleToRemoveTrialAssistant = Context.Guild.GetRole(706543894288007209); //removes trial assistant

                      // if (ctx.User.Id != runnerUser2.Guild.OwnerId &&
                      // (runnerUser2.GetRoles().Max(x => x.Position) <= roleToRemoveManager.Position) ||
                      // (runnerUser2.GetRoles().Max(x => x.Position) <= roleToRemoveSecretary.Position) ||
                      // (runnerUser2.GetRoles().Max(x => x.Position) <= roleToRemoveCook.Position) ||
                      // (runnerUser2.GetRoles().Max(x => x.Position) <= roleToRemoveAssistant.Position) ||
                      // (runnerUser2.GetRoles().Max(x => x.Position) <= roleToRemoveTrialAssistant.Position))
                      //        await ReplyErrorLocalizedAsync("demote_hierarchy_error").ConfigureAwait(false);
                      //        return;
                      try
                       { // removes all staff role from 1 user.
                          if (roleCheck.RoleIds.Any(id => id == roleCook))
                          {
                            if (roleToAdd == Context.Guild.GetRole(roleSecretary))
                            {
                              await ReplyErrorLocalizedAsync("promote_hierarchy_error").ConfigureAwait(false); // error
                              return;
                            }
                            if (roleToAdd == Context.Guild.GetRole(roleCook))
                            {
                              await ReplyErrorLocalizedAsync("promote_hierarchy_error").ConfigureAwait(false); // error
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
                      // var roleToAdd = Context.Guild.GetRole(706503570622775332);

                      if ((ctx.User.Id != ctx.Guild.OwnerId) && runner1MaxRolePosition <= roleToAdd.Position)
                      await ReplyErrorLocalizedAsync("promote_addrole_error").ConfigureAwait(false);
                      return;
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

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [UserPerm(GuildPerm.ManageRoles)]
            [BotPerm(GuildPerm.ManageRoles)]
            public async Task SetRole(IGuildUser targetUser, [Leftover] IRole roleToAdd)
            {
                var runnerUser = (IGuildUser)ctx.User;
                var runnerMaxRolePosition = runnerUser.GetRoles().Max(x => x.Position);
                if ((ctx.User.Id != ctx.Guild.OwnerId) && runnerMaxRolePosition <= roleToAdd.Position)
                    return;
                try
                {
                    await targetUser.AddRoleAsync(roleToAdd).ConfigureAwait(false);

                    await ReplyConfirmLocalizedAsync("setrole", Format.Bold(roleToAdd.Name), Format.Bold(targetUser.ToString()))
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await ReplyErrorLocalizedAsync("setrole_err").ConfigureAwait(false);
                    _log.Info(ex);
                }
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [UserPerm(GuildPerm.ManageRoles)]
            [BotPerm(GuildPerm.ManageRoles)]
            public async Task RemoveRole(IGuildUser targetUser, [Leftover] IRole roleToRemove)
            {
                var runnerUser = (IGuildUser)ctx.User;
                if (ctx.User.Id != runnerUser.Guild.OwnerId && runnerUser.GetRoles().Max(x => x.Position) <= roleToRemove.Position)
                    return;
                try
                {
                    await targetUser.RemoveRoleAsync(roleToRemove).ConfigureAwait(false);
                    await ReplyConfirmLocalizedAsync("remrole", Format.Bold(roleToRemove.Name), Format.Bold(targetUser.ToString())).ConfigureAwait(false);
                }
                catch
                {
                    await ReplyErrorLocalizedAsync("remrole_err").ConfigureAwait(false);
                }
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [UserPerm(GuildPerm.ManageRoles)]
            [BotPerm(GuildPerm.ManageRoles)]
            public async Task RenameRole(IRole roleToEdit, string newname)
            {
                var guser = (IGuildUser)ctx.User;
                if (ctx.User.Id != guser.Guild.OwnerId && guser.GetRoles().Max(x => x.Position) <= roleToEdit.Position)
                    return;
                try
                {
                    if (roleToEdit.Position > (await ctx.Guild.GetCurrentUserAsync().ConfigureAwait(false)).GetRoles().Max(r => r.Position))
                    {
                        await ReplyErrorLocalizedAsync("renrole_perms").ConfigureAwait(false);
                        return;
                    }
                    await roleToEdit.ModifyAsync(g => g.Name = newname).ConfigureAwait(false);
                    await ReplyConfirmLocalizedAsync("renrole").ConfigureAwait(false);
                }
                catch (Exception)
                {
                    await ReplyErrorLocalizedAsync("renrole_err").ConfigureAwait(false);
                }
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [UserPerm(GuildPerm.ManageRoles)]
            [BotPerm(GuildPerm.ManageRoles)]
            public async Task RemoveAllRoles([Leftover] IGuildUser user)
            {
                var guser = (IGuildUser)ctx.User;

                var userRoles = user.GetRoles().Except(new[] { guser.Guild.EveryoneRole });
                if (user.Id == ctx.Guild.OwnerId || (ctx.User.Id != ctx.Guild.OwnerId && guser.GetRoles().Max(x => x.Position) <= userRoles.Max(x => x.Position)))
                    return;
                try
                {
                    await user.RemoveRolesAsync(userRoles).ConfigureAwait(false);
                    await ReplyConfirmLocalizedAsync("rar", Format.Bold(user.ToString())).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    await ReplyErrorLocalizedAsync("rar_err").ConfigureAwait(false);
                }
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [UserPerm(GuildPerm.ManageRoles)]
            [BotPerm(GuildPerm.ManageRoles)]
            public async Task CreateRole([Leftover] string roleName = null)
            {
                if (string.IsNullOrWhiteSpace(roleName))
                    return;

                var r = await ctx.Guild.CreateRoleAsync(roleName, isMentionable: false).ConfigureAwait(false);
                await ReplyConfirmLocalizedAsync("cr", Format.Bold(r.Name)).ConfigureAwait(false);
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [UserPerm(GuildPerm.ManageRoles)]
            [BotPerm(GuildPerm.ManageRoles)]
            public async Task DeleteRole([Leftover] IRole role)
            {
                var guser = (IGuildUser)ctx.User;
                if (ctx.User.Id != guser.Guild.OwnerId
                    && guser.GetRoles().Max(x => x.Position) <= role.Position)
                    return;

                await role.DeleteAsync().ConfigureAwait(false);
                await ReplyConfirmLocalizedAsync("dr", Format.Bold(role.Name)).ConfigureAwait(false);
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [UserPerm(GuildPerm.ManageRoles)]
            [BotPerm(GuildPerm.ManageRoles)]
            public async Task RoleHoist(IRole role)
            {
                var newHoisted = !role.IsHoisted;
                await role.ModifyAsync(r => r.Hoist = newHoisted).ConfigureAwait(false);
                if (newHoisted)
                {
                    await ReplyConfirmLocalizedAsync("rolehoist_enabled", Format.Bold(role.Name)).ConfigureAwait(false);
                }
                else
                {
                    await ReplyConfirmLocalizedAsync("rolehoist_disabled", Format.Bold(role.Name)).ConfigureAwait(false);
                }
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [Priority(1)]
            public async Task RoleColor([Leftover] IRole role)
            {
                await ctx.Channel.SendConfirmAsync("Role Color", role.Color.RawValue.ToString("x6")).ConfigureAwait(false);
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [UserPerm(GuildPerm.ManageRoles)]
            [BotPerm(GuildPerm.ManageRoles)]
            [Priority(0)]
            public async Task RoleColor(IRole role, SixLabors.ImageSharp.Color color)
            {
                try
                {
                    var rgba32 = color.ToPixel<Rgba32>();
                    await role.ModifyAsync(r => r.Color = new Color(rgba32.R, rgba32.G, rgba32.B)).ConfigureAwait(false);
                    await ReplyConfirmLocalizedAsync("rc", Format.Bold(role.Name)).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    await ReplyErrorLocalizedAsync("rc_perms").ConfigureAwait(false);
                }
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            [UserPerm(GuildPerm.MentionEveryone)]
            [BotPerm(GuildPerm.ManageRoles)]
            public async Task MentionRole([Leftover] IRole role)
            {
                if (!role.IsMentionable)
                {
                    await role.ModifyAsync(x => x.Mentionable = true).ConfigureAwait(false);
                    await ctx.Channel.SendMessageAsync(role.Mention).ConfigureAwait(false);
                    await role.ModifyAsync(x => x.Mentionable = false).ConfigureAwait(false);
                }
                else
                {
                    await ctx.Channel.SendMessageAsync(role.Mention).ConfigureAwait(false);
                }
            }
        }
    }
}
