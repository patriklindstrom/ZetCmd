using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace ZetCmd
{
    public abstract class SetOperator
    {
        #region Properties for SetOperator class
        private Dictionary<string, string> _returnDictionary;
        public Dictionary<string, string> ReturnDictionary
        {
            get
            {                
               var setDiffSw = Stopwatch.StartNew();
                try
                {
                    _returnDictionary = SetOperatorFunc(AChunk.LineDictionary, BChunk.LineDictionary, KeyOnly);
                }
                catch (Exception e)
                { 
                    Console.WriteLine(e);
                    if (SetOperatorFunc == null)   Console.WriteLine("SetOperator function is null subclass should set this function");
                }
                if (Opt.Verbose)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    // TODO: This text about do not exist in file is only true for subclass DiffB. Must move it out from here.
                    Console.WriteLine("File {0} ({4}) has {1} keys. {2} of them do not exist in file {3} ({5})", Path.GetFileName(BChunk.DataPath), BChunk.LineDictionary.Count(), _returnDictionary.Count(), Path.GetFileName(AChunk.DataPath), BChunk.Name, AChunk.Name);
                    Console.WriteLine("Time after creating set operator {0} ms", setDiffSw.ElapsedMilliseconds);
                }
                setDiffSw.Stop();                
                return _returnDictionary;
            }
        }
        public DataChunk AChunk { get; set; }
        public DataChunk BChunk { get; set; }
        public DictCompareOnKeyOnly KeyOnly { get; set; }
        public Options Opt { get; set; }
        //Here is the clever method that is as disgusied as a function that is set in the subclass constructor method.
        protected Func
                <Dictionary<string, string>, Dictionary<string, string>, DictCompareOnKeyOnly,
                    Dictionary<string, string>> SetOperatorFunc { get; set; }
        #endregion
        protected SetOperator(DataChunk b,DataChunk a,DictCompareOnKeyOnly keyOnly,Options options)
        {
            BChunk = b;
            AChunk = a;
            KeyOnly = keyOnly;
            Opt = options;
            SetOperatorFunc = null; // This function should be set in subclass.
        }
    }

    public class DiffB : SetOperator
    {       
        public DiffB(DataChunk b, DataChunk a, DictCompareOnKeyOnly keyOnly, Options options) : base(b, a, keyOnly, options)
        {
            // Here comes the magic simple Except and Intersect and force it back to Dictionary
            // all in Lambda Link style using built in Dot Net set operations for the collections.
            SetOperatorFunc =  (aDict, bDict, k) => bDict.Except(aDict, k).ToDictionary(ld => ld.Key, ld => ld.Value);
        }
    }
    public class UnionAB : SetOperator
    {
        public UnionAB(DataChunk b, DataChunk a, DictCompareOnKeyOnly keyOnly, Options options)
            : base(b, a, keyOnly, options)
        {
            // Here comes the magic a simple Union and force it back to Dictionary
            // all in Lambda Link style using built in Dot Net set operations for the collections.
            SetOperatorFunc = (aDict, bDict, k) => bDict.Union(aDict, k).ToDictionary(ld => ld.Key, ld => ld.Value);
        }
    }
}
