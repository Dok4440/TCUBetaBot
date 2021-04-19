using Discord;
using Discord.Commands;
using NadekoBot.Common.Attributes;
using NadekoBot.Core.Modules.Searches.Services;
using NadekoBot.Extensions;
using System;
using System.Threading.Tasks;

namespace NadekoBot.Modules.Searches
{
    public partial class Searches
    {
        public class CryptoCommands : NadekoSubmodule<CryptoService>
        {
            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            public async Task Crypto(string name)
            {
                name = name?.ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(name))
                    return;

                var (crypto, nearest) = await _service.GetCryptoData(name).ConfigureAwait(false);

                if (nearest != null)
                {
                    var embed = new EmbedBuilder()
                            .WithTitle(GetText("crypto_not_found"))
                            .WithDescription(GetText("did_you_mean", Format.Bold($"{nearest.Name} ({nearest.Symbol})")));

                    if (await PromptUserConfirmAsync(embed).ConfigureAwait(false))
                    {
                        crypto = nearest;
                    }
                }

                if (crypto == null)
                {
                    await ReplyErrorLocalizedAsync("crypto_not_found").ConfigureAwait(false);
                    return;
                }

                var crypto_Perfect_Change_7d_1 = crypto.Quote.Usd.Percent_Change_7d.Substring(0,6);
                var crypto_Perfect_Change_24h_1 = crypto.Quote.Usd.Percent_Change_24h.Substring(0, 6);

                await ctx.Channel.EmbedAsync(new EmbedBuilder()
                    .WithOkColor()
                    .WithTitle($"{crypto.Name} ({crypto.Symbol})")
                    .WithUrl($"https://coinmarketcap.com/currencies/{crypto.Slug}/")
                    .WithThumbnailUrl($"https://s2.coinmarketcap.com/static/img/coins/128x128/{crypto.Id}.png")
                    .AddField(GetText("market_cap"), $"${crypto.Quote.Usd.Market_Cap:n0}", false)
                    .AddField(GetText("price"), $"${Math.Round(crypto.Quote.Usd.Price, 2)}", false)
                    .AddField(GetText("change_7d_24h"), $"{crypto_Perfect_Change_7d_1}% / {crypto_Perfect_Change_24h_1}%", false));
                    
                    //(GetText("volume_24h"), $"${crypto.Quote.Usd.Volume_24h:n0}", false)
                    //.AddField(GetText("change_7d_24h"), $"{Math.Round(Convert.ToDouble(crypto.Quote.Usd.Percent_Change_7d))}% / {Math.Round(Convert.ToDouble(crypto.Quote.Usd.Percent_Change_24h))}%", false)
                    /*.WithImageUrl($"https://s2.coinmarketcap.com/generated/sparklines/web/7d/usd/{crypto.Id}.png"))*/
            }
        }
    }
}

