using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Threading;
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
        public static Commands commands; // ex use only
        #endregion

        #region global variables

        private bool gigityCheck = true;
        private long timeSinceLastGigCheck;

        #endregion


        public static async Task Scan(SocketUserMessage message, SocketCommandContext context) // scans all messages
        {
            Commands _commands = new Commands();
            commands = _commands;
            _commands._context = context;
            _commands._message = message;
            _commands.wordArrays = Base._wordArrays;

            if (message.Content.Contains("giggity")) // if giggity is said
                await _commands.Giggity();
            if (message.Content.Contains("/fart")) // if /fart is said
                await _commands.Fart();

            foreach (string word in _commands.wordArrays.funnyWords) // giggity response triggers
            {
                if (message.Content.Contains(word))
                {
                    await _commands.Giggity();
                    break;
                }
            }
            
            foreach (string word in _commands.wordArrays.hotBoob)
            {
                if (message.Content.Contains(word))
                {
                    await _commands.Booba();
                    break;
                }
            }

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
            await Task.Delay(4000);
            await ReplyAsync("lol jkjk");
            await Task.Delay(500);
            embedBuilder.WithTitle("Commands");
            embedBuilder.WithImageUrl("https://media.discordapp.net/attachments/734949688754700482/794991036309045348/quag.gif");
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

        private async Task Bike() => await _context.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/388795923360120834/795146188432080926/bike.mp4");
        private async Task Booba() => await _context.Channel.SendFileAsync("I agree, booba kinda pog doe.");

        #endregion

        #region other functions



        #endregion
       
    }
}