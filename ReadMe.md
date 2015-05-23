#Set operations on textfiles cmd style
This project aims to build a commandline program so that you can make set operations.
  We want eg the following set operations: 
 not A and B => DiffFile  see [Explanation of expression](http://www.wolframalpha.com/input/?i=not+A+and+B "link to Wolframealpha") or see more easily [Venn diagram](http://www.wolframalpha.com/share/clip?f=d41d8cd98f00b204e9800998ecf8427e41kvo33uui "link to graph on Wolframealpha")
A and B => IntersectionFile see [Explanation of expression](http://www.wolframalpha.com/input/?i=A+and+B "link to Wolframealpha") or see more easily [Venn diagram]( http://www.wolframalpha.com/share/clip?f=d41d8cd98f00b204e9800998ecf8427e7e2qko5194 "link to graph on Wolframealpha")
 NotaBene combined => (not A and B) or (A and B) see [Explanation of expression](http://www.wolframalpha.com/input/?i=%28not+A+and+B%29+or+%28A+and+B%29 "link to Wolframealpha") or see more easily [Venn diagram](http://www.wolframalpha.com/share/clip?f=d41d8cd98f00b204e9800998ecf8427eguh00j5eik "link to graph on Wolframealpha")
DiffFile+IntersctionFile => FileB
## Why?
This is when you want a quick way to do set operations on textfiles and you do not have access to a database. Or you want to clean data quickly before you import it to a database. This idea of a program came from when we had two 10 GB files csv text files that were two databasedumps a few days apart. The legacy system should import data only wanted the difference. This sort of program crushed the data in five minutes consuming lots of ram memory meanwhile but everyone was happy. 
## Example
Make an intersection between file a and b the key are in column 4,6,7 seperator in the csv files a and b are semicolon (;) make it verbose.
> zetcmd -v -a"s:\Darkcompare\UAFF#.140206.TXT"  -b"s:\Darkcompare\UAFF#.140603.TXT" -k4 6 7 -s;

##Roadmap
Plan on a powershell version as well. 