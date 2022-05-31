using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;

namespace XmlImportExport
{
//-----------------------------------------------------------------------------------------
    class Dokument
    {
        private String _kontrahent;
        private String _kontrahent1;
        private String _kontrahent2;
        private String _kontrahentTyp;
        private String _kontrahentNip;
        private String _kontrahentNipEu;
        private String _adrKontrahenta;
        private String _adrKontrKodMiejsc;
        private String _adrKontrUlNr;
        private String _rachunekBank;
        private String _numer;
        private String _typ;
        private Object _dataWyst;
        private Object _dataSprz;
        private Object _dataVat;
        private String _termin;
        private String _zaplata;
        private String _netto;
        private String _vat;
        private String _typPlatn;
        private String _kursWaluty;
        private String _nazwaWaluty;
        private String _tabelaWalut;
        private Object _dataTabeli;
        private String _uwagi;
        private DokumentPozycje _pozycje;
      
                  
        public Dokument()
        {
        this._kontrahent="";
        this._kontrahent1="*";
        this._kontrahent2="";
        this._kontrahentTyp="";
        this._kontrahentNip="";
        this._kontrahentNipEu="";
        this._adrKontrahenta = "";
        this._adrKontrKodMiejsc="";
        this._adrKontrUlNr="";
        this._rachunekBank="";
        this._numer="";
        this._typ="";
        this._dataWyst = null;
        this._dataSprz = null;
        this._dataVat=null;
        this._termin="";
        this._zaplata="";
        this._netto="";
        this._vat="";
        this._typPlatn="";
        this._kursWaluty="";
        this._nazwaWaluty="";
        this._tabelaWalut="";
        this._dataTabeli=null;
        this._uwagi="";
        this._pozycje=new DokumentPozycje(this);
        }

        public String Kontrahent
        {
            get
            {
                return this._kontrahent;
            }
            set
            {
                this._kontrahent1 = "*";
                this._kontrahent2 = "";
                this._kontrahent = value.Trim();
                if (this._kontrahent.Length > 0)
                {
                    String[] kontrahentTab= MyClass.Podziel(this._kontrahent, 2, 50);
                    this._kontrahent1 = kontrahentTab[0];
                    this._kontrahent2 = kontrahentTab[1];
                }
                
                /*
                
                if (this._kontrahent.Length > 100)
                    this._kontrahent = this._kontrahent.Remove(100);
                if (this._kontrahent.Length>50)
                {
                    this._kontrahent2 = this._kontrahent.Substring(50);
                    this._kontrahent1 = this._kontrahent.Substring(0, 50);
                }
                if ((this._kontrahent.Length > 0) && (this._kontrahent.Length <= 50))
                    this._kontrahent1 = this._kontrahent;
               */
            }
        }
        
        public String Kontrahent1
        {
            get
            {
                return this._kontrahent1;
            }
        } 

        public String Kontrahent2
        {
            get
            {
                return this._kontrahent2;
            }
        }
                
        public String KontrahentTyp
        {
            get
            {
                return this._kontrahentTyp;
            }
            set
            {
                this._kontrahentTyp = value;
            }
        }
                
        public String KontrahentNip
        {
            get
            {
                return this._kontrahentNip;
            }
            set
            {
                this._kontrahentNip = value;
                if (this._kontrahentNip.Length > 30)
                    this._kontrahentNip = this._kontrahentNip.Remove(30);
            }
        }

        public String KontrahentNipEu
        {
            get
            {
                return this._kontrahentNipEu;
            }
            set
            {
                this._kontrahentNipEu = value;
                if (this._kontrahentNipEu.Length > 30)
                    this._kontrahentNipEu = this._kontrahentNipEu.Remove(30);
            }
        }
       
