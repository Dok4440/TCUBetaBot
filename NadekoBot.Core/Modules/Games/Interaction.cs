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
            if (ctx.User.Id == usr.Id)
            {
                await ctx.Channel.EmbedAsync(
                    new EmbedBuilder().WithErrorColor()
                    .WithAuthor(eab => eab.WithName("Trying to slap yourself? That's adorable...")));

                return;
            }
            string[] reply =
            {
              "Imagine slapping " + usr.Nickname + ".. They're too precious.",
              "Don't slap " + usr.Nickname + "! thats mean >:(",
              "slaps " + usr.Nickname + "'s butt cheeks",
              "You slapped " + usr.Nickname + "! Meanie!",
              "You slapped "+ usr.Nickname + ". I thought you were friends :(",
              "You slapped " + usr.Nickname + ", why don't you ever slap me daddy UwU",
              "You slapped " + usr.Nickname + " nyaaa~~~~",
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
                || (reply[replyString] == "Don't slap " + usr.Nickname + "! thats mean >:(")) // 2 "error" (no hug) situations to mix things up
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
            if (ctx.User.Id == usr.Id)
            {
                await ctx.Channel.EmbedAsync(
                    new EmbedBuilder().WithErrorColor()
                    .WithAuthor(eab => eab.WithName("Trying to hug yourself? Pathetic.")));

                return;
            }

            string[] reply =
            {
              "You aggressively hug " + usr.Nickname + ".",
              "You hug " + usr.Nickname + " tightly, cute!",
              "You awkwardly place your arms around " + usr.Nickname + " without actually touching their body.",
              usr.Nickname + " pushed you away and RKO'd you!",
              "You hug " + usr.Nickname + ", you can feel the warmth from their body.",
              usr.Nickname + " rejects a hug from you. that's what you get for being a SIMP.",
              "WOAH! You hug " + usr.Nickname + " like you've never hugged someone before",
            };
            string[] image =
            {
                "https://media.giphy.com/media/llmZp6fCVb4ju/giphy.gif",
                "https://media.giphy.com/media/Kf44fYvVuSbJu/giphy.gif",
                "https://media.giphy.com/media/z20kiXXSroFErlcDTf/giphy.gif",
                "http://pa1.narvii.com/7526/dc2c2a8f1e93e2829456a81c0fea0fb8b2ea6ea4r1-245-220_00.gif",
                "https://www.icegif.com/wp-content/uploads/hug-kiss-icegif.gif",
                "https://media.tenor.com/images/72a6612f9ee8586cd272de2342230791/tenor.gif",
                "https://media.tenor.com/images/ff4a60a02557236c910f864611271df2/tenor.gif",
                "https://media.tenor.com/images/2bb9e56d8982c9e806d33aed404a62c0/tenor.gif",
                "https://media.tenor.com/images/f6f20cda181ac07db50be80cdc4fa0c8/tenor.gif",
                "https://media.tenor.com/images/2d45b2e842cac286aa91cec91a7a17d7/tenor.gif"
            };

            Random rand = new Random();
            int replyString = rand.Next(reply.Length);
            int imageURL = rand.Next(image.Length);
            var av = ctx.User.RealAvatarUrl();
            int option = 0;

            if ((reply[replyString] == usr.Nickname + " pushed you away and RKO'd you!")
                || (reply[replyString] == usr.Nickname + " rejects a hug from you. that's what you get for being a SIMP.")) // 2 "error" (no hug) situations to mix things up
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
        public async Task fuck(IGuildUser usr, [Leftover] string msg = null)
        {
            if (ctx.User.Id == usr.Id)
            {
                await ctx.Channel.EmbedAsync(
                    new EmbedBuilder().WithErrorColor()
                    .WithAuthor(eab => eab.WithName("Trying to fuck yourself? That's pretty gay, ngl.")));

                return;
            }

            string[] reply =
            {
              //"You fuck {user}'s tight asshole/pussy",
              // "you slowly insert your cock into {user}, Nyaaaaa",
              // "you slowly start pegging {user}, kinky uwu",
              // "you have been arrested for attempted rape",
              "Sexxing " + usr.Username + " sexxing " + usr.Username + " sexxing " + usr.Username + "!",
              "You paid " + usr.Username + " to sex you.",
              "You aggressively sex " + usr.Username + ".",
              "YEAHHHHHH SEX WITH " + usr.Username + ".",
              "Sex with " + usr.Username + " is always awkward.",
              usr.Username + " a-ahh~ not so h-hard~~.",
              "Nee papa nee, te diep kut aaaaaaaaaaaaaaaaaaaaaaaaaa.",
              "You perform sex TERRIBLY on " + usr.Username + ".",
              "Why would you ever wanna fuck that?!",
            };

            Random rand = new Random();
            int replyString = rand.Next(reply.Length);
            var av = ctx.User.RealAvatarUrl();
            int option = 0;

            if (reply[replyString] == "Why would you ever wanna fuck that?!") // 1 "error" (no hug) situation to mix things up
                 { option = 1; }
            else { option = 2; }


            if (option == 2)
            {
                await ctx.Channel.EmbedAsync(
                    new EmbedBuilder().WithOkColor()
                    .WithAuthor(eab => eab.WithName(reply[replyString])
                                          .WithIconUrl(av.ToString())));
            }

            if (option == 1)
            {
                await ctx.Channel.EmbedAsync(
                    new EmbedBuilder().WithErrorColor()
                    .WithAuthor(eab => eab.WithName(reply[replyString])));
            }
        }
    }
}