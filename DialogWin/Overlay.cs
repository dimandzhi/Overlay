using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Sony.Vegas;
using System.Windows.Forms;

namespace Overlay
{
    public class EntryPoint
    {
        public void FromVegas(Vegas vegas)
        {
            double frames = 0;
            double time = 0;
            ProcessStartInfo StartInfo = new ProcessStartInfo
            {
                FileName = AppDomain.CurrentDomain.BaseDirectory + @"Script Menu\DialogWin.exe",
                Arguments = vegas.Project.Video.FrameRate.ToString(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process DialgWin = Process.Start(StartInfo);
            int i = 0;
            while (!DialgWin.StandardOutput.EndOfStream)
            {
                switch (i)
                {
                    case 0:
                        frames = Convert.ToDouble(DialgWin.StandardOutput.ReadLine());
                        break;
                    case 1:
                        time = Convert.ToDouble(DialgWin.StandardOutput.ReadLine());
                        break;
                }
                i++;
            }
            DialgWin.WaitForExit();
            i = 0;
            double timecode = 0;
            int ind;
            int prev_ind = -1;
            foreach (Track track in vegas.Project.Tracks)
            {
                List<TrackEvent> Events = new List<TrackEvent>();
                foreach (TrackEvent trev in track.Events) Events.Add(trev);
                Events.Sort(new SortByTime());
                foreach (TrackEvent trev in Events)
                {
                    if (trev.Selected)
                    {
                        ind = Events.FindIndex((TrackEvent tr) => { return tr.Start == trev.Start; });
                        if (ind == prev_ind + 1)
                        {
                            if (i == 0)
                            {
                                timecode = trev.End.ToMilliseconds();
                                i++;
                            }
                            else
                            {
                                trev.Start = new Timecode(timecode - time);
                                timecode = trev.End.ToMilliseconds();
                            }
                        }
                        else i = 0;
                        prev_ind = ind;
                    }
                }
                prev_ind = -1;
                i = 0;
            }
            MessageBox.Show("Success!");
        }

        public class SortByTime : IComparer<TrackEvent>
        {
            public int Compare(TrackEvent a, TrackEvent b)
            {
                if (a.Start > b.Start) return 1;
                else if (a.Start < b.Start) return -1;
                else return 0;
            }
        }
    }
}