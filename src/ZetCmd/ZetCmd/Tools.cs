using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace ZetCmd
{

    #region Tool classes like extented method and argument parsers

    public static class Dictionary
    {
        /// <summary>
        /// Extended method that saves the value part of a string string dictionary as rows in a textfile
        /// </summary>
        /// <param name="dict">this dictionary</param>
        /// <param name="filePath">The path to where the files should be stored eg: c:\temp</param>
        public static void SaveValuesAsFile(this Dictionary<string, string> dict, string filePath, Options opt)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
                foreach (var item in dict)

                    writer.WriteLine("{0}",
                        opt.FieldCompression ? StringCompressor.DecompressString(item.Value) : item.Value);
        }
    }

    public class DictCompareOnKeyOnly : IEqualityComparer<KeyValuePair<string, string>>
    {
        public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            return x.Key.Equals(y.Key);
        }

        public int GetHashCode(KeyValuePair<string, string> obj)
        {
            return obj.Key.GetHashCode();
        }
    }

    public interface IOptions
    {
        string FileA { get; set; }
        string FileB { get; set; }
        char Fieldseparator { get; set; }
        bool DiffB { get; set; }
        bool IntersectAandB { get; set; }
        bool Verbose { get; set; }
        bool FieldCompression { get; set; }
        int[] Keycolumns { get; set; }

        string GetUsage();
    }
    /// <summary>
    /// The CommandLine parser is based on https://github.com/cosmo0/commandline which is a fork from https://github.com/gsscoder/commandline
    /// </summary>
    public class Options : IOptions
    {
        // Good test parameters for deafult meaning: verbose fileA and fileB key is in combination of column [4,6,7] - where first column is called 1. Separator char is semikolon 
        //-v -a"s:\Darkcompare\UAFF#.140206.TXT"  -b"s:\Darkcompare\UAFF#.140603.TXT" -k4 6 7 -s;
        [Option('a', "fileA", Required = true, HelpText = "Input A csv file to read.")]
        public string FileA { get; set; }

        [Option('b', "fileb", Required = true, HelpText = "Input B csv file to read.")]
        public string FileB { get; set; }


        [Option('d', "DiffB", Required = false, HelpText = "Calculate and output Diff B csv file.")]
        public bool DiffB { get; set; }
        [Option('r', "DiffA", Required = false, HelpText = "Calculate and output Diff A csv file.")]
        public bool DiffA { get; set; }
        [Option('i', "IntersectAandB", Required = false, HelpText = "Calculate and output intersectAandB csv file.")]
        public bool IntersectAandB { get; set; }
        [Option('u', "UnionAandB", Required = false, HelpText = "Calculate and output intersectAandB csv file.")]
        public bool UnionAandB { get; set; }
        [OptionArray('k', "keycolumns", Required = false, DefaultValue = new int[] { 4, 6, 7 }, HelpText = "What columns combined are the key of every row.")]
        public int[] Keycolumns { get; set; }
        [Option('v', null, Required = false, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
        [Option('s', "fieldseparator", Required = false, DefaultValue = ';', HelpText = "Char that separates every column")]
        public char Fieldseparator { get; set; }
        [Option('c', "fieldcompression", Required = false, DefaultValue = false, HelpText = "Compressess the row. Takes less memory but maybe longer time.")]
        public bool FieldCompression { get; set; }
        [Option('e', "version", Required = false, HelpText = "Prints version number of program.")]
        public string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

        }

       // [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            //  or using HelpText.AutoBuild
            var usage = new StringBuilder();
            usage.AppendLine(
                String.Format(
                    "ZetCmd Application takes Difference between two cvs files on columns 4,6,7 version {0}",
                    Assembly.GetExecutingAssembly().GetName().Version));
            usage.AppendLine("give help as param for help. Simple usage -a[fileA] -b[fileB] ");
            usage.AppendLine("Developed by Patrik Lindström 2015-06-01");
            return usage.ToString();
        }
    }

    internal static class StringCompressor
    {
        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }

    #endregion

}
