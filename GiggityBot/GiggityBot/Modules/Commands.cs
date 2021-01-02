using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace GiggityBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private Discord.EmbedBuilder embedBuilder = new Discord.EmbedBuilder();

        public static async Task Scan(Discord.WebSocket.SocketUserMessage message)
        {
            Commands _commands = new Commands();
            if (message.Content.Contains("giggity"))
                await _commands.Giggity();
        }

        #region prefix commands

        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong, Latency: **" + Base._client.Latency + "ms**");
        }

        [Command("help")]
        public async Task Help()
        {
            await ReplyAsync("fuck off loser");
            await Task.Delay(8000);
            await ReplyAsync("lol jkjk");
            await Task.Delay(500);
            embedBuilder.WithTitle("Commands");
            embedBuilder.WithImageUrl("https://media.discordapp.net/attachments/734949688754700482/794991036309045348/quag.gif");
            embedBuilder.AddField("q!fart", "funny fart lol xd", true);
            embedBuilder.AddField("q!ping", "returns the ping", true);
            embedBuilder.WithColor(Discord.Color.Red);
            await Task.Delay(100);
            await ReplyAsync("", false, embedBuilder.Build());
        }

        [Command("fart")]
        public async Task Fart()
        {
            await ReplyAsync("gian pls send fart copypasta");
        }

        #endregion

        #region scan commands

        private async Task Giggity()
        {
            await ReplyAsync("giggity goo"); 
        }

        #endregion
    }
}
