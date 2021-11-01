using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlaytimeTrackerLib
{
    public struct PlaytimeEntry
    {
        public TimeSpan TimeSpan;
        public DateTime Date;
    };

    public class Playtime
    {
        public void AddPlaytime(TimeSpan time)
        {
            if (PlaytimeEntries.Count != 0 &&
                PlaytimeEntries.Last().Date == DateTime.Today)
            {
                var lastEntry = PlaytimeEntries.Last();
                lastEntry.TimeSpan += time;
                PlaytimeEntries[PlaytimeEntries.Count - 1] = lastEntry;
                return;
            }
            PlaytimeEntries.Add(new PlaytimeEntry { TimeSpan = time, Date = DateTime.Today });
        }
        public TimeSpan TotalTime()
        {
            TimeSpan totalTime = new TimeSpan();
            foreach (var entry in PlaytimeEntries)
            {
                totalTime += entry.TimeSpan;
            }
            return totalTime;
        }

        public void WriteToFile(string fileName)
        {
            if (!File.Exists(fileName))
                File.Create(fileName).Close();
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Truncate)))
            {
                writer.Write(PlaytimeEntries.Count);
                foreach (var entry in PlaytimeEntries)
                {
                    writer.Write(entry.TimeSpan.Ticks);
                    writer.Write(entry.Date.ToBinary());
                }
            }
        }

        public static Playtime FromFile(string path)
        {
            if (!File.Exists(path))
                return null;

            Playtime playtime = new Playtime();
            using (var BinaryReader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read)))
            {
                var n = BinaryReader.ReadInt32();
                for (var i = 0; i < n; ++i)
                {
                    var time = new TimeSpan(BinaryReader.ReadInt64());
                    var date = DateTime.FromBinary(BinaryReader.ReadInt64());
                    var entry = new PlaytimeEntry { TimeSpan = time, Date = date };
                    playtime.PlaytimeEntries.Add(entry);
                }
            }
            return playtime;
        }

        public PlaytimeEntry[] GetPlaytimeEntries()
        {
            return PlaytimeEntries.ToArray();
        }

        public DateTime[] Dates()
        {
            DateTime[] date = new DateTime[PlaytimeEntries.Count];
            for (int i = 0; i < date.Length; ++i)
            {
                date[i] = PlaytimeEntries[i].Date;
            }
            return date;
        }

        private List<PlaytimeEntry> PlaytimeEntries = new List<PlaytimeEntry>();
    };
}
