using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace ASCIISorting
{
    public partial class MainWindow : Window
    {
        private int _iMax = 0;
        public MainWindow()
        {
            InitializeComponent();
            AddHandler(FrameworkElement.MouseDownEvent, new MouseButtonEventHandler(OnInputTextBoxMouseDown), true);
        }

        private void OnInputTextChanged(object sender, TextChangedEventArgs e)
        {
            if (inputTextBox != null)
            {
                DisableSortbtn();
                String Input = inputTextBox.Text;

                if (Input.Length != 0)
                {
                    EnableSortbtn();
                    ClearOutputTextBox();
                }
                string[] word = inputTextBox.Text.Split(new string[] { " ", "\t", "\n", "\r\n", "\r" },
                   StringSplitOptions.RemoveEmptyEntries);

                _iMax = word.Length;
            }

            UpdateTaskbar();
        }
        private void OnInputTextBoxMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UpdateTaskbar();
        }
        private void OnOutputTextPopulated(object sender, TextChangedEventArgs e)
        {
            if (outputTextBox != null)
            {
                DisableSaveandReversebtn();
                if (outputTextBox.Text != "")
                {
                    EnableSaveandReversebtn();
                    DisableSortbtn();
                }
            }
        }
        private void OnRecentMenuItemClick(object sender, RoutedEventArgs e)
        {
            List<DataPair> inputPairlst = new List<DataPair>();
            List<DataPair> outputPairlst = new List<DataPair>();

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(Environment.CurrentDirectory + "\\Recent.xml");

                XmlNodeList inputList = xmlDoc.GetElementsByTagName("InputPair");
                foreach (XmlElement item in inputList)
                {
                    DataPair dp = new DataPair(item.Attributes["Text"].Value, Convert.ToInt32(item.Attributes["Sum"].Value));
                    inputPairlst.Add(dp);
                }

                XmlNodeList outputList = xmlDoc.GetElementsByTagName("OutputPair");
                foreach (XmlElement item in outputList)
                {
                    DataPair dp = new DataPair(item.Attributes["Text"].Value, Convert.ToInt32(item.Attributes["Sum"].Value));
                    outputPairlst.Add(dp);
                }
                string inputStr = " ";
                foreach (var word in inputPairlst)
                {
                    inputStr += word.WordStr + " ";
                }
                inputTextBox.Text = inputStr;

                string outputStr = " ";
                foreach (var word in outputPairlst)
                {
                    outputStr += word.WordStr + " ";
                }
                outputTextBox.Text = outputStr;

                List<DataPair> ReversePairlstTemp = new List<DataPair>(outputPairlst);
                DataSorter._reversePairlst = ReversePairlstTemp;
                DisableSortbtn();
                LogWriter.LogWrite("INFO: Data read from XML.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogWriter.LogWrite("INFO: Exception while reading from XML.");
            }
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, outputTextBox.Text);
                ClearinputTextBox();
                ClearOutputTextBox();
            }
        }
        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (inputTextBox != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text Files (*.txt)|*.txt";

                if (openFileDialog.ShowDialog() == true)
                {
                    ClearinputTextBox();
                    inputTextBox.Text = File.ReadAllText(openFileDialog.FileName);
                    LogWriter.LogWrite("INFO: Input text inported from file");
                }
            }
        }
        private void SortCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (inputTextBox != null && outputTextBox != null)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                worker.RunWorkerAsync();

                DataSorter sortObj = new DataSorter();
                List<DataPair> outputTextPair = sortObj.GetSortedTextList(inputTextBox.Text);
                string outputstr = "";
                foreach (var pair in outputTextPair)
                {
                    outputstr += pair.WordStr+" ";
                }
                outputTextBox.Text = outputstr;

                if (TbReverse != null)
                {
                    TbReverse.IsEnabled = true;
                }
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //var x = Math.Round((decimal)(100 / _iMax));
            var backgroundWorker = (sender as BackgroundWorker);
            backgroundWorker.ReportProgress(0);
            for (int i = 0; i < _iMax; i++)
            {
                //TODO: Update the progress bar after sorting each word here...
                Thread.Sleep(10);
                backgroundWorker.ReportProgress((i * 5));
            }
            backgroundWorker.ReportProgress(100);
        }
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
        private void ReverseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (outputTextBox != null && DataSorter._reversePairlst != null)
            {
                DataSorter._reversePairlst.Reverse();
                string ReverseStr = "";
                foreach (var word in DataSorter._reversePairlst)
                {
                    ReverseStr += word.WordStr + " ";
                }
            
                outputTextBox.Text = ReverseStr;
                LogWriter.LogWrite("INFO: Data sorted text revered");
            }
        }
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataSorter sortObj = new DataSorter();
            sortObj.GetSortedTextList(inputTextBox.Text);

            LogWriter.LogWrite("INFO: Data sorted and saved to XML on Exit.");
            this.Close();
        }
        private void ClearinputTextBox()
        {
            if (inputTextBox != null)
            {
                inputTextBox.SelectAll();
                inputTextBox.Text = "";
                LogWriter.LogWrite("INFO: Clearing InputTextBox.");
            }
        }
        private void ClearOutputTextBox()
        {
            if (outputTextBox != null && TbReverse != null)
            {
                outputTextBox.Text = "";
                TbReverse.IsEnabled = false;
            }
        }
        private void DisableSortbtn()
        {
            if (BSort != null && TbSort != null)
            {
                BSort.IsEnabled = false;
                TbSort.IsEnabled = false;
            }
        }
        private void EnableSortbtn()
        {
            if (BSort != null && TbSort != null)
            {
                BSort.IsEnabled = true;
                TbSort.IsEnabled = true;
            }
        }
        private void DisableSaveandReversebtn()
        {
            if (BSave != null && MSave != null && TbSave != null && TbReverse != null)
            {
                BSave.IsEnabled = false;
                MSave.IsEnabled = false;
                TbSave.IsEnabled = false;
                TbReverse.IsEnabled = false;
            }
        }
        private void EnableSaveandReversebtn()
        {
            if (BSave != null && MSave != null && TbSave != null && TbReverse != null)
            {
                BSave.IsEnabled = true;
                MSave.IsEnabled = true;
                TbSave.IsEnabled = true;
                TbReverse.IsEnabled = true;
            }
        }
        private void UpdateTaskbar()
        {
            if (lblCursorPosition != null)
            {
                int row = inputTextBox.GetLineIndexFromCharacterIndex(inputTextBox.CaretIndex);
                int col = inputTextBox.CaretIndex - inputTextBox.GetCharacterIndexFromLineIndex(row);
                lblCursorPosition.Text = "Line " + (row + 1) + ", Char " + (col + 1);
            }
        }
        private void InputTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (lblCursorPosition != null)
            {
                lblCursorPosition.Text = "";
            }
        }
        private void OnHelpMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process wordProcess = new Process();
                wordProcess.StartInfo.FileName = Environment.CurrentDirectory + "\\Help.docx";
                wordProcess.StartInfo.UseShellExecute = true;
                wordProcess.Start();
                LogWriter.LogWrite("INFO: Help file opened");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                LogWriter.LogWrite("INFO: Help file not found");
            }

        }
        // CanExecute
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (TbSave != null || BSave != null || MSave != null)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }
        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ReverseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (TbReverse != null)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }
        private void SortCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (BSort != null || TbSort != null)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }
    }
}
