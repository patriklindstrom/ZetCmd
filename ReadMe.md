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
### Simple intersection
Make an intersection between file a and b the key are in column 4,6,7 seperator in the csv files a and b are semicolon (;) make it verbose.
> zetcmd -v -a"s:\Darkcompare\A_TestFile.cs"  -b"s:\Darkcompare\B_TestFile.cs" -k4 6 7 -s;
In pseudo SQL it would be something like:
> SELECT a.* from A_TestFile as a INNER JOIN B_TestFile as b on a.k4=b.k4 and a.k6=b.k6 and a.k7=b.k7
### Many Setoperation on two sets
On the two sets A_TestFile and B_TestFile defined by the key on column 1 and 2 where the column separator is semicolon (;)
make sets that are DiffB and DiffA and the intersection of the two. Describe all in a verbose style.
>ZetCmd.exe -v  -a".\A_TestFile.csv"  -b".\B_TestFile.csv" -k1 2 -s; -r -d -i
In pseudo SQL it would be something like:
> SELECT b.* from B_TestFile as b  WHERE b.key_1_2 not in (Select a.key_1_2 from A_TestFile as a)
> SELECT a.* from A_TestFile as a  WHERE a.key_1_2 not in (Select a.key_1_2 from B_TestFile as b)
> SELECT a.* from A_TestFile as a  INNER JOIN B_TestFile as b ON (a.key_1_2 = b.key_1_2 )

## How
Download the ZetCmd.Exe file from here  https://github.com/patriklindstrom/ZetCmd/tree/master/src/ZetCmd/Build%2020150616
It is a combination with iLMerge of the ZetCmd.exe and CommandLineParser (https://github.com/gsscoder/commandline/tree/stable-1.9.71.2) .
More safe is of course that you download the project and compile it yourself. 

##Roadmap
Plan on a powershell version as well. 
