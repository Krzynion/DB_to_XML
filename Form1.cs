using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace XmlImportExport
{
    public partial class Form1 : Form
    {
        public delegate void DelDoLogu(String tekst, int typKomunikatu);
        private Dokumenty dokumentyBaza;
        private XmlDocument xmlDoc;
        private String _logFile = "LogFile.log";

        public Form1()
        {
            InitializeComponent();
            StreamWriter streamWriter = new StreamWriter(this._logFile, true);
            streamWriter.WriteLine("-------------------------------------------");
            streamWriter.Close();
                       
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            DoLogu(DateTime.Now.ToString(), 0);
            try
            {
                Inicjalizacja.Inicjalizuj();
                button1.Enabled = true;
                DateTime dt = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1);
                dt = dt - TimeSpan.FromDays(1);
                textBox1.Text = dt.Year.ToString();
                textBox2.Text = dt.Month.ToString("D2");
            }
            catch(Exception ex)
            {
                this.DoLogu(ex.Message, 2);
            }
        }

        public void DoLogu(String Tekst, int TypKomunikatu)
        // TypKomunikatu 0-Info, 1-Błąd, 2-Wyjątek Krytyczny 
        {
            if (Tekst == null)
                return;
            if ((TypKomunikatu < 0) || (TypKomunikatu > 2))
                return;
            Brush kolor = Brushes.Black;
            String[] teksty=null;
            if (TypKomunikatu == 0)
            {
                kolor = Brushes.Black;
                teksty = Tekst.Split('\n');
            }
            if(TypKomunikatu==1)
            {
                kolor = Brushes.Red;
                teksty = ("Błąd!:\n" + Tekst).Split('\n');
            }
            if (TypKomunikatu == 2)
            {
                kolor = Brushes.Red;
                teksty = ("Wyjątek Krytyczny!:\n" + Tekst+"\nDziałanie programu zotało przerwane!").Split('\n');
            }
            foreach (String tekst in teksty)
                listBox1.Items.Add(new Komunikat(tekst, kolor));
            listBox1.TopIndex = listBox1.Items.Count - 1;
            StreamWriter streamWriter = new StreamWriter(this._logFile, true);
            foreach (String tekst in teksty)
                streamWriter.WriteLine(tekst);
            streamWriter.Close();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Komunikat _komunikat = listBox1.Items[e.Index] as Komunikat;
                e.Graphics.DrawString(_komunikat.Tekst,e.Font, _komunikat.Kolor,new PointF(e.Bounds.X,e.Bounds.Y));
                SizeF roztext = e.Graphics.MeasureString(_komunikat.Tekst, e.Font);
                double a=Math.Ceiling(roztext.Width);
                if (a>listBox1.HorizontalExtent)
                    listBox1.HorizontalExtent= Convert.ToInt32(a);
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            dokumentyBaza = null;
            button2.Enabled = false;
            button3.Enabled = false;
            
            DoLogu("Otwieranie bazy danych SKP PRO", 0);
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            try
            {
                dokumentyBaza = LoadData.ReadDataFromDB(Inicjalizacja.ConnectionString, textBox1.Text,textBox2.Text, new DelDoLogu(DoLogu));
                this.Cursor = Cursors.Default;
                if(dokumentyBaza!=null)
                {
                    this.DoLogu("Ilość poprawnie odczytanych dokumentów: " + dokumentyBaza.Count.ToString(), 0);
                    button2.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                this.Cursor = Cursors.Default;
                DoLogu(ex.Message, 2);
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                xmlDoc = null;
                xmlDoc = dokumentyBaza.ConvertToXmlDocument();
                if(xmlDoc!=null)
                {
                    XmlElement root = xmlDoc.DocumentElement;
                    XmlNodeList faktury = root.SelectNodes("faktura");
                    if(faktury!=null)
                    {
                        int ilFaktur = faktury.Count;
                        this.DoLogu("Ilość przetworzonych dokumentów: " + ilFaktur.ToString(), 0);
                        if (ilFaktur > 0)
                            button3.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                DoLogu(ex.Message, 2);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = Inicjalizacja.SavePath;
            DateTime dt = (DateTime)dokumentyBaza[0].DataWystawienia;
            String rok = dt.Year.ToString();
            String miesiac = dt.Month.ToString("D2");
            String nazwaPliku = Inicjalizacja.SaveFileName.Replace("{rok}", rok);
            nazwaPliku = nazwaPliku.Replace("{miesiac}", miesiac);
            saveFileDialog1.FileName = nazwaPliku;
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
                return;
            String plik = saveFileDialog1.FileName;
            try
            {
                this.DoLogu("Zapisywanie pliku.", 0);
                if (xmlDoc != null)
                {
                    xmlDoc.Save(plik);
                    this.DoLogu("Plik:\n" + plik + "\nzostał zapisany!", 0);
                    button3.Enabled = false;
                    button2.Enabled = false;
                    MessageBox.Show("Plik  " + saveFileDialog1.FileName + "  został poprawnie zapisany.", "Plik zapisano", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    this.DoLogu("Brak danych do zapisania!", 1);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                DoLogu(ex.Message, 2);
            }
        }

       
    }

    class Komunikat
    {
        private String _tekst;
        private Brush _kolor;

        public Komunikat(String Tekst, Brush Kolor)
        {
            this._tekst = Tekst;
            this._kolor = Kolor;
        }

        public String Tekst
        {
            get { return _tekst; }
        }
        public Brush Kolor
        {
            get { return _kolor; }
        }
    }
}
