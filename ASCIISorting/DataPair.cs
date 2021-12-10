using System;
using System.Xml.Serialization;

namespace ASCIISorting 
{
    public class DataPair: IComparable<DataPair>
    {
        public string WordStr { get; set; }
        public int Sum { get; set; }
        public DataPair(string wordStr, int sum)
        {
            WordStr = wordStr;
            Sum = sum;
        }
        public DataPair() {}
        public int CompareTo(DataPair Obj)
        {
            if(this.Sum > Obj.Sum)
            {
                return -1;
            }
            if (this.Sum < Obj.Sum)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
