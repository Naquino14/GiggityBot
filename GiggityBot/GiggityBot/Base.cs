using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using GiggityBot.Modules;
using GiggityBot.Resources;

namespace GiggityBot
{
    class Base
    {
        public static Saver saver;
        public static QuagData data;

        static readonly string dataPath = "main.qdata";

        private static bool checkArgsFO = true;
        static void Main(string[] args)
        {
            if (checkArgsFO)
            {
                if (args[0] == "")
                {
                    Console.WriteLine("Critical error! No launch parameters inputted. The two available are: [-dev] [-normal]");
                    throw new InvalidOperationException();
                }
                else
                    StartupWithArgs(args[0]);
                checkArgsFO = false;
            }
            new Base().RunBotAsync().GetAwaiter().GetResult();
        }

        public static DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public static WordArrays _wordArrays;

        public static bool ErrorStatus = false;

        private static RunMode currentMode;
        private enum RunMode
        {
            dev,
            debug,
            normal,
            anncmnt
        }

        private async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string token = "Nzk0OTc3NjYxNTE1NDY0NzM2.X_CqpA.wDBfdy9sokrVHq7XWDOFVNR9gQk";

            _client.Log += _client_Log;

            await RegisterCommandAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            saver = new Saver(dataPath);

            await Task.Delay(3000);
            await StartBot();

            await Task.Delay(-1); // delay indefinitelly
        }

        private Task _client_Log(LogMessage arg) // da logger :flushed:
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = (SocketUserMessage) arg;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) // if bot sends message, ignore
                return;

            int argPos = 0;
            if (message.HasStringPrefix("q!", ref argPos)) // bot prefix
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                    Console.WriteLine(result.Error);
            }


            await Commands.Scan(message, context);

        }

        private static void StartupWithArgs(string arg)
        {
            if (arg == "-dev")
                currentMode = RunMode.dev;
            else if (arg == "-debug")
                currentMode = RunMode.debug;
            else if (arg == "-normal")
                currentMode = RunMode.normal;
            else if (arg == "-anncmnt")
                currentMode = RunMode.anncmnt;
            else
            {
                Console.WriteLine("Error! Invalid launch parameter(s)");
                throw new InvalidOperationException();
            }

        }

        private async Task StartBot()
        {
            Console.WriteLine("Starting...");
            if (currentMode == RunMode.dev)
            {
                Commands.isDev = true;
                await _client.SetGameAsync("in development", null, ActivityType.Playing);
                await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            }
            if (currentMode == RunMode.debug)
            {
                await _client.SetGameAsync("in debug mode", null, ActivityType.Playing);
                await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            }
            if (currentMode == RunMode.normal)
            {
                await _client.SetGameAsync("Get. Fucking. Real.", null, ActivityType.Playing);
                await _client.SetStatusAsync(UserStatus.Online);
            }
            if (currentMode == RunMode.anncmnt)
                throw new NotImplementedException();

            //var save = saver.Load();
            //if (save.state != Saver.DataState.ok)
            //{
            //    Console.WriteLine("Quag failed to initialize. Shutting down...");
            //    throw new Exception();
            //}
            //data = save.data;

            _wordArrays = new WordArrays();
            _wordArrays.InitArrays();

            //Commands.UpdateTime();
            
            Console.WriteLine("Finished starting up...");
        }

        public static void Exit(int ec = 0)
        { saver.Save(data); Environment.Exit(ec); }
    }
}
