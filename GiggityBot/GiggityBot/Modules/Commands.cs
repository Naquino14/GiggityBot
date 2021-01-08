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
using GiggityBot.Modules;

namespace GiggityBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        #region required variables
        private EmbedBuilder embedBuilder = new EmbedBuilder();
        private SocketUserMessage _message;
        private SocketCommandContext _context;
        private WordArrays wordArrays;
        public static Commands commands; // ex use only
        private Random random;
        public SaveData saveData = new SaveData(); 
        #endregion

        #region global variables

        private int _char;
        private DateTime timeNow = new DateTime();
        private DateTime lastTime = new DateTime();
        private TimeSpan elapsedTime = new TimeSpan();
        bool updateTimeFireOnce = true;
        public ArrayList whitelistedChannels = new ArrayList();


        #endregion


        public static async Task Scan(SocketUserMessage message, SocketCommandContext context) // scans all messages
        {
            Commands _commands = new Commands();
            commands = _commands;
            if (_commands.updateTimeFireOnce)
            {
                UpdateTime();
                _commands.updateTimeFireOnce = false;
            }
            _commands._context = context;
            _commands._message = message;
            _commands.wordArrays = Base._wordArrays;

            if (message.Content.Contains("giggity")) // if giggity is said
                await _commands.Giggity();
            if (message.Content.Contains("/fart")) // if /fart is said
                await _commands.Fart();
            if (message.Content.Contains("/bike"))
                await _commands.Bike();

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

            foreach (string word in _commands.wordArrays.familyManCharacters)
            {
                if (message.Content.Contains(word))
                {
                    await _commands.CharacterResponse(_commands._char);
                    break;
                }
                _commands._char++;
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
            embedBuilder.AddField("q!info", "bot info", true);
            embedBuilder.AddField("q!whitelistchannel", "blacklist a channel from bot responses (not working atm)", false);
            embedBuilder.WithColor(Discord.Color.Red);
            await Task.Delay(100);
            await ReplyAsync("", false, embedBuilder.Build());
        }

        [Command("info")]
        public async Task Info()
        {
            embedBuilder.WithTitle("Info");
            embedBuilder.AddField("Author", "Vinetta#5601");
            embedBuilder.AddField("Am I a family guy fan?", "I actually dont watch family guy.");
            embedBuilder.AddField("Bot birthday", "1/2/2021");
            //UpTime();
            //embedBuilder.AddField("Uptime", "Days: " + elapsedTime.Days + " Hours: " + elapsedTime.Hours + " Minutes: " + elapsedTime.Hours + " Seconds: " + elapsedTime.Seconds);
            embedBuilder.WithColor(Discord.Color.Red);
            await Task.Delay(100);
            await ReplyAsync("", false, embedBuilder.Build());
        }

        [Command("blacklistchannel")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.KickMembers)] // mod check
        public async Task BlacklistChannel(string channel)
        {
            try
            {
                if (channel == null) // check if parameter null
                {
                    await ReplyAsync("Channel name cannot be empty.");
                    return;
                }

                ArrayList channels = new ArrayList();
                bool _continue = false;
                foreach (SocketGuildChannel channelName in Context.Guild.Channels) // for every channel name in the server's channels
                {
                    if (channelName.Name == channel) // if one is found, continue
                        _continue = true;

                }
                if (!_continue) // if not found, return
                {
                    await ReplyAsync("Channel " + channel +" not found.");
                    return;
                }

                // add to json and whitelist array
                whitelistedChannels.Add(channel);
                string serverName = Context.Guild.Name;
                string _channel = channel.Split('#')[1]; // get everything after the #
                saveData.Save(serverName, _channel);

                await ReplyAsync("Successfully blacklisted " + _channel);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }

        #endregion

        #region scan commands

        private async Task Giggity()
        {
            random = new Random();
            int randC = random.Next(0, 100); // 75% chance that it dont do anything
            if (randC > 25)
                return;

            int arM = wordArrays.funnyResponses.Count;
            int rand = random.Next(0, arM);
            string randS = (string)wordArrays.funnyResponses[rand];
            await _context.Channel.SendMessageAsync(randS);
        }
        private async Task Fart()
        {
            string _user = _context.User.Username;
            await _context.Channel.SendMessageAsync("**[ATTENTION ALL PERSONELL]:** " + _user + " has relieved himself of his pain, and has fumed a gigantic **FART!**");
        }

        private async Task Bike() => await _context.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/388795923360120834/795146188432080926/bike.mp4");
        private async Task Booba()
        {
            random = new Random();
            int randC = random.Next(0, 100); // 75% chance that it dont do anything
            if (randC > 25)
                return;

            int arM = wordArrays.hotBoobResponse.Count;
            int rand = random.Next(0, arM);
            string randS = (string)wordArrays.hotBoobResponse[rand];
            await _context.Channel.SendMessageAsync(randS);
        }

        private async Task CharacterResponse(int _char)
        {
            random = new Random();
            int randC = random.Next(0, 100);
            if (randC > 50)
                return;
            int arM;
            int rand;
            string randS = null;

            if (_char == 0) // peter's 3 responses
            {
                arM = 3;
                rand = random.Next(0, arM);
                randS = (string)wordArrays.familyManResponses[rand];
            } else
            if (_char > 0 && _char <= 2) // brian's 2 responses
            {
                arM = 2;
                rand = random.Next(0, arM) + 3;
                randS = (string)wordArrays.familyManResponses[rand];
            } else
            if (_char > 2 && _char <= 4) // lois's 2 responses
            {
                arM = 2;
                rand = random.Next(0, arM) + 5;
                randS = (string)wordArrays.familyManResponses[rand];
            } else
            if (_char > 4 && _char <= 5) // quagmire's 2 responses
            {
                arM = 4;
                rand = random.Next(0, arM) + 7;
                randS = (string)wordArrays.familyManResponses[rand];
            } else
            if (_char > 5 && _char <= 6) // stewie's 2 responses
            {
                arM = 2;
                rand = random.Next(0, arM) + 11;
                randS = (string)wordArrays.familyManResponses[rand];
            } else
            if (_char > 6 && _char <= 7) // chris's 3 responses
            {
                arM = 3;
                rand = random.Next(0, arM) + 13;
                randS = (string)wordArrays.familyManResponses[rand];
            } else
            if (_char > 7 && _char <= 9) // joe's 2 responses
            {
                arM = 2;
                rand = random.Next(0, arM) + 17;
                randS = (string)wordArrays.familyManResponses[rand];
            } else
            {
                randS = "havent seen them in ages...";
            }
            await _context.Channel.SendMessageAsync(randS);

        }
        #endregion

        #region other functions

        public void UpTime()
        {
            timeNow = DateTime.Now;
            elapsedTime = timeNow - lastTime;
        }
        public static void UpdateTime()
        {
            Commands __commands = commands;
            __commands.lastTime = DateTime.Now;
        }

        #endregion
       
    }
}