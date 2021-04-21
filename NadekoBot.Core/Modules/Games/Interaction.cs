using Discord;
using Discord.Commands;
using NadekoBot.Common.Attributes;
using NadekoBot.Extensions;
using NadekoBot.Modules.Games.Services;
using System;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace NadekoBot.Modules.Games
{
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

                var jsonGif = new WebClient().DownloadString("https://gitlab.com/Dok4440/TCUBetaBot/-/raw/develop/TCURawJsons/SlapGifs.json");
                string[] gifs = JsonConvert.DeserializeObject<string[]>(jsonGif);

                Random rand = new Random();
                int imageURL = rand.Next(gifs.Length);
                var av = ctx.User.RealAvatarUrl();

                var nickOrUser = usr.Nickname;
                if (usr.Nickname == null) { nickOrUser = usr.Username; }

                await ctx.Channel.EmbedAsync(
                    new EmbedBuilder().WithOkColor()
                    .WithAuthor(eab => eab.WithName("*slaps " + nickOrUser + "*")
                                          .WithIconUrl(av.ToString()))
                    .WithImageUrl(gifs[imageURL]));
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

            var av = ctx.User.RealAvatarUrl();
            var nickOrUser = usr.Nickname; if (usr.Nickname == null) { nickOrUser = usr.Username; }

            var jsonGif = new WebClient().DownloadString("https://gitlab.com/Dok4440/TCUBetaBot/-/raw/develop/TCURawJsons/HugGifs.json");
            string[] gifs = JsonConvert.DeserializeObject<string[]>(jsonGif);

            Random rand = new Random();
            int imageURL = rand.Next(gifs.Length);

            await ctx.Channel.EmbedAsync(
                    new EmbedBuilder().WithOkColor()
                    .WithAuthor(eab => eab.WithName("*hugs " + nickOrUser + "*")
                                          .WithIconUrl(av.ToString()))
                    .WithImageUrl(gifs[imageURL]));
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

                var jsonGif = new WebClient().DownloadString("https://gitlab.com/Dok4440/TCUBetaBot/-/raw/develop/TCURawJsons/KissGifs.json");
                string[] gifs = JsonConvert.DeserializeObject<string[]>(jsonGif);

                Random rand = new Random();
                int imageURL = rand.Next(gifs.Length);
                var av = ctx.User.RealAvatarUrl();

                var nickOrUser = usr.Nickname;
                if (usr.Nickname == null) { nickOrUser = usr.Username; }

                await ctx.Channel.EmbedAsync(
                        new EmbedBuilder().WithOkColor()
                        .WithAuthor(eab => eab.WithName("*kisses " + nickOrUser + "*")
                                              .WithIconUrl(av.ToString()))
                        .WithImageUrl(gifs[imageURL]));
            }
        }
    }
}
