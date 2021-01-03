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
        public ArrayList hotBoob = new ArrayList();
        public void InitArrays()
        {
            InitFunny();
            InitBooba();
        }

        private void InitFunny()
        {
            funnyWords.Add("sex");
            funnyWords.Add("cum");
            funnyWords.Add("pee pee");
        }
        private void InitBooba()
        {
            hotBoob.Add("boober");
            hotBoob.Add("honkers");
            hotBoob.Add("booba");
            hotBoob.Add("milkers");
        }

    }
}
