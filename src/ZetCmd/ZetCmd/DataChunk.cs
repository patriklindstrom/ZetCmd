using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZetCmd
{
    #region Stuff to abstract the StreamText Reader into DataReader

    public interface ILineReader : IDisposable
    {
        string ReadLine();
    }

    public class FileLineReader : ILineReader, IDisposable
    {
        public StreamReader StreamReader { get; set; }

        public FileLineReader(string path)
        {
            Debug.Assert(String.IsNullOrEmpty(path) == false);
            Debug.WriteLine(path);
            Debug.Assert(File.Exists(path));
            if (!(File.Exists(path)))
            {
                throw new ArgumentException("File do not exist");
            }
            StreamReader = new StreamReader(File.OpenRead(path));
        }

        public string ReadLine()
        {
            return StreamReader.ReadLine();
        }

        public void Dispose()
        {
            StreamReader.Dispose();
        }
    }

    #endregion

    public interface IDataChunk
    {
        string Name { get; set; }
        string DataPath { get; set; }
        IOptions Option { get; set; }
        Dictionary<string, string> LineDictionary { get; set; }
        void GetDataContent(ILineReader dr);
        void BuildRowKey(ref StringBuilder rowKey, string line, char splitChar, int[] keyColumns);
    }
    public class DataChunk : IDataChunk
    {
        public string Name { get; set; }
        public string DataPath { get; set; }
        public Dictionary<string, string> LineDictionary { get; set; }
        public IOptions Option { get; set; }

        public DataChunk(string dataPath, string name, IOptions option)
        {
            Option = option;
            DataPath = dataPath;
            Name = name;
            LineDictionary = new Dictionary<string, string>();
        }

        public void GetDataContent(ILineReader dr)
        {
            int i = 0; //rowcounter to see where error occured
            try
            {
                var fileStopwatch = Stopwatch.StartNew();
                #region Verbose output

                if (Option.Verbose)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Start reading {0}", DataPath);
                }

                #endregion
                using (dr)
                {
                    string line;
                    var rowKey = new StringBuilder();
                    var colKeys = Option.Keycolumns;
                    var sepChar = Option.Fieldseparator;

                    while ((line = dr.ReadLine()) != null)
                    {
                        i += 1;

                        //Fields 4,6,7 makes the row unique according to rumours. Not that fieldArr is nollbased so it is: 3,5,6                   
                        BuildRowKey(ref rowKey, line, sepChar, colKeys);
                        if (Option.FieldCompression)
                        {
                            line = StringCompressor.CompressString(line);
                        }
                        LineDictionary.Add(rowKey.ToString(), line);
                        rowKey.Clear();
                    }
                }
                fileStopwatch.Stop();
                #region Verbose output

                if (Option.Verbose)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Done Reading {0} called file {3}. It took {1} ms and contained {2} rows", DataPath, fileStopwatch.Elapsed.Milliseconds, LineDictionary.Count, Name);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }

                #endregion
            }
            #region Catch if error

            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The file {0} could not be transformed to Dictionary structure error in line {1}:",
                    DataPath, i);
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }

            #endregion
        }

        public void BuildRowKey(ref StringBuilder rowKey, string line, char splitChar, int[] keyColumns)
        {
            var fieldArr = line.Split(splitChar);
            for (int index = 0; index < keyColumns.Length - 1; index++)
            {
                rowKey.Append(fieldArr[keyColumns[index] - 1]).Append("|");
            }
            rowKey.Append(fieldArr[keyColumns.Last() - 1]);
        }
    }

}