        public String KontrahentAdres
        {
            get
            {
                return this._adrKontrahenta;
            }
            set
            {
                this._adrKontrKodMiejsc = "";
                this._adrKontrUlNr = "";
                this._adrKontrahenta = value.Trim();
                Match match = Regex.Match(this._adrKontrahenta, @"(\d{2}-\d{3}\s)|(\d{2}\s\d{3}\s)|(\d{5}\s)");
                if (match.Success)
                {
                    String kodPocz = match.Value.Trim();
                    String miejscowosc = this._adrKontrahenta.Substring(match.Index + match.Length);
                    miejscowosc = miejscowosc.Trim();
                    this._adrKontrKodMiejsc = kodPocz + " " + miejscowosc;
                    if (this._adrKontrKodMiejsc.Length > 50)
                        this._adrKontrKodMiejsc = this._adrKontrKodMiejsc.Remove(50);
                    this._adrKontrUlNr = this._adrKontrahenta.Remove(match.Index);
                    this._adrKontrUlNr = _adrKontrUlNr.Trim();
                    if (this._adrKontrUlNr[this._adrKontrUlNr.Length - 1] == ',')
                        this._adrKontrUlNr = this._adrKontrUlNr.Remove(this._adrKontrUlNr.Length - 1);
                    if (this._adrKontrUlNr.Length > 50)
                        this._adrKontrUlNr = this._adrKontrUlNr.Remove(50);
                }
                else
                {
                    if ((this._adrKontrahenta.Length > 0) && (this._adrKontrahenta.Length <= 50))
                        this._adrKontrKodMiejsc = this._adrKontrahenta;
                    if((this._adrKontrahenta.Length > 50) && (this._adrKontrahenta.Length <= 100))
                    {
                        this._adrKontrKodMiejsc = this._adrKontrahenta.Substring(0, 50);
                        this._adrKontrUlNr = this._adrKontrahenta.Substring(50);
                    }
                    if (this._adrKontrahenta.Length > 100)
                    {
                        this._adrKontrKodMiejsc = this._adrKontrahenta.Substring(0, 50);
                        this._adrKontrUlNr = this._adrKontrahenta.Substring(50,50);
                    }
                }
            }
        }

        public String KontrahentAdr1
        {
            get
            {
                return this._adrKontrKodMiejsc;
            }
            set
            {
                this._adrKontrKodMiejsc = value.Trim();
            }
        }

        public String KontrahentAdr2
        {
            get
            {
                return this._adrKontrUlNr;
            }
            set { this._adrKontrUlNr = value.Trim(); }
        }

        public String KontrahentRachBank
        {
            get
            {
                return this._rachunekBank;
            }
            set
            {
                this._rachunekBank = value.Trim();
                if (this._rachunekBank.Length > 40)
                    this._rachunekBank = this._rachunekBank.Remove(40);
            }
        }

        public String NumerDokumentu
        {
            get
            {
                return this._numer;
            }
            set
            {
                this._numer = value.Trim();
                if (this._numer.Length > 30)
                    this._numer = this._numer.Remove(30);
            }
        }

        public String Typ
        {
            get
            {
                return this._typ;
            }
            set
            {
                this._typ = value.Trim();
            }
        }

        public Object DataWystawienia
        {
            get
            {
                return this._dataWyst;            
            }
            set
            {
                this._dataWyst = value;
            }
        }
        
        public Object DataSprzedazy
        {
            get
            {
                return this._dataSprz;
            }
            set
            {
                this._dataSprz = value;
            }
        }
                
        public Object DataVat
        {
            get
            {
                return this._dataVat;
            }
            set
            {
                this._dataVat = value;
            }
        }

        public String Termin
        {
            get
            {
                return this._termin;
            }
            set
            {
                this._termin = value.Trim();
            }
        }

        public String Zaplata
        {
            get
            {
                return this._zaplata;
            }
            set
            {
                this._zaplata = value.Trim();
            }
        }

        public void SetZaplata(Double Zaplata)
        {
            this._zaplata = Zaplata.ToString("F2", CultureInfo.InvariantCulture);
        }

