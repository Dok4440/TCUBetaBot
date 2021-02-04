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
    public partial class Games : NadekoTopLevelModule<GamesService>
    {
        private readonly IImageCache _images;
        private readonly IHttpClientFactory _httpFactory;
        private readonly Random _rng = new Random();

        public Games(IDataCache data, IHttpClientFactory factory)
        {
            _images = data.LocalImages;
            _httpFactory = factory;
        }

        [NadekoCommand, Usage, Description, Aliases]
        public async Task Choose([Leftover] string list = null)
        {
            if (string.IsNullOrWhiteSpace(list))
                return;
            var listArr = list.Split(';');
            if (listArr.Length < 2)
                return;
            var rng = new NadekoRandom();
            await ctx.Channel.SendConfirmAsync("🤔", listArr[rng.Next(0, listArr.Length)]).ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        public async Task EightBall([Leftover] string question = null)
        {
            if (string.IsNullOrWhiteSpace(question))
                return;

            await ctx.Channel.EmbedAsync(new EmbedBuilder().WithColor(NadekoBot.OkColor)
                .WithDescription(ctx.User.ToString())
                .AddField(efb => efb.WithName("❓ " + GetText("question")).WithValue(question).WithIsInline(false))
                .AddField(efb => efb.WithName("🎱 " + GetText("8ball")).WithValue(_service.EightBallResponses[new NadekoRandom().Next(0, _service.EightBallResponses.Length)]).WithIsInline(false))).ConfigureAwait(false);
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        public async Task RateGirl(IGuildUser usr)
        {
            var gr = _service.GirlRatings.GetOrAdd(usr.Id, GetGirl);
            var originalStream = await gr.Stream;

            if (originalStream == null)
            {
                await ReplyErrorLocalizedAsync("something_went_wrong").ConfigureAwait(false);
                return;
            }

            using (var imgStream = new MemoryStream())
            {
                lock (gr)
                {
                    originalStream.Position = 0;
                    originalStream.CopyTo(imgStream);
                }
                imgStream.Position = 0;
                await ctx.Channel.SendFileAsync(stream: imgStream,
                    filename: $"girl_{usr}.png",
                    text: Format.Bold($"{ctx.User.Mention} Girl Rating For {usr}"),
                    embed: new EmbedBuilder()
                        .WithOkColor()
                        .AddField(efb => efb.WithName("Hot").WithValue(gr.Hot.ToString("F2")).WithIsInline(true))
                        .AddField(efb => efb.WithName("Crazy").WithValue(gr.Crazy.ToString("F2")).WithIsInline(true))
                        .AddField(efb => efb.WithName("Advice").WithValue(gr.Advice).WithIsInline(false))
                        .Build()).ConfigureAwait(false);
            }
        }

        private double NextDouble(double x, double y)
        {
            return _rng.NextDouble() * (y - x) + x;
        }

        private GirlRating GetGirl(ulong uid)
        {
            var rng = new NadekoRandom();

            var roll = rng.Next(1, 1001);

            var ratings = _service.Ratings.GetAwaiter().GetResult();

            double hot;
            double crazy;
            string advice;
            if (roll < 500)
            {
                hot = NextDouble(0, 5);
                crazy = NextDouble(4, 10);
                advice = ratings.Nog;
            }
            else if (roll < 750)
            {
                hot = NextDouble(5, 8);
                crazy = NextDouble(4, .6 * hot + 4);
                advice = ratings.Fun;
            }
            else if (roll < 900)
            {
                hot = NextDouble(5, 10);
                crazy = NextDouble(.61 * hot + 4, 10);
                advice = ratings.Dan;
            }
            else if (roll < 951)
            {
                hot = NextDouble(8, 10);
                crazy = NextDouble(7, .6 * hot + 4);
                advice = ratings.Dat;
            }
            else if (roll < 990)
            {
                hot = NextDouble(8, 10);
                crazy = NextDouble(5, 7);
                advice = ratings.Wif;
            }
            else if (roll < 999)
            {
                hot = NextDouble(8, 10);
                crazy = NextDouble(2, 3.99d);
                advice = ratings.Tra;
            }
            else
            {
                hot = NextDouble(8, 10);
                crazy = NextDouble(4, 5);
                advice = ratings.Uni;
            }

            return new GirlRating(_images, _httpFactory, crazy, hot, roll, advice);
        }
    }
}
