using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using GiggityBot.Resources;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace GiggityBot
{
    public class SaveData
    {
        public BlackList blackList;
        public void Save(string serverName, ulong channelID)
        {
            try
            {
                Console.WriteLine("Attempting to save data...");
                blackList = new BlackList()
                {
                    ServerName = serverName,
                    ChannelID = channelID
                };
                string jsonStringRes = JsonConvert.SerializeObject(blackList);
                File.WriteAllText(@"BlackList.json", jsonStringRes);
                Console.WriteLine("Success!");
            } catch (Exception ex)
            {
                Console.WriteLine("Save Data failed...");
                Console.WriteLine(ex);
            }

        }

        public string ReadBlacklistServerName()
        {
            string jsonFromFile;
            using (var reader = new StreamReader(@"Blacklist.json"))
            {
                jsonFromFile = reader.ReadToEnd();
            }
            var serverNameJSONResult = JsonConvert.DeserializeObject<BlackList>(jsonFromFile);
            string result = serverNameJSONResult.ServerName;
            return result;
        }
        
        public uint[] ReadBlacklistChannelId()
        {
            return null;
        }

    }
}
