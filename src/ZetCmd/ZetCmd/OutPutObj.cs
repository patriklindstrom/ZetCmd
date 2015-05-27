using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZetCmd
{
    #region Output to file or whateever Abstraktion

    public interface IOutputObj
    {
        string Name { get; set; }
        Dictionary<string, string> Dict { get; set; }
        Options Opt { get; set; }
        void Output();
    }

    public class OutputObj : IOutputObj
    {
        public string Name { get; set; }
        public Dictionary<string, string> Dict { get; set; }
        public Options Opt { get; set; }

        public OutputObj(string name, Dictionary<string, string> dict, Options opt)
        {
            Name = name;
            Dict = dict;
            Opt = opt;
        }

        public void Output()
        {
            string dir = Path.GetDirectoryName(Opt.FileB);
            string ext = Path.GetExtension(Opt.FileB);
            if (!String.IsNullOrEmpty(dir))
            {
                var fileWriteStopWatch = Stopwatch.StartNew();
                Directory.Exists(dir);
                string fileName = Path.Combine(dir, Name + "_" + DateTime.Now.ToString("yyyMMddTHHmmss") + ext);
                Dict.SaveValuesAsFile(fileName, Opt);
                if (Opt.Verbose)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Done Writing {0} called file {3}. It took {1} ms and contained {2} rows", fileName, fileWriteStopWatch.Elapsed.Milliseconds, Dict.Count, Name);
                }
                fileWriteStopWatch.Stop();
            }
        }
    }

    #endregion
}
