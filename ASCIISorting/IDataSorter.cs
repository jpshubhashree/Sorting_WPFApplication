using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIISorting
{
    public interface IDataSorter
    {
        void SaveDatatoXML(List<DataPair> inputPairlst, List<DataPair> outputPairlst);
        List<DataPair> GetSortedPair(List<DataPair> inputPairlst);
        List<DataPair> GetInputPair(string inputStr);
        List<DataPair> GetSortedTextList(string inputStr);
    }
}
