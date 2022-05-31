using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.Odbc;
using System.Data;

namespace XmlImportExport
{
    class LoadData
    {
        static public Dokumenty ReadDataFromDB(String ConnectionString, String Rok, String Miesiac, Form1.DelDoLogu DoLogu)
        {
            Dokumenty dokumenty=null;
            int rok;
            int miesiac;
            try
            {
                rok = Int32.Parse(Rok.Trim());
                if((rok<2010)||(rok>DateTime.Now.Year))
                {
                    DoLogu("Rok musi być z przedziału 2010 - " + DateTime.Now.Year.ToString() + ".",1);
                    return null;
                }
                miesiac = Int32.Parse(Miesiac.Trim());
                if((miesiac<1)||(miesiac>12))
                {
                    DoLogu("Miesiąc musi być z przedziału 1 - 12.", 1);
                    return null;
                }
            }
            catch
            {
                DoLogu("Okres powinien być wpisany w formacie: RRRR / MM .", 1);
                return null;
            }
            DoLogu("Pobieranie danych za okres: " + rok.ToString() + " / " + miesiac.ToString("D2") + ".",0);
            DateTime dateB = new DateTime(rok, miesiac, 1);
            DateTime dateE = new DateTime(rok, miesiac, DateTime.DaysInMonth(rok, miesiac));
            DataTable faktura = new DataTable("Faktura");
            DataTable fakturaPozycje = new DataTable("Pozycje");
            OdbcConnection connection = new OdbcConnection(ConnectionString);
            try
            {
                connection.Open();
                OdbcDataAdapter dataAdapter = new OdbcDataAdapter();
                dataAdapter.SelectCommand = new OdbcCommand();
                dataAdapter.SelectCommand.Connection = connection;
                dataAdapter.SelectCommand.CommandText = "SELECT * FROM dokument LEFT OUTER JOIN odbiorca ON dokument.ID_odbiorca=odbiorca.ID "
                    + "WHERE (dokument.Typ_dokumentu = 1 OR dokument.Typ_dokumentu = 2) AND dokument.D_AKTUALNY=true "
                 + "AND dokument.Data_wystawienia between{d'" + dateB.ToShortDateString() + "'}and{d'" + dateE.ToShortDateString() + "'}";
                dataAdapter.Fill(faktura);
                dataAdapter.SelectCommand.CommandText = "SELECT * FROM (dokument INNER JOIN sprzedaz ON dokument.ID=sprzedaz.ID_dokument) INNER JOIN magazyn ON sprzedaz.ID_towar=magazyn.M_ID "
                    + "WHERE (dokument.Typ_dokumentu=1 OR dokument.Typ_dokumentu=2) AND dokument.D_AKTUALNY=true "
                    + "AND dokument.Data_wystawienia between{d'" + dateB.ToShortDateString() + "'}and{d'" + dateE.ToShortDateString() + "'}";
                dataAdapter.Fill(fakturaPozycje);
                connection.Close();
            }
            catch(Exception ex)
            {
                connection.Close();
                DoLogu(ex.Message, 2);
                return null;
            }
            if(faktura.Rows.Count==0)
            {
                DoLogu("Brak dokumentów do pobrania.\nSprawdz czy wartość Okresu za jaki mają być pobrane dokumenty jest poprawnie wpisana.", 1);
                return null;
            }
            DoLogu("W podanym okresie zostało odnalezionych " + faktura.Rows.Count.ToString() + " dokumentów", 0);
            dokumenty = new Dokumenty();
            foreach (DataRow dokumentZB in faktura.Rows)
            {
                Dokument dokument = new Dokument();
                //Nr Faktury
                Object wartosc = dokumentZB["Nr_dokumentu"];
                if(!(wartosc is System.String))
                    continue;
                dokument.NumerDokumentu = wartosc.ToString();
                if (dokument.NumerDokumentu.Length == 0)
                    continue;
                //Nazwa Kontrahenta
                wartosc = dokumentZB["Firma"];
                if (wartosc is System.String)
                    dokument.Kontrahent = wartosc.ToString();
                //Adres kontrahenta
                wartosc = dokumentZB["Kod_pocztowy"];
                if (wartosc is System.String)
                    dokument.KontrahentAdr1 = wartosc.ToString();
                wartosc = dokumentZB["Miasto"];
                if (wartosc is System.String)
                    dokument.KontrahentAdr1 += " "+wartosc.ToString();
                wartosc = dokumentZB["Ulica"];
                if (wartosc is System.String)
                    dokument.KontrahentAdr2 = wartosc.ToString();
                wartosc = dokumentZB["Nr_posesji"];
                if (wartosc is System.String)
                    dokument.KontrahentAdr2 += " " + wartosc.ToString();
                wartosc = dokumentZB["Nr_lokalu"];
                if (wartosc is System.String)
                    dokument.KontrahentAdr2 += "/" + wartosc.ToString();
                //NIP
                wartosc = dokumentZB["NIP"];
                if (wartosc is System.String)
                {
                    String nrNip = wartosc.ToString().Trim();
                    if (nrNip.Length > 0)
                    {
                        dokument.KontrahentNip = nrNip;
                        dokument.KontrahentTyp = "1";
                        /*
                        Match match = Regex.Match(nrNip, @"\A([A-Z]|[a-z]){2}");
                        if (match.Success)
                        {
                            if (match.Value.ToUpper() == "PL")
                            {
                                dokument.KontrahentNip = nrNip;
                                dokument.KontrahentTyp = "1";
                            }
                            else
                            {
                                dokument.KontrahentNipEu = nrNip;
                                dokument.KontrahentTyp = "2";
                            }
                        }
                        else
                        {
                            dokument.KontrahentNip = nrNip;
                            dokument.KontrahentTyp = "1";
                        }
                        */
                    }
                    else
                        dokument.KontrahentTyp = "3";
                }
                else
                    dokument.KontrahentTyp = "3";
                //Rachunek Bankowy
                wartosc = dokumentZB["Rachunek"];
                if (wartosc is System.String)
                    dokument.KontrahentRachBank = wartosc.ToString();
                //Data wystawienia
                wartosc = dokumentZB["Data_wystawienia"];
                if (wartosc is System.DateTime)
                    dokument.DataWystawienia = (DateTime)wartosc;
                //Data sprzedazy
                wartosc = dokumentZB["Data_sprzedazy"];
                if (wartosc is System.DateTime)
                    dokument.DataSprzedazy = (DateTime)wartosc;
                //Data Vat
                wartosc = dokumentZB["Data_sprzedazy"];
                if (wartosc is System.DateTime)
                    dokument.DataVat = (DateTime)wartosc;
                //Typ
                dokument.Typ = "2";
                //Termin platnosci
                int terPlat = 0;
                if(dokument.DataWystawienia is System.DateTime)
                {
                    wartosc = dokumentZB["Termin_platnosci"];
                    if(wartosc is System.DateTime)
                    {
                        DateTime dataPlat = (DateTime)wartosc;
                        DateTime dataWyst = (DateTime)dokument.DataWystawienia;
                        TimeSpan ts =dataPlat - dataWyst;
                        terPlat = ts.Days;
                    }
                }
                dokument.Termin = terPlat.ToString();
                //Typ Platnosci
                wartosc = dokumentZB["Karta_platnicza"];
                if (wartosc is System.Boolean)
                {
                    Boolean bo = (Boolean)wartosc;
                    if (bo)
                        dokument.TypPlatnosci = "5";
                }
                if (dokument.TypPlatnosci.Length == 0)
                {
                    wartosc = dokumentZB["Gotowka"];
                    if (wartosc is System.Boolean)
                    {
                        Boolean bo = (Boolean)wartosc;
                        if (bo)
                            dokument.TypPlatnosci = "1";
                    }
                }
                if (dokument.TypPlatnosci.Length == 0)
                {
                    wartosc = dokumentZB["Przelew"];
                    if (wartosc is System.Boolean)
                    {
                        Boolean bo = (Boolean)wartosc;
                        if (bo)
                            dokument.TypPlatnosci = "2";
                    }
                }
                dokumenty.Add(dokument);
            }
            // Pozycje dokumentu
            foreach(Dokument dokument in dokumenty)
            {
                Double nettoSuma = 0;
                Double vatSuma = 0;
                foreach (DataRow pozycjaFakt in fakturaPozycje.Rows)
                {
                    Object wartosc = pozycjaFakt["Nr_dokumentu"];
                    if(wartosc is String)
                    {
                        String nrFaktury = wartosc.ToString().Trim();
                        if (nrFaktury==dokument.NumerDokumentu)
                        {
                            DokumentPozycja dokumentPozycja = new DokumentPozycja();
                            //Towar
                            wartosc = pozycjaFakt["M_Nazwa_towaru"];
                            if (wartosc is String)
                                dokumentPozycja.Opis = wartosc.ToString();
                            //Jm
                            wartosc = pozycjaFakt["M_Jm"];
                            if (wartosc is String)
                                dokumentPozycja.Jm = wartosc.ToString();
                            //Ilosc
                            Double ilosc=0;
                            wartosc = pozycjaFakt["Ilosc"];
                            if (wartosc is Double)
                                ilosc = (Double) wartosc;
                            dokumentPozycja.SetIlosc(ilosc);
                            //Vat
                            Double vat = 0;
                            String vatText = "";
                            dokumentPozycja.StawkaVatID = "7";
                            wartosc = pozycjaFakt["Stawka_VAT"];
                            if (wartosc is String)
                                vatText = wartosc.ToString().Trim();
                            if (Regex.IsMatch(vatText,"zw|ZW|zW|Zw"))
                            {
                                vat = 0;
                                dokumentPozycja.StawkaVatID = "0";
                            }
                            if(vatText=="0")
                            {
                                vat = 0;
                                dokumentPozycja.StawkaVatID = "2";
                            }
                            if(vatText=="23")
                            {
                                vat =0.23;
                                dokumentPozycja.StawkaVatID = "6";
                            }
                            if(vatText=="8")
                            {
                                vat =0.08;
                                dokumentPozycja.StawkaVatID = "4";
                            }
                            if(vatText=="5")
                            {
                                vat =0.05;
                                dokumentPozycja.StawkaVatID = "8";
                            }
                            //Cena
                            Double cenaBrutto = 0;
                            wartosc = pozycjaFakt["Cena_jednostkowa"];
                            if (wartosc is Double)
                                cenaBrutto = (Double)wartosc;
                            Double cenaNetto = cenaBrutto / (1+vat);
                            dokumentPozycja.SetCena(cenaNetto);
                            //Kwota VAT
                            Double wartoscNetto = ilosc * cenaNetto;
                            Double kwotaVat = wartoscNetto * vat;
                            dokumentPozycja.SetVat(kwotaVat);
                            dokument.Pozycje.Add(dokumentPozycja);
                            nettoSuma += wartoscNetto;
                            vatSuma += kwotaVat;
                        }
                    }
                }
                dokument.SetNetto(nettoSuma);
                dokument.SetVat(vatSuma);
                dokument.SetZaplata(nettoSuma + vatSuma);
            }
            return dokumenty; 
        }
    }
}

