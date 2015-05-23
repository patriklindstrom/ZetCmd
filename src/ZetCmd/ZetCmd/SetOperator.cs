using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZetCmd
{
    public abstract class SetOperator
    {
        public Dictionary<string, string> ReturnDictionary { get; set; }
        public DataChunk AChunk { get; set; }
        public DataChunk BChunk { get; set; }
        public DictCompareOnKeyOnly KeyOnly { get; set; }
        public Options Opt { get; set; }

        protected SetOperator(DataChunk b,DataChunk a,DictCompareOnKeyOnly keyOnly,Options options)
        {
            BChunk = b;
            AChunk = a;
            KeyOnly = keyOnly;
            Opt = options;
        }


        // Operate should take lambda function as param.
        public Dictionary<string, string> Operate()
        {
              //Here comes the magic simple Except and Intersect and force it back to Dictionary.
            var setDiffSw = Stopwatch.StartNew();
            var diffB = AChunk.LineDictionary.Except(AChunk.LineDictionary, KeyOnly).ToDictionary(ld => ld.Key, ld => ld.Value);

            if (Opt.Verbose)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File {0} ({4}) has {1} keys. {2} of them do not exist in file {3} ({5})", Path.GetFileName(b.DataPath), b.LineDictionary.Count(), diffB.Count(), Path.GetFileName(a.DataPath), b.Name, a.Name);
                Console.WriteLine("Time after creating set operator {0} ms", setDiffSw.ElapsedMilliseconds);
            }
            setDiffSw.Stop();
            return diffB;
        }
    }

    public class DiffB : SetOperator
    {
        public DiffB(DataChunk b, DataChunk a, DictCompareOnKeyOnly keyOnly, Options options) : base(b, a, keyOnly, options)
        {
            BChunk = b;
            AChunk = a;
            KeyOnly = keyOnly;
            Opt = options;
        }

    }
}
