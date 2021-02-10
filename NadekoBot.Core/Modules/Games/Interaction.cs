using Discord;
using Discord.Commands;
using NadekoBot.Common;
using NadekoBot.Common.Attributes;
using NadekoBot.Core.Services;
using NadekoBot.Extensions;
using NadekoBot.Modules.Games.Common;
using NadekoBot.Modules.Games.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace NadekoBot.Modules.Games
{
    /* more games
    - Shiritori
    - Simple RPG adventure
    */
    public partial class Interaction : NadekoTopLevelModule<GamesService>
    {
        [NadekoCommand, Usage, Description, Aliases]
        public async Task slap(IGuildUser usr, [Leftover] string msg = null)
        {
            string[] reply =
            {
              "Imagine slapping " + usr.Nickname + ".. They're too precious.",
              "Don't slap " + usr.Nickname + "! thats meannnn",
              "slaps " + usr.Nickname + "'s butt cheeks",
              "You slapped " + usr.Nickname + "! Meanie!",
              "You slapped "+ usr.Nickname + ". I thought you were friends :(",
              "You slapped " + usr.Nickname + ", why don't you ever slap me daddy UwU",
              "You slapped " + usr.Nickname + " nyaaa~~~~",
              "slapping " + usr.Nickname + " is inappropriate >:(",
              "slapping " + usr.Nickname + " is understandable, i don't blame you",
              "slip slap slop, " + usr.Nickname + " fucking died.",
            };
            string[] image =
            {
                "https://i.gifer.com/XaaW.gif",
                "https://i.gifer.com/V6EH.gif",
                "https://i.gifer.com/VVm2.gif",
                "https://i.gifer.com/90du.gif",
                "https://i.gifer.com/n1.gif",
                "https://i.gifer.com/X35p.gif",
                "https://i.gifer.com/RK9x.gif",
                "https://i.gifer.com/2VjD.gif",
                "https://i.gifer.com/tmp.gif",
                "https://i.gifer.com/nM.gif",
                "https://i.gifer.com/7HBx.gif",
                "https://i.gifer.com/7Kr.gif"
            };

                Random rand = new Random();
                int replyString = rand.Next(reply.Length);
                int imageURL = rand.Next(image.Length);
                var av = ctx.User.RealAvatarUrl();
                int option=0;

            if ((reply[replyString] == "Imagine slapping " + usr.Nickname + ".. They're too precious.")
                || (reply[replyString] == "Don't slap " + usr.Nickname + "! thats meannnn")
                || (reply[replyString] == "slapping " + usr.Nickname + " is inappropriate >:("))
                 { option = 1; }
            else { option = 2; }


            if (option == 2)
            {
                await ctx.Channel.EmbedAsync(
                    new EmbedBuilder().WithOkColor()
                    .WithAuthor(eab => eab.WithName(reply[replyString])
                                          .WithIconUrl(av.ToString()))
                    .WithImageUrl(image[imageURL]));
            }

            if (option == 1)
            {
                await ctx.Channel.EmbedAsync(
                    new EmbedBuilder().WithErrorColor()
                    .WithAuthor(eab => eab.WithName(reply[replyString])));
            }

        }


        [NadekoCommand, Usage, Description, Aliases]
        public async Task hug(IGuildUser usr, [Leftover] string msg = null)
        {
            string[] reply = // will continue to update this list whenever i'm in the mood.
          {
              "You aggressively hug " + usr.Mention + ".",
              "You hug " + usr.Mention + " tightly, cute!",
              "You awkwardly place your arms around " + usr.Mention + " without actually touching their body.",
              usr.Mention + " pushed you away and RKO'd you!",
              "You hug " + usr.Mention + " you can feel the warmth from their body.",
              usr.Mention + " rejects a hug from you. that's what you get for being a SIMP.",
              "You just got arrested for sexual assault, yikes.",
          };

            Random rand = new Random();
            int index = rand.Next(reply.Length);

            await ctx.Channel.SendMessageAsync(reply[index]);
        }

        [NadekoCommand, Usage, Description, Aliases]
        public async Task fuck(IGuildUser usr, [Leftover] string msg = null)
        {
            string[] reply = // will continue to update this list whenever i'm in the mood.
          {
              //"You fuck {user}'s tight asshole/pussy",
              // "you slowly insert your cock into {user}, Nyaaaaa",
              // "you slowly start pegging {user}, kinky uwu",
              // "you have been arrested for attempted rape",
              "Sexxing " + usr.Mention + " sexxing " + usr.Mention + " sexxing " + usr.Mention + "!",
              "You paid " + usr.Mention + " to sex you.",
              "You aggressively sex " + usr.Mention + ".",
              "YEAHHHHHH SEX WITH " + usr.Mention + ".",
              "Sex with " + usr.Mention + " is always awkward.",
              usr.Mention + " a-ahh~ not so h-hard~~.",
              "Nee papa nee, te diep kut aaaaaaaaaaaaaaaaaaaaaaaaaa.",
              "You perform sex TERRIBLY on " + usr.Mention + ".",
              "Why would you ever wanna fuck that?!",
          };

            Random rand = new Random();
            int index = rand.Next(reply.Length);

            await ctx.Channel.SendMessageAsync(reply[index]);
        }
    }
}   
