using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GiggityBot.Resources;

namespace GiggityBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        #region required variables
        private Discord.EmbedBuilder embedBuilder = new Discord.EmbedBuilder();
        private SocketUserMessage _message;
        private SocketCommandContext _context;
        private WordArrays wordArrays;
        #endregion

        #region global variables
        


        #endregion


        public static async Task Scan(Discord.WebSocket.SocketUserMessage message, Discord.Commands.SocketCommandContext context) // scans all messages
        {
            Commands _commands = new Commands();
            _commands._context = context;
            _commands._message = message;
            _commands.wordArrays = Base._wordArrays;

            if (message.Content.Contains("giggity") && _commands._context != null) // if giggity is said
                await _commands.Giggity();
            if (message.Content.Contains("/fart"))
                await _commands.Fart();
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
            embedBuilder.WithImageUrl("Resources/Images/quag.gif");
            embedBuilder.AddField("q!fart", "funny fart lol xd", true);
            embedBuilder.AddField("q!ping", "returns the ping", true);
            embedBuilder.WithColor(Discord.Color.Red);
            await Task.Delay(100);
            await ReplyAsync("", false, embedBuilder.Build());
        }

        #endregion

        #region scan commands

        private async Task Giggity() => await _context.Channel.SendMessageAsync("giggity giggity goo");
        private async Task Fart()
        {
            string _user = _context.User.Username;
            await _context.Channel.SendMessageAsync("**[ATTENTION ALL PERSONELL]:** " + _user + " has relieved himself of his pain, and has fumed a gigantic **FART!**");
        }

        #endregion
    }
}
