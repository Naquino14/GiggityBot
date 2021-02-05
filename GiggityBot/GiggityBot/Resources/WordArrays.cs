using System.Collections;
using GiggityBot.Modules;

namespace GiggityBot.Resources
{
    class WordArrays
    {
        #region important arrays

        public ArrayList DMIds = new ArrayList();

        #endregion
        public ArrayList funnyWords = new ArrayList();
        public ArrayList funnyResponses = new ArrayList();
        public ArrayList hotBoob = new ArrayList();
        public ArrayList hotBoobResponse = new ArrayList();
        public ArrayList familyManCharacters = new ArrayList();
        public ArrayList familyManResponses = new ArrayList();
        
        public void InitArrays()
        {
            InitBlacklist();

            InitFunny();
            InitFunnyResponses();
            InitBooba();
            InitBoobaResponse();
            InitFamilyMan();
            InitFamilyManResponse();
        }
        private void InitBlacklist()
        {
            Commands commands = new Commands();
            commands.InitBlacklist();
        }

        private void InitFunny() // scan words
        {
            funnyWords.Add("sex");
            funnyWords.Add("cum");
            funnyWords.Add("pee pee");
            funnyWords.Add("kinky");
            funnyWords.Add("kink");
            funnyWords.Add("porn");
        }
        private void InitFunnyResponses() // responses
        {
            funnyResponses.Add("giggity giggity goo");
            funnyResponses.Add("giggity");
            funnyResponses.Add("Aw man, I was going to tattoo my penis to look like a rocket, but peter already took that idea.");
            funnyResponses.Add("All i want for christmas is a bunch of asian chicks to choke me unconcious.");
            funnyResponses.Add("You aren't the same horse from last night.");
            funnyResponses.Add("Hot shit bro.");
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
        private void InitFamilyMan()
        {
            familyManCharacters.Add("peter griffin"); // 0 peter
            familyManCharacters.Add("dog brian"); // 1 brian
            familyManCharacters.Add("brian griffin"); // 2 brian
            familyManCharacters.Add("lois"); // 3 lois
            familyManCharacters.Add("lois griffin"); // 4 lois
            familyManCharacters.Add("quagmire"); // 5 quagmire
            familyManCharacters.Add("stweie"); // 6 stewie
            familyManCharacters.Add("chris griffin"); // 7 chris
            familyManCharacters.Add("joe swanson"); // 8 joe
            familyManCharacters.Add("swanson"); // 9 joe
        }
        private void InitFamilyManResponse()
        {
            familyManResponses.Add("Remember that time peter got surgery and became a chad?"); // 0 peter griffin
            familyManResponses.Add("Im still pissed at peter for killing my poor James."); // 1 peter griffin
            familyManResponses.Add("I heard he tried to breastfeed stewie."); // 3 peter
            familyManResponses.Add("I heard that dog actually tried to run off with someone's wife."); // 4 brian
            familyManResponses.Add("Its sad that he abandoned his son."); // 5 brian
            familyManResponses.Add("I was angry when that woman was using our tax money."); // 6 lois
            familyManResponses.Add("Theres this one time I saw lois getting out of a car with Bill Clinton."); // 7 lois
            familyManResponses.Add("Giggity, thats me!"); // 8 quagmire
            familyManResponses.Add("What did you say about me."); // 9 quagmire
            familyManResponses.Add("Dewin your mom."); // 10 quagmire
            familyManResponses.Add("That child isnt mine officer."); // 11 quagmire
            familyManResponses.Add("Remember when he cloned himself?"); // 12 stewie
            familyManResponses.Add("Stewie is a cold blooded murderer."); // 13 stewie
            familyManResponses.Add("They took pity on the poor guy once and made him homecoming king."); // 14 chris
            familyManResponses.Add("Imagine beating up your dog because you are desperate to talk to girls."); // 15 chris
            familyManResponses.Add("Poor loser got robbed by his goldfish once."); // 16 chris
            familyManResponses.Add("I dont think Joe is actually crippled."); // 17 joe
            familyManResponses.Add("He once said sex was overrated. He doesnt get enough of that giggity to understand."); // 18 joe
        }

    }
}
