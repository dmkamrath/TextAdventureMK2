using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Server
{
    public partial class TAServer
    {
        public void sendStoryMessage(string storyMsg, TAClient target)
        {
            sendMessage(makeStoryString(storyMsg), target);
        }

        public void sendStoryAll(string storyMsg)
        {
            sendAll(makeStoryString(storyMsg));
        }

        public void sendStoryAllBut(string storyMsg, TAClient exclude)
        {
            sendAllBut(makeStoryString(storyMsg), exclude);
        }

        //A special client gets story1, everyone else gets story2
        public void sendSelectiveStory(string story1, string story2, TAClient story1rcv)
        {
            sendStoryAllBut(story2, story1rcv);
            sendStoryMessage(story1, story1rcv);
        }

        public string getMsgCode(MessageType msgType)
        {
            string ret;
            messageTypeDictionary.TryGetValue(msgType, out ret);
            return ret;
        }

        public string makeStoryString(string original)
        {
            return (getMsgCode(MessageType.STORY) + original);
        }

        public void sendProgressBarEntry(string name, int maxValue, params TAClient[] targets)
        {
            string progressString = "g"+ name + "|" + maxValue.ToString();
            sendMessage(progressString, targets);
        }
    }
}