        public String Netto
        {
            get
            {
                return this._netto;
            }
            set
            {
                this._netto = value.Trim();
            }
        }

        public void SetNetto(Double Netto)
        {
            this._netto = Netto.ToString("F2", CultureInfo.InvariantCulture);
        }

        public String Vat
        {
            get
            {
                return this._vat;
            }
            set
            {
                this._vat = value.Trim();
            }
        }

        public void SetVat(Double Vat)
        {
            this._vat = Vat.ToString("F2", CultureInfo.InvariantCulture);
        }

        public String TypPlatnosci
        {
            get
            {
                return this._typPlatn;
            }
            set
            {
                this._typPlatn = value.Trim();
            }
        }

        public String KursWaluty
        {
            get
            {
                return this._kursWaluty;
            }
            set
            {
                this._kursWaluty = value.Trim();
            }
        }

        public String NazwaWaluty
        {
            get
            {
                return this._nazwaWaluty;
            }
            set
            {
                this._nazwaWaluty = value.Trim();
                if (this._nazwaWaluty.Length > 4)
                    this._nazwaWaluty = this._nazwaWaluty.Remove(4);
            }
        }

        public String TabelaWaluty
        {
            get
            {
                return this._tabelaWalut;
            }
            set
            {
                this._tabelaWalut = value.Trim();
                if (this._tabelaWalut.Length > 21)
                    this._tabelaWalut = this._tabelaWalut.Remove(21);
            }
        }

        public Object DataTabeli
        {
            get
            {
                return this._dataTabeli;
            }
            set
            {
                this._dataTabeli = value;
            }
        }

        public String Uwagi
        {
            get
            {
                return this._uwagi;
            }
            set
            {
                this._uwagi = value.Trim();
            }
        }

        public DokumentPozycje Pozycje
            
        {
            get
            {
                return this._pozycje;
            }
        }
    }

//---------------------------------------------------------------------------------------------
    class DokumentPozycja
    {
        private String _opis;
        private String _ilosc;
        private String _jm;
        private String _cena;
        private String _cenaWaluta;
        private String _kwotaVat;
        private String _stawkaVatId;
        private String _towar1;
        private String _towar2;
        private String _towar3;


        public DokumentPozycja()
        {
            this._opis = "";
            this._ilosc = "";
            this._jm = "";
            this._cena = "";
            this._cenaWaluta = "";
            this._kwotaVat = "";
            this._stawkaVatId = "";
            this._towar1 = "*";
            this._towar2 = "";
            this._towar3 = "";
        }

        public void SetIlosc(Double Ilosc)
        {
            this._ilosc = Ilosc.ToString("F4", CultureInfo.InvariantCulture);
        }

        public void SetCena(Double Cena)
        {
            this._cena = Cena.ToString("F4", CultureInfo.InvariantCulture);
        }

        public void SetCenaWaluta(Double Cena)
        {
            this._cenaWaluta = Cena.ToString("F4", CultureInfo.InvariantCulture);
        }

        public void SetVat(Double KwotaVat)
        {
            this._kwotaVat = KwotaVat.ToString("F2", CultureInfo.InvariantCulture);
        }
        
        public String Opis
        {
            get
            {
                return this._opis;
            }
            set
            {
                this._towar1 = "*";
                this._towar2 = "";
                this._towar3 = "";
                this._opis = value.Trim();
                if(this._opis.Length>0)
                {
                    String[] towarTab = MyClass.Podziel(this._opis, 3, 50);
                    this._towar1 = towarTab[0];
                    this._towar2 = towarTab[1];
                    this._towar3 = towarTab[2];
                }
                
                /*
                if (this._opis.Length > 150)
                {
                    this._towar3 = this._opis.Substring(100, 50);
                    this._towar2 = this._opis.Substring(50, 50);
                    this._towar1 = this._opis.Substring(0, 50);
                }
                if ((this._opis.Length<=150)&&(this._opis.Length>100))
                {
                    this._towar3 = this._opis.Substring(100);
                    this._towar2 = this._opis.Substring(50, 50);
                    this._towar1 = this._opis.Substring(0, 50);
                }
                if ((this._opis.Length <= 100) && (this._opis.Length > 50))
                {
                    this._towar2 = this._opis.Substring(50);
                    this._towar1 = this._opis.Substring(0, 50);
                }
                if ((this._opis.Length <= 50) && (this._opis.Length > 0))
                    this._towar1 = this._opis;
                */
            }
        }

