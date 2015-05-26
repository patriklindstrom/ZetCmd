
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;


namespace ZetCmd.Test
{
    [TestFixture]
    public class TestSetOperationsForDiffB
    {
        private static DiffB ArrangeSutExpectedDict(Dictionary<string, string> mockSetA,
            Dictionary<string, string> mockSetB)
        {
            var opt = new Options
            {
                FileA = Mv.FileA
                ,
                FileB = Mv.FileB
                ,
                DiffB = Mv.DiffB
                ,
                IntersectAandB = Mv.IntersectAandB
                ,
                Verbose = Mv.Verbose
                ,
                Fieldseparator = Mv.Splitchar
                ,
                Keycolumns = Mv.ColKeys
            };
            var keyOnly = new DictCompareOnKeyOnly();
            var mockChunckA = new DataChunk(dataPath: Mv.DataPath, name: Mv.Name, option: opt)
            {
                LineDictionary = mockSetA
            };
            var mockChunkB = new DataChunk(dataPath: Mv.DataPath, name: Mv.Name, option: opt)
            {
                LineDictionary = mockSetB
            };
            var sut = new DiffB(a: mockChunckA, b: mockChunkB, keyOnly: keyOnly, options: opt);
            return sut;
        }
        [Test]
          public void SetBIsOneRowMore()
        {
            //Arrange
           var mockSetA = new Dictionary<string, string>
            {
                {Mv.Key123Line, Mv.Line},
                {Mv.Key123Line2, Mv.Line2},
                {Mv.Key123Line3, Mv.Line3}
            };
          var  mockSetB = new Dictionary<string, string>
            {
                {Mv.Key123Line, Mv.Line},
                {Mv.Key123Line2, Mv.Line2},
                {Mv.Key123Line3, Mv.Line3},
                {Mv.Key123Line4, Mv.Line4}
            };
            var expectedDict = new Dictionary<string, string> { { Mv.Key123Line4, Mv.Line4 } };
            var sut = ArrangeSutExpectedDict( mockSetA,  mockSetB);
            //Act
            var sutLineDict = sut.ReturnDictionary;
            //Assert
           Assert.AreEqual(expectedDict.Except(sutLineDict).Count(),
             sutLineDict.Except(expectedDict).Count());
        }

       

        [Test]
        static public void SetBIsOneRowMoreAndAOneRowLess()
        {
            //Arrange
            Dictionary<string, string> mockSetA = new Dictionary<string, string>
            {
                {Mv.Key123Line, Mv.Line},
                //{Mv.Key123Line2, Mv.Line2},
                {Mv.Key123Line3, Mv.Line3}
            };
            Dictionary<string, string> mockSetB = new Dictionary<string, string>
            {
                {Mv.Key123Line, Mv.Line},
                {Mv.Key123Line2, Mv.Line2},
                {Mv.Key123Line3, Mv.Line3},
                {Mv.Key123Line4, Mv.Line4}
            };
            var expectedDict = new Dictionary<string, string> { { Mv.Key123Line4, Mv.Line4 }, { Mv.Key123Line2, Mv.Line2 } };
            var sut = ArrangeSutExpectedDict(mockSetA, mockSetB);           
            //Act
            var sutLineDict = sut.ReturnDictionary;
            //Assert
            Assert.AreEqual(expectedDict.Except(sutLineDict).Count(),
              sutLineDict.Except(expectedDict).Count());
        }
        [Test]
        static public void SetBIsOnLessAndAIsOneRowLess()
        {
            //Arrange           
            Dictionary<string, string> mockSetA = new Dictionary<string, string>
            {
                {Mv.Key123Line, Mv.Line},
               // {Mv.Key123Line2, Mv.Line2},
                {Mv.Key123Line3, Mv.Line3}
            };
            Dictionary<string, string> mockSetB = new Dictionary<string, string>
            {
               // {"fum;fan;foo", Mv.Line},
                {Mv.Key123Line2, Mv.Line2},
                {Mv.Key123Line3, Mv.Line3},
                {Mv.Key123Line4, Mv.Line4}
            };
            var expectedDict = new Dictionary<string, string> { { Mv.Key123Line4, Mv.Line4 }, { Mv.Key123Line2, Mv.Line2 } };
            var sut = ArrangeSutExpectedDict(mockSetA, mockSetB);  
            //Act
            var sutLineDict = sut.ReturnDictionary;
            //Assert
            Assert.AreEqual(expectedDict.Except(sutLineDict).Count(),
              sutLineDict.Except(expectedDict).Count());
        }
    }
}

