using System.Collections;

namespace GiggityBot.Resources
{
    class WordArrays
    {
        public ArrayList funnyWords = new ArrayList();
        public ArrayList funnyResponses = new ArrayList();
        public ArrayList hotBoob = new ArrayList();
        public ArrayList hotBoobResponse = new ArrayList();
        public void InitArrays()
        {
            InitFunny();
            InitFunnyResponses();
            InitBooba();
            InitBoobaResponse();
        }

        private void InitFunny()
        {
            funnyWords.Add("sex");
            funnyWords.Add("cum");
            funnyWords.Add("pee pee");
            funnyWords.Add("kinky");
            funnyWords.Add("kink");
            funnyWords.Add("porn");
        }
        private void InitFunnyResponses()
        {
            funnyResponses.Add("giggity giggity goo");
            funnyResponses.Add("giggity");
            funnyResponses.Add("Aw man, I was going to tattoo my penis to look like a rocket, but peter already took that idea.");
            funnyResponses.Add("All i want for christmas is a bunch of asian chicks to choke me unconcious.");
            funnyResponses.Add("You aren't the same horse from last night.");
        }
        private void InitBooba() // scan words
        {
            hotBoob.Add("boober");
            hotBoob.Add("honkers");
            hotBoob.Add("booba");
            hotBoob.Add("milkers");
        }
        private void InitBoobaResponse() // responses
        {
            hotBoobResponse.Add("I agree, booba kinda pog doe.");
            hotBoobResponse.Add("God i wish i could hold one.");
            hotBoobResponse.Add("Are you guys talking ab- about... *sweats* b- bb- b- boob??????");
            hotBoobResponse.Add("Awoooooga!");
            hotBoobResponse.Add("*eyes pop out of head*");
        }

    }
}
