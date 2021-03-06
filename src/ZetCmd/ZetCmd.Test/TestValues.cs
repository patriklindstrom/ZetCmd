﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZetCmd.Test
{
    /// <summary>
    /// Handy MockValues to use wherever in tests.
    /// </summary>
    public static class Mv
    {
        public static string Line = "fum;fan;foo;bar;king;barter;28;496;8128;;;endisnear;really?";
        public static string Key123Line = "fum;fan;foo";
        public static string Line2 = "fum;fan;fee;bar;king;stop;28;496;8128;;True;endisnear;really?";
        public static string Key123Line2 = "fum;fan;fee";
        public static string Line3 = "456;fan;faa;bar;king;barter;28;496;8128;fist;;endi!snear;rally";
        public static string Key123Line3 = "456;fan;faa";
        public static string Line4 = "NewFi;foo;fum;bar;king;barter;28;496;8128;fist;;endi!snear;rally";
        public static string Key123Line4 = "NewFi;foo;fum";
        public static string cSsepLine = "fum,fan,foo,bar,king,barter,28,49;6,8128,,,endisnear,Wally";
        public static string cSsepLine2 = "fum,fan,fee,bar,king,stop,28,496,8128,,True,endisnear,really?";
        public static string cSsepLine3 = "fum,fan,fee,bar,king,stop,28,496,25964951,,False,endisnear,Sally?";
        public static string ExpectedKey = "foo|bar|barter";

        public static int[] ColKeys = { 3, 4, 6 };
        public static char Splitchar = ';';
        public static string DataPath = "C:\\temp\\testpath";
        public static string Name = "TestName_Tore";

        public static string FileA = "D:\\DataImport\\TestFileA.csv}";
        public static string FileB = "D:\\DataImport\\TestFileB.csv}";
        public static bool IntersectAandB = true;
        public static bool Verbose;
        public static bool DiffB = true;
        public static bool DiffA = true;
        public static bool UnionAB = true;
    }
}
