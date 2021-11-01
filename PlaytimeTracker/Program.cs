using System;
using PlaytimeTrackerLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlaytimeTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Playtime pt = Playtime.FromFile("test.dat");

            if (pt == null)
                pt = new Playtime();

            pt.AddPlaytime(TimeSpan.FromSeconds(19));

            pt.WriteToFile("test.dat");
        }
    }
}
