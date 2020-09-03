using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IndentDmisCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            IndentCode(txtFolder.Text);
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog ofd = new CommonOpenFileDialog();
            ofd.IsFolderPicker = true;
            if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
                txtFolder.Text = ofd.FileName;
        }


        /// <summary>
        /// List of keywords that indent.
        /// </summary>
        private static readonly List<string> m_indent = new List<string>
        {
            "IF/",
            "MEAS/",
            "DO/",
            "$$<",
            "DMISMD/",
            "DMISMN/",
            "M("
        };

        /// <summary>
        /// List of keywords that un-indent.
        /// </summary>
        private static readonly List<string> m_unindent = new List<string>
        {
            "ENDIF",
            "ELSE",
            "ENDDO",
            "$$<\\",
            "ENDFIL",
            "ENDMAC",
            "ENDMES"
        };


        /// <summary>
        /// Indents the code for all files in the specified folder.
        /// </summary>
        /// <param name="folderName"></param>
        private void IndentCode(string folderName)
        {
            int tabLength = 2;
            List<FileInfo> dmisFiles = new DirectoryInfo(folderName).GetFiles("*.dmi").ToList();

            // loop through each file
            foreach (FileInfo dmisFile in dmisFiles)
            {
                // read the contents of the file
                List<string> fileLines = new List<string>();
                using (StreamReader sr = new StreamReader(dmisFile.FullName))
                {
                    while (!sr.EndOfStream)
                        fileLines.Add(sr.ReadLine().Trim());
                    sr.Close();
                }


                List<string> outputText = new List<string>();
                int numTabs = 0;

                // loop through each line in the current file.
                foreach (string line in fileLines)
                {
                    
                    string tag = null;
                    // loop through all of the indent keywords
                    foreach (string indent in m_indent)
                    {
                        // does the line start with an indent keyword?
                        if (line.ToUpper().StartsWith(indent))
                        {
                            // yes, set the tag
                            tag = indent;
                            // if the line does not start with one of the special modus closing xml tags
                            if (!line.StartsWith("$$<\\") && !line.StartsWith("$$</>"))
                            {
                                // add the current line with the existing number of tabs
                                outputText.Add(new string(' ', tabLength * numTabs) + line);

                                // increase the number of tabs
                                numTabs++;
                            }
                        }
                    }

                    // loop through all of the unindent keywords
                    foreach (string unindent in m_unindent)
                    {
                        // does the line start with an unindent keyword?
                        if (line.ToUpper().StartsWith(unindent))
                        {
                            // yes, set the tag
                            tag = unindent;

                            // decrease the number of tabs
                            numTabs--;

                            // output the current line with the new number of tabs
                            outputText.Add(new string(' ', numTabs < 0 ? tabLength : tabLength * numTabs) + line);
                        }

                    }

                    // special case for labels, never indent these.
                    if (line.ToUpper().StartsWith("("))
                        outputText.Add(new string(' ', tabLength * 0) + line);
                    // if the tag is null, output the current line with the current number of tabs
                    else if (tag == null)
                        outputText.Add(new string(' ', tabLength * numTabs) + line);
                    // if the tag is else, re-indent
                    else if (tag == "ELSE")
                        numTabs++;
                }

                // backup the current file
                DirectoryInfo backupDir = new DirectoryInfo(folderName).CreateSubdirectory("ModusArchive");
                string backupFileName = $"{backupDir}\\{dmisFile.Name.Replace(dmisFile.Extension, "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + dmisFile.Extension)}";
                dmisFile.CopyTo(backupFileName);

                // output the new file.
                using (StreamWriter sw = new StreamWriter(dmisFile.FullName))
                {
                    foreach (string line in outputText)
                        sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
        }
    }
}
