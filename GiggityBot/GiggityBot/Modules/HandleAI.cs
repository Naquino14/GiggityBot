using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GianDetectorML.ConsoleApp;

namespace GiggityBot.Modules
{
    public class HandleAI
    {
        public const string pathToComparison = @"nate pls add path here k thx";

        private const int predictedDownloadTime = 10; // just as a starter

        public static async void Start(string url, SocketCommandContext _context)
        {
            bool curlSuccess = (Curler.Curl(url, _context) == Curler.status.success);
            if (!curlSuccess)
            {
                await _context.Channel.SendMessageAsync("Failed to get image.");
                return;
            }
            if (curlSuccess)
            {
                await _context.Channel.SendMessageAsync("Successfully found image. Downloading...");
                await Task.Delay(predictedDownloadTime);
            }

            await _context.Channel.SendMessageAsync("Successfully downloaded. Comparing...");

            // AI stuff here...
            string result = WhoIsThis.CompareImage(pathToComparison);
            await _context.Channel.SendMessageAsync(result);
            // then eventually...


            Process.Start("cmd.exe", "/c del /f " + pathToComparison); // delete reference image to make space for a new one
        }
    }
}
