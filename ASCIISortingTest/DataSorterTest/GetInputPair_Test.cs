using ASCIISorting;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace ASCIISortingTest.DataSorterTest
{
    class GetInputPair_Test
    {
        [Test]
        public void GetInputPair_InputNull()
        {
            // Arrange
            var Mockobj = new Mock<IDataSorter>();
            var DataSorterobj = new DataSorter(Mockobj.Object);

            //Act //Assert
            Assert.AreEqual((DataSorterobj.GetInputPair(null)), "");
        }
        [Test]
        public void GetInputPair_InputEmptyString()
        {
            // Arrange
            var Mockobj = new Mock<IDataSorter>();
            var DataSorterobj = new DataSorter(Mockobj.Object);

            //Act //Assert
            Assert.AreEqual((DataSorterobj.GetInputPair("")), "");
        }

        [Test]
        public void GetInputPair_InputPairList()
        {
            // Arrange
            var Mockobj = new Mock<IDataSorter>();
            var DataSorterobj = new DataSorter(Mockobj.Object);
            List<DataPair> ExpectedPairlst = new List<DataPair>();

            Mockobj.Setup(x => x.GetInputPair(It.IsAny<string>()));
            Mockobj.Setup(x => x.GetSortedPair(It.IsAny<List<DataPair>>()));
            Mockobj.Setup(x => x.SaveDatatoXML(It.IsAny<List<DataPair>>(), It.IsAny<List<DataPair>>()));

            List<DataPair> inputPairlst = DataSorterobj.GetInputPair("The quick brown fox.");

            //Assert 
            Assert.AreEqual(inputPairlst.Count, 4);
           
            DataPair ExpectedPairlst1 = new DataPair("The", 289);
            DataPair ExpectedPairlst2 = new DataPair("quick", 541);
            DataPair ExpectedPairlst3 = new DataPair("brown", 552); 
            DataPair ExpectedPairlst4 = new DataPair("fox.", 379); 
            ExpectedPairlst.Add(ExpectedPairlst1);
            ExpectedPairlst.Add(ExpectedPairlst2);
            ExpectedPairlst.Add(ExpectedPairlst3);
            ExpectedPairlst.Add(ExpectedPairlst4);

            DataPair Expectediteam = new DataPair();
            DataPair outputiteam = new DataPair();
            for (int i = 0; i < 4; i++)
            {
                Expectediteam = ExpectedPairlst[i];
                outputiteam = inputPairlst[i];
                Assert.AreEqual(outputiteam.WordStr, Expectediteam.WordStr);
                Assert.AreEqual(outputiteam.Sum, Expectediteam.Sum);
            }
        }
    }
}
