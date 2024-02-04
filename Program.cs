using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace affLoader
{
    class Center
    {
        public String abbreviation;
        public String name;

        public Center(String a, String n)
        {
            this.abbreviation = a;
            this.name = n;
        }
    }

    class Program
    {
        static List<Center> centers = new List<Center>();

        static Char[] recordType_001_04 = new Char[04];

        static Char[] facilityId_079_04 = new Char[04];
        static Char[] frequency_044_07 = new Char[07];
        static Char[] usage_005_04 = new Char[04];
        static Char[] siteLocation_009_30 = new Char[30];

        static StreamWriter ofileAFF = new StreamWriter("twrFrequency2.txt");

        static void Main(String[] args)
        {
            centers.Add(new Center("ZAB", "ALBUQUERQUE"));
            centers.Add(new Center("ZAN", "ANCHORAGE"));
            centers.Add(new Center("ZAU", "CHICAGO"));
            centers.Add(new Center("ZBW", "BOSTON"));
            centers.Add(new Center("ZDC", "WASHINGTON DC"));
            centers.Add(new Center("ZDV", "DENVER"));
            centers.Add(new Center("ZFW", "FORT WORTH"));
            centers.Add(new Center("ZHN", "HONOLULU"));
            centers.Add(new Center("ZHU", "HOUSTON"));
            centers.Add(new Center("ZID", "INDIANAPOLIS"));
            centers.Add(new Center("ZJX", "JACKSONVILLE"));
            centers.Add(new Center("ZKC", "KANSAS CITY"));
            centers.Add(new Center("ZLA", "LOS ANGELES"));
            centers.Add(new Center("ZLC", "SALTLAKE CITY"));
            centers.Add(new Center("ZMA", "MIAMI"));
            centers.Add(new Center("ZME", "MEMPHIS"));
            centers.Add(new Center("ZMP", "MINNEAPOLIS"));
            centers.Add(new Center("ZNY", "NEWYORK"));
            centers.Add(new Center("ZOA", "OAKLAND"));
            centers.Add(new Center("ZOB", "CLEVELAND"));
            centers.Add(new Center("ZSE", "SEATTLE"));
            centers.Add(new Center("ZSU", "SAN JUAN"));
            centers.Add(new Center("ZTL", "ATLANTA"));
            centers.Add(new Center("ZUA", "GUAM"));

            String userprofileFolder = Environment.GetEnvironmentVariable("USERPROFILE");
            String[] fileEntries = Directory.GetFiles(userprofileFolder + "\\Downloads\\", "28DaySubscription*.zip");

            ZipArchive archive = ZipFile.OpenRead(fileEntries[0]);
            ZipArchiveEntry entry = archive.GetEntry("AFF.txt");
            entry.ExtractToFile("AFF.txt", true);

            StreamReader file = new StreamReader("AFF.txt");

            String rec = file.ReadLine();

            while (!file.EndOfStream)
            {
                ProcessRecord(rec);
                rec = file.ReadLine();
            }

            ProcessRecord(rec);

            file.Close();

            ofileAFF.Close();

        }

        static void ProcessRecord(String record)
        {
            recordType_001_04 = record.ToCharArray(0, 4);

            String rt = new String(recordType_001_04);

            Int32 r = String.Compare(rt, "AFF3");
            if (r == 0)
            {
                facilityId_079_04 = record.ToCharArray(78, 04);
                String s = new String(facilityId_079_04).Trim();
                if (s != "")
                {
                    ofileAFF.Write(s);
                    ofileAFF.Write('~');

                    frequency_044_07 = record.ToCharArray(43, 07);
                    s = new String(frequency_044_07).Trim().PadRight(7, '0');
                    ofileAFF.Write(s);
                    ofileAFF.Write('~');

                    usage_005_04 = record.ToCharArray(04, 04);
                    s = new String(usage_005_04).Trim();

                    Center center = centers.Find(sr => sr.abbreviation == s);

                    ofileAFF.Write(s);
                    if(center == null)
                    {
                        Console.WriteLine(s);
                    }
                    else
                    {
                        ofileAFF.Write("-" + center.name);
                    }

                    ofileAFF.Write('~');

                    siteLocation_009_30 = record.ToCharArray(08, 30);
                    s = new String(siteLocation_009_30).Trim();
                    ofileAFF.Write(s);

                    ofileAFF.Write(ofileAFF.NewLine);
                }

            }

        }

    }
}
