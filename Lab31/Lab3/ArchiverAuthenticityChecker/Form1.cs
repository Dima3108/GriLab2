using CryptoLibrary;
using Hardware.Info;
using LibraryValidator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArchiverAuthenticityChecker.Exceptions;
using System.Runtime.CompilerServices;
namespace ArchiverAuthenticityChecker
{
    public partial class Form1 : Form
    {
        private string v1, v2;
        private CryptoEncoder cryptoEncoder;
        private CryptoDecoder cryptoDecoder;
        private ValuesValidator valuesValidator;
        private HashGenerator hashGenerator;
        public Form1()
        {
            var hardinf = new HardwareInfo();
            hardinf.RefreshNetworkAdapterList();
            v1 = hardinf.NetworkAdapterList[0].MACAddress;
            hardinf.RefreshCPUList(); 
            v2 =
                //hardinf.NetworkAdapterList[0].IPAddressList[0].ToString();
                hardinf.CpuList[0].MaxClockSpeed.ToString();
            cryptoEncoder=new CryptoEncoder(UnicodeEncoding.Unicode.GetBytes(v1));
            cryptoDecoder = new CryptoDecoder(UnicodeEncoding.Unicode.GetBytes(v1));
            valuesValidator = new ValuesValidator(v1, v2);
            hashGenerator=new HashGenerator(v1 , v2);
#if debug
            Console.WriteLine(valuesValidator.ValidHash(hashGenerator.Hash_));
#endif
            InitializeComponent();
        }
        private static void CreateArhiveFromFile(Stream iStream,Stream oStream,CryptoEncoder cryptoEncoder,HashGenerator hashGenerator)
        {
            throw new NotImplementedException();    
        }
        public const string fileEx = ".mysa";

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                //Создание архива
                if (File.Exists(openFileDialog1.FileName))
                {
                    using(Stream file_=File.Open(openFileDialog1.FileName,FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using(Stream ofile = File.OpenWrite(folderBrowserDialog1.SelectedPath+"/"+openFileDialog1.SafeFileName+fileEx))
                        {
                            using(IO.OutputStream oStr=new IO.OutputStream(ofile, cryptoEncoder, hashGenerator))
                            {
                                file_.CopyTo(oStr);
                                oStr.Flush();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Отсутсвует обрабатываемый файл!");
                }
            }
            else
            {
                if (File.Exists(openFileDialog1.FileName))
                {
                    Stream file_ = null;
                    Stream ofile = null;
                    try
                    {
                        using (file_ = File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (ofile = File.OpenWrite(folderBrowserDialog1.SelectedPath + "/" + openFileDialog1.SafeFileName.Replace(fileEx, "")))
                            {
                                using (IO.InputStream iStr = new IO.InputStream(file_, cryptoDecoder, valuesValidator))
                                {
                                    iStr.CopyTo(ofile);
                                    ofile.Flush();
                                }
                            }
                        }
                    }
                    catch (ComputerMismatchException errror)
                    {
                        MessageBox.Show("файл упакован на другом компьютере!");
                        //return;
                    }
                    catch (CorruptedSourceData error)
                    {
                        MessageBox.Show("данные повреждены!");
                        //return;
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show($"непредвиденная ошибка!{err.Message}!");
                    }
                    finally
                    {
                        if (file_ != null)
                        {
                            file_.Close();
                        }
                        if (ofile != null)
                        {
                            ofile.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Отсутсвует обрабатываемый файл!");
                }
            }
            MessageBox.Show("Обработка завершена!");
        }

        private void buttonSavePath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            switch (folderBrowserDialog1.ShowDialog())
            {
                case DialogResult.OK:
                    textBox2.Text = folderBrowserDialog1.SelectedPath;
                    break;
                case DialogResult.Yes:
                    textBox2.Text = folderBrowserDialog1.SelectedPath;
                    break;
            }
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                //Создать архив
                openFileDialog1.Multiselect = false;
                openFileDialog1.Filter = "";
               switch(openFileDialog1.ShowDialog())
                {
                    case DialogResult.OK:
                        textBox1.Text= openFileDialog1.FileName;
                        break;
                    case DialogResult.Yes:
                        textBox1.Text = openFileDialog1.FileName;
                        break;
                }
                
                //saveFileDialog1.AddExtension = true;
                //saveFileDialog1.DefaultExt = fileEx;
                //saveFileDialog1.Filter = "Archive *" + fileEx + "|";
               // CreateArhiveFromFile(File.OpenRead(openFileDialog1.FileName), File.OpenWrite(saveFileDialog1.FileName), cryptoEncoder, hashGenerator);
            }
            else
            {
                openFileDialog1.Multiselect = false;
                openFileDialog1.Filter= "Archive |*" + fileEx + "";
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.CheckPathExists = true; 
                openFileDialog1.SupportMultiDottedExtensions = true;
                switch (openFileDialog1.ShowDialog())
                {
                    case DialogResult.OK:
                        textBox1.Text = openFileDialog1.FileName;
                        break;
                    case DialogResult.Yes:
                        textBox1.Text = openFileDialog1.FileName;
                        break;
                }
                //Распоковать архив
            }
           
        }
    }
}
