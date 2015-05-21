using System;


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
            System.Console.WriteLine("Welcome to ZetCmd a program to perform setbased operation on datafiles.");
            System.Console.WriteLine("Cmd Syntax is Getopt style. Type ? to get help. See: http://en.wikipedia.org/wiki/Getopt ");
            Console.ReadKey();

        }
    }
}
