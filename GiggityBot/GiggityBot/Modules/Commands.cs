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

        private string serverWithBlacklist;
        private uint channelBlacklist;

        private bool gromSpeakFO = true;

        const string mcServerExecutable = "java.exe";
        const ulong gamingChannelId = 615369865305260047;
        const ulong mcServerGangRoleId = 736043415875223574;
        const string serverExecPath = @"C:\Users\naqui\Desktop\mc server\TOMCServer\Minecraft server survuival 1.12.2\start.bat";
        #endregion

        #region media variables

        private readonly string funny = "https://cdn.discordapp.com/attachments/566874876296691712/799042385485103124/video0.mp4";
        private readonly string funny2 = "https://cdn.discordapp.com/attachments/388795923360120834/798924241256972318/speak.mp4";

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
            if (message.Content.Contains("trollface video"))
                await _commands.Troll();
            if (message.Content.Contains("remy speak"))
                await _commands.Speak();
            if (message.Content.Contains("speak") && message.Author.Id == 757473997326647386 && _commands.gromSpeakFO)
                await _commands.GromSpeak();


            if ((message.Content.Contains("quag") || message.Content.Contains("quagmire")) && (message.Content.Contains("real") || message.Content.Contains("true")))
                await _commands.RealOrFake();



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
        public async Task Help(string parameter = null)
        {
            if (parameter == "server" && Context.Channel.Id == gamingChannelId)
            {
                embedBuilder.WithTitle("MC Server Commands");
                embedBuilder.WithImageUrl("https://cdn.iconscout.com/icon/free/png-512/minecraft-15-282774.png");
                embedBuilder.AddField("q!serverstat", "Returns wether or not the server executable is running on the host.", true);
                embedBuilder.AddField("q!startserver", "Tells me to run the minecraft server.", true);
                embedBuilder.AddField("q!stopserver", "Tells me to stop the minecraft server. (DOES NOT SAVE!!!)", true);
                embedBuilder.AddField("q!restartserver", "Tells me to restart the minecraft server. (DOES NOT SAVE!!!)", true);
                embedBuilder.WithColor(Discord.Color.Green);
                await ReplyAsync("", false, embedBuilder.Build());
                return;
            } else if (parameter == "server" && Context.Channel.Id != gamingChannelId)
            {
                await ReplyAsync("This channel does not meet the requirements to execute this command.");
                return;
            }
            await ReplyAsync("fuck off loser");
            await Task.Delay(4000);
            await ReplyAsync("lol jkjk");
            await Task.Delay(500);
            embedBuilder.WithTitle("Commands");
            embedBuilder.WithImageUrl("https://media.discordapp.net/attachments/734949688754700482/794991036309045348/quag.gif");
            embedBuilder.AddField("q!ping", "returns the ping", true);
            embedBuilder.AddField("q!info", "bot info", true);
            embedBuilder.AddField("q!blacklistchannel [channel]", "blacklist a channel from bot responses (not working atm)", false);
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
                ulong _id = 0;
                channel = channel.Split('#')[1]; // what the fuck
                channel = channel.Split('>')[0];
                _id = Convert.ToUInt64(channel);
                foreach (SocketGuildChannel channelName in Context.Guild.Channels) // for every channel name in the server's channels
                {
                    if (channelName.Id == _id) // if one is found, continue
                        _continue = true;

                }
                if (!_continue) // if not found, return
                {
                    await ReplyAsync("Channel " + channel + " not found.");
                    return;
                }
                if (_id == 0)
                {
                    await ReplyAsync("Error getting channel id.");
                    return;
                }

                // add to json and whitelist array
                whitelistedChannels.Add(_id);
                string serverName = Context.Guild.Name;
                saveData.Save(serverName, _id);

                await ReplyAsync("Successfully blacklisted " + channel);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }

        [Command("trysplit")]
        public async Task TrySplt(string channelName)
        {
            try
            {
                if (channelName == null) // check if parameter null
                {
                    await ReplyAsync("Channel name cannot be empty.");
                    return;
                }
                channelName = channelName.Split('>')[0];
                channelName = channelName.Split('#')[1];
                ulong _id = Convert.ToUInt64(channelName);
                await ReplyAsync("Channel as string: " + channelName);
                await ReplyAsync("Channel as id: " + _id);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        [Command("tryshit")]
        public async Task TryShit()
        {
            await ReplyAsync("Shut the fuck up " + Context.User + " you are literally not funny.");
        }

        [Command("tryreaddata")]
        public async Task TryReadData()
        {
            string result = saveData.ReadBlacklistServerName();
            await ReplyAsync(result);
        }

        [Command("penis")]
        public async Task Penis()
        {
            await ReplyAsync("cokc and balls");
        }

        [Command("serverstat")]
        public async Task McStat()
        {
            if (Context.Channel.Id != gamingChannelId)
            {
                await ReplyAsync("This channel does not meet the requirements to execute this command.");
                return;
            }
            if (Context.Channel.Id == gamingChannelId)
            {
                if (Process.GetProcessesByName(mcServerExecutable.Split('.')[0]).Length == 0)
                {
                    await ReplyAsync("Server Executable for Thot Obliterators is offline.");
            }
                if (Process.GetProcessesByName(mcServerExecutable.Split('.')[0]).Length > 0)
                {
                    await ReplyAsync("The Thot Obliterators MC Server is currently running!");
                }
            }
        }

        [Command("startserver")]
        public async Task StartServer()
        {
            bool moveAlong = false;
            bool _moveAlong = true;
            if (Context.Channel.Id != gamingChannelId)
            {
                await ReplyAsync("This channel does not meet the requirements to execute this command.");
                return;
            }
            if (Context.Channel.Id == gamingChannelId)
            {
                foreach(SocketRole role in ((SocketGuildUser)Context.Message.Author).Roles)
                {
                    if (role.Id == mcServerGangRoleId)
                    {
                        await ReplyAsync("Starting Server...");
                        Process.Start(serverExecPath);
                        _moveAlong = false;
                    } else if (role.Id != mcServerGangRoleId)
                    {
                        moveAlong = true;
                    }
                }
                if (moveAlong && _moveAlong)
                    await ReplyAsync("You do not meet the requirements to execute this command.");
            }
        }

        [Command("restartserver")]
        public async Task RestartServer()
        {
            bool moveAlong = false;
            bool _moveAlong = true;
            if (Context.Channel.Id != gamingChannelId)
            {
                await ReplyAsync("This channel does not meet the requirements to execute this command.");
                return;
            }
            if (Context.Channel.Id == gamingChannelId)
            {
                foreach (SocketRole role in ((SocketGuildUser)Context.Message.Author).Roles)
                {
                    if (role.Id == mcServerGangRoleId)
                    {
                        await ReplyAsync("Restarting Server...");
                        try
                        {
                            Process serverProcess = Process.GetProcessesByName(mcServerExecutable.Split('.')[0])[0];
                            serverProcess.Kill();
                            await ReplyAsync("Killed server. Starting...");
                            Process.Start(serverExecPath);
                            _moveAlong = false;
                        } catch (Exception ex)
                        {
                            await ReplyAsync(ex.ToString());
                        }
                    }
                    else if (role.Id != mcServerGangRoleId)
                    {
                        moveAlong = true;
                    }
                }
                if (moveAlong && _moveAlong)
                    await ReplyAsync("You do not meet the requirements to execute this command.");
            }
        }

        [Command("stopserver")]
        public async Task StopServer()
        {
            bool moveAlong = false;
            bool _moveAlong = true;
            if (Context.Channel.Id != gamingChannelId)
            {
                await ReplyAsync("This channel does not meet the requirements to execute this command.");
                return;
            }
            if (Context.Channel.Id == gamingChannelId)
            {
                foreach (SocketRole role in ((SocketGuildUser)Context.Message.Author).Roles)
                {
                    if (role.Id == mcServerGangRoleId)
                    {
                        await ReplyAsync("Restarting Server...");
                        try
                        {
                            Process serverProcess = Process.GetProcessesByName(mcServerExecutable)[0];
                            serverProcess.Kill();
                            _moveAlong = false;
                        }
                        catch (Exception ex)
                        {
                            await ReplyAsync(ex.ToString());
                        }
                    }
                    else if (role.Id != mcServerGangRoleId)
                    {
                        moveAlong = true;
                    }
                }
                if (moveAlong && _moveAlong)
                    await ReplyAsync("You do not meet the requirements to execute this command.");
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

        private async Task Troll() { await _context.Channel.SendFileAsync(@"video0.mp4"); }
        private async Task Speak() { await _context.Channel.SendFileAsync(@"speak.mp4"); }

        private async Task GromSpeak()
        {
            await _context.Channel.SendMessageAsync("There I finally stopped posting that video are you happy now grommet?");
            gromSpeakFO = false;
        }

        private async Task RealOrFake()
        {
            Random _random = new Random();
            int randC = _random.Next(0, 100);
            if (randC > 50)
                await _context.Channel.SendMessageAsync("(real)");
            else if (randC <= 50)
                await _context.Channel.SendMessageAsync("(not real)");
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

        public void InitBlacklist()
        {
            // read json
            
        }

        #endregion
       
    }
}