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

        protected Func
                <Dictionary<string, string>, Dictionary<string, string>, DictCompareOnKeyOnly,
                    Dictionary<string, string>> SetOperatorFunc;
        protected SetOperator(DataChunk b,DataChunk a,DictCompareOnKeyOnly keyOnly,Options options)
        {
            BChunk = b;
            AChunk = a;
            KeyOnly = keyOnly;
            Opt = options;
        }


        public virtual Dictionary<string, string> Operate()
        { 
            var setDiffSw = Stopwatch.StartNew();
            ReturnDictionary = SetOperatorFunc(AChunk.LineDictionary, BChunk.LineDictionary, KeyOnly);
            if (Opt.Verbose)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                // TODO: This text about do not exist in file is only true for subclass DiffB. Must move it out from here.
                Console.WriteLine("File {0} ({4}) has {1} keys. {2} of them do not exist in file {3} ({5})", Path.GetFileName(BChunk.DataPath), BChunk.LineDictionary.Count(), ReturnDictionary.Count(), Path.GetFileName(AChunk.DataPath), BChunk.Name, AChunk.Name);
                Console.WriteLine("Time after creating set operator {0} ms", setDiffSw.ElapsedMilliseconds);
            }
            setDiffSw.Stop();
            return ReturnDictionary;
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
            //Here comes the magic simple Except and Intersect and force it back to Dictionary.
            SetOperatorFunc =  (aDict, bDict, k) => bDict.Except(aDict, k).ToDictionary(ld => ld.Key, ld => ld.Value);
        }
    }
}
