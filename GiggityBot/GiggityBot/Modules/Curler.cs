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
    public class Curler
    {
        public enum status
        {
            nul,
            success,
            fail
        }
        public status CurlStatus;
        public static string CurlerException;

        public static status Curl(string url, SocketCommandContext _context)
        {
            try
            {
                Process.Start("cmd.exe", "/c curl -o " + HandleAI.pathToComparison);
                return status.success;
            } catch (Exception ex)
            {
                CurlerException = ex.ToString();
                return status.fail;
            }

            //return status.nul;
        }
    }
}
