using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace GiggityBot.Modules
{
    public class HandleAI
    {
        public const string pathToComparison = @"D:\Users\naqui\Desktop\curll\lol.png"; // temp path

        private const int predictedDownloadTime = 3000; // just as a starter
        private const int predictedComparisonTime = 10000; // just as a starter

        public static async Task Start(string url, SocketCommandContext _context)
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
            }
            await Task.Delay(predictedDownloadTime);
            await _context.Channel.SendMessageAsync("Successfully downloaded. Comparing...");

            //AI stuff here...
            //string result = WhoIsThis.CompareImage(pathToComparison);
            string result = "This Function is broken, please do not use it. Failed to compare image. This operation is not implemented @HandleAI.cs L35";
            await Task.Delay(predictedComparisonTime);
            if (result == null)
                result = "failed to compare image.";
            await _context.Channel.SendMessageAsync(result);
            // then eventually...


            Process.Start("cmd.exe", "/c del /f " + pathToComparison); // delete reference image to make space for a new one
        }
    }
}
