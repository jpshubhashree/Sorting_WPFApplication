using ASCIISorting;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace ASCIISortingTest.DataSorterTest
{
    class GetSortedPair_Test
    {
        [Test]
        public void GetSortedPair_InputNull()
        {
            // Arrange
            var Mockobj = new Mock<IDataSorter>();
            var DataSorterobj = new DataSorter(Mockobj.Object);

            //Act //Assert
            Assert.AreEqual((DataSorterobj.GetSortedPair(null)), null);
        }
        [Test]
        public void GetSortedPair_InputEmptyPairLst()
        {
            // Arrange
            var Mockobj = new Mock<IDataSorter>();
            var DataSorterobj = new DataSorter(Mockobj.Object);
            List<DataPair> inputPairlst = new List<DataPair>();

            //Act //Assert
            Assert.AreEqual((DataSorterobj.GetSortedPair(inputPairlst)), inputPairlst);
        }

        [Test]
        public void GetSortedPair_QuickSort()
        {
            // Arrange
            var Mockobj = new Mock<IDataSorter>();
            var DataSorterobj = new DataSorter(Mockobj.Object);

            List<DataPair> inputPairlst = new List<DataPair>();
            List<DataPair> ExpectedPairlst = new List<DataPair>();
            List<DataPair> outputPairlst = new List<DataPair>();

            DataPair inputPairlst1 = new DataPair("The", 289);
            DataPair inputPairlst2 = new DataPair("quick", 541);
            DataPair inputPairlst3 = new DataPair("brown", 552);
            DataPair inputPairlst4 = new DataPair("fox.", 379);
            inputPairlst.Add(inputPairlst1);
            inputPairlst.Add(inputPairlst2);
            inputPairlst.Add(inputPairlst3);
            inputPairlst.Add(inputPairlst4);

            //Act //Assert
            outputPairlst = DataSorterobj.GetSortedPair(inputPairlst);

            //Assert 
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
