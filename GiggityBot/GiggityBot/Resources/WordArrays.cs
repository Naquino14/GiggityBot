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

namespace GiggityBot.Resources
{
    class WordArrays
    {
        public ArrayList funnyWords = new ArrayList();
        public async Task InitArrays()
        {
            await InitFunny();
        }

        private async Task InitFunny()
        {
            funnyWords.Add("__test");
            funnyWords.Add("test__");
        }

    }
}