        public String Ilosc
        {
            get
            {
                return this._ilosc;
            }
            set
            {
                this._ilosc = value.Trim();
            }
        }

        public String Jm
        {
            get
            {
                return this._jm;
            }
            set
            {
                this._jm = value.Trim();
                if (this._jm.Length > 10)
                    this._jm = this._jm.Remove(10);
            }
        }

        public String Cena
        {
            get
            {
                return this._cena;
            }
            set
            {
                this._cena = value.Trim();
            }
        }

        public String CenaWaluta
        {
            get
            {
                return this._cenaWaluta;
            }
            set
            {
                this._cenaWaluta = value;
            }
        }

        public String KwotaVat
        {
            get
            {
                return this._kwotaVat;
            }
            set
            {
                this._kwotaVat = value;
            }
        }

        public String StawkaVatID
        {
            get
            {
                return this._stawkaVatId;
            }
            set
            {
                this._stawkaVatId = value.Trim();
            }
        }
                        
        public String Towar1
        {
            get
            {
                return this._towar1;
            }
        }

        public String Towar2
        {
            get
            {
                return this._towar2;
            }
        }

        public String Towar3
        {
            get
            {
                return this._towar3;
            }
        }
    }
//----------------------------------------------------------------------------------------------------
    class DokumentPozycje:List<DokumentPozycja>
    {
        private Dokument owner;

        public DokumentPozycje(Dokument dokument)
        {
            this.owner = dokument;
            
        }
    }
//------------------------------------------------------------------------------------------------

    class Dokumenty:List<Dokument>
    {
        public Dokumenty()
        {
        }

