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
    public partial class Games
    {
        [Group]
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

                var nickOrUser = usr.Nickname;
                if (usr.Nickname == null) { nickOrUser = usr.Username; }

                string[] reply =
                {
              "Don't slap " + nickOrUser + "! thats mean >:(",
              "slaps " + nickOrUser + "'s butt cheeks",
              "You slapped " + nickOrUser + "! Meanie!",
              "You slapped "+ nickOrUser + ". I thought you were friends :(",
              "You slapped " + nickOrUser + ", why don't you ever slap me daddy UwU",
              "You slapped " + nickOrUser + " nyaaa~~~~",
              "slapping " + nickOrUser + " is understandable, i don't blame you",
              "slip slap slop, " + nickOrUser + " fucking died.",
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
                int option = 0;

                if (reply[replyString] == "Don't slap " + nickOrUser + "! thats mean >:(") // 1 "error" (no slap) situation to mix things up
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

                var nickOrUser = usr.Nickname;
                if (usr.Nickname == null) { nickOrUser = usr.Username; }

                string[] reply =
                {
              "You aggressively hug " + nickOrUser + ".",
              "You hug " + nickOrUser + " tightly, cute!",
              "You awkwardly place your arms around " + nickOrUser + " without actually touching their body.",
              "You hug " + nickOrUser + ", you can feel the warmth from their body.",
              nickOrUser + " rejects a hug from you. that's what you get for being a SIMP.",
              "WOAH! You hug " + nickOrUser + " like you've never hugged someone before",
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

                if ((reply[replyString] == nickOrUser + " rejects a hug from you. that's what you get for being a SIMP.")) // 1 "error" (no hug) situations to mix things up
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

                var nickOrUser = usr.Nickname;
                if (usr.Nickname == null) { nickOrUser = usr.Username; }


                string[] reply =
                {
              "Sexxing " + nickOrUser + " sexxing " + usr.Username + " sexxing " + usr.Username + "!",
              "You paid " + nickOrUser + " to sex you.",
              "You aggressively sex " + nickOrUser + ".",
              "YEAHHHHHH SEX WITH " + nickOrUser + ".",
              "Sex with " + nickOrUser + " is always awkward.",
              nickOrUser + " a-ahh~ not so h-hard~~.",
              "Nee papa nee, te diep kut aaaaaaaaaaaaaaaaaaaaaaaaaa.",
              "You perform sex TERRIBLY on " + nickOrUser + ".",
              "Why would you ever wanna fuck that?!",
            };

                Random rand = new Random();
                int replyString = rand.Next(reply.Length);
                var av = ctx.User.RealAvatarUrl();
                int option = 0;

                if (reply[replyString] == "Why would you ever wanna fuck that?!") // 1 "error" (no fuck) situation to mix things up
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

            [NadekoCommand, Usage, Description, Aliases]
            public async Task kiss(IGuildUser usr, [Leftover] string msg = null)
            {
                if (ctx.User.Id == usr.Id)
                {
                    await ctx.Channel.EmbedAsync(
                        new EmbedBuilder().WithErrorColor()
                        .WithAuthor(eab => eab.WithName("Trying to kiss yourself? LOL.")));

                    return;
                }

                var nickOrUser = usr.Nickname;
                if (usr.Nickname == null) { nickOrUser = usr.Username; }

                string[] reply =
                {
              "you slowly insert your tongue into " + nickOrUser + "'s mouth, Nyaaaaa",
              "Kissing " + nickOrUser + " kissing " + nickOrUser + " kissing " + nickOrUser + "!",
              "You paid " + nickOrUser + " to kiss you. DERP",
              "You aggressively tongue " + nickOrUser + "..",
              "YEAHHHHHH " + nickOrUser + " GETS THE BIG KISSES.",
              "Kissing with " + nickOrUser + " is always awkward... but they like it.",
              nickOrUser + " was kissed for the first time in their life. WOOOOOOOO",
              nickOrUser + " rejected your kiss, you fucking die.",
              "You kiss " + nickOrUser + " TERRIBLY.",
            };

                string[] image =
                {
                "https://media.tenor.com/images/45a799f31a273318e1c3f78490f5e34d/tenor.gif",
                "https://media.tenor.com/images/536feb2229b55c1657add7630ef4ffdb/tenor.gif",
                "https://i.pinimg.com/originals/37/f9/f2/37f9f27715e7dec6f2f4b7d63ad1af13.gif",
                "https://i.pinimg.com/originals/3b/c2/ad/3bc2ad46f9870a093a222ff5171639d2.gif",
                "https://media.tenor.com/images/00f8945a8749e2de6876525ea32dd34d/tenor.gif",
                "https://thumbs.gfycat.com/IdolizedNecessaryCow-size_restricted.gif",
                "https://i.pinimg.com/originals/2c/ef/2b/2cef2bb1185493a6d2f723b3d04bd299.gif",
                "https://media.tenor.com/images/e7de08e18e5778d41e7b6884a2ffb7bd/tenor.gif",
                "https://media.tenor.com/images/134bad0dd7b1e2f20f8b4c36ebf8b5b2/tenor.gif",
                "https://i.pinimg.com/originals/0a/a0/ba/0aa0ba9d6c80adefd3077823fde42ff5.gif",
                "https://media.tenor.com/images/84900d5c4088c08cac576f241bfe5d1a/tenor.gif",
                "https://media.tenor.com/images/83bceada9e9a957a3909934de9c4a0f6/tenor.gif",
                "https://media.tenor.com/images/7def7aef1e4ce366c1da194d12d8bc83/tenor.gif",
                "https://i.pinimg.com/originals/35/2d/cc/352dccdf2450e0fe1ee4fce239e372a9.gif",
                "https://media.tenor.com/images/97196e32b62715fca06a151a8e2bd1cd/tenor.gif",
                "https://media.tenor.com/images/f773c40896a968633b613f5cafa08de2/tenor.gif",
                "https://cdn.statically.io/img/i.pinimg.com/originals/bb/32/ce/bb32cea39d78161b9afd34604b88e18a.gif"
            };

                Random rand = new Random();
                int replyString = rand.Next(reply.Length);
                int imageURL = rand.Next(image.Length);
                var av = ctx.User.RealAvatarUrl();
                int option = 0;

                if (reply[replyString] == nickOrUser + " rejected your kiss, you fucking die.") // 1 "error" (no kiss) situation to mix things up
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
        }
    }
}