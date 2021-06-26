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
        public ArrayList funnyFamilyGuyClips = new ArrayList();
        
        public void InitArrays()
        {
            InitBlacklist();
            InitFunny();
            InitFunnyResponses();
            InitBooba();
            InitBoobaResponse();
            InitFamilyMan();
            InitFamilyManResponse();
            InitFamilyGuyClips();
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
        private void InitFamilyGuyClips()
        {
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/811773568005439508/811776546184101918/Death_2.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/811773568005439508/811777820791537704/Quag_Died.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/811773568005439508/811775379622199326/Death.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/769802428966240256/811692956523167744/Peter.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/801462827017437224/813443484933423164/b09f198122f5a3c23ddf41a2773d1fa6ed36aea4cac93ccbe69f03024eaf60b2_1.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/758894046298374165/817599175377682452/wait_here.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/736545350952681545/819760478245879818/video0-3-3.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/714406876519071804/819936051425443921/amogus1.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/811773568005439508/819437950268997642/get_down_here.MP4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/759825412343201792/789505356325716008/y2mate.com_-_Shitpost_status_480p_2.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/811773568005439508/820793733456330802/056f596b611b8dce7ae93ec1dd132ae049fee66e70cc13c65d8cb9adb1ee4314_1.MP4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/472407264700006420/826221277835427890/really.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/505818199749558290/826587084030345226/video0-17.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/770094079102681198/828754152002945044/x0W3fvGCTRjACcmV.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/811773568005439508/829090900180336770/chris_where_have_you_been.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/811773568005439508/830185960578744360/video0.mov");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/811773568005439508/830262354138759208/video0.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/336402396026765313/830570495375835136/video0-16.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/745069366868967434/854675252315553802/video0.mov");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/811773568005439508/852304047783411732/video0.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/229925297506615297/846231739336097792/toilets.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/745069366868967434/840984438312665088/Family_Guy_S8E3___Have_sex_with_children_and_kill_them_.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/557831750659866626/840233848985747496/5f22f4f2914c208f3190f6db9b0f2e626c60e8e53ac7244db6827f8086c59ff6_1.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/617120220661678080/835283091059769344/video0-3-4.mp4");
            funnyFamilyGuyClips.Add("https://cdn.discordapp.com/attachments/811773568005439508/833699481027477535/video0.mp4");
        }

    }
}
