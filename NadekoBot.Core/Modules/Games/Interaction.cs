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
        public async Task slap(IGuildUser usr, [Leftover] string msg=null)
        {
            string[] reply = // will continue to update this list whenever i'm in the mood.
          {
              "Imagine slapping " + usr.Mention + ".. They're too precious.",
              "Don't slap " + usr.Mention + "! thats meannnn",
              "slaps " + usr.Mention + "'s butt cheeks",
              "You slapped " + usr.Mention + "! Meanie!",
              "You slapped "+ usr.Mention + ". I thought you were friends :(",
              "You slapped " + usr.Mention + ", why don't you ever slap me daddy UwU",
              "You slapped " + usr.Mention + " nyaaa~~~~",
              "slapping " + usr.Mention + " is inappropriate >:(",
              "slapping " + usr.Mention + " is understandable, i don't blame you"
          };

                Random rand = new Random();
                int index = rand.Next(reply.Length);

                await ctx.Channel.SendMessageAsync(reply[index]);
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