        public XmlDocument ConvertToXmlDocument()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><dokumenty></dokumenty>");
            XmlElement root = xmlDoc.DocumentElement;
            for (int idF = 0; idF < this.Count; idF++)
            {
                XmlNode fakturaNode = xmlDoc.CreateNode(XmlNodeType.Element, "faktura", "");
                root.AppendChild(fakturaNode);
                XmlNode faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "kontrahent1", "");
                faktElNode.InnerText = this[idF].Kontrahent1;
                fakturaNode.AppendChild(faktElNode);
                if(this[idF].Kontrahent2.Length>0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "kontrahent2", "");
                    faktElNode.InnerText = this[idF].Kontrahent2;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].KontrahentTyp.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "kontrahentTyp", "");
                    faktElNode.InnerText = this[idF].KontrahentTyp;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].KontrahentNip.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "kontrahentNIP", "");
                    faktElNode.InnerText = this[idF].KontrahentNip;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].KontrahentNipEu.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "kontrahentNIPEURO", "");
                    faktElNode.InnerText = this[idF].KontrahentNipEu;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].KontrahentAdr1.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "kontrahentAdres1", "");
                    faktElNode.InnerText = this[idF].KontrahentAdr1;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].KontrahentAdr2.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "kontrahentAdres2", "");
                    faktElNode.InnerText = this[idF].KontrahentAdr2;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].KontrahentRachBank.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "kontrahentRachBank", "");
                    faktElNode.InnerText = this[idF].KontrahentRachBank;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].NumerDokumentu.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "numer", "");
                    faktElNode.InnerText = this[idF].NumerDokumentu;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].Typ.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "typ", "");
                    faktElNode.InnerText = this[idF].Typ;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].DataWystawienia is DateTime)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "data_wyst", "");
                    DateTime dt = (DateTime) this[idF].DataWystawienia;
                    faktElNode.InnerText = dt.ToString("dd.MM.yyyy");
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].DataSprzedazy is DateTime)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "data_sprzed", "");
                    DateTime dt = (DateTime) this[idF].DataSprzedazy;
                    faktElNode.InnerText = dt.ToString("dd.MM.yyyy");
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].DataVat is DateTime)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "data_vat", "");
                    DateTime dt = (DateTime)this[idF].DataVat;
                    faktElNode.InnerText = dt.ToString("dd.MM.yyyy");
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].Termin.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "termin", "");
                    faktElNode.InnerText = this[idF].Termin;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].Zaplata.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "zaplata", "");
                    faktElNode.InnerText = this[idF].Zaplata;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].Netto.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "netto", "");
                    faktElNode.InnerText = this[idF].Netto;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].Vat.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "vat", "");
                    faktElNode.InnerText = this[idF].Vat;
                    fakturaNode.AppendChild(faktElNode);
                }
                faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "sumyDok", "");
                faktElNode.InnerText = "1";
                fakturaNode.AppendChild(faktElNode);
                if (this[idF].TypPlatnosci.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "typ_platn", "");
                    faktElNode.InnerText = this[idF].TypPlatnosci;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].KursWaluty.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "kurs_waluty", "");
                    faktElNode.InnerText = this[idF].KursWaluty;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].NazwaWaluty.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "nazwa_waluty", "");
                    faktElNode.InnerText = this[idF].NazwaWaluty;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].TabelaWaluty.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "tabela_walut", "");
                    faktElNode.InnerText = this[idF].TabelaWaluty;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].DataTabeli is DateTime)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "data_tabeli", "");
                    DateTime dt = (DateTime)this[idF].DataTabeli;
                    faktElNode.InnerText = dt.ToString("dd.MM.yyyy");
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].Uwagi.Length > 0)
                {
                    faktElNode = xmlDoc.CreateNode(XmlNodeType.Element, "uwagi", "");
                    faktElNode.InnerText = this[idF].Uwagi;
                    fakturaNode.AppendChild(faktElNode);
                }
                if (this[idF].Pozycje.Count>0)
                {
                    DokumentPozycje dokPozycje = this[idF].Pozycje;
                    XmlNode pozycjeElNode = xmlDoc.CreateNode(XmlNodeType.Element, "pozycje", "");
                    fakturaNode.AppendChild(pozycjeElNode);
                    for(int idFP=0;idFP<dokPozycje.Count;idFP++)
                    {
                        XmlNode pozycjaNode = xmlDoc.CreateNode(XmlNodeType.Element, "pozycja","");
                        pozycjeElNode.AppendChild(pozycjaNode);
                        XmlNode pozycjaElNode = xmlDoc.CreateNode(XmlNodeType.Element, "towar1","");
                        pozycjaElNode.InnerText = dokPozycje[idFP].Towar1;
                        pozycjaNode.AppendChild(pozycjaElNode);
                        if(dokPozycje[idFP].Towar2.Length>0)
                        {
                            pozycjaElNode = xmlDoc.CreateNode(XmlNodeType.Element, "towar2", "");
                            pozycjaElNode.InnerText = dokPozycje[idFP].Towar2;
                            pozycjaNode.AppendChild(pozycjaElNode);
                        }
                        if (dokPozycje[idFP].Towar3.Length > 0)
                        {
                            pozycjaElNode = xmlDoc.CreateNode(XmlNodeType.Element, "towar3", "");
                            pozycjaElNode.InnerText = dokPozycje[idFP].Towar3;
                            pozycjaNode.AppendChild(pozycjaElNode);
                        }
                        pozycjaElNode = xmlDoc.CreateNode(XmlNodeType.Element, "towarBezEwid", "");
                        pozycjaElNode.InnerText = "1";
                        pozycjaNode.AppendChild(pozycjaElNode);
                        if (dokPozycje[idFP].Jm.Length > 0)
                        {
                            pozycjaElNode = xmlDoc.CreateNode(XmlNodeType.Element, "towarJm", "");
                            pozycjaElNode.InnerText = dokPozycje[idFP].Jm;
                            pozycjaNode.AppendChild(pozycjaElNode);
                        }
                        if (dokPozycje[idFP].Ilosc.Length > 0)
                        {
                            pozycjaElNode = xmlDoc.CreateNode(XmlNodeType.Element, "ilosc", "");
                            pozycjaElNode.InnerText = dokPozycje[idFP].Ilosc;
                            pozycjaNode.AppendChild(pozycjaElNode);
                        }
                        if (dokPozycje[idFP].Cena.Length > 0)
                        {
                            pozycjaElNode = xmlDoc.CreateNode(XmlNodeType.Element, "cena", "");
                            pozycjaElNode.InnerText = dokPozycje[idFP].Cena;
                            pozycjaNode.AppendChild(pozycjaElNode);
                        }
                        if (dokPozycje[idFP].CenaWaluta.Length > 0)
                        {
                            pozycjaElNode = xmlDoc.CreateNode(XmlNodeType.Element, "cenaWaluta", "");
                            pozycjaElNode.InnerText = dokPozycje[idFP].CenaWaluta;
                            pozycjaNode.AppendChild(pozycjaElNode);
                        }
                        if (dokPozycje[idFP].KwotaVat.Length > 0)
                        {
                            pozycjaElNode = xmlDoc.CreateNode(XmlNodeType.Element, "vat", "");
                            pozycjaElNode.InnerText = dokPozycje[idFP].KwotaVat;
                            pozycjaNode.AppendChild(pozycjaElNode);
                        }
                        if (dokPozycje[idFP].StawkaVatID.Length > 0)
                        {
                            pozycjaElNode = xmlDoc.CreateNode(XmlNodeType.Element, "stawkaVat", "");
                            pozycjaElNode.InnerText = dokPozycje[idFP].StawkaVatID;
                            pozycjaNode.AppendChild(pozycjaElNode);
                        }
                    }
                }
            }
            return xmlDoc;
        }
    }

