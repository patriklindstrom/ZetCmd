using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text; // if you want text formatting helpers (recommended)


namespace ZetCmd
{
    // params used while running -v -a"s:\Darkcompare\UAFF#.140206.TXT"  -b"s:\Darkcompare\UAFF#.140603.TXT" -k4 6 7 -s;

    /// <summary>
    /// We want the following set operations: 
    /// not A and B => DiffFile  (see http://www.wolframalpha.com/input/?i=not+A+and+B ) (Venn diagram http://www.wolframalpha.com/share/clip?f=d41d8cd98f00b204e9800998ecf8427e41kvo33uui)
    /// A and B => IntersectionFile (see http://www.wolframalpha.com/input/?i=A+and+B ) ( Venn diagram  http://www.wolframalpha.com/share/clip?f=d41d8cd98f00b204e9800998ecf8427e7e2qko5194 )
    /// NotaBene combined => (not A and B) or (A and B) (see http://www.wolframalpha.com/input/?i=%28not+A+and+B%29+or+%28A+and+B%29 ) (Venn Diagram http://www.wolframalpha.com/share/clip?f=d41d8cd98f00b204e9800998ecf8427eguh00j5eik)
    /// DiffFile+IntersctionFile => FileB
    /// </summary>
    /// 
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");
            Console.WriteLine("Welcome to ZetCmd a program to perform setbased operation on datafiles.");
            Console.WriteLine("Cmd Syntax is Getopt style. Type ? to get help. See: http://en.wikipedia.org/wiki/Getopt ");
            Console.ReadKey();

           var options =  new Options();
            var result = Parser.Default.ParseArguments<Options>(args);
             if (!result.Errors.Any())
             {
                var programStopwatch = Stopwatch.StartNew();
                var chunkList = new List<DataChunk>
                {
                    new DataChunk(dataPath:options.FileA,name:"A", option:options),
                    new DataChunk(dataPath:options.FileB,name:"B", option:options)
                };
                //Multithread the reading of files and making dictionary of all lines in file
                Parallel.ForEach(chunkList, dl => dl.GetDataContent(new FileLineReader(dl.DataPath)));
                //Give the sets nicer names
                var a = chunkList.First(f => f.Name == "A");
                var b = chunkList.First(f => f.Name == "B"); 
                //We need Compare to use set logic on keys only not Key and Value which is the default - odd that is the default and we have to override it.
                var keyOnly = new DictCompareOnKeyOnly();
                var setOp = new DiffB(a:a,b:b,keyOnly:keyOnly,options:options);
                //Here we save the output as text files and do magic in the DiffDB function
                var outPutList = new List<OutputObj>
                {
                    new OutputObj(name:"DiffB",dict: setOp.ReturnDictionary,opt: options),
                };
                //Multithread the output of files
                Parallel.ForEach(outPutList, oL => oL.Output());
                programStopwatch.Stop();
                if (options.Verbose)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Done ! hit any key to exit program. ExecutionTime was {0} ms",
                        programStopwatch.Elapsed.Milliseconds);
                    Console.ReadLine();
                }
            }
        }

        }
    
}
