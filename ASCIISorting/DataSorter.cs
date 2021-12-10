using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml;

namespace ASCIISorting
{
    public class DataSorter
    {
        public static List<DataPair> _reversePairlst;
        private readonly IDataSorter _memberManager;
        public DataSorter(IDataSorter memberManager)
        {
            this._memberManager = memberManager;
        }
        public DataSorter()
        {
        }

        public List<DataPair> GetSortedTextList(string inputStr)
        {
            List<DataPair> inputPairlst = new List<DataPair>();
            List<DataPair> outputPairlst = new List<DataPair>();
            if(inputStr == null )
            {
                return outputPairlst;
            }
            if( inputStr.Length == 0)
            {
                return outputPairlst;
            }

            inputPairlst = GetInputPair(inputStr);
            if(inputPairlst.Count == 0)
            {
                return outputPairlst;
            }

            List<DataPair> inputPairlstTemp = new List<DataPair>(inputPairlst);
            outputPairlst = GetSortedPair(inputPairlstTemp);
            if (outputPairlst.Count == 0)
            {
                return outputPairlst;
            }

            // save input and output pair to xml
            SaveDatatoXML(inputPairlst, outputPairlst);

            List<DataPair> ReversePairlstTemp = new List<DataPair>(outputPairlst);
            _reversePairlst = ReversePairlstTemp;
            return outputPairlst;
        }
        
        public List<DataPair> GetInputPair(string inputStr)
        {
            List<DataPair> inputPairlst = new List<DataPair>();
            if (inputStr == null)
            {
                return inputPairlst;
            }
            if (inputStr.Length == 0)
            {
                return inputPairlst;
            }
            string[] word = inputStr.Split(new string[] { " ", "\t", "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string iteam in word)
            {
                int sum = 0;
                for (int i = 0; i < iteam.Length; i++)
                {
                    sum += iteam[i];
                }
                DataPair inputPairlstTemp = new DataPair(iteam, sum);
                inputPairlst.Add(inputPairlstTemp);
            }
            
            return inputPairlst;
        }
        public List<DataPair> GetSortedPair(List<DataPair> inputPairlst)
        {
            if(inputPairlst==null)
            {
                return inputPairlst;
            }
            if (inputPairlst.Count <= 1)
            {
                return inputPairlst;
            }
            List<DataPair> smaller = new List<DataPair>();
            List<DataPair> greater = new List<DataPair>();
            int pivotPosition = inputPairlst.Count / 2;
            DataPair Obj = inputPairlst[pivotPosition];
            List<DataPair> sorted = new List<DataPair>();
            try
            {
                inputPairlst.RemoveAt(pivotPosition);
                foreach (var item in inputPairlst)
                {
                    int k = item.CompareTo(Obj);
                    if (k < 0)
                    {
                        smaller.Add(item);
                    }
                    else
                    {
                        greater.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sorted = GetSortedPair(smaller);
                sorted.Add(Obj);
                sorted.AddRange(GetSortedPair(greater));
                LogWriter.LogWrite("INFO: quicksort done");
                
            }
            return sorted;
        }
        private void SaveDatatoXML(List<DataPair> inputPairlst, List<DataPair> outputPairlst)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("Recent");
                XmlElement input = doc.CreateElement("Input");
                foreach (var item in inputPairlst)
                {
                    XmlElement inputpair = doc.CreateElement("InputPair");
                    inputpair.SetAttribute("Text", item.WordStr);
                    inputpair.SetAttribute("Sum", item.Sum.ToString());
                    input.AppendChild(inputpair);
                }
                root.AppendChild(input);
                XmlElement output = doc.CreateElement("Output");
                foreach (var item in outputPairlst)
                {
                    XmlElement outputpair = doc.CreateElement("OutputPair");
                    outputpair.SetAttribute("Text", item.WordStr);
                    outputpair.SetAttribute("Sum", item.Sum.ToString());
                    output.AppendChild(outputpair);
                }
                root.AppendChild(output);

                doc.AppendChild(root);

                doc.Save(Environment.CurrentDirectory + "\\Recent.xml");
                LogWriter.LogWrite("INFO: Data saved to XML.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                LogWriter.LogWrite("INFO: Exception while writting to XML");
            }
        }
    }
}

