using NUnit.Framework;
using ASCIISorting;
using Moq;
using System.Collections.Generic;

namespace ASCIISortingTest
{
    namespace DataSorterTest
    {
        [TestFixture]
        public class GetSortedTextList_Test
        {
            [Test]
            public void GetSortedTextList_InputNull()
            {
                // Arrange
                var Mockobj = new Mock<IDataSorter>();
                var DataSorterobj = new DataSorter(Mockobj.Object);
                
                //Act //Assert
                Assert.AreEqual((DataSorterobj.GetSortedTextList(null)), "");
            }
            [Test]
            public void GetSortedTextList_InputEmptyString()
            {
                // Arrange
                var Mockobj = new Mock<IDataSorter>();
                var DataSorterobj = new DataSorter(Mockobj.Object);

                //Act //Assert
                Assert.AreEqual((DataSorterobj.GetSortedTextList("")), "");
            }

            [Test]
            public void GetSortedTextList_SortedOutputList()
            {
                // Arrange
                var Mockobj = new Mock<IDataSorter>();
                var DataSorterobj = new DataSorter(Mockobj.Object);
                List<DataPair> ExpectedPairlst = new List<DataPair>();

                Mockobj.Setup(x => x.GetInputPair(It.IsAny<string>()));
                Mockobj.Setup(x => x.GetSortedPair(It.IsAny<List<DataPair>>()));

                //Act
                List<DataPair> outputPairlst = DataSorterobj.GetSortedTextList("The quick brown fox.");

                //Assert
                //Mockobj.Verify(x => x.GetInputPair(It.IsAny<string>()), Times.Exactly(1));
                //Mockobj.Verify(x => x.GetSortedPair(It.IsAny<List<DataPair>>()), Times.Exactly(1));

                Assert.AreEqual(outputPairlst.Count, 4);

                DataPair ExpectedPairlst1 = new DataPair("brown", 552);
                DataPair ExpectedPairlst2 = new DataPair("quick", 541);
                DataPair ExpectedPairlst3 = new DataPair("fox.", 379);
                DataPair ExpectedPairlst4 = new DataPair("The", 289);
                ExpectedPairlst.Add(ExpectedPairlst1);
                ExpectedPairlst.Add(ExpectedPairlst2);
                ExpectedPairlst.Add(ExpectedPairlst3);
                ExpectedPairlst.Add(ExpectedPairlst4);

                DataPair Expectediteam = new DataPair();
                DataPair outputiteam = new DataPair();
                for (int i = 0; i < 4; i++)
                {
                    Expectediteam = ExpectedPairlst[i];
                    outputiteam = outputPairlst[i];
                    Assert.AreEqual(outputiteam.WordStr, Expectediteam.WordStr);
                    Assert.AreEqual(outputiteam.Sum, Expectediteam.Sum);
                }
            }

        }
    }
}
