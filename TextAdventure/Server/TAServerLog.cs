using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TextAdventure.Server
{
    enum LogType
    {
        MISC,
        SERVER_ACTION,
        SERVER_INPUT,
        CLIENT_MESSAGE_RX,
        CLIENT_MESSAGE_TX,
        PLAYER_KICKED,
        ERROR
    };

    struct LogEntry
    {
        public string logData;
        public LogType type;
        public DateTime timeStamp;
        public LogEntry(string logData, LogType type)
        {
            this.logData = logData;
            this.type = type;
            timeStamp = DateTime.Now;
        }
    }

    class TAServerLog
    {
        public static List<LogEntry> logHistory = new List<LogEntry>();
        public static Mutex logLock = new Mutex();
        public static void log(string msg, LogType type)
        {
            logLock.WaitOne();
            msg += "\n";
            Console.WriteLine(msg);
            logHistory.Add(new LogEntry(msg,type));
            dumpLog();
            logLock.ReleaseMutex();
        }

        public static void dumpLog(string saveName = "LogDump")
        {
            var streamWriter = new System.IO.StreamWriter(saveName + ".txt");
            foreach (LogEntry entry in logHistory)
            {
                streamWriter.WriteLine(entry.timeStamp + " " + entry.type.ToString() + ": " + entry.logData + "\n\n");
            }
            streamWriter.Dispose();
        }
    }

    
}
