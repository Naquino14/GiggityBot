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
        public void Save(string serverName, string channelName)
        {
            blackList = new BlackList()
            {
                ServerName = serverName,
                ChannelName = channelName
            };
            string jsonStringRes = JsonConvert.SerializeObject(blackList);
            File.WriteAllText(@"BlackList.json", jsonStringRes);

        } 
        

    }
}
