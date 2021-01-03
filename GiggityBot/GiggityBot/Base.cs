using System;
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
        static void Main(string[] args) => new Base().RunBotAsync().GetAwaiter().GetResult();

        public static DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public static WordArrays _wordArrays;

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
                if (!result.IsSuccess) // error debugging
                    Console.WriteLine(result.Error);
            }

            await Commands.Scan(message, context);

        }

        private async Task StartBot()
        {
            Console.WriteLine("Starting...");
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            await _client.SetGameAsync("with your mom", null, ActivityType.Playing);

            _wordArrays = new WordArrays();
            _wordArrays.InitArrays();
            
            Console.WriteLine("Finished starting up...");
        }
    }
}
