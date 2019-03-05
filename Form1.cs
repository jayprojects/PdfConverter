using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;

namespace BatchPdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        String inputFolder;
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult r =  folderBrowserDialog1.ShowDialog();
            inputFolder = folderBrowserDialog1.SelectedPath;
            if (Directory.Exists(inputFolder))
            {
                richTextBox1.AppendText("Input folder : " + inputFolder);
                comboBox1.Enabled = true;

            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
        String outputFilePath;
        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            
            if (saveFileDialog1.CheckPathExists)
            {
                buttonConvert.Enabled = true;
                outputFilePath = saveFileDialog1.FileName;
                richTextBox1.AppendText("\noutput file path : " + outputFilePath);
            }

        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {



            string str = "";
            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = "TXT to PDF";
            XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
            try
            {
                foreach (String file in Files)
                {
                    string line = null;
                    System.IO.TextReader readFile = new StreamReader(file);
                    int yPoint = 20;
                    PdfPage pdfPage = pdf.AddPage();
                    XGraphics graph = XGraphics.FromPdfPage(pdfPage);

                    graph.DrawString(file, font, XBrushes.Black, new XRect(18, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    yPoint = yPoint + 15;
                    graph.DrawString("--------------------------------------------", font, XBrushes.Black, new XRect(18, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    yPoint = yPoint + 30;
                    

                    int lineCount = 1;
                    while (true)
                    {
                        line = readFile.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        else
                        {
                            graph.DrawString(line, font, XBrushes.Black, new XRect(18, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            yPoint = yPoint + 18;
                        }
                        if (lineCount++ > 38)
                        {
                            yPoint = 20;
                            lineCount = 1;
                            pdfPage = pdf.AddPage();
                            graph = XGraphics.FromPdfPage(pdfPage);
                        }
                    }
                    readFile.Close();
                    readFile = null;

                }

                
                pdf.Save(outputFilePath);

                Process.Start(outputFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        String fileExtension;
        String[] Files;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().Contains("*."))
            {
                fileExtension = comboBox1.SelectedItem.ToString();
                richTextBox1.AppendText("\nSelected file Extension : " + inputFolder);

                //DirectoryInfo d = new DirectoryInfo(inputFolder);
                //Files = d.GetFiles(fileExtension);
                Files = Directory.GetFiles(inputFolder, fileExtension , SearchOption.AllDirectories);
                if (Files.Length > 0)
                {
                    richTextBox1.AppendText("\n "+Files.Length +" " + fileExtension + " files found in the input folder ");
                    buttonSaveAs.Enabled = true;
                }
                else
                {
                    richTextBox1.AppendText("\n no " + fileExtension + " file found in the input folder ");
                }
            }
        }
    }
}