/*
        static public Dokumenty ReadDataFromFile(String FileName, Form1.DelDoLogu DoLogu)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FileName);
            XmlElement root = xmlDoc.DocumentElement;
            if(root.Name!="tns:JPK")
            {
                DoLogu("Otwierany plik nie jest prawidłowym plikiem JPK_FA.\nOtwórz prawidłowy plik JPK_FA.xml", 1);
                return null;
            }
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("tns", "http://jpk.mf.gov.pl/wzor/2016/03/09/03095/");
            XmlNodeList dokumentyXml = root.SelectNodes("tns:Faktura",nsmgr);
            if(dokumentyXml.Count==0)
            {
                DoLogu("Plik nie zawiera dokumentów do załadowania lub format pliku jest nieprawidłowy."
                        + "\nOtwórz prawidłowy plik JPK_FA.xml", 1);
                return null;
            }
            DoLogu("Ilość znalezionych dokumentów w pliku: " + dokumentyXml.Count.ToString(), 0);
            Dokumenty dokumentyBaza = new Dokumenty();
            foreach (XmlNode dokumentXml in dokumentyXml)
            {
                XmlNode rodzFakt = dokumentXml.SelectSingleNode("tns:RodzajFaktury", nsmgr);
                if (rodzFakt.InnerText == "VAT")
                {
                    Dokument nowyDok = new Dokument();
                    XmlNode elFaktury = null;
                    //Kontrahent
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_3A", nsmgr);
                    if (elFaktury != null)
                        nowyDok.Kontrahent = elFaktury.InnerText;
                    //NIP
                    String kodKraj = "";
                    String nr_Nip = "";
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_5A", nsmgr);
                    if (elFaktury != null)
                        kodKraj = elFaktury.InnerText;
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_5B", nsmgr);
                    if (elFaktury != null)
                        nr_Nip = elFaktury.InnerText;
                    String nrNipCaly = kodKraj + nr_Nip;
                    if (nrNipCaly.Length > 0)
                    {
                        if ((kodKraj.Length > 0) && (kodKraj != "PL"))
                        {
                            nowyDok.KontrahentNipEu = nrNipCaly;
                            nowyDok.KontrahentTyp = "2";   //Kontrahent_Typ - EU
                        }
                        else
                        {
                            nowyDok.KontrahentNip = nrNipCaly;
                            nowyDok.KontrahentTyp = "1"; //Kontrahent_Typ - Krajowy}
                        }
                    }
                    //Nr_Dokumentu
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_2A", nsmgr);
                    if (elFaktury != null)
                        nowyDok.NumerDokumentu = elFaktury.InnerText;
                    //Adres
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_3B", nsmgr);
                    if (elFaktury != null)
                        nowyDok.KontrahentAdres = elFaktury.InnerText;
                    //Data_wystawienia
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_1", nsmgr);
                    if (elFaktury != null)
                        nowyDok.DataWystawienia = elFaktury.InnerText;
                    //Data_sprzedazy
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_6", nsmgr);
                    if (elFaktury != null)
                        nowyDok.DataSprzedazy = elFaktury.InnerText;
                    //Netto
                    double nettoSuma = 0;
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_13_1", nsmgr);
                    if (elFaktury != null)
                    {
                        double netto = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        nettoSuma += netto;
                    }
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_13_2", nsmgr);
                    if (elFaktury != null)
                    {
                        double netto = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        nettoSuma += netto;
                    }
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_13_3", nsmgr);
                    if (elFaktury != null)
                    {
                        double netto = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        nettoSuma += netto;
                    }
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_13_4", nsmgr);
                    if (elFaktury != null)
                    {
                        double netto = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        nettoSuma += netto;
                    }
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_13_5", nsmgr);
                    if (elFaktury != null)
                    {
                        double netto = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        nettoSuma += netto;
                    }
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_13_6", nsmgr);
                    if (elFaktury != null)
                    {
                        double netto = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        nettoSuma += netto;
                    }
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_13_7", nsmgr);
                    if (elFaktury != null)
                    {
                        double netto = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        nettoSuma += netto;
                    }
                    nowyDok.Netto = nettoSuma.ToString("F", NumberFormatInfo.InvariantInfo);
                    //Vat
                    double vatSuma = 0;
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_14_1", nsmgr);
                    if (elFaktury != null)
                    {
                        double vat = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        vatSuma += vat;
                    }
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_14_2", nsmgr);
                    if (elFaktury != null)
                    {
                        double vat = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        vatSuma += vat;
                    }
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_14_3", nsmgr);
                    if (elFaktury != null)
                    {
                        double vat = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        vatSuma += vat;
                    }
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_14_4", nsmgr);
                    if (elFaktury != null)
                    {
                        double vat = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        vatSuma += vat;
                    }
                    elFaktury = dokumentXml.SelectSingleNode("tns:P_14_5", nsmgr);
                    if (elFaktury != null)
                    {
                        double vat = Double.Parse(elFaktury.InnerText.Trim(), NumberFormatInfo.InvariantInfo);
                        vatSuma += vat;
                    }
                    nowyDok.Vat = vatSuma.ToString("F", NumberFormatInfo.InvariantInfo);
                    // Typ - Faktura VAT
                    nowyDok.Typ = "2";
                    // Zaplata
                    double brutto = nettoSuma + vatSuma;
                    nowyDok.Zaplata = brutto.ToString("F", NumberFormatInfo.InvariantInfo);
                    //-------------------------------------------------------------------------
                    dokumentyBaza.Add(nowyDok);
                }
                else
                {
                    XmlNode nrDok = dokumentXml.SelectSingleNode("tns:P_2A", nsmgr);
                    String nrDokText = "";
                    if (nrDok != null)
                        nrDokText = nrDok.InnerText;
                    DoLogu("Dokument nr: " + nrDokText + " nie jest fakturą VAT.", 1);
                }
            }
            //Pozycje Dokumentow
            XmlNodeList wierszeXml = root.SelectNodes("tns:FakturaWiersz", nsmgr);
            foreach(Dokument dokument in dokumentyBaza)
            {
                if (dokument.NumerDokumentu.Length == 0)
                    continue;
                foreach (XmlNode wierszXml in wierszeXml)
                {
                    XmlNode elWiersza = null;
                    elWiersza = wierszXml.SelectSingleNode("tns:P_2B", nsmgr);
                    if (elWiersza!=null)
                    {
                        String nrDokumentu = elWiersza.InnerText.Trim();
                        if(nrDokumentu.Length>0)
                        {
                            if(nrDokumentu==dokument.NumerDokumentu)
                            {
                                DokumentPozycja nowaPozDok = new DokumentPozycja();
                                //Towar
                                elWiersza = wierszXml.SelectSingleNode("tns:P_7", nsmgr);
                                if (elWiersza != null)
                                    nowaPozDok.Opis = elWiersza.InnerText.Trim();
                                //Jednostka Miary
                                elWiersza = wierszXml.SelectSingleNode("tns:P_8A", nsmgr);
                                if (elWiersza != null)
                                    nowaPozDok.Jm = elWiersza.InnerText.Trim();
                                //Ilosc
                                elWiersza = wierszXml.SelectSingleNode("tns:P_8B", nsmgr);
                                if (elWiersza != null)
                                    nowaPozDok.Ilosc = elWiersza.InnerText.Trim();
                                //Cena
                                elWiersza = wierszXml.SelectSingleNode("tns:P_9A", nsmgr);
                                if (elWiersza != null)
                                    nowaPozDok.Cena = elWiersza.InnerText.Trim();
                                //kwota VAT
                                double netto = 0;
                                elWiersza = wierszXml.SelectSingleNode("tns:P_11", nsmgr);
                                if (elWiersza != null)
                                    netto = Double.Parse(elWiersza.InnerText.Trim(),NumberFormatInfo.InvariantInfo);
                                double procentVat = 0;
                                elWiersza = wierszXml.SelectSingleNode("tns:P_12", nsmgr);
                                if (elWiersza != null)
                                {
                                    String procentVatText = elWiersza.InnerText.Trim();
                                    //------ Stawka VAT ID -----------
                                    if ((procentVatText == "zw") || (procentVatText == "ZW"))
                                        nowaPozDok.StawkaVatID = "0";
                                    if (procentVatText == "8")
                                        nowaPozDok.StawkaVatID = "4";
                                    if(procentVatText == "7")
                                        nowaPozDok.StawkaVatID = "11";
                                    if (procentVatText == "23")
                                        nowaPozDok.StawkaVatID = "6";
                                    if (procentVatText == "5")
                                        nowaPozDok.StawkaVatID = "8";
                                    if (procentVatText == "4")
                                        nowaPozDok.StawkaVatID = "16";
                                    if ((procentVatText == "22")||(procentVatText == "3"))
                                        nowaPozDok.StawkaVatID = "7";
                                    if (procentVatText == "0")
                                    {
                                        if (dokument.KontrahentTyp == "2")
                                            nowaPozDok.StawkaVatID = "13";
                                        else
                                            nowaPozDok.StawkaVatID = "2";
                                    }
                                    //Kwota VAT cd ------------------------------
                                    if ((procentVatText == "zw") || (procentVatText == "ZW"))
                                        procentVat = 0;
                                    else
                                        procentVat = Double.Parse(procentVatText, NumberFormatInfo.InvariantInfo);
                                }
                                double kwotaVat = (netto * procentVat) / 100;
                                nowaPozDok.KwotaVat = kwotaVat.ToString("F", NumberFormatInfo.InvariantInfo);
                                dokument.Pozycje.Add(nowaPozDok);
                            }
                        }
                    }
                }
            }
            if (dokumentyBaza.Count == 0)
                return null;
            return dokumentyBaza;
        }
        */