//-------------------------------------------------------------------------------------------------
    class StawkaVat
    {
        private int _id;
        private String _vat;
        private String _opis;

        public StawkaVat(int Id, String Vat, String Opis)
        {
            this._id = Id;
            this._vat = Vat;
            this._opis = Opis;
        }

        public int Id
        {
            get
            {
                return this._id;
            }
        }

        public String Vat
        {
            get
            {
                return this._vat;
            }
        }

        public String Opis
        {
            get
            {
                return this._opis;
            }
        }
    }
//-------------------------------------------------------------------------------------------------
    static class Inicjalizacja
    {
        private static StawkaVat[] _StawkiVat;
        private static String _connectionString;
        private static String _savePath="";
        private static String _saveFileName="";
        
        public static StawkaVat[] StawkaVat
        {
            get { return _StawkiVat; }
        }

        public static String ConnectionString
        {
            get { return _connectionString; }
        }

        public static String SavePath
        {
            get { return _savePath; }
        }

        public static String SaveFileName
        {
            get { return _saveFileName; }
        }

        public static void Inicjalizuj()
        {
            //ZaladowanieStawekVat();
            ZaladowanieConfigXml();
        }


                /* Załadowanie Stawek Vat z pliku StawkiVat.xml */
        private static void ZaladowanieStawekVat()
        {
            XmlDocument dokVat = new XmlDocument();
            dokVat.Load("StawkiVat.xml");
            XmlElement root = dokVat.DocumentElement;
            XmlNodeList stawkiVat= root.SelectNodes("Stawka");
            _StawkiVat = new StawkaVat[stawkiVat.Count];
            for(int x=0;x<stawkiVat.Count;x++)
            {
                XmlNode _stVat = stawkiVat[x];
                int _idVat = int.Parse(_stVat.Attributes["Id"].Value);
                String _vat=_stVat.SelectSingleNode("Vat").InnerText;
                String _opis = _stVat.SelectSingleNode("Opis").InnerText;
                StawkaVat stawkaVat = new StawkaVat(_idVat, _vat.Trim(), _opis.Trim());
                _StawkiVat[x] = stawkaVat;
            }
        }
           // Załadowanie piku config.xml
        private static void ZaladowanieConfigXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("config.xml");
            XmlElement root = doc.DocumentElement;
            XmlNode xmlEl = root.SelectSingleNode("connectionString");
            _connectionString = xmlEl.InnerText.Trim();
            xmlEl = root.SelectSingleNode("savePath");
            if (xmlEl != null)
                _savePath = xmlEl.InnerText.Trim();
            xmlEl = root.SelectSingleNode("saveFileName");
            if (xmlEl != null)
                _saveFileName = xmlEl.InnerText.Trim();
        }
    }
 //-----------------------------------------------------------------------------------------------

    public static class MyClass
    {
        public static String[] Podziel(String Text, int ile, int dlugosc)
        {
            if (ile < 2 || dlugosc < 2)
                return null;
            String[] wynik = new String[ile];
            for (int x = 0; x < wynik.Length; x++)
                wynik[x] = "";
            String text = Text.Trim();
            for (int nrW = 0; nrW < ile; nrW++)
            {
                if (text.Length > dlugosc)
                {
                    if (nrW < ile - 1)
                    {
                        Boolean spaceFind = false;
                        for (int x = dlugosc; x > 0; x--)
                        {
                            Char znak = text[x];
                            if (znak == ' ')
                            {
                                wynik[nrW] = text.Substring(0, x);
                                text = text.Remove(0, x + 1);
                                spaceFind = true;
                                break;
                            }
                        }
                        if (!spaceFind)
                        {
                            wynik[nrW] = text.Substring(0, dlugosc);
                            text = text.Remove(0, dlugosc);
                        }
                    }
                    else
                    {
                        wynik[nrW] = text.Substring(0, dlugosc);
                        wynik[nrW] = wynik[nrW].TrimEnd();
                    }
                }
                else
                {
                    wynik[nrW] = text;
                    break;
                }
            }
            return wynik;
        }
        /*
        public static String[] Podziel(String Text, int ile, int dlugosc)
        {
            if (ile < 2 || dlugosc < 2)
                return null;
            String[] wynik = new String[ile];
            for (int x = 0; x < wynik.Length; x++)
                wynik[x] = "";
            String pozostalyText = Text.Trim();
            if (pozostalyText.Length <= dlugosc)
            {
                wynik[0] = pozostalyText;
                return wynik;
            }
            for (int x = 0; x < ile; x++)
            {
                pozostalyText = pozostalyText.Trim();
                if (pozostalyText.Length > dlugosc + 1)
                    wynik[x] = pozostalyText.Substring(0, dlugosc + 1);
                else
                    wynik[x] = pozostalyText;
                if (x < ile - 1)
                {
                    for (int a = wynik[x].Length - 1; a > 0; a--)
                    {
                        Char t1 = wynik[x][a];
                        Char t0 = wynik[x][a - 1];
                        if ((t1 == ' ') && (t0 != ' '))
                        {
                            wynik[x] = wynik[x].Remove(a);
                            break;
                        }
                    }
                }
                if (wynik[x].Length > dlugosc)
                    wynik[x] = wynik[x].Remove(dlugosc);
                pozostalyText = pozostalyText.Substring(wynik[x].Length);
                if (pozostalyText.Length == 0)
                    break;
            }
            return wynik;
        }*/
    }
}

