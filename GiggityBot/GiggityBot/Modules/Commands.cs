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
using System.Runtime.InteropServices;
using System.Linq;
using System.IO;
using WindowsInput;

namespace GiggityBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        #pragma warning disable CS0168

        #region required variables
        private EmbedBuilder embedBuilder = new EmbedBuilder();
        private SocketUserMessage _message;
        private SocketCommandContext _context;
        private WordArrays wordArrays;
        public static Commands commands; // ex use only
        private Random random;
        public SaveData saveData = new SaveData();
        Saver saver;
        private InputSimulator inputSimulator = new InputSimulator();
        #endregion

        #region dll import

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        #region global variables

        private int _char;
        private DateTime timeNow = new DateTime();
        private DateTime lastTime = new DateTime();
        private TimeSpan elapsedTime = new TimeSpan();
        bool updateTimeFireOnce = true;
        public ArrayList whitelistedChannels = new ArrayList();

        public static bool isDev;

        private string serverWithBlacklist;
        private uint channelBlacklist;

        private const string mcServerExecutable = "java.exe";
        private const string mcServerExecutableWindowName = @"t - """ + mod16serverPath + @""" ";

        private const string kspServerExecutable = "luna.exe"; // MIGHT BE WRONG!!!!!!!!!!!!!!
        private const string kspServerExecutableWindowName = @"t - """ + ksp11serverPath + @""" ";

        private const ulong gamingChannelId = 615369865305260047;
        private const ulong mcServerGangRoleId = 736043415875223574;
        private const string van12serverPath = @"C:\Users\QuagmireServer\Desktop\mc server\TOMCServer\Minecraft server survuival 1.12.2\start.bat";
        private const string mod12serverPath = @"C:\Users\QuagmireServer\Desktop\mc server\TOMCServer\Minecraft 1.12.2 Modded\start.bat";
        private const string mod16serverPath = @"C:\Users\QuagmireServer\Desktop\mc server\TOMCServer\Minecraft 1.16.5 Modded\start.bat";
        private const string ksp11serverPath = "LMPServer\\Server.exe";
        private Process _serverProcess;
        enum ServerType
        {
            mod16,
            mod12,
            van12,
            ksp11
        }
        private static ServerType currentServerType;

        const ulong vinettaId = 388440073219211265;
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


            if ((message.Content.Contains("quag") || message.Content.Contains("quagmire")) && (message.Content.Contains("real") || message.Content.Contains("true")))
                await _commands.RealOrFake();

            if (context.IsPrivate)
                await _commands.AnswerDm();

            foreach (string word in _commands.wordArrays.funnyWords) // giggity response triggers
                if (message.Content.Contains(word))
                {
                    await _commands.Giggity();
                    break;
                }

            foreach (string word in _commands.wordArrays.hotBoob)
                if (message.Content.Contains(word))
                {
                    await _commands.Booba();
                    break;
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

            if ((message.Content.Contains("quag kill yourself") || message.Content.Contains("quag kys")))
                await _commands.Suicide();
            if (message.Content.Contains("quag restart") || message.Content.Contains("quag restart host"))
                await _commands.RestartHost();
            if (message.Content.Contains("quag cancel") || message.Content.Contains("actually cancel") || message.Content.Contains("nvm"))
                await _commands.CancelHostRestart();

        }

        #region prefix commands

        [Command("ping")]
        public async Task Ping() => await ReplyAsync("pong, Latency: **" + Base._client.Latency + "ms**");

        [Command("help")]
        public async Task Help(string parameter = null)
        {
            if (parameter == "server" && Context.Channel.Id == gamingChannelId)
            {
                embedBuilder.WithTitle("MC Server Commands");
                embedBuilder.WithImageUrl("https://cdn.iconscout.com/icon/free/png-512/minecraft-15-282774.png");
                embedBuilder.AddField("q!serverstatus", "Returns wether or not the server executable is running on the host.", true);
                embedBuilder.AddField("q!startserver [server type]", "Tells me to run the minecraft server.", true);
                embedBuilder.AddField("q!stopserver", "Tells me to stop the minecraft server.", true);
                embedBuilder.AddField("q!restartserver", "Tells me to restart the minecraft server.", true);
                embedBuilder.AddField("q!saveserver", "Issues a command to the server to save it.", true);
                embedBuilder.AddField("q!servercommand [command]", "Issues [command] to the server.", true);
                embedBuilder.AddField("Server Start parameter types:", "`mod16` - modded 1.16.5 | `mod12` - modded 1.12.2 | `van12` - vanilla 1.12.2");
                embedBuilder.WithColor(Discord.Color.Green);
                await ReplyAsync("", false, embedBuilder.Build());
                return;
            } else if (parameter == "ksp" && Context.Channel.Id == gamingChannelId)
            {
                embedBuilder.WithTitle("Ksp Server Commands");
                embedBuilder.WithImageUrl("https://external-preview.redd.it/CJvMs4PptcN1uypfZslT1wT5HA46a8xX5THWZr9AIoQ.jpg?auto=webp&s=abf2bb515a55a5be6c1a609f38d0ed1c36a72d95");
                embedBuilder.AddField("q!kspstart", "Start the ksp server. (moded 1.11.1)", true);
                embedBuilder.AddField("q!kspstop", "Stop the ksp server.", true);
                embedBuilder.AddField("q!kspstat", "Returns wether or not the server executable is running on the host.", true);
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
            embedBuilder.AddField("q!familyguy", "sends a random family guy clip. More will be added as time goes by.", false);
            embedBuilder.AddField("q!compare", "expirimental ai shit, dont use if you dont know what its for", false);
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
                    if (channelName.Id == _id) // if one is found, continue
                        _continue = true;
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
        public async Task TryShit() => await ReplyAsync("Shut the fuck up " + Context.User + " you are literally not funny.");

        [Command("tryreaddata")]
        public async Task TryReadData()
        {
            string result = saveData.ReadBlacklistServerName();
            await ReplyAsync(result);
        }

        [Command("penis")]
        public async Task Penis() => await ReplyAsync("cokc and balls");

        [Command("serverstatus")]
        public async Task McStat()
        {
            if (Context.Channel.Id != gamingChannelId)
            {
                await ReplyAsync("This channel does not meet the requirements to execute this command.");
                return;
            }
            if (isDev)
            {
                await ReplyAsync("Unable to comply. I am currently in Dev mode so I may or may not be running on the host.");
                return;
            }
            if (Context.Channel.Id == gamingChannelId)
            {
                if (Process.GetProcessesByName(mcServerExecutable.Split('.')[0]).Length == 0)
                    await ReplyAsync("Server Executable for Thot Obliterators is offline.");
                if (Process.GetProcessesByName(mcServerExecutable.Split('.')[0]).Length > 0)
                    await ReplyAsync("The Thot Obliterators MC Server is currently running `" + currentServerType.ToString() + "`");
            }
        }

        [Command("startserver")]
        public async Task StartServer(string serverType = null)
        {
            if (Context.Channel.Id != gamingChannelId)
            {
                await ReplyAsync("This channel does not meet the requirements to execute this command.");
                return;
            }
            if (isDev)
            {
                await ReplyAsync("Unable to comply. I am currently in Dev mode so I may or may not be running on the host.");
                return;
            }
            if (serverType == null)
            {
                await ReplyAsync("Bro which one? type q!help server for a list.");
                return;
            }
            if (serverType != ServerType.mod16.ToString() ^ serverType != ServerType.mod12.ToString() ^ serverType != ServerType.van12.ToString())
            {
                await ReplyAsync("Dood thats not a valid server type, see q!help server for a list.");
                return;
            }

            bool moveAlong = false;
            bool _moveAlong = true;
            try
            {
                _serverProcess = Process.GetProcessesByName(mcServerExecutable.Split('.')[0])[0];
            } catch (Exception unused)
            {
                // unused, but important, dont delete
            }
            if (_serverProcess != null)
            {
                await ReplyAsync("Server is already running...");
                return;
            }
            if (Context.Channel.Id == gamingChannelId)
            {
                foreach(SocketRole role in ((SocketGuildUser)Context.Message.Author).Roles)
                {
                    if (role.Id == mcServerGangRoleId)
                    {
                        if (serverType == ServerType.mod16.ToString())
                        {
                            currentServerType = ServerType.mod16;
                            await ReplyAsync("Starting Server `" + currentServerType.ToString() + "`...");
                            Process.Start(mod16serverPath);
                        }
                        if (serverType == ServerType.mod12.ToString())
                        {
                            currentServerType = ServerType.mod12;
                            await ReplyAsync("Starting Server `" + currentServerType.ToString() + "`...");
                            Process.Start(mod12serverPath);
                        }
                        if (serverType == ServerType.van12.ToString())
                        {
                            currentServerType = ServerType.van12;
                            await ReplyAsync("Starting Server `" + currentServerType.ToString() + "`...");
                            Process.Start(van12serverPath);
                        }
                        _moveAlong = false;
                    } else if (role.Id != mcServerGangRoleId)
                        moveAlong = true;
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
            if (isDev)
            {
                await ReplyAsync("Unable to comply. I am currently in Dev mode so I may or may not be running on the host.");
                return;
            }
            if (Context.Channel.Id == gamingChannelId)
            {
                foreach (SocketRole role in ((SocketGuildUser)Context.Message.Author).Roles)
                {
                    if (role.Id == mcServerGangRoleId)
                    {
                        Process serverProcess;
                        try
                        {
                            if (currentServerType == ServerType.ksp11)
                                serverProcess = Process.GetProcessesByName(kspServerExecutable.Split('.')[0])[0];
                            else
                                serverProcess = Process.GetProcessesByName(mcServerExecutable.Split('.')[0])[0];
                        }
                        catch (Exception unused)
                        {
                            await ReplyAsync("Server is not running, start it with q!startserver.");
                            return;
                        }
                        if (serverProcess != null)
                        {
                            await ReplyAsync("Restarting Server...");
                            try
                            {
                                IntPtr _zero = IntPtr.Zero;
                                for (int i = 0; (i < 60) && (_zero == IntPtr.Zero); i++ /* 60 window max scan */)
                                {
                                    await Task.Delay(20); // delay to not murder the laptop
                                    if (currentServerType == ServerType.ksp11)
                                        _zero = FindWindow(null, kspServerExecutableWindowName);
                                    else
                                        _zero = FindWindow(null, mcServerExecutableWindowName);
                                }
                                if (_zero != null) // keypress issued here
                                {
                                    if (currentServerType != ServerType.ksp11)
                                    {
                                        SetForegroundWindow(_zero);
                                        //SendKeys.SendWait("save-all");
                                        //SendKeys.SendWait("{ENTER}");
                                        //SendKeys.Flush();
                                        inputSimulator.Keyboard.TextEntry("save-all");
                                        inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                                        await Task.Delay(3000);
                                        serverProcess.Kill();
                                        _serverProcess = null;
                                    }
                                    else
                                    {
                                        await Task.Delay(100);
                                        serverProcess.Close();
                                        _serverProcess = null;
                                    }
                                }
                                await ReplyAsync("Saved and killed server. Starting...");
                                if (currentServerType == ServerType.mod16)
                                    _serverProcess = Process.Start(mod16serverPath);
                                if (currentServerType == ServerType.mod12)
                                    _serverProcess = Process.Start(mod12serverPath);
                                if (currentServerType == ServerType.van12)
                                    _serverProcess = Process.Start(van12serverPath);
                                if (currentServerType == ServerType.ksp11)
                                    _serverProcess = Process.Start(ksp11serverPath);
                                _moveAlong = false;
                            }
                            catch (Exception ex)
                            {
                                await ReplyAsync(ex.ToString());
                            }
                        }
                    }
                    else if (role.Id != mcServerGangRoleId)
                        moveAlong = true;
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
            if (isDev)
            {
                await ReplyAsync("Unable to comply. I am currently in Dev mode so I may or may not be running on the host.");
                return;
            }
            if (Context.Channel.Id == gamingChannelId)
            {
                foreach (SocketRole role in ((SocketGuildUser)Context.Message.Author).Roles)
                {
                    Process serverProcess;
                    try
                    {
                        if (currentServerType == ServerType.ksp11)
                            serverProcess = Process.GetProcessesByName(kspServerExecutable.Split('.')[0])[0];
                        else
                            serverProcess = Process.GetProcessesByName(mcServerExecutable.Split('.')[0])[0];
                    }
                    catch (Exception unused)
                    {
                        await ReplyAsync("Server cannot be stopped because it is not running.");
                        return;
                    }
                    if (serverProcess != null && role.Id == mcServerGangRoleId)
                    {
                        await ReplyAsync("Stopping Server...");
                        try
                        {
                            IntPtr _zero = IntPtr.Zero;
                            for (int i = 0; (i < 60) && (_zero == IntPtr.Zero); i++ /* 60 window max scan */)
                            {
                                await Task.Delay(20); // delay to not murder the laptop
                                _zero = FindWindow(null, mcServerExecutableWindowName);
                            }
                            if (_zero != null) // keypress issued here
                            {
                                SetForegroundWindow(_zero);
                                //SendKeys.SendWait("save-all");
                                //SendKeys.SendWait("{ENTER}");
                                //SendKeys.Flush();
                                inputSimulator.Keyboard.TextEntry("save-all");
                                inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                                await Task.Delay(3000);
                                serverProcess.Kill();
                                _serverProcess = null;
                            }
                            else
                            {
                                await Task.Delay(100);
                                serverProcess.Close();
                                _serverProcess = null;

                            }
                            _moveAlong = false;
                        }
                        catch (Exception ex)
                        {
                            await ReplyAsync(ex.ToString());
                        }
                        await ReplyAsync("Successfully saved and stopped the server");
                    }
                    else if (role.Id != mcServerGangRoleId)
                        moveAlong = true;
                }
                if (moveAlong && _moveAlong)
                    await ReplyAsync("You do not meet the requirements to execute this command.");
            }
        }


        [Command("saveserver")]
        public async Task SaveServer()
        {
            if (Context.Channel.Id != gamingChannelId)
            {
                await ReplyAsync("This channel does not meet the requirements to execute this command.");
                return;
            }
            if (isDev)
            {
                await ReplyAsync("Unable to comply. I am currently in Dev mode so I may or may not be running on the host.");
                return;
            }
            if (currentServerType == ServerType.ksp11)
            {
                await ReplyAsync("KSP server saves automatically, use q!stopserver instead.");
                return;
            }
            Process serverProcess;
            try
            {
                serverProcess = Process.GetProcessesByName(mcServerExecutable.Split('.')[0])[0];
            }
            catch (Exception unused)
            {
                await ReplyAsync("Could not issue command because server is not running.");
                return;
            }
            if (Context.Channel.Id == gamingChannelId)
            {
                foreach (SocketRole role in ((SocketGuildUser)Context.Message.Author).Roles)
                {
                    if (role.Id == mcServerGangRoleId)
                    {
                        await ReplyAsync("Attempting to save server...");
                        try
                        {
                            IntPtr _zero = IntPtr.Zero;
                            for (int i = 0; (i < 60) && (_zero == IntPtr.Zero); i++ /* 60 window max scan */)
                            {
                                await Task.Delay(20); // delay to not murder the laptop
                                _zero = FindWindow(null, mcServerExecutableWindowName);
                            }
                            if (_zero != null) // keypress issued here
                            {
                                SetForegroundWindow(_zero);
                                //SendKeys.SendWait("save-all");
                                //SendKeys.SendWait("{ENTER}");
                                //SendKeys.Flush();
                                inputSimulator.Keyboard.TextEntry("save-all");
                                inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                                await Task.Delay(3000);
                            }
                            await ReplyAsync("Successfully sent save command.");
                        }
                        catch (Exception ex)
                        {
                            await ReplyAsync("Unable to save server.");
                            await ReplyAsync(ex.ToString());
                        }
                    }
                }
            }
        }

        [Command("servercommand")]
        public async Task ServerCommand(string args)
        {
            if (Context.Channel.Id != gamingChannelId)
            {
                await ReplyAsync("This channel does not meet the requirements to execute this command.");
                return;
            }
            if (isDev)
            {
                await ReplyAsync("Unable to comply. I am currently in Dev mode so I may or may not be running on the host.");
                return;
            }
            if (args == null)
            {
                await ReplyAsync("Arguments are null. Please enter a command.");
                return;
            }

            Process serverProcess;
            try
            {
                if (currentServerType == ServerType.ksp11)
                    serverProcess = Process.GetProcessesByName(kspServerExecutable.Split('.')[0])[0];
                else
                    serverProcess = Process.GetProcessesByName(mcServerExecutable.Split('.')[0])[0];
            }
            catch (Exception unused)
            {
                await ReplyAsync("Could not issue command because server is not running.");
                return;
            }
            try
            {
                IntPtr _zero = IntPtr.Zero;
                for (int i = 0; (i < 60) && (_zero == IntPtr.Zero); i++ /* 60 window max scan */)
                {
                    await Task.Delay(20); // delay to not murder the laptop
                    if (currentServerType == ServerType.ksp11)
                        _zero = FindWindow(null, kspServerExecutableWindowName);
                    else
                        _zero = FindWindow(null, mcServerExecutableWindowName);
                }
                if (_zero != null) // keypress issued here
                {
                    await ReplyAsync("Issuing command `" + args + "`.");
                    SetForegroundWindow(_zero);
                    //SendKeys.SendWait(args);
                    //SendKeys.SendWait("{ENTER}");
                    //SendKeys.Flush();
                    inputSimulator.Keyboard.TextEntry(args);
                    inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.ToString());
            }
        }


        [Command("familyguy")]
        public async Task FamilyGuy()
        {
            Random random = new Random();
            wordArrays = new WordArrays();
            wordArrays.InitArrays();
            int randC = random.Next(0, wordArrays.funnyFamilyGuyClips.Count);
            string randS = (string)wordArrays.funnyFamilyGuyClips[randC];
            await ReplyAsync(randS);
        }

        [Command("spitdebug")]
        public async Task SpitDebug(string w, string w2)
        {
            await ReplyAsync(w);
            await ReplyAsync(w2);
        }

        [Command("debug")]
        public async Task Debug(string args, string parameter1)
        {
            if (args == "curl") {
                try
                {
                    await ReplyAsync(Curler.Curl(parameter1, Context).ToString());
                    await Task.Delay(2000);
                    await Context.Channel.SendFileAsync(HandleAI.pathToComparison);
                } catch (Exception ex)
                {
                    await ReplyAsync(ex.ToString());
                }
            }
            if (args == "trycompare")
            {
                try
                {
                    await HandleAI.Start(parameter1, Context);
                } catch (Exception ex)
                {
                    await ReplyAsync(ex.ToString());
                }
            }
        }

        [Command("compare")]
        public async Task Compare(string url)
        {
            try
            {
                throw new NotImplementedException();
            } catch (Exception ex)
            {
                await ReplyAsync(ex.ToString());
            }
        }

        [Command("hostcommand")]
        public async Task HostCommand(string args, string _override = "nul")
        {
            try
            {
                bool __override = false;
                if (_override == "override" || _override == "o")
                    __override = true;
                if (Context.Message.Author.Id != vinettaId)
                {
                    await ReplyAsync("Bro, only Vinetta can run that command...");
                    return;
                }
                if (Context.Message.Author.Id == vinettaId)
                {
                    if (isDev && !__override)
                    {
                        await ReplyAsync("Cannot run command, I am not on the host.");
                        return;
                    }
                    await ReplyAsync("Running " + args);
                    Process.Start("cmd.exe", args);
                }
            } catch (Exception ex)
            {
                await ReplyAsync(ex.ToString());
            }
        }

        [Command("kspstart")]
        public async Task StartKsp()
        {
            await ReplyAsync("not implimented");
        }

        [Command("stopksp")]
        public async Task StopKsp()
        {
            await ReplyAsync("not implimented");

        }

        [Command("kspstat")]
        public async Task KspStat()
        {
            await ReplyAsync("not implimented");

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

        private async Task CharacterResponse(int _char) // simplify somehow later
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
                randS = "havent seen them in ages...";
            await _context.Channel.SendMessageAsync(randS);

        }

        private async Task Troll() { await _context.Channel.SendFileAsync(@"video0.mp4"); }
        private async Task Speak() { await _context.Channel.SendFileAsync(@"speak.mp4"); }


        private async Task RealOrFake()
        {
            Random _random = new Random();
            int randC = _random.Next(0, 100);
            if (randC > 50)
                await _context.Channel.SendMessageAsync("(real)");
            else if (randC <= 50)
                await _context.Channel.SendMessageAsync("(not real)");
        }

        private async Task Suicide()
        {
            if (_context.Message.Author.Id == vinettaId)
            {
                await _context.Channel.SendMessageAsync("ight, imma head out");
                await _context.Channel.SendMessageAsync("https://cdn.discordapp.com/emojis/741089413626331176.gif?v=1");

                Base.Exit(0);
            } else
            {
                await _context.Channel.SendMessageAsync("Why dont you kill yourself?");
                await _context.Channel.SendMessageAsync("Loser");
                await _context.Channel.SendMessageAsync("Get rekt lolollolololol");
            }
        }

        private async Task RestartHost()
        {
            if (_context.Message.Author.Id == vinettaId)
            {
                if (isDev)
                {
                    await _context.Channel.SendMessageAsync("Unable to restart, I am in Dev mode so I may or may not be on the host.");
                    return;
                } else
                {
                    await _context.Channel.SendMessageAsync("Aight, restarting host... (this may take awhile)");
                    Process.Start("cmd.exe", "/c shutdown /r");
                }
            } else
            {
                await _context.Channel.SendMessageAsync("YOU NOT MY DAD!");
                await _context.Channel.SendMessageAsync("YOU ALWAYS WANA HEAR SOEMTHING!");
                await _context.Channel.SendMessageAsync("ugly ass fuckin' noodlehead.");
            }
        }

        private async Task CancelHostRestart()
        {
            if (_context.Message.Author.Id == vinettaId)
            {
                if (isDev)
                {
                    await _context.Channel.SendMessageAsync("Unable to cancel restart, I am in Dev mode so I may or may not be on the host.");
                    return;
                } else
                {
                    await _context.Channel.SendMessageAsync("Cancelling host restart.");
                    Process.Start("cmd.exe", "/c shutdown /a");
                }
            } else
            {
                await _context.Channel.SendMessageAsync("Have you not learned,");
                await Task.Delay(500);
                await _context.Channel.SendMessageAsync("that only vin can run these commands...");
                await _context.Channel.SendMessageAsync("smh, idiots nowadays.");
            }
        }

        private async Task AnswerDm()
        {
            await _context.Channel.SendMessageAsync("Dude, Nate literally explained that the bot doesnt asnwer dms, or if you did this accidentally or out of curiosity, what makes you think");
            await _context.Channel.SendMessageAsync("that a discord bot will be useful answering dms like, you are just wasting my server's resources bro. Kinda cringe...");
        }

        #endregion

        #region other functions

        // just a heads up, all of these dont work
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

        public async Task ExternalReport(string message) => await _context.Channel.SendMessageAsync(message);

        #endregion

    }
} // ayo 1k lines again!