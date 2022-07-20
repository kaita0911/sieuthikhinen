//
//
// The code and information here are provided "as is", without 
// warranty of any kind, either expressed or implied, including 
// but not limited to the implied warranties of merchantability 
// and/or fitness for a particular purpose. 
//
// This file can be distributed free of charge, as long as this 
// header remains unchanged, and that any changes to the code are 
// noted in the appropriate places.
//
//  Email:  Evan@travelogues.net
//
//  Copyright (C) 2006, Evan Stein
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Unichar
{
    /// <summary>
    /// A supplemental class for manipulating and normalizing
    /// Unicode strings.
    /// </summary>
    public static class UnicodeStrings
    {
        private static Hashtable mCharacterTable = null;
        private static string mPattern = null;

        /// <summary>
        /// Strips diacritics from a Latin-character
        /// Unicode string. Leaves non-Latin characters as they
        /// were.
        /// </summary>
        /// <param name="InString">A Unicode string.</param>
        /// <returns>ASCII-range characters, plus any non-Latin characters in the string</returns>
        public static string LatinToAscii(string InString)
        {
            string returnString = "", ch;

            if (mCharacterTable == null)
                InitializeCharacterTable();

            if (InString != null)
            {
                for (int i = 0; i < InString.Length; i++)
                {
                    ch = InString.Substring(i, 1);
                    if (!mCharacterTable.Contains(ch))
                        returnString += ch;
                    else
                        returnString += mCharacterTable[ch];
                }
            }
            return returnString;
        }
        public static string RemoveHyphene(string InString)
        {
            
            string returnString = "";

            if (mCharacterTable == null)
                InitializeCharacterTable();

            returnString= Regex.Replace(InString, mPattern, ReplaceCC);
            returnString = Regex.Replace(returnString, "[^a-zA-Z0-9]+", "-");
            
            return returnString;


        }
        private static string ReplaceCC(Match m)
        // Replace each Regex cc match with the number of the occurrence.
        {
            return mCharacterTable[m.Value].ToString();
           
        }
        /// <summary>
        /// Takes a hexadecimal string and converts it to a 
        /// Unicode character
        /// </summary>
        /// <param name="HexString">A four-digit number in hex notation (eg, 00E7).</param>
        /// <returns>A unicode character, as string.</returns>
        public static string ToUnichar(string HexString)
        {
            string returnChar;
            byte[] b = new byte[2];
            UnicodeEncoding ue = new UnicodeEncoding();

            // Take hexadecimal as text and make a Unicode char number
            b[0] = Convert.ToByte(HexString.Substring(2, 2), 16);
            b[1] = Convert.ToByte(HexString.Substring(0, 2), 16);
            // Get the character the number represents
            returnChar = ue.GetString(b);
            return returnChar;
        }



        /// <summary>
        /// Stores Unicode characters and their "normalized"
        /// values to a hash table. Character codes are referenced
        /// by hex numbers because that's the most common way to
        /// refer to them.
        /// </summary>
        /* Upper-case comments are identifiers from the Unicode database. 
         * Lower- and mixed-case comments are the author's
        */
        private static void InitializeCharacterTable()
        {
            mCharacterTable = new Hashtable();
            try
            {
                //AddToTable(ToUnichar("0041"), "A");
                //AddToTable(ToUnichar("0042"), "B");
                //AddToTable(ToUnichar("0043"), "C");
                //AddToTable(ToUnichar("0044"), "D");
                //AddToTable(ToUnichar("0045"), "E");
                //AddToTable(ToUnichar("0046"), "F");
                //AddToTable(ToUnichar("0047"), "G");
                //AddToTable(ToUnichar("0048"), "H");
                //AddToTable(ToUnichar("0049"), "I");
                //AddToTable(ToUnichar("004A"), "J");
                //AddToTable(ToUnichar("004B"), "K");
                //AddToTable(ToUnichar("004C"), "L");
                //AddToTable(ToUnichar("004D"), "M");
                //AddToTable(ToUnichar("004E"), "N");
                //AddToTable(ToUnichar("004F"), "O");
                //AddToTable(ToUnichar("0050"), "P");
                //AddToTable(ToUnichar("0051"), "Q");
                //AddToTable(ToUnichar("0052"), "R");
                //AddToTable(ToUnichar("0053"), "S");
                //AddToTable(ToUnichar("0054"), "T");
                //AddToTable(ToUnichar("0055"), "U");
                //AddToTable(ToUnichar("0056"), "V");
                //AddToTable(ToUnichar("0057"), "W");
                //AddToTable(ToUnichar("0058"), "X");
                //AddToTable(ToUnichar("0059"), "Y");
                //AddToTable(ToUnichar("005A"), "Z");
                //AddToTable(ToUnichar("0061"), "a");
                //AddToTable(ToUnichar("0062"), "b");
                //AddToTable(ToUnichar("0063"), "c");
                //AddToTable(ToUnichar("0064"), "d");
                //AddToTable(ToUnichar("0065"), "e");
                //AddToTable(ToUnichar("0066"), "f");
                //AddToTable(ToUnichar("0067"), "g");
                //AddToTable(ToUnichar("0068"), "h");
                //AddToTable(ToUnichar("0069"), "i");
                //AddToTable(ToUnichar("006A"), "j");
                //AddToTable(ToUnichar("006B"), "k");
                //AddToTable(ToUnichar("006C"), "l");
                //AddToTable(ToUnichar("006D"), "m");
                //AddToTable(ToUnichar("006E"), "n");
                //AddToTable(ToUnichar("006F"), "o");
                //AddToTable(ToUnichar("0070"), "p");
                //AddToTable(ToUnichar("0071"), "q");
                //AddToTable(ToUnichar("0072"), "r");
                //AddToTable(ToUnichar("0073"), "s");
                //AddToTable(ToUnichar("0074"), "t");
                //AddToTable(ToUnichar("0075"), "u");
                //AddToTable(ToUnichar("0076"), "v");
                //AddToTable(ToUnichar("0077"), "w");
                //AddToTable(ToUnichar("0078"), "x");
                //AddToTable(ToUnichar("0079"), "y");
                //AddToTable(ToUnichar("007A"), "z");
                AddToTable(ToUnichar("00AA"), "a");    // FEMININE ORDINAL INDICATOR
                AddToTable(ToUnichar("00BA"), "o");	// MASCULINE ORDINAL INDICATOR
                AddToTable(ToUnichar("00C0"), "A");	// LATIN CAPITAL LETTER A WITH GRAVE
                AddToTable(ToUnichar("00C1"), "A");	// LATIN CAPITAL LETTER A WITH ACUTE
                AddToTable(ToUnichar("00C2"), "A");	// LATIN CAPITAL LETTER A WITH CIRCUMFLEX
                AddToTable(ToUnichar("00C3"), "A");	// LATIN CAPITAL LETTER A WITH TILDE
                AddToTable(ToUnichar("00C4"), "A");	// LATIN CAPITAL LETTER A WITH DIAERESIS
                AddToTable(ToUnichar("00C5"), "A");	// LATIN CAPITAL LETTER A WITH RING ABOVE
                AddToTable(ToUnichar("00C6"), "AE");	// LATIN CAPITAL LETTER AE -- no decomposition
                AddToTable(ToUnichar("00C7"), "C");	// LATIN CAPITAL LETTER C WITH CEDILLA
                AddToTable(ToUnichar("00C8"), "E");	// LATIN CAPITAL LETTER E WITH GRAVE
                AddToTable(ToUnichar("00C9"), "E");	// LATIN CAPITAL LETTER E WITH ACUTE
                AddToTable(ToUnichar("00CA"), "E");	// LATIN CAPITAL LETTER E WITH CIRCUMFLEX
                AddToTable(ToUnichar("00CB"), "E");	// LATIN CAPITAL LETTER E WITH DIAERESIS
                AddToTable(ToUnichar("00CC"), "I");	// LATIN CAPITAL LETTER I WITH GRAVE
                AddToTable(ToUnichar("00CD"), "I");	// LATIN CAPITAL LETTER I WITH ACUTE
                AddToTable(ToUnichar("00CE"), "I");	// LATIN CAPITAL LETTER I WITH CIRCUMFLEX
                AddToTable(ToUnichar("00CF"), "I");	// LATIN CAPITAL LETTER I WITH DIAERESIS
                AddToTable(ToUnichar("00D0"), "D");	// LATIN CAPITAL LETTER ETH -- no decomposition  	// Eth [D for Vietnamese]
                AddToTable(ToUnichar("00D1"), "N");	// LATIN CAPITAL LETTER N WITH TILDE
                AddToTable(ToUnichar("00D2"), "O");	// LATIN CAPITAL LETTER O WITH GRAVE
                AddToTable(ToUnichar("00D3"), "O");	// LATIN CAPITAL LETTER O WITH ACUTE
                AddToTable(ToUnichar("00D4"), "O");	// LATIN CAPITAL LETTER O WITH CIRCUMFLEX
                AddToTable(ToUnichar("00D5"), "O");	// LATIN CAPITAL LETTER O WITH TILDE
                AddToTable(ToUnichar("00D6"), "O");	// LATIN CAPITAL LETTER O WITH DIAERESIS
                AddToTable(ToUnichar("00D8"), "O");	// LATIN CAPITAL LETTER O WITH STROKE -- no decom
                AddToTable(ToUnichar("00D9"), "U");	// LATIN CAPITAL LETTER U WITH GRAVE
                AddToTable(ToUnichar("00DA"), "U");	// LATIN CAPITAL LETTER U WITH ACUTE
                AddToTable(ToUnichar("00DB"), "U");	// LATIN CAPITAL LETTER U WITH CIRCUMFLEX
                AddToTable(ToUnichar("00DC"), "U");	// LATIN CAPITAL LETTER U WITH DIAERESIS
                AddToTable(ToUnichar("00DD"), "Y");	// LATIN CAPITAL LETTER Y WITH ACUTE
                AddToTable(ToUnichar("00DE"), "Th");	// LATIN CAPITAL LETTER THORN -- no decomposition; // Thorn - Could be nothing other than thorn
                AddToTable(ToUnichar("00DF"), "s");	// LATIN SMALL LETTER SHARP S -- no decomposition
                AddToTable(ToUnichar("00E0"), "a");	// LATIN SMALL LETTER A WITH GRAVE
                AddToTable(ToUnichar("00E1"), "a");	// LATIN SMALL LETTER A WITH ACUTE
                AddToTable(ToUnichar("00E2"), "a");	// LATIN SMALL LETTER A WITH CIRCUMFLEX
                AddToTable(ToUnichar("00E3"), "a");	// LATIN SMALL LETTER A WITH TILDE
                AddToTable(ToUnichar("00E4"), "a");	// LATIN SMALL LETTER A WITH DIAERESIS
                AddToTable(ToUnichar("00E5"), "a");	// LATIN SMALL LETTER A WITH RING ABOVE
                AddToTable(ToUnichar("00E6"), "ae");	// LATIN SMALL LETTER AE -- no decomposition
                AddToTable(ToUnichar("00E7"), "c");	// LATIN SMALL LETTER C WITH CEDILLA
                AddToTable(ToUnichar("00E8"), "e");	// LATIN SMALL LETTER E WITH GRAVE
                AddToTable(ToUnichar("00E9"), "e");	// LATIN SMALL LETTER E WITH ACUTE
                AddToTable(ToUnichar("00EA"), "e");	// LATIN SMALL LETTER E WITH CIRCUMFLEX
                AddToTable(ToUnichar("00EB"), "e");	// LATIN SMALL LETTER E WITH DIAERESIS
                AddToTable(ToUnichar("00EC"), "i");	// LATIN SMALL LETTER I WITH GRAVE
                AddToTable(ToUnichar("00ED"), "i");	// LATIN SMALL LETTER I WITH ACUTE
                AddToTable(ToUnichar("00EE"), "i");	// LATIN SMALL LETTER I WITH CIRCUMFLEX
                AddToTable(ToUnichar("00EF"), "i");	// LATIN SMALL LETTER I WITH DIAERESIS
                AddToTable(ToUnichar("00F0"), "d");	// LATIN SMALL LETTER ETH -- no decomposition         // small eth, "d" for benefit of Vietnamese
                AddToTable(ToUnichar("00F1"), "n");	// LATIN SMALL LETTER N WITH TILDE
                AddToTable(ToUnichar("00F2"), "o");	// LATIN SMALL LETTER O WITH GRAVE
                AddToTable(ToUnichar("00F3"), "o");	// LATIN SMALL LETTER O WITH ACUTE
                AddToTable(ToUnichar("00F4"), "o");	// LATIN SMALL LETTER O WITH CIRCUMFLEX
                AddToTable(ToUnichar("00F5"), "o");	// LATIN SMALL LETTER O WITH TILDE
                AddToTable(ToUnichar("00F6"), "o");	// LATIN SMALL LETTER O WITH DIAERESIS
                AddToTable(ToUnichar("00F8"), "o");	// LATIN SMALL LETTER O WITH STROKE -- no decompo
                AddToTable(ToUnichar("00F9"), "u");	// LATIN SMALL LETTER U WITH GRAVE
                AddToTable(ToUnichar("00FA"), "u");	// LATIN SMALL LETTER U WITH ACUTE
                AddToTable(ToUnichar("00FB"), "u");	// LATIN SMALL LETTER U WITH CIRCUMFLEX
                AddToTable(ToUnichar("00FC"), "u");	// LATIN SMALL LETTER U WITH DIAERESIS
                AddToTable(ToUnichar("00FD"), "y");	// LATIN SMALL LETTER Y WITH ACUTE
                AddToTable(ToUnichar("00FE"), "th");	// LATIN SMALL LETTER THORN -- no decomposition  // Small thorn
                AddToTable(ToUnichar("00FF"), "y");	// LATIN SMALL LETTER Y WITH DIAERESIS
                AddToTable(ToUnichar("0100"), "A");	// LATIN CAPITAL LETTER A WITH MACRON
                AddToTable(ToUnichar("0101"), "a");	// LATIN SMALL LETTER A WITH MACRON
                AddToTable(ToUnichar("0102"), "A");	// LATIN CAPITAL LETTER A WITH BREVE
                AddToTable(ToUnichar("0103"), "a");	// LATIN SMALL LETTER A WITH BREVE
                AddToTable(ToUnichar("0104"), "A");	// LATIN CAPITAL LETTER A WITH OGONEK
                AddToTable(ToUnichar("0105"), "a");	// LATIN SMALL LETTER A WITH OGONEK
                AddToTable(ToUnichar("0106"), "C");	// LATIN CAPITAL LETTER C WITH ACUTE
                AddToTable(ToUnichar("0107"), "c");	// LATIN SMALL LETTER C WITH ACUTE
                AddToTable(ToUnichar("0108"), "C");	// LATIN CAPITAL LETTER C WITH CIRCUMFLEX
                AddToTable(ToUnichar("0109"), "c");	// LATIN SMALL LETTER C WITH CIRCUMFLEX
                AddToTable(ToUnichar("010A"), "C");	// LATIN CAPITAL LETTER C WITH DOT ABOVE
                AddToTable(ToUnichar("010B"), "c");	// LATIN SMALL LETTER C WITH DOT ABOVE
                AddToTable(ToUnichar("010C"), "C");	// LATIN CAPITAL LETTER C WITH CARON
                AddToTable(ToUnichar("010D"), "c");	// LATIN SMALL LETTER C WITH CARON
                AddToTable(ToUnichar("010E"), "D");	// LATIN CAPITAL LETTER D WITH CARON
                AddToTable(ToUnichar("010F"), "d");	// LATIN SMALL LETTER D WITH CARON
                AddToTable(ToUnichar("0110"), "D");	// LATIN CAPITAL LETTER D WITH STROKE -- no decomposition                     // Capital D with stroke
                AddToTable(ToUnichar("0111"), "d");	// LATIN SMALL LETTER D WITH STROKE -- no decomposition                       // small D with stroke
                AddToTable(ToUnichar("0112"), "E");	// LATIN CAPITAL LETTER E WITH MACRON
                AddToTable(ToUnichar("0113"), "e");	// LATIN SMALL LETTER E WITH MACRON
                AddToTable(ToUnichar("0114"), "E");	// LATIN CAPITAL LETTER E WITH BREVE
                AddToTable(ToUnichar("0115"), "e");	// LATIN SMALL LETTER E WITH BREVE
                AddToTable(ToUnichar("0116"), "E");	// LATIN CAPITAL LETTER E WITH DOT ABOVE
                AddToTable(ToUnichar("0117"), "e");	// LATIN SMALL LETTER E WITH DOT ABOVE
                AddToTable(ToUnichar("0118"), "E");	// LATIN CAPITAL LETTER E WITH OGONEK
                AddToTable(ToUnichar("0119"), "e");	// LATIN SMALL LETTER E WITH OGONEK
                AddToTable(ToUnichar("011A"), "E");	// LATIN CAPITAL LETTER E WITH CARON
                AddToTable(ToUnichar("011B"), "e");	// LATIN SMALL LETTER E WITH CARON
                AddToTable(ToUnichar("011C"), "G");	// LATIN CAPITAL LETTER G WITH CIRCUMFLEX
                AddToTable(ToUnichar("011D"), "g");	// LATIN SMALL LETTER G WITH CIRCUMFLEX
                AddToTable(ToUnichar("011E"), "G");	// LATIN CAPITAL LETTER G WITH BREVE
                AddToTable(ToUnichar("011F"), "g");	// LATIN SMALL LETTER G WITH BREVE
                AddToTable(ToUnichar("0120"), "G");	// LATIN CAPITAL LETTER G WITH DOT ABOVE
                AddToTable(ToUnichar("0121"), "g");	// LATIN SMALL LETTER G WITH DOT ABOVE
                AddToTable(ToUnichar("0122"), "G");	// LATIN CAPITAL LETTER G WITH CEDILLA
                AddToTable(ToUnichar("0123"), "g");	// LATIN SMALL LETTER G WITH CEDILLA
                AddToTable(ToUnichar("0124"), "H");	// LATIN CAPITAL LETTER H WITH CIRCUMFLEX
                AddToTable(ToUnichar("0125"), "h");	// LATIN SMALL LETTER H WITH CIRCUMFLEX
                AddToTable(ToUnichar("0126"), "H");	// LATIN CAPITAL LETTER H WITH STROKE -- no decomposition
                AddToTable(ToUnichar("0127"), "h");	// LATIN SMALL LETTER H WITH STROKE -- no decomposition
                AddToTable(ToUnichar("0128"), "I");	// LATIN CAPITAL LETTER I WITH TILDE
                AddToTable(ToUnichar("0129"), "i");	// LATIN SMALL LETTER I WITH TILDE
                AddToTable(ToUnichar("012A"), "I");	// LATIN CAPITAL LETTER I WITH MACRON
                AddToTable(ToUnichar("012B"), "i");	// LATIN SMALL LETTER I WITH MACRON
                AddToTable(ToUnichar("012C"), "I");	// LATIN CAPITAL LETTER I WITH BREVE
                AddToTable(ToUnichar("012D"), "i");	// LATIN SMALL LETTER I WITH BREVE
                AddToTable(ToUnichar("012E"), "I");	// LATIN CAPITAL LETTER I WITH OGONEK
                AddToTable(ToUnichar("012F"), "i");	// LATIN SMALL LETTER I WITH OGONEK
                AddToTable(ToUnichar("0130"), "I");	// LATIN CAPITAL LETTER I WITH DOT ABOVE
                AddToTable(ToUnichar("0131"), "i");	// LATIN SMALL LETTER DOTLESS I -- no decomposition
                AddToTable(ToUnichar("0132"), "I");	// LATIN CAPITAL LIGATURE IJ    
                AddToTable(ToUnichar("0133"), "i");	// LATIN SMALL LIGATURE IJ      
                AddToTable(ToUnichar("0134"), "J");	// LATIN CAPITAL LETTER J WITH CIRCUMFLEX
                AddToTable(ToUnichar("0135"), "j");	// LATIN SMALL LETTER J WITH CIRCUMFLEX
                AddToTable(ToUnichar("0136"), "K");	// LATIN CAPITAL LETTER K WITH CEDILLA
                AddToTable(ToUnichar("0137"), "k");	// LATIN SMALL LETTER K WITH CEDILLA
                AddToTable(ToUnichar("0138"), "k");	// LATIN SMALL LETTER KRA -- no decomposition
                AddToTable(ToUnichar("0139"), "L");	// LATIN CAPITAL LETTER L WITH ACUTE
                AddToTable(ToUnichar("013A"), "l");	// LATIN SMALL LETTER L WITH ACUTE
                AddToTable(ToUnichar("013B"), "L");	// LATIN CAPITAL LETTER L WITH CEDILLA
                AddToTable(ToUnichar("013C"), "l");	// LATIN SMALL LETTER L WITH CEDILLA
                AddToTable(ToUnichar("013D"), "L");	// LATIN CAPITAL LETTER L WITH CARON
                AddToTable(ToUnichar("013E"), "l");	// LATIN SMALL LETTER L WITH CARON
                AddToTable(ToUnichar("013F"), "L");	// LATIN CAPITAL LETTER L WITH MIDDLE DOT
                AddToTable(ToUnichar("0140"), "l");	// LATIN SMALL LETTER L WITH MIDDLE DOT
                AddToTable(ToUnichar("0141"), "L");	// LATIN CAPITAL LETTER L WITH STROKE -- no decomposition
                AddToTable(ToUnichar("0142"), "l");	// LATIN SMALL LETTER L WITH STROKE -- no decomposition
                AddToTable(ToUnichar("0143"), "N");	// LATIN CAPITAL LETTER N WITH ACUTE
                AddToTable(ToUnichar("0144"), "n");	// LATIN SMALL LETTER N WITH ACUTE
                AddToTable(ToUnichar("0145"), "N");	// LATIN CAPITAL LETTER N WITH CEDILLA
                AddToTable(ToUnichar("0146"), "n");	// LATIN SMALL LETTER N WITH CEDILLA
                AddToTable(ToUnichar("0147"), "N");	// LATIN CAPITAL LETTER N WITH CARON
                AddToTable(ToUnichar("0148"), "n");	// LATIN SMALL LETTER N WITH CARON
                AddToTable(ToUnichar("0149"), "'n");	// LATIN SMALL LETTER N PRECEDED BY APOSTROPHE                              ;
                AddToTable(ToUnichar("014A"), "NG");	// LATIN CAPITAL LETTER ENG -- no decomposition                             ;
                AddToTable(ToUnichar("014B"), "ng");	// LATIN SMALL LETTER ENG -- no decomposition                               ;
                AddToTable(ToUnichar("014C"), "O");	// LATIN CAPITAL LETTER O WITH MACRON
                AddToTable(ToUnichar("014D"), "o");	// LATIN SMALL LETTER O WITH MACRON
                AddToTable(ToUnichar("014E"), "O");	// LATIN CAPITAL LETTER O WITH BREVE
                AddToTable(ToUnichar("014F"), "o");	// LATIN SMALL LETTER O WITH BREVE
                AddToTable(ToUnichar("0150"), "O");	// LATIN CAPITAL LETTER O WITH DOUBLE ACUTE
                AddToTable(ToUnichar("0151"), "o");	// LATIN SMALL LETTER O WITH DOUBLE ACUTE
                AddToTable(ToUnichar("0152"), "OE");	// LATIN CAPITAL LIGATURE OE -- no decomposition
                AddToTable(ToUnichar("0153"), "oe");	// LATIN SMALL LIGATURE OE -- no decomposition
                AddToTable(ToUnichar("0154"), "R");	// LATIN CAPITAL LETTER R WITH ACUTE
                AddToTable(ToUnichar("0155"), "r");	// LATIN SMALL LETTER R WITH ACUTE
                AddToTable(ToUnichar("0156"), "R");	// LATIN CAPITAL LETTER R WITH CEDILLA
                AddToTable(ToUnichar("0157"), "r");	// LATIN SMALL LETTER R WITH CEDILLA
                AddToTable(ToUnichar("0158"), "R");	// LATIN CAPITAL LETTER R WITH CARON
                AddToTable(ToUnichar("0159"), "r");	// LATIN SMALL LETTER R WITH CARON
                AddToTable(ToUnichar("015A"), "S");	// LATIN CAPITAL LETTER S WITH ACUTE
                AddToTable(ToUnichar("015B"), "s");	// LATIN SMALL LETTER S WITH ACUTE
                AddToTable(ToUnichar("015C"), "S");	// LATIN CAPITAL LETTER S WITH CIRCUMFLEX
                AddToTable(ToUnichar("015D"), "s");	// LATIN SMALL LETTER S WITH CIRCUMFLEX
                AddToTable(ToUnichar("015E"), "S");	// LATIN CAPITAL LETTER S WITH CEDILLA
                AddToTable(ToUnichar("015F"), "s");	// LATIN SMALL LETTER S WITH CEDILLA
                AddToTable(ToUnichar("0160"), "S");	// LATIN CAPITAL LETTER S WITH CARON
                AddToTable(ToUnichar("0161"), "s");	// LATIN SMALL LETTER S WITH CARON
                AddToTable(ToUnichar("0162"), "T");	// LATIN CAPITAL LETTER T WITH CEDILLA
                AddToTable(ToUnichar("0163"), "t");	// LATIN SMALL LETTER T WITH CEDILLA
                AddToTable(ToUnichar("0164"), "T");	// LATIN CAPITAL LETTER T WITH CARON
                AddToTable(ToUnichar("0165"), "t");	// LATIN SMALL LETTER T WITH CARON
                AddToTable(ToUnichar("0166"), "T");	// LATIN CAPITAL LETTER T WITH STROKE -- no decomposition
                AddToTable(ToUnichar("0167"), "t");	// LATIN SMALL LETTER T WITH STROKE -- no decomposition
                AddToTable(ToUnichar("0168"), "U");	// LATIN CAPITAL LETTER U WITH TILDE
                AddToTable(ToUnichar("0169"), "u");	// LATIN SMALL LETTER U WITH TILDE
                AddToTable(ToUnichar("016A"), "U");	// LATIN CAPITAL LETTER U WITH MACRON
                AddToTable(ToUnichar("016B"), "u");	// LATIN SMALL LETTER U WITH MACRON
                AddToTable(ToUnichar("016C"), "U");	// LATIN CAPITAL LETTER U WITH BREVE
                AddToTable(ToUnichar("016D"), "u");	// LATIN SMALL LETTER U WITH BREVE
                AddToTable(ToUnichar("016E"), "U");	// LATIN CAPITAL LETTER U WITH RING ABOVE
                AddToTable(ToUnichar("016F"), "u");	// LATIN SMALL LETTER U WITH RING ABOVE
                AddToTable(ToUnichar("0170"), "U");	// LATIN CAPITAL LETTER U WITH DOUBLE ACUTE
                AddToTable(ToUnichar("0171"), "u");	// LATIN SMALL LETTER U WITH DOUBLE ACUTE
                AddToTable(ToUnichar("0172"), "U");	// LATIN CAPITAL LETTER U WITH OGONEK
                AddToTable(ToUnichar("0173"), "u");	// LATIN SMALL LETTER U WITH OGONEK
                AddToTable(ToUnichar("0174"), "W");	// LATIN CAPITAL LETTER W WITH CIRCUMFLEX
                AddToTable(ToUnichar("0175"), "w");	// LATIN SMALL LETTER W WITH CIRCUMFLEX
                AddToTable(ToUnichar("0176"), "Y");	// LATIN CAPITAL LETTER Y WITH CIRCUMFLEX
                AddToTable(ToUnichar("0177"), "y");	// LATIN SMALL LETTER Y WITH CIRCUMFLEX
                AddToTable(ToUnichar("0178"), "Y");	// LATIN CAPITAL LETTER Y WITH DIAERESIS
                AddToTable(ToUnichar("0179"), "Z");	// LATIN CAPITAL LETTER Z WITH ACUTE
                AddToTable(ToUnichar("017A"), "z");	// LATIN SMALL LETTER Z WITH ACUTE
                AddToTable(ToUnichar("017B"), "Z");	// LATIN CAPITAL LETTER Z WITH DOT ABOVE
                AddToTable(ToUnichar("017C"), "z");	// LATIN SMALL LETTER Z WITH DOT ABOVE
                AddToTable(ToUnichar("017D"), "Z");	// LATIN CAPITAL LETTER Z WITH CARON
                AddToTable(ToUnichar("017E"), "z");	// LATIN SMALL LETTER Z WITH CARON
                AddToTable(ToUnichar("017F"), "s");	// LATIN SMALL LETTER LONG S    
                AddToTable(ToUnichar("0180"), "b");	// LATIN SMALL LETTER B WITH STROKE -- no decomposition
                AddToTable(ToUnichar("0181"), "B");	// LATIN CAPITAL LETTER B WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0182"), "B");	// LATIN CAPITAL LETTER B WITH TOPBAR -- no decomposition
                AddToTable(ToUnichar("0183"), "b");	// LATIN SMALL LETTER B WITH TOPBAR -- no decomposition
                AddToTable(ToUnichar("0184"), "6");	// LATIN CAPITAL LETTER TONE SIX -- no decomposition
                AddToTable(ToUnichar("0185"), "6");	// LATIN SMALL LETTER TONE SIX -- no decomposition
                AddToTable(ToUnichar("0186"), "O");	// LATIN CAPITAL LETTER OPEN O -- no decomposition
                AddToTable(ToUnichar("0187"), "C");	// LATIN CAPITAL LETTER C WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0188"), "c");	// LATIN SMALL LETTER C WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0189"), "D");	// LATIN CAPITAL LETTER AFRICAN D -- no decomposition
                AddToTable(ToUnichar("018A"), "D");	// LATIN CAPITAL LETTER D WITH HOOK -- no decomposition
                AddToTable(ToUnichar("018B"), "D");	// LATIN CAPITAL LETTER D WITH TOPBAR -- no decomposition
                AddToTable(ToUnichar("018C"), "d");	// LATIN SMALL LETTER D WITH TOPBAR -- no decomposition
                AddToTable(ToUnichar("018D"), "d");	// LATIN SMALL LETTER TURNED DELTA -- no decomposition
                AddToTable(ToUnichar("018E"), "E");	// LATIN CAPITAL LETTER REVERSED E -- no decomposition
                AddToTable(ToUnichar("018F"), "E");	// LATIN CAPITAL LETTER SCHWA -- no decomposition
                AddToTable(ToUnichar("0190"), "E");	// LATIN CAPITAL LETTER OPEN E -- no decomposition
                AddToTable(ToUnichar("0191"), "F");	// LATIN CAPITAL LETTER F WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0192"), "f");	// LATIN SMALL LETTER F WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0193"), "G");	// LATIN CAPITAL LETTER G WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0194"), "G");	// LATIN CAPITAL LETTER GAMMA -- no decomposition
                AddToTable(ToUnichar("0195"), "hv");	// LATIN SMALL LETTER HV -- no decomposition
                AddToTable(ToUnichar("0196"), "I");	// LATIN CAPITAL LETTER IOTA -- no decomposition
                AddToTable(ToUnichar("0197"), "I");	// LATIN CAPITAL LETTER I WITH STROKE -- no decomposition
                AddToTable(ToUnichar("0198"), "K");	// LATIN CAPITAL LETTER K WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0199"), "k");	// LATIN SMALL LETTER K WITH HOOK -- no decomposition
                AddToTable(ToUnichar("019A"), "l");	// LATIN SMALL LETTER L WITH BAR -- no decomposition
                AddToTable(ToUnichar("019B"), "l");	// LATIN SMALL LETTER LAMBDA WITH STROKE -- no decomposition
                AddToTable(ToUnichar("019C"), "M");	// LATIN CAPITAL LETTER TURNED M -- no decomposition
                AddToTable(ToUnichar("019D"), "N");	// LATIN CAPITAL LETTER N WITH LEFT HOOK -- no decomposition
                AddToTable(ToUnichar("019E"), "n");	// LATIN SMALL LETTER N WITH LONG RIGHT LEG -- no decomposition
                AddToTable(ToUnichar("019F"), "O");	// LATIN CAPITAL LETTER O WITH MIDDLE TILDE -- no decomposition
                AddToTable(ToUnichar("01A0"), "O");	// LATIN CAPITAL LETTER O WITH HORN
                AddToTable(ToUnichar("01A1"), "o");	// LATIN SMALL LETTER O WITH HORN
                AddToTable(ToUnichar("01A2"), "OI");	// LATIN CAPITAL LETTER OI -- no decomposition
                AddToTable(ToUnichar("01A3"), "oi");	// LATIN SMALL LETTER OI -- no decomposition
                AddToTable(ToUnichar("01A4"), "P");	// LATIN CAPITAL LETTER P WITH HOOK -- no decomposition
                AddToTable(ToUnichar("01A5"), "p");	// LATIN SMALL LETTER P WITH HOOK -- no decomposition
                AddToTable(ToUnichar("01A6"), "YR");	// LATIN LETTER YR -- no decomposition
                AddToTable(ToUnichar("01A7"), "2");	// LATIN CAPITAL LETTER TONE TWO -- no decomposition
                AddToTable(ToUnichar("01A8"), "2");	// LATIN SMALL LETTER TONE TWO -- no decomposition
                AddToTable(ToUnichar("01A9"), "S");	// LATIN CAPITAL LETTER ESH -- no decomposition
                AddToTable(ToUnichar("01AA"), "s");	// LATIN LETTER REVERSED ESH LOOP -- no decomposition
                AddToTable(ToUnichar("01AB"), "t");	// LATIN SMALL LETTER T WITH PALATAL HOOK -- no decomposition
                AddToTable(ToUnichar("01AC"), "T");	// LATIN CAPITAL LETTER T WITH HOOK -- no decomposition
                AddToTable(ToUnichar("01AD"), "t");	// LATIN SMALL LETTER T WITH HOOK -- no decomposition
                AddToTable(ToUnichar("01AE"), "T");	// LATIN CAPITAL LETTER T WITH RETROFLEX HOOK -- no decomposition
                AddToTable(ToUnichar("01AF"), "U");	// LATIN CAPITAL LETTER U WITH HORN
                AddToTable(ToUnichar("01B0"), "u");	// LATIN SMALL LETTER U WITH HORN
                AddToTable(ToUnichar("01B1"), "u");	// LATIN CAPITAL LETTER UPSILON -- no decomposition
                AddToTable(ToUnichar("01B2"), "V");	// LATIN CAPITAL LETTER V WITH HOOK -- no decomposition
                AddToTable(ToUnichar("01B3"), "Y");	// LATIN CAPITAL LETTER Y WITH HOOK -- no decomposition
                AddToTable(ToUnichar("01B4"), "y");	// LATIN SMALL LETTER Y WITH HOOK -- no decomposition
                AddToTable(ToUnichar("01B5"), "Z");	// LATIN CAPITAL LETTER Z WITH STROKE -- no decomposition
                AddToTable(ToUnichar("01B6"), "z");	// LATIN SMALL LETTER Z WITH STROKE -- no decomposition
                AddToTable(ToUnichar("01B7"), "Z");	// LATIN CAPITAL LETTER EZH -- no decomposition
                AddToTable(ToUnichar("01B8"), "Z");	// LATIN CAPITAL LETTER EZH REVERSED -- no decomposition
                AddToTable(ToUnichar("01B9"), "Z");	// LATIN SMALL LETTER EZH REVERSED -- no decomposition
                AddToTable(ToUnichar("01BA"), "z");	// LATIN SMALL LETTER EZH WITH TAIL -- no decomposition
                AddToTable(ToUnichar("01BB"), "2");	// LATIN LETTER TWO WITH STROKE -- no decomposition
                AddToTable(ToUnichar("01BC"), "5");	// LATIN CAPITAL LETTER TONE FIVE -- no decomposition
                AddToTable(ToUnichar("01BD"), "5");	// LATIN SMALL LETTER TONE FIVE -- no decomposition
                AddToTable(ToUnichar("01BE"), "´");	// LATIN LETTER INVERTED GLOTTAL STOP WITH STROKE -- no decomposition
                AddToTable(ToUnichar("01BF"), "w");	// LATIN LETTER WYNN -- no decomposition
                AddToTable(ToUnichar("01C0"), "!");	// LATIN LETTER DENTAL CLICK -- no decomposition
                AddToTable(ToUnichar("01C1"), "!");	// LATIN LETTER LATERAL CLICK -- no decomposition
                AddToTable(ToUnichar("01C2"), "!");	// LATIN LETTER ALVEOLAR CLICK -- no decomposition
                AddToTable(ToUnichar("01C3"), "!");	// LATIN LETTER RETROFLEX CLICK -- no decomposition
                AddToTable(ToUnichar("01C4"), "DZ");	// LATIN CAPITAL LETTER DZ WITH CARON
                AddToTable(ToUnichar("01C5"), "DZ");	// LATIN CAPITAL LETTER D WITH SMALL LETTER Z WITH CARON
                AddToTable(ToUnichar("01C6"), "d");	// LATIN SMALL LETTER DZ WITH CARON
                AddToTable(ToUnichar("01C7"), "Lj");	// LATIN CAPITAL LETTER LJ
                AddToTable(ToUnichar("01C8"), "Lj");	// LATIN CAPITAL LETTER L WITH SMALL LETTER J
                AddToTable(ToUnichar("01C9"), "lj");	// LATIN SMALL LETTER LJ
                AddToTable(ToUnichar("01CA"), "NJ");	// LATIN CAPITAL LETTER NJ
                AddToTable(ToUnichar("01CB"), "NJ");	// LATIN CAPITAL LETTER N WITH SMALL LETTER J
                AddToTable(ToUnichar("01CC"), "nj");	// LATIN SMALL LETTER NJ
                AddToTable(ToUnichar("01CD"), "A");	// LATIN CAPITAL LETTER A WITH CARON
                AddToTable(ToUnichar("01CE"), "a");	// LATIN SMALL LETTER A WITH CARON
                AddToTable(ToUnichar("01CF"), "I");	// LATIN CAPITAL LETTER I WITH CARON
                AddToTable(ToUnichar("01D0"), "i");	// LATIN SMALL LETTER I WITH CARON
                AddToTable(ToUnichar("01D1"), "O");	// LATIN CAPITAL LETTER O WITH CARON
                AddToTable(ToUnichar("01D2"), "o");	// LATIN SMALL LETTER O WITH CARON
                AddToTable(ToUnichar("01D3"), "U");	// LATIN CAPITAL LETTER U WITH CARON
                AddToTable(ToUnichar("01D4"), "u");	// LATIN SMALL LETTER U WITH CARON
                AddToTable(ToUnichar("01D5"), "U");	// LATIN CAPITAL LETTER U WITH DIAERESIS AND MACRON
                AddToTable(ToUnichar("01D6"), "u");	// LATIN SMALL LETTER U WITH DIAERESIS AND MACRON
                AddToTable(ToUnichar("01D7"), "U");	// LATIN CAPITAL LETTER U WITH DIAERESIS AND ACUTE
                AddToTable(ToUnichar("01D8"), "u");	// LATIN SMALL LETTER U WITH DIAERESIS AND ACUTE
                AddToTable(ToUnichar("01D9"), "U");	// LATIN CAPITAL LETTER U WITH DIAERESIS AND CARON
                AddToTable(ToUnichar("01DA"), "u");	// LATIN SMALL LETTER U WITH DIAERESIS AND CARON
                AddToTable(ToUnichar("01DB"), "U");	// LATIN CAPITAL LETTER U WITH DIAERESIS AND GRAVE
                AddToTable(ToUnichar("01DC"), "u");	// LATIN SMALL LETTER U WITH DIAERESIS AND GRAVE
                AddToTable(ToUnichar("01DD"), "e");	// LATIN SMALL LETTER TURNED E -- no decomposition
                AddToTable(ToUnichar("01DE"), "A");	// LATIN CAPITAL LETTER A WITH DIAERESIS AND MACRON
                AddToTable(ToUnichar("01DF"), "a");	// LATIN SMALL LETTER A WITH DIAERESIS AND MACRON
                AddToTable(ToUnichar("01E0"), "A");	// LATIN CAPITAL LETTER A WITH DOT ABOVE AND MACRON
                AddToTable(ToUnichar("01E1"), "a");	// LATIN SMALL LETTER A WITH DOT ABOVE AND MACRON
                AddToTable(ToUnichar("01E2"), "AE");	// LATIN CAPITAL LETTER AE WITH MACRON
                AddToTable(ToUnichar("01E3"), "ae");	// LATIN SMALL LETTER AE WITH MACRON
                AddToTable(ToUnichar("01E4"), "G");	// LATIN CAPITAL LETTER G WITH STROKE -- no decomposition
                AddToTable(ToUnichar("01E5"), "g");	// LATIN SMALL LETTER G WITH STROKE -- no decomposition
                AddToTable(ToUnichar("01E6"), "G");	// LATIN CAPITAL LETTER G WITH CARON
                AddToTable(ToUnichar("01E7"), "g");	// LATIN SMALL LETTER G WITH CARON
                AddToTable(ToUnichar("01E8"), "K");	// LATIN CAPITAL LETTER K WITH CARON
                AddToTable(ToUnichar("01E9"), "k");	// LATIN SMALL LETTER K WITH CARON
                AddToTable(ToUnichar("01EA"), "O");	// LATIN CAPITAL LETTER O WITH OGONEK
                AddToTable(ToUnichar("01EB"), "o");	// LATIN SMALL LETTER O WITH OGONEK
                AddToTable(ToUnichar("01EC"), "O");	// LATIN CAPITAL LETTER O WITH OGONEK AND MACRON
                AddToTable(ToUnichar("01ED"), "o");	// LATIN SMALL LETTER O WITH OGONEK AND MACRON
                AddToTable(ToUnichar("01EE"), "Z");	// LATIN CAPITAL LETTER EZH WITH CARON
                AddToTable(ToUnichar("01EF"), "Z");	// LATIN SMALL LETTER EZH WITH CARON
                AddToTable(ToUnichar("01F0"), "j");	// LATIN SMALL LETTER J WITH CARON
                AddToTable(ToUnichar("01F1"), "DZ");	// LATIN CAPITAL LETTER DZ
                AddToTable(ToUnichar("01F2"), "DZ");	// LATIN CAPITAL LETTER D WITH SMALL LETTER Z
                AddToTable(ToUnichar("01F3"), "dz");	// LATIN SMALL LETTER DZ
                AddToTable(ToUnichar("01F4"), "G");	// LATIN CAPITAL LETTER G WITH ACUTE
                AddToTable(ToUnichar("01F5"), "g");	// LATIN SMALL LETTER G WITH ACUTE
                AddToTable(ToUnichar("01F6"), "hv");	// LATIN CAPITAL LETTER HWAIR -- no decomposition
                AddToTable(ToUnichar("01F7"), "w");	// LATIN CAPITAL LETTER WYNN -- no decomposition
                AddToTable(ToUnichar("01F8"), "N");	// LATIN CAPITAL LETTER N WITH GRAVE
                AddToTable(ToUnichar("01F9"), "n");	// LATIN SMALL LETTER N WITH GRAVE
                AddToTable(ToUnichar("01FA"), "A");	// LATIN CAPITAL LETTER A WITH RING ABOVE AND ACUTE
                AddToTable(ToUnichar("01FB"), "a");	// LATIN SMALL LETTER A WITH RING ABOVE AND ACUTE
                AddToTable(ToUnichar("01FC"), "AE");	// LATIN CAPITAL LETTER AE WITH ACUTE
                AddToTable(ToUnichar("01FD"), "ae");	// LATIN SMALL LETTER AE WITH ACUTE
                AddToTable(ToUnichar("01FE"), "O");	// LATIN CAPITAL LETTER O WITH STROKE AND ACUTE
                AddToTable(ToUnichar("01FF"), "o");	// LATIN SMALL LETTER O WITH STROKE AND ACUTE
                AddToTable(ToUnichar("0200"), "A");	// LATIN CAPITAL LETTER A WITH DOUBLE GRAVE
                AddToTable(ToUnichar("0201"), "a");	// LATIN SMALL LETTER A WITH DOUBLE GRAVE
                AddToTable(ToUnichar("0202"), "A");	// LATIN CAPITAL LETTER A WITH INVERTED BREVE
                AddToTable(ToUnichar("0203"), "a");	// LATIN SMALL LETTER A WITH INVERTED BREVE
                AddToTable(ToUnichar("0204"), "E");	// LATIN CAPITAL LETTER E WITH DOUBLE GRAVE
                AddToTable(ToUnichar("0205"), "e");	// LATIN SMALL LETTER E WITH DOUBLE GRAVE
                AddToTable(ToUnichar("0206"), "E");	// LATIN CAPITAL LETTER E WITH INVERTED BREVE
                AddToTable(ToUnichar("0207"), "e");	// LATIN SMALL LETTER E WITH INVERTED BREVE
                AddToTable(ToUnichar("0208"), "I");	// LATIN CAPITAL LETTER I WITH DOUBLE GRAVE
                AddToTable(ToUnichar("0209"), "i");	// LATIN SMALL LETTER I WITH DOUBLE GRAVE
                AddToTable(ToUnichar("020A"), "I");	// LATIN CAPITAL LETTER I WITH INVERTED BREVE
                AddToTable(ToUnichar("020B"), "i");	// LATIN SMALL LETTER I WITH INVERTED BREVE
                AddToTable(ToUnichar("020C"), "O");	// LATIN CAPITAL LETTER O WITH DOUBLE GRAVE
                AddToTable(ToUnichar("020D"), "o");	// LATIN SMALL LETTER O WITH DOUBLE GRAVE
                AddToTable(ToUnichar("020E"), "O");	// LATIN CAPITAL LETTER O WITH INVERTED BREVE
                AddToTable(ToUnichar("020F"), "o");	// LATIN SMALL LETTER O WITH INVERTED BREVE
                AddToTable(ToUnichar("0210"), "R");	// LATIN CAPITAL LETTER R WITH DOUBLE GRAVE
                AddToTable(ToUnichar("0211"), "r");	// LATIN SMALL LETTER R WITH DOUBLE GRAVE
                AddToTable(ToUnichar("0212"), "R");	// LATIN CAPITAL LETTER R WITH INVERTED BREVE
                AddToTable(ToUnichar("0213"), "r");	// LATIN SMALL LETTER R WITH INVERTED BREVE
                AddToTable(ToUnichar("0214"), "U");	// LATIN CAPITAL LETTER U WITH DOUBLE GRAVE
                AddToTable(ToUnichar("0215"), "u");	// LATIN SMALL LETTER U WITH DOUBLE GRAVE
                AddToTable(ToUnichar("0216"), "U");	// LATIN CAPITAL LETTER U WITH INVERTED BREVE
                AddToTable(ToUnichar("0217"), "u");	// LATIN SMALL LETTER U WITH INVERTED BREVE
                AddToTable(ToUnichar("0218"), "S");	// LATIN CAPITAL LETTER S WITH COMMA BELOW
                AddToTable(ToUnichar("0219"), "s");	// LATIN SMALL LETTER S WITH COMMA BELOW
                AddToTable(ToUnichar("021A"), "T");	// LATIN CAPITAL LETTER T WITH COMMA BELOW
                AddToTable(ToUnichar("021B"), "t");	// LATIN SMALL LETTER T WITH COMMA BELOW
                AddToTable(ToUnichar("021C"), "Z");	// LATIN CAPITAL LETTER YOGH -- no decomposition
                AddToTable(ToUnichar("021D"), "z");	// LATIN SMALL LETTER YOGH -- no decomposition
                AddToTable(ToUnichar("021E"), "H");	// LATIN CAPITAL LETTER H WITH CARON
                AddToTable(ToUnichar("021F"), "h");	// LATIN SMALL LETTER H WITH CARON
                AddToTable(ToUnichar("0220"), "N");	// LATIN CAPITAL LETTER N WITH LONG RIGHT LEG -- no decomposition
                AddToTable(ToUnichar("0221"), "d");	// LATIN SMALL LETTER D WITH CURL -- no decomposition
                AddToTable(ToUnichar("0222"), "OU");	// LATIN CAPITAL LETTER OU -- no decomposition
                AddToTable(ToUnichar("0223"), "ou");	// LATIN SMALL LETTER OU -- no decomposition
                AddToTable(ToUnichar("0224"), "Z");	// LATIN CAPITAL LETTER Z WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0225"), "z");	// LATIN SMALL LETTER Z WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0226"), "A");	// LATIN CAPITAL LETTER A WITH DOT ABOVE
                AddToTable(ToUnichar("0227"), "a");	// LATIN SMALL LETTER A WITH DOT ABOVE
                AddToTable(ToUnichar("0228"), "E");	// LATIN CAPITAL LETTER E WITH CEDILLA
                AddToTable(ToUnichar("0229"), "e");	// LATIN SMALL LETTER E WITH CEDILLA
                AddToTable(ToUnichar("022A"), "O");	// LATIN CAPITAL LETTER O WITH DIAERESIS AND MACRON
                AddToTable(ToUnichar("022B"), "o");	// LATIN SMALL LETTER O WITH DIAERESIS AND MACRON
                AddToTable(ToUnichar("022C"), "O");	// LATIN CAPITAL LETTER O WITH TILDE AND MACRON
                AddToTable(ToUnichar("022D"), "o");	// LATIN SMALL LETTER O WITH TILDE AND MACRON
                AddToTable(ToUnichar("022E"), "O");	// LATIN CAPITAL LETTER O WITH DOT ABOVE
                AddToTable(ToUnichar("022F"), "o");	// LATIN SMALL LETTER O WITH DOT ABOVE
                AddToTable(ToUnichar("0230"), "O");	// LATIN CAPITAL LETTER O WITH DOT ABOVE AND MACRON
                AddToTable(ToUnichar("0231"), "o");	// LATIN SMALL LETTER O WITH DOT ABOVE AND MACRON
                AddToTable(ToUnichar("0232"), "Y");	// LATIN CAPITAL LETTER Y WITH MACRON
                AddToTable(ToUnichar("0233"), "y");	// LATIN SMALL LETTER Y WITH MACRON
                AddToTable(ToUnichar("0234"), "l");	// LATIN SMALL LETTER L WITH CURL -- no decomposition
                AddToTable(ToUnichar("0235"), "n");	// LATIN SMALL LETTER N WITH CURL -- no decomposition
                AddToTable(ToUnichar("0236"), "t");	// LATIN SMALL LETTER T WITH CURL -- no decomposition
                AddToTable(ToUnichar("0250"), "a");	// LATIN SMALL LETTER TURNED A -- no decomposition
                AddToTable(ToUnichar("0251"), "a");	// LATIN SMALL LETTER ALPHA -- no decomposition
                AddToTable(ToUnichar("0252"), "a");	// LATIN SMALL LETTER TURNED ALPHA -- no decomposition
                AddToTable(ToUnichar("0253"), "b");	// LATIN SMALL LETTER B WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0254"), "o");	// LATIN SMALL LETTER OPEN O -- no decomposition
                AddToTable(ToUnichar("0255"), "c");	// LATIN SMALL LETTER C WITH CURL -- no decomposition
                AddToTable(ToUnichar("0256"), "d");	// LATIN SMALL LETTER D WITH TAIL -- no decomposition
                AddToTable(ToUnichar("0257"), "d");	// LATIN SMALL LETTER D WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0258"), "e");	// LATIN SMALL LETTER REVERSED E -- no decomposition
                AddToTable(ToUnichar("0259"), "e");	// LATIN SMALL LETTER SCHWA -- no decomposition
                AddToTable(ToUnichar("025A"), "e");	// LATIN SMALL LETTER SCHWA WITH HOOK -- no decomposition
                AddToTable(ToUnichar("025B"), "e");	// LATIN SMALL LETTER OPEN E -- no decomposition
                AddToTable(ToUnichar("025C"), "e");	// LATIN SMALL LETTER REVERSED OPEN E -- no decomposition
                AddToTable(ToUnichar("025D"), "e");	// LATIN SMALL LETTER REVERSED OPEN E WITH HOOK -- no decomposition
                AddToTable(ToUnichar("025E"), "e");	// LATIN SMALL LETTER CLOSED REVERSED OPEN E -- no decomposition
                AddToTable(ToUnichar("025F"), "j");	// LATIN SMALL LETTER DOTLESS J WITH STROKE -- no decomposition
                AddToTable(ToUnichar("0260"), "g");	// LATIN SMALL LETTER G WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0261"), "g");	// LATIN SMALL LETTER SCRIPT G -- no decomposition
                AddToTable(ToUnichar("0262"), "G");	// LATIN LETTER SMALL CAPITAL G -- no decomposition
                AddToTable(ToUnichar("0263"), "g");	// LATIN SMALL LETTER GAMMA -- no decomposition
                AddToTable(ToUnichar("0264"), "y");	// LATIN SMALL LETTER RAMS HORN -- no decomposition
                AddToTable(ToUnichar("0265"), "h");	// LATIN SMALL LETTER TURNED H -- no decomposition
                AddToTable(ToUnichar("0266"), "h");	// LATIN SMALL LETTER H WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0267"), "h");	// LATIN SMALL LETTER HENG WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0268"), "i");	// LATIN SMALL LETTER I WITH STROKE -- no decomposition
                AddToTable(ToUnichar("0269"), "i");	// LATIN SMALL LETTER IOTA -- no decomposition
                AddToTable(ToUnichar("026A"), "I");	// LATIN LETTER SMALL CAPITAL I -- no decomposition
                AddToTable(ToUnichar("026B"), "l");	// LATIN SMALL LETTER L WITH MIDDLE TILDE -- no decomposition
                AddToTable(ToUnichar("026C"), "l");	// LATIN SMALL LETTER L WITH BELT -- no decomposition
                AddToTable(ToUnichar("026D"), "l");	// LATIN SMALL LETTER L WITH RETROFLEX HOOK -- no decomposition
                AddToTable(ToUnichar("026E"), "lz");	// LATIN SMALL LETTER LEZH -- no decomposition
                AddToTable(ToUnichar("026F"), "m");	// LATIN SMALL LETTER TURNED M -- no decomposition
                AddToTable(ToUnichar("0270"), "m");	// LATIN SMALL LETTER TURNED M WITH LONG LEG -- no decomposition
                AddToTable(ToUnichar("0271"), "m");	// LATIN SMALL LETTER M WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0272"), "n");	// LATIN SMALL LETTER N WITH LEFT HOOK -- no decomposition
                AddToTable(ToUnichar("0273"), "n");	// LATIN SMALL LETTER N WITH RETROFLEX HOOK -- no decomposition
                AddToTable(ToUnichar("0274"), "N");	// LATIN LETTER SMALL CAPITAL N -- no decomposition
                AddToTable(ToUnichar("0275"), "o");	// LATIN SMALL LETTER BARRED O -- no decomposition
                AddToTable(ToUnichar("0276"), "OE");	// LATIN LETTER SMALL CAPITAL OE -- no decomposition
                AddToTable(ToUnichar("0277"), "o");	// LATIN SMALL LETTER CLOSED OMEGA -- no decomposition
                AddToTable(ToUnichar("0278"), "ph");	// LATIN SMALL LETTER PHI -- no decomposition
                AddToTable(ToUnichar("0279"), "r");	// LATIN SMALL LETTER TURNED R -- no decomposition
                AddToTable(ToUnichar("027A"), "r");	// LATIN SMALL LETTER TURNED R WITH LONG LEG -- no decomposition
                AddToTable(ToUnichar("027B"), "r");	// LATIN SMALL LETTER TURNED R WITH HOOK -- no decomposition
                AddToTable(ToUnichar("027C"), "r");	// LATIN SMALL LETTER R WITH LONG LEG -- no decomposition
                AddToTable(ToUnichar("027D"), "r");	// LATIN SMALL LETTER R WITH TAIL -- no decomposition
                AddToTable(ToUnichar("027E"), "r");	// LATIN SMALL LETTER R WITH FISHHOOK -- no decomposition
                AddToTable(ToUnichar("027F"), "r");	// LATIN SMALL LETTER REVERSED R WITH FISHHOOK -- no decomposition
                AddToTable(ToUnichar("0280"), "R");	// LATIN LETTER SMALL CAPITAL R -- no decomposition
                AddToTable(ToUnichar("0281"), "r");	// LATIN LETTER SMALL CAPITAL INVERTED R -- no decomposition
                AddToTable(ToUnichar("0282"), "s");	// LATIN SMALL LETTER S WITH HOOK -- no decomposition
                AddToTable(ToUnichar("0283"), "s");	// LATIN SMALL LETTER ESH -- no decomposition
                AddToTable(ToUnichar("0284"), "j");	// LATIN SMALL LETTER DOTLESS J WITH STROKE AND HOOK -- no decomposition
                AddToTable(ToUnichar("0285"), "s");	// LATIN SMALL LETTER SQUAT REVERSED ESH -- no decomposition
                AddToTable(ToUnichar("0286"), "s");	// LATIN SMALL LETTER ESH WITH CURL -- no decomposition
                AddToTable(ToUnichar("0287"), "y");	// LATIN SMALL LETTER TURNED T -- no decomposition
                AddToTable(ToUnichar("0288"), "t");	// LATIN SMALL LETTER T WITH RETROFLEX HOOK -- no decomposition
                AddToTable(ToUnichar("0289"), "u");	// LATIN SMALL LETTER U BAR -- no decomposition
                AddToTable(ToUnichar("028A"), "u");	// LATIN SMALL LETTER UPSILON -- no decomposition
                AddToTable(ToUnichar("028B"), "u");	// LATIN SMALL LETTER V WITH HOOK -- no decomposition
                AddToTable(ToUnichar("028C"), "v");	// LATIN SMALL LETTER TURNED V -- no decomposition
                AddToTable(ToUnichar("028D"), "w");	// LATIN SMALL LETTER TURNED W -- no decomposition
                AddToTable(ToUnichar("028E"), "y");	// LATIN SMALL LETTER TURNED Y -- no decomposition
                AddToTable(ToUnichar("028F"), "Y");	// LATIN LETTER SMALL CAPITAL Y -- no decomposition
                AddToTable(ToUnichar("0290"), "z");	// LATIN SMALL LETTER Z WITH RETROFLEX HOOK -- no decomposition
                AddToTable(ToUnichar("0291"), "z");	// LATIN SMALL LETTER Z WITH CURL -- no decomposition
                AddToTable(ToUnichar("0292"), "z");	// LATIN SMALL LETTER EZH -- no decomposition
                AddToTable(ToUnichar("0293"), "z");	// LATIN SMALL LETTER EZH WITH CURL -- no decomposition
                AddToTable(ToUnichar("0294"), "'");	// LATIN LETTER GLOTTAL STOP -- no decomposition
                AddToTable(ToUnichar("0295"), "'");	// LATIN LETTER PHARYNGEAL VOICED FRICATIVE -- no decomposition
                AddToTable(ToUnichar("0296"), "'");	// LATIN LETTER INVERTED GLOTTAL STOP -- no decomposition
                AddToTable(ToUnichar("0297"), "C");	// LATIN LETTER STRETCHED C -- no decomposition
                AddToTable(ToUnichar("0298"), "O˜");	// LATIN LETTER BILABIAL CLICK -- no decomposition
                AddToTable(ToUnichar("0299"), "B");	// LATIN LETTER SMALL CAPITAL B -- no decomposition
                AddToTable(ToUnichar("029A"), "e");	// LATIN SMALL LETTER CLOSED OPEN E -- no decomposition
                AddToTable(ToUnichar("029B"), "G");	// LATIN LETTER SMALL CAPITAL G WITH HOOK -- no decomposition
                AddToTable(ToUnichar("029C"), "H");	// LATIN LETTER SMALL CAPITAL H -- no decomposition
                AddToTable(ToUnichar("029D"), "j");	// LATIN SMALL LETTER J WITH CROSSED-TAIL -- no decomposition
                AddToTable(ToUnichar("029E"), "k");	// LATIN SMALL LETTER TURNED K -- no decomposition
                AddToTable(ToUnichar("029F"), "L");	// LATIN LETTER SMALL CAPITAL L -- no decomposition
                AddToTable(ToUnichar("02A0"), "q");	// LATIN SMALL LETTER Q WITH HOOK -- no decomposition
                AddToTable(ToUnichar("02A1"), "'");	// LATIN LETTER GLOTTAL STOP WITH STROKE -- no decomposition
                AddToTable(ToUnichar("02A2"), "'");	// LATIN LETTER REVERSED GLOTTAL STOP WITH STROKE -- no decomposition
                AddToTable(ToUnichar("02A3"), "dz");	// LATIN SMALL LETTER DZ DIGRAPH -- no decomposition
                AddToTable(ToUnichar("02A4"), "dz");	// LATIN SMALL LETTER DEZH DIGRAPH -- no decomposition
                AddToTable(ToUnichar("02A5"), "dz");	// LATIN SMALL LETTER DZ DIGRAPH WITH CURL -- no decomposition
                AddToTable(ToUnichar("02A6"), "ts");	// LATIN SMALL LETTER TS DIGRAPH -- no decomposition
                AddToTable(ToUnichar("02A7"), "ts");	// LATIN SMALL LETTER TESH DIGRAPH -- no decomposition
                AddToTable(ToUnichar("02A8"), ""); // LATIN SMALL LETTER TC DIGRAPH WITH CURL -- no decomposition
                AddToTable(ToUnichar("02A9"), "fn");	// LATIN SMALL LETTER FENG DIGRAPH -- no decomposition
                AddToTable(ToUnichar("02AA"), "ls");	// LATIN SMALL LETTER LS DIGRAPH -- no decomposition
                AddToTable(ToUnichar("02AB"), "lz");	// LATIN SMALL LETTER LZ DIGRAPH -- no decomposition
                AddToTable(ToUnichar("02AC"), "w");	// LATIN LETTER BILABIAL PERCUSSIVE -- no decomposition
                AddToTable(ToUnichar("02AD"), "t");	// LATIN LETTER BIDENTAL PERCUSSIVE -- no decomposition
                AddToTable(ToUnichar("02AE"), "h");	// LATIN SMALL LETTER TURNED H WITH FISHHOOK -- no decomposition
                AddToTable(ToUnichar("02AF"), "h");	// LATIN SMALL LETTER TURNED H WITH FISHHOOK AND TAIL -- no decomposition
                AddToTable(ToUnichar("02B0"), "h");	// MODIFIER LETTER SMALL H
                AddToTable(ToUnichar("02B1"), "h");	// MODIFIER LETTER SMALL H WITH HOOK
                AddToTable(ToUnichar("02B2"), "j");	// MODIFIER LETTER SMALL J
                AddToTable(ToUnichar("02B3"), "r");	// MODIFIER LETTER SMALL R
                AddToTable(ToUnichar("02B4"), "r");	// MODIFIER LETTER SMALL TURNED R
                AddToTable(ToUnichar("02B5"), "r");	// MODIFIER LETTER SMALL TURNED R WITH HOOK
                AddToTable(ToUnichar("02B6"), "R");	// MODIFIER LETTER SMALL CAPITAL INVERTED R
                AddToTable(ToUnichar("02B7"), "w");	// MODIFIER LETTER SMALL W
                AddToTable(ToUnichar("02B8"), "y");	// MODIFIER LETTER SMALL Y
                AddToTable(ToUnichar("02E1"), "l");	// MODIFIER LETTER SMALL L
                AddToTable(ToUnichar("02E2"), "s");	// MODIFIER LETTER SMALL S
                AddToTable(ToUnichar("02E3"), "x");	// MODIFIER LETTER SMALL X
                AddToTable(ToUnichar("02E4"), "'");	// MODIFIER LETTER SMALL REVERSED GLOTTAL STOP
                AddToTable(ToUnichar("1D00"), "A");	// LATIN LETTER SMALL CAPITAL A -- no decomposition
                AddToTable(ToUnichar("1D01"), "AE");	// LATIN LETTER SMALL CAPITAL AE -- no decomposition
                AddToTable(ToUnichar("1D02"), "ae");	// LATIN SMALL LETTER TURNED AE -- no decomposition
                AddToTable(ToUnichar("1D03"), "B");	// LATIN LETTER SMALL CAPITAL BARRED B -- no decomposition
                AddToTable(ToUnichar("1D04"), "C");	// LATIN LETTER SMALL CAPITAL C -- no decomposition
                AddToTable(ToUnichar("1D05"), "D");	// LATIN LETTER SMALL CAPITAL D -- no decomposition
                AddToTable(ToUnichar("1D06"), "TH");	// LATIN LETTER SMALL CAPITAL ETH -- no decomposition
                AddToTable(ToUnichar("1D07"), "E");	// LATIN LETTER SMALL CAPITAL E -- no decomposition
                AddToTable(ToUnichar("1D08"), "e");	// LATIN SMALL LETTER TURNED OPEN E -- no decomposition
                AddToTable(ToUnichar("1D09"), "i");	// LATIN SMALL LETTER TURNED I -- no decomposition
                AddToTable(ToUnichar("1D0A"), "J");	// LATIN LETTER SMALL CAPITAL J -- no decomposition
                AddToTable(ToUnichar("1D0B"), "K");	// LATIN LETTER SMALL CAPITAL K -- no decomposition
                AddToTable(ToUnichar("1D0C"), "L");	// LATIN LETTER SMALL CAPITAL L WITH STROKE -- no decomposition
                AddToTable(ToUnichar("1D0D"), "M");	// LATIN LETTER SMALL CAPITAL M -- no decomposition
                AddToTable(ToUnichar("1D0E"), "N");	// LATIN LETTER SMALL CAPITAL REVERSED N -- no decomposition
                AddToTable(ToUnichar("1D0F"), "O");	// LATIN LETTER SMALL CAPITAL O -- no decomposition
                AddToTable(ToUnichar("1D10"), "O");	// LATIN LETTER SMALL CAPITAL OPEN O -- no decomposition
                AddToTable(ToUnichar("1D11"), "o");	// LATIN SMALL LETTER SIDEWAYS O -- no decomposition
                AddToTable(ToUnichar("1D12"), "o");	// LATIN SMALL LETTER SIDEWAYS OPEN O -- no decomposition
                AddToTable(ToUnichar("1D13"), "o");	// LATIN SMALL LETTER SIDEWAYS O WITH STROKE -- no decomposition
                AddToTable(ToUnichar("1D14"), "oe");	// LATIN SMALL LETTER TURNED OE -- no decomposition
                AddToTable(ToUnichar("1D15"), "ou");	// LATIN LETTER SMALL CAPITAL OU -- no decomposition
                AddToTable(ToUnichar("1D16"), "o");	// LATIN SMALL LETTER TOP HALF O -- no decomposition
                AddToTable(ToUnichar("1D17"), "o");	// LATIN SMALL LETTER BOTTOM HALF O -- no decomposition
                AddToTable(ToUnichar("1D18"), "P");	// LATIN LETTER SMALL CAPITAL P -- no decomposition
                AddToTable(ToUnichar("1D19"), "R");	// LATIN LETTER SMALL CAPITAL REVERSED R -- no decomposition
                AddToTable(ToUnichar("1D1A"), "R");	// LATIN LETTER SMALL CAPITAL TURNED R -- no decomposition
                AddToTable(ToUnichar("1D1B"), "T");	// LATIN LETTER SMALL CAPITAL T -- no decomposition
                AddToTable(ToUnichar("1D1C"), "U");	// LATIN LETTER SMALL CAPITAL U -- no decomposition
                AddToTable(ToUnichar("1D1D"), "u");	// LATIN SMALL LETTER SIDEWAYS U -- no decomposition
                AddToTable(ToUnichar("1D1E"), "u");	// LATIN SMALL LETTER SIDEWAYS DIAERESIZED U -- no decomposition
                AddToTable(ToUnichar("1D1F"), "m");	// LATIN SMALL LETTER SIDEWAYS TURNED M -- no decomposition
                AddToTable(ToUnichar("1D20"), "V");	// LATIN LETTER SMALL CAPITAL V -- no decomposition
                AddToTable(ToUnichar("1D21"), "W");	// LATIN LETTER SMALL CAPITAL W -- no decomposition
                AddToTable(ToUnichar("1D22"), "Z");	// LATIN LETTER SMALL CAPITAL Z -- no decomposition
                AddToTable(ToUnichar("1D23"), "EZH");	// LATIN LETTER SMALL CAPITAL EZH -- no decomposition
                AddToTable(ToUnichar("1D24"), "'");	// LATIN LETTER VOICED LARYNGEAL SPIRANT -- no decomposition
                AddToTable(ToUnichar("1D25"), "L");	// LATIN LETTER AIN -- no decomposition
                AddToTable(ToUnichar("1D2C"), "A");	// MODIFIER LETTER CAPITAL A
                AddToTable(ToUnichar("1D2D"), "AE");	// MODIFIER LETTER CAPITAL AE
                AddToTable(ToUnichar("1D2E"), "B");	// MODIFIER LETTER CAPITAL B
                AddToTable(ToUnichar("1D2F"), "B");	// MODIFIER LETTER CAPITAL BARRED B -- no decomposition
                AddToTable(ToUnichar("1D30"), "D");	// MODIFIER LETTER CAPITAL D
                AddToTable(ToUnichar("1D31"), "E");	// MODIFIER LETTER CAPITAL E
                AddToTable(ToUnichar("1D32"), "E");	// MODIFIER LETTER CAPITAL REVERSED E
                AddToTable(ToUnichar("1D33"), "G");	// MODIFIER LETTER CAPITAL G
                AddToTable(ToUnichar("1D34"), "H");	// MODIFIER LETTER CAPITAL H
                AddToTable(ToUnichar("1D35"), "I");	// MODIFIER LETTER CAPITAL I
                AddToTable(ToUnichar("1D36"), "J");	// MODIFIER LETTER CAPITAL J
                AddToTable(ToUnichar("1D37"), "K");	// MODIFIER LETTER CAPITAL K
                AddToTable(ToUnichar("1D38"), "L");	// MODIFIER LETTER CAPITAL L
                AddToTable(ToUnichar("1D39"), "M");	// MODIFIER LETTER CAPITAL M
                AddToTable(ToUnichar("1D3A"), "N");	// MODIFIER LETTER CAPITAL N
                AddToTable(ToUnichar("1D3B"), "N");	// MODIFIER LETTER CAPITAL REVERSED N -- no decomposition
                AddToTable(ToUnichar("1D3C"), "O");	// MODIFIER LETTER CAPITAL O
                AddToTable(ToUnichar("1D3D"), "OU");	// MODIFIER LETTER CAPITAL OU
                AddToTable(ToUnichar("1D3E"), "P");	// MODIFIER LETTER CAPITAL P
                AddToTable(ToUnichar("1D3F"), "R");	// MODIFIER LETTER CAPITAL R
                AddToTable(ToUnichar("1D40"), "T");	// MODIFIER LETTER CAPITAL T
                AddToTable(ToUnichar("1D41"), "U");	// MODIFIER LETTER CAPITAL U
                AddToTable(ToUnichar("1D42"), "W");	// MODIFIER LETTER CAPITAL W
                AddToTable(ToUnichar("1D43"), "a");	// MODIFIER LETTER SMALL A
                AddToTable(ToUnichar("1D44"), "a");	// MODIFIER LETTER SMALL TURNED A
                AddToTable(ToUnichar("1D46"), "ae");	// MODIFIER LETTER SMALL TURNED AE
                AddToTable(ToUnichar("1D47"), "b");    // MODIFIER LETTER SMALL B
                AddToTable(ToUnichar("1D48"), "d");    // MODIFIER LETTER SMALL D
                AddToTable(ToUnichar("1D49"), "e");    // MODIFIER LETTER SMALL E
                AddToTable(ToUnichar("1D4A"), "e");    // MODIFIER LETTER SMALL SCHWA
                AddToTable(ToUnichar("1D4B"), "e");    // MODIFIER LETTER SMALL OPEN E
                AddToTable(ToUnichar("1D4C"), "e");    // MODIFIER LETTER SMALL TURNED OPEN E
                AddToTable(ToUnichar("1D4D"), "g");    // MODIFIER LETTER SMALL G
                AddToTable(ToUnichar("1D4E"), "i");    // MODIFIER LETTER SMALL TURNED I -- no decomposition
                AddToTable(ToUnichar("1D4F"), "k");    // MODIFIER LETTER SMALL K
                AddToTable(ToUnichar("1D50"), "m");	// MODIFIER LETTER SMALL M
                AddToTable(ToUnichar("1D51"), "g");	// MODIFIER LETTER SMALL ENG
                AddToTable(ToUnichar("1D52"), "o");	// MODIFIER LETTER SMALL O
                AddToTable(ToUnichar("1D53"), "o");	// MODIFIER LETTER SMALL OPEN O
                AddToTable(ToUnichar("1D54"), "o");	// MODIFIER LETTER SMALL TOP HALF O
                AddToTable(ToUnichar("1D55"), "o");	// MODIFIER LETTER SMALL BOTTOM HALF O
                AddToTable(ToUnichar("1D56"), "p");	// MODIFIER LETTER SMALL P
                AddToTable(ToUnichar("1D57"), "t");	// MODIFIER LETTER SMALL T
                AddToTable(ToUnichar("1D58"), "u");	// MODIFIER LETTER SMALL U
                AddToTable(ToUnichar("1D59"), "u");	// MODIFIER LETTER SMALL SIDEWAYS U
                AddToTable(ToUnichar("1D5A"), "m");	// MODIFIER LETTER SMALL TURNED M
                AddToTable(ToUnichar("1D5B"), "v");	// MODIFIER LETTER SMALL V
                AddToTable(ToUnichar("1D62"), "i");	// LATIN SUBSCRIPT SMALL LETTER I
                AddToTable(ToUnichar("1D63"), "r");	// LATIN SUBSCRIPT SMALL LETTER R
                AddToTable(ToUnichar("1D64"), "u");	// LATIN SUBSCRIPT SMALL LETTER U
                AddToTable(ToUnichar("1D65"), "v");	// LATIN SUBSCRIPT SMALL LETTER V
                AddToTable(ToUnichar("1D6B"), "ue");	// LATIN SMALL LETTER UE -- no decomposition
                AddToTable(ToUnichar("1E00"), "A");	// LATIN CAPITAL LETTER A WITH RING BELOW
                AddToTable(ToUnichar("1E01"), "a");	// LATIN SMALL LETTER A WITH RING BELOW
                AddToTable(ToUnichar("1E02"), "B");	// LATIN CAPITAL LETTER B WITH DOT ABOVE
                AddToTable(ToUnichar("1E03"), "b");	// LATIN SMALL LETTER B WITH DOT ABOVE
                AddToTable(ToUnichar("1E04"), "B");	// LATIN CAPITAL LETTER B WITH DOT BELOW
                AddToTable(ToUnichar("1E05"), "b");	// LATIN SMALL LETTER B WITH DOT BELOW
                AddToTable(ToUnichar("1E06"), "B");	// LATIN CAPITAL LETTER B WITH LINE BELOW
                AddToTable(ToUnichar("1E07"), "b");	// LATIN SMALL LETTER B WITH LINE BELOW
                AddToTable(ToUnichar("1E08"), "C");	// LATIN CAPITAL LETTER C WITH CEDILLA AND ACUTE
                AddToTable(ToUnichar("1E09"), "c");	// LATIN SMALL LETTER C WITH CEDILLA AND ACUTE
                AddToTable(ToUnichar("1E0A"), "D");	// LATIN CAPITAL LETTER D WITH DOT ABOVE
                AddToTable(ToUnichar("1E0B"), "d");	// LATIN SMALL LETTER D WITH DOT ABOVE
                AddToTable(ToUnichar("1E0C"), "D");	// LATIN CAPITAL LETTER D WITH DOT BELOW
                AddToTable(ToUnichar("1E0D"), "d");	// LATIN SMALL LETTER D WITH DOT BELOW
                AddToTable(ToUnichar("1E0E"), "D");	// LATIN CAPITAL LETTER D WITH LINE BELOW
                AddToTable(ToUnichar("1E0F"), "d");	// LATIN SMALL LETTER D WITH LINE BELOW
                AddToTable(ToUnichar("1E10"), "D");	// LATIN CAPITAL LETTER D WITH CEDILLA
                AddToTable(ToUnichar("1E11"), "d");	// LATIN SMALL LETTER D WITH CEDILLA
                AddToTable(ToUnichar("1E12"), "D");	// LATIN CAPITAL LETTER D WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E13"), "d");	// LATIN SMALL LETTER D WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E14"), "E");	// LATIN CAPITAL LETTER E WITH MACRON AND GRAVE
                AddToTable(ToUnichar("1E15"), "e");	// LATIN SMALL LETTER E WITH MACRON AND GRAVE
                AddToTable(ToUnichar("1E16"), "E");	// LATIN CAPITAL LETTER E WITH MACRON AND ACUTE
                AddToTable(ToUnichar("1E17"), "e");	// LATIN SMALL LETTER E WITH MACRON AND ACUTE
                AddToTable(ToUnichar("1E18"), "E");	// LATIN CAPITAL LETTER E WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E19"), "e");	// LATIN SMALL LETTER E WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E1A"), "E");	// LATIN CAPITAL LETTER E WITH TILDE BELOW
                AddToTable(ToUnichar("1E1B"), "e");	// LATIN SMALL LETTER E WITH TILDE BELOW
                AddToTable(ToUnichar("1E1C"), "E");	// LATIN CAPITAL LETTER E WITH CEDILLA AND BREVE
                AddToTable(ToUnichar("1E1D"), "e");	// LATIN SMALL LETTER E WITH CEDILLA AND BREVE
                AddToTable(ToUnichar("1E1E"), "F");	// LATIN CAPITAL LETTER F WITH DOT ABOVE
                AddToTable(ToUnichar("1E1F"), "f");	// LATIN SMALL LETTER F WITH DOT ABOVE
                AddToTable(ToUnichar("1E20"), "G");	// LATIN CAPITAL LETTER G WITH MACRON
                AddToTable(ToUnichar("1E21"), "g");	// LATIN SMALL LETTER G WITH MACRON
                AddToTable(ToUnichar("1E22"), "H");	// LATIN CAPITAL LETTER H WITH DOT ABOVE
                AddToTable(ToUnichar("1E23"), "h");	// LATIN SMALL LETTER H WITH DOT ABOVE
                AddToTable(ToUnichar("1E24"), "H");	// LATIN CAPITAL LETTER H WITH DOT BELOW
                AddToTable(ToUnichar("1E25"), "h");	// LATIN SMALL LETTER H WITH DOT BELOW
                AddToTable(ToUnichar("1E26"), "H");	// LATIN CAPITAL LETTER H WITH DIAERESIS
                AddToTable(ToUnichar("1E27"), "h");	// LATIN SMALL LETTER H WITH DIAERESIS
                AddToTable(ToUnichar("1E28"), "H");	// LATIN CAPITAL LETTER H WITH CEDILLA
                AddToTable(ToUnichar("1E29"), "h");	// LATIN SMALL LETTER H WITH CEDILLA
                AddToTable(ToUnichar("1E2A"), "H");	// LATIN CAPITAL LETTER H WITH BREVE BELOW
                AddToTable(ToUnichar("1E2B"), "h");	// LATIN SMALL LETTER H WITH BREVE BELOW
                AddToTable(ToUnichar("1E2C"), "I");	// LATIN CAPITAL LETTER I WITH TILDE BELOW
                AddToTable(ToUnichar("1E2D"), "i");	// LATIN SMALL LETTER I WITH TILDE BELOW
                AddToTable(ToUnichar("1E2E"), "I");	// LATIN CAPITAL LETTER I WITH DIAERESIS AND ACUTE
                AddToTable(ToUnichar("1E2F"), "i");	// LATIN SMALL LETTER I WITH DIAERESIS AND ACUTE
                AddToTable(ToUnichar("1E30"), "K");	// LATIN CAPITAL LETTER K WITH ACUTE
                AddToTable(ToUnichar("1E31"), "k");	// LATIN SMALL LETTER K WITH ACUTE
                AddToTable(ToUnichar("1E32"), "K");	// LATIN CAPITAL LETTER K WITH DOT BELOW
                AddToTable(ToUnichar("1E33"), "k");	// LATIN SMALL LETTER K WITH DOT BELOW
                AddToTable(ToUnichar("1E34"), "K");	// LATIN CAPITAL LETTER K WITH LINE BELOW
                AddToTable(ToUnichar("1E35"), "k");	// LATIN SMALL LETTER K WITH LINE BELOW
                AddToTable(ToUnichar("1E36"), "L");	// LATIN CAPITAL LETTER L WITH DOT BELOW
                AddToTable(ToUnichar("1E37"), "l");	// LATIN SMALL LETTER L WITH DOT BELOW
                AddToTable(ToUnichar("1E38"), "L");	// LATIN CAPITAL LETTER L WITH DOT BELOW AND MACRON
                AddToTable(ToUnichar("1E39"), "l");	// LATIN SMALL LETTER L WITH DOT BELOW AND MACRON
                AddToTable(ToUnichar("1E3A"), "L");	// LATIN CAPITAL LETTER L WITH LINE BELOW
                AddToTable(ToUnichar("1E3B"), "l");	// LATIN SMALL LETTER L WITH LINE BELOW
                AddToTable(ToUnichar("1E3C"), "L");	// LATIN CAPITAL LETTER L WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E3D"), "l");	// LATIN SMALL LETTER L WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E3E"), "M");	// LATIN CAPITAL LETTER M WITH ACUTE
                AddToTable(ToUnichar("1E3F"), "m");	// LATIN SMALL LETTER M WITH ACUTE
                AddToTable(ToUnichar("1E40"), "M");	// LATIN CAPITAL LETTER M WITH DOT ABOVE
                AddToTable(ToUnichar("1E41"), "m");	// LATIN SMALL LETTER M WITH DOT ABOVE
                AddToTable(ToUnichar("1E42"), "M");	// LATIN CAPITAL LETTER M WITH DOT BELOW
                AddToTable(ToUnichar("1E43"), "m");	// LATIN SMALL LETTER M WITH DOT BELOW
                AddToTable(ToUnichar("1E44"), "N");	// LATIN CAPITAL LETTER N WITH DOT ABOVE
                AddToTable(ToUnichar("1E45"), "n");	// LATIN SMALL LETTER N WITH DOT ABOVE
                AddToTable(ToUnichar("1E46"), "N");	// LATIN CAPITAL LETTER N WITH DOT BELOW
                AddToTable(ToUnichar("1E47"), "n");	// LATIN SMALL LETTER N WITH DOT BELOW
                AddToTable(ToUnichar("1E48"), "N");	// LATIN CAPITAL LETTER N WITH LINE BELOW
                AddToTable(ToUnichar("1E49"), "n");	// LATIN SMALL LETTER N WITH LINE BELOW
                AddToTable(ToUnichar("1E4A"), "N");	// LATIN CAPITAL LETTER N WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E4B"), "n");	// LATIN SMALL LETTER N WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E4C"), "O");	// LATIN CAPITAL LETTER O WITH TILDE AND ACUTE
                AddToTable(ToUnichar("1E4D"), "o");	// LATIN SMALL LETTER O WITH TILDE AND ACUTE
                AddToTable(ToUnichar("1E4E"), "O");	// LATIN CAPITAL LETTER O WITH TILDE AND DIAERESIS
                AddToTable(ToUnichar("1E4F"), "o");	// LATIN SMALL LETTER O WITH TILDE AND DIAERESIS
                AddToTable(ToUnichar("1E50"), "O");	// LATIN CAPITAL LETTER O WITH MACRON AND GRAVE
                AddToTable(ToUnichar("1E51"), "o");	// LATIN SMALL LETTER O WITH MACRON AND GRAVE
                AddToTable(ToUnichar("1E52"), "O");	// LATIN CAPITAL LETTER O WITH MACRON AND ACUTE
                AddToTable(ToUnichar("1E53"), "o");	// LATIN SMALL LETTER O WITH MACRON AND ACUTE
                AddToTable(ToUnichar("1E54"), "P");	// LATIN CAPITAL LETTER P WITH ACUTE
                AddToTable(ToUnichar("1E55"), "p");	// LATIN SMALL LETTER P WITH ACUTE
                AddToTable(ToUnichar("1E56"), "P");	// LATIN CAPITAL LETTER P WITH DOT ABOVE
                AddToTable(ToUnichar("1E57"), "p");	// LATIN SMALL LETTER P WITH DOT ABOVE
                AddToTable(ToUnichar("1E58"), "R");	// LATIN CAPITAL LETTER R WITH DOT ABOVE
                AddToTable(ToUnichar("1E59"), "r");	// LATIN SMALL LETTER R WITH DOT ABOVE
                AddToTable(ToUnichar("1E5A"), "R");	// LATIN CAPITAL LETTER R WITH DOT BELOW
                AddToTable(ToUnichar("1E5B"), "r");	// LATIN SMALL LETTER R WITH DOT BELOW
                AddToTable(ToUnichar("1E5C"), "R");	// LATIN CAPITAL LETTER R WITH DOT BELOW AND MACRON
                AddToTable(ToUnichar("1E5D"), "r");	// LATIN SMALL LETTER R WITH DOT BELOW AND MACRON
                AddToTable(ToUnichar("1E5E"), "R");	// LATIN CAPITAL LETTER R WITH LINE BELOW
                AddToTable(ToUnichar("1E5F"), "r");	// LATIN SMALL LETTER R WITH LINE BELOW
                AddToTable(ToUnichar("1E60"), "S");	// LATIN CAPITAL LETTER S WITH DOT ABOVE
                AddToTable(ToUnichar("1E61"), "s");	// LATIN SMALL LETTER S WITH DOT ABOVE
                AddToTable(ToUnichar("1E62"), "S");	// LATIN CAPITAL LETTER S WITH DOT BELOW
                AddToTable(ToUnichar("1E63"), "s");	// LATIN SMALL LETTER S WITH DOT BELOW
                AddToTable(ToUnichar("1E64"), "S");	// LATIN CAPITAL LETTER S WITH ACUTE AND DOT ABOVE
                AddToTable(ToUnichar("1E65"), "s");	// LATIN SMALL LETTER S WITH ACUTE AND DOT ABOVE
                AddToTable(ToUnichar("1E66"), "S");	// LATIN CAPITAL LETTER S WITH CARON AND DOT ABOVE
                AddToTable(ToUnichar("1E67"), "s");	// LATIN SMALL LETTER S WITH CARON AND DOT ABOVE
                AddToTable(ToUnichar("1E68"), "S");	// LATIN CAPITAL LETTER S WITH DOT BELOW AND DOT ABOVE
                AddToTable(ToUnichar("1E69"), "s");	// LATIN SMALL LETTER S WITH DOT BELOW AND DOT ABOVE
                AddToTable(ToUnichar("1E6A"), "T");	// LATIN CAPITAL LETTER T WITH DOT ABOVE
                AddToTable(ToUnichar("1E6B"), "t");	// LATIN SMALL LETTER T WITH DOT ABOVE
                AddToTable(ToUnichar("1E6C"), "T");	// LATIN CAPITAL LETTER T WITH DOT BELOW
                AddToTable(ToUnichar("1E6D"), "t");	// LATIN SMALL LETTER T WITH DOT BELOW
                AddToTable(ToUnichar("1E6E"), "T");	// LATIN CAPITAL LETTER T WITH LINE BELOW
                AddToTable(ToUnichar("1E6F"), "t");	// LATIN SMALL LETTER T WITH LINE BELOW
                AddToTable(ToUnichar("1E70"), "T");	// LATIN CAPITAL LETTER T WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E71"), "t");	// LATIN SMALL LETTER T WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E72"), "U");	// LATIN CAPITAL LETTER U WITH DIAERESIS BELOW
                AddToTable(ToUnichar("1E73"), "u");	// LATIN SMALL LETTER U WITH DIAERESIS BELOW
                AddToTable(ToUnichar("1E74"), "U");	// LATIN CAPITAL LETTER U WITH TILDE BELOW
                AddToTable(ToUnichar("1E75"), "u");	// LATIN SMALL LETTER U WITH TILDE BELOW
                AddToTable(ToUnichar("1E76"), "U");	// LATIN CAPITAL LETTER U WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E77"), "u");	// LATIN SMALL LETTER U WITH CIRCUMFLEX BELOW
                AddToTable(ToUnichar("1E78"), "U");	// LATIN CAPITAL LETTER U WITH TILDE AND ACUTE
                AddToTable(ToUnichar("1E79"), "u");	// LATIN SMALL LETTER U WITH TILDE AND ACUTE
                AddToTable(ToUnichar("1E7A"), "U");	// LATIN CAPITAL LETTER U WITH MACRON AND DIAERESIS
                AddToTable(ToUnichar("1E7B"), "u");	// LATIN SMALL LETTER U WITH MACRON AND DIAERESIS
                AddToTable(ToUnichar("1E7C"), "V");	// LATIN CAPITAL LETTER V WITH TILDE
                AddToTable(ToUnichar("1E7D"), "v");	// LATIN SMALL LETTER V WITH TILDE
                AddToTable(ToUnichar("1E7E"), "V");	// LATIN CAPITAL LETTER V WITH DOT BELOW
                AddToTable(ToUnichar("1E7F"), "v");	// LATIN SMALL LETTER V WITH DOT BELOW
                AddToTable(ToUnichar("1E80"), "W");	// LATIN CAPITAL LETTER W WITH GRAVE
                AddToTable(ToUnichar("1E81"), "w");	// LATIN SMALL LETTER W WITH GRAVE
                AddToTable(ToUnichar("1E82"), "W");	// LATIN CAPITAL LETTER W WITH ACUTE
                AddToTable(ToUnichar("1E83"), "w");	// LATIN SMALL LETTER W WITH ACUTE
                AddToTable(ToUnichar("1E84"), "W");	// LATIN CAPITAL LETTER W WITH DIAERESIS
                AddToTable(ToUnichar("1E85"), "w");	// LATIN SMALL LETTER W WITH DIAERESIS
                AddToTable(ToUnichar("1E86"), "W");	// LATIN CAPITAL LETTER W WITH DOT ABOVE
                AddToTable(ToUnichar("1E87"), "w");	// LATIN SMALL LETTER W WITH DOT ABOVE
                AddToTable(ToUnichar("1E88"), "W");	// LATIN CAPITAL LETTER W WITH DOT BELOW
                AddToTable(ToUnichar("1E89"), "w");	// LATIN SMALL LETTER W WITH DOT BELOW
                AddToTable(ToUnichar("1E8A"), "X");	// LATIN CAPITAL LETTER X WITH DOT ABOVE
                AddToTable(ToUnichar("1E8B"), "x");	// LATIN SMALL LETTER X WITH DOT ABOVE
                AddToTable(ToUnichar("1E8C"), "X");	// LATIN CAPITAL LETTER X WITH DIAERESIS
                AddToTable(ToUnichar("1E8D"), "x");	// LATIN SMALL LETTER X WITH DIAERESIS
                AddToTable(ToUnichar("1E8E"), "Y");	// LATIN CAPITAL LETTER Y WITH DOT ABOVE
                AddToTable(ToUnichar("1E8F"), "y");	// LATIN SMALL LETTER Y WITH DOT ABOVE
                AddToTable(ToUnichar("1E90"), "Z");	// LATIN CAPITAL LETTER Z WITH CIRCUMFLEX
                AddToTable(ToUnichar("1E91"), "z");	// LATIN SMALL LETTER Z WITH CIRCUMFLEX
                AddToTable(ToUnichar("1E92"), "Z");	// LATIN CAPITAL LETTER Z WITH DOT BELOW
                AddToTable(ToUnichar("1E93"), "z");	// LATIN SMALL LETTER Z WITH DOT BELOW
                AddToTable(ToUnichar("1E94"), "Z");	// LATIN CAPITAL LETTER Z WITH LINE BELOW
                AddToTable(ToUnichar("1E95"), "z");	// LATIN SMALL LETTER Z WITH LINE BELOW
                AddToTable(ToUnichar("1E96"), "h");	// LATIN SMALL LETTER H WITH LINE BELOW
                AddToTable(ToUnichar("1E97"), "t");	// LATIN SMALL LETTER T WITH DIAERESIS
                AddToTable(ToUnichar("1E98"), "w");	// LATIN SMALL LETTER W WITH RING ABOVE
                AddToTable(ToUnichar("1E99"), "y");	// LATIN SMALL LETTER Y WITH RING ABOVE
                AddToTable(ToUnichar("1E9A"), "a");	// LATIN SMALL LETTER A WITH RIGHT HALF RING
                AddToTable(ToUnichar("1E9B"), "s");	// LATIN SMALL LETTER LONG S WITH DOT ABOVE
                AddToTable(ToUnichar("1EA0"), "A");	// LATIN CAPITAL LETTER A WITH DOT BELOW
                AddToTable(ToUnichar("1EA1"), "a");	// LATIN SMALL LETTER A WITH DOT BELOW
                AddToTable(ToUnichar("1EA2"), "A");	// LATIN CAPITAL LETTER A WITH HOOK ABOVE
                AddToTable(ToUnichar("1EA3"), "a");	// LATIN SMALL LETTER A WITH HOOK ABOVE
                AddToTable(ToUnichar("1EA4"), "A");	// LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND ACUTE
                AddToTable(ToUnichar("1EA5"), "a");	// LATIN SMALL LETTER A WITH CIRCUMFLEX AND ACUTE
                AddToTable(ToUnichar("1EA6"), "A");	// LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND GRAVE
                AddToTable(ToUnichar("1EA7"), "a");	// LATIN SMALL LETTER A WITH CIRCUMFLEX AND GRAVE
                AddToTable(ToUnichar("1EA8"), "A");	// LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND HOOK ABOVE
                AddToTable(ToUnichar("1EA9"), "a");	// LATIN SMALL LETTER A WITH CIRCUMFLEX AND HOOK ABOVE
                AddToTable(ToUnichar("1EAA"), "A");	// LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND TILDE
                AddToTable(ToUnichar("1EAB"), "a");	// LATIN SMALL LETTER A WITH CIRCUMFLEX AND TILDE
                AddToTable(ToUnichar("1EAC"), "A");	// LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND DOT BELOW
                AddToTable(ToUnichar("1EAD"), "a");	// LATIN SMALL LETTER A WITH CIRCUMFLEX AND DOT BELOW
                AddToTable(ToUnichar("1EAE"), "A");	// LATIN CAPITAL LETTER A WITH BREVE AND ACUTE
                AddToTable(ToUnichar("1EAF"), "a");	// LATIN SMALL LETTER A WITH BREVE AND ACUTE
                AddToTable(ToUnichar("1EB0"), "A");	// LATIN CAPITAL LETTER A WITH BREVE AND GRAVE
                AddToTable(ToUnichar("1EB1"), "a");	// LATIN SMALL LETTER A WITH BREVE AND GRAVE
                AddToTable(ToUnichar("1EB2"), "A");	// LATIN CAPITAL LETTER A WITH BREVE AND HOOK ABOVE
                AddToTable(ToUnichar("1EB3"), "a");	// LATIN SMALL LETTER A WITH BREVE AND HOOK ABOVE
                AddToTable(ToUnichar("1EB4"), "A");	// LATIN CAPITAL LETTER A WITH BREVE AND TILDE
                AddToTable(ToUnichar("1EB5"), "a");	// LATIN SMALL LETTER A WITH BREVE AND TILDE
                AddToTable(ToUnichar("1EB6"), "A");	// LATIN CAPITAL LETTER A WITH BREVE AND DOT BELOW
                AddToTable(ToUnichar("1EB7"), "a");	// LATIN SMALL LETTER A WITH BREVE AND DOT BELOW
                AddToTable(ToUnichar("1EB8"), "E");	// LATIN CAPITAL LETTER E WITH DOT BELOW
                AddToTable(ToUnichar("1EB9"), "e");	// LATIN SMALL LETTER E WITH DOT BELOW
                AddToTable(ToUnichar("1EBA"), "E");	// LATIN CAPITAL LETTER E WITH HOOK ABOVE
                AddToTable(ToUnichar("1EBB"), "e");	// LATIN SMALL LETTER E WITH HOOK ABOVE
                AddToTable(ToUnichar("1EBC"), "E");	// LATIN CAPITAL LETTER E WITH TILDE
                AddToTable(ToUnichar("1EBD"), "e");	// LATIN SMALL LETTER E WITH TILDE
                AddToTable(ToUnichar("1EBE"), "E");	// LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND ACUTE
                AddToTable(ToUnichar("1EBF"), "e");	// LATIN SMALL LETTER E WITH CIRCUMFLEX AND ACUTE
                AddToTable(ToUnichar("1EC0"), "E");	// LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND GRAVE
                AddToTable(ToUnichar("1EC1"), "e");	// LATIN SMALL LETTER E WITH CIRCUMFLEX AND GRAVE
                AddToTable(ToUnichar("1EC2"), "E");	// LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND HOOK ABOVE
                AddToTable(ToUnichar("1EC3"), "e");	// LATIN SMALL LETTER E WITH CIRCUMFLEX AND HOOK ABOVE
                AddToTable(ToUnichar("1EC4"), "E");	// LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND TILDE
                AddToTable(ToUnichar("1EC5"), "e");	// LATIN SMALL LETTER E WITH CIRCUMFLEX AND TILDE
                AddToTable(ToUnichar("1EC6"), "E");	// LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND DOT BELOW
                AddToTable(ToUnichar("1EC7"), "e");	// LATIN SMALL LETTER E WITH CIRCUMFLEX AND DOT BELOW
                AddToTable(ToUnichar("1EC8"), "I");	// LATIN CAPITAL LETTER I WITH HOOK ABOVE
                AddToTable(ToUnichar("1EC9"), "i");	// LATIN SMALL LETTER I WITH HOOK ABOVE
                AddToTable(ToUnichar("1ECA"), "I");	// LATIN CAPITAL LETTER I WITH DOT BELOW
                AddToTable(ToUnichar("1ECB"), "i");	// LATIN SMALL LETTER I WITH DOT BELOW
                AddToTable(ToUnichar("1ECC"), "O");	// LATIN CAPITAL LETTER O WITH DOT BELOW
                AddToTable(ToUnichar("1ECD"), "o");	// LATIN SMALL LETTER O WITH DOT BELOW
                AddToTable(ToUnichar("1ECE"), "O");	// LATIN CAPITAL LETTER O WITH HOOK ABOVE
                AddToTable(ToUnichar("1ECF"), "o");	// LATIN SMALL LETTER O WITH HOOK ABOVE
                AddToTable(ToUnichar("1ED0"), "O");	// LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND ACUTE
                AddToTable(ToUnichar("1ED1"), "o");	// LATIN SMALL LETTER O WITH CIRCUMFLEX AND ACUTE
                AddToTable(ToUnichar("1ED2"), "O");	// LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND GRAVE
                AddToTable(ToUnichar("1ED3"), "o");	// LATIN SMALL LETTER O WITH CIRCUMFLEX AND GRAVE
                AddToTable(ToUnichar("1ED4"), "O");	// LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND HOOK ABOVE
                AddToTable(ToUnichar("1ED5"), "o");	// LATIN SMALL LETTER O WITH CIRCUMFLEX AND HOOK ABOVE
                AddToTable(ToUnichar("1ED6"), "O");	// LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND TILDE
                AddToTable(ToUnichar("1ED7"), "o");	// LATIN SMALL LETTER O WITH CIRCUMFLEX AND TILDE
                AddToTable(ToUnichar("1ED8"), "O");	// LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND DOT BELOW
                AddToTable(ToUnichar("1ED9"), "o");	// LATIN SMALL LETTER O WITH CIRCUMFLEX AND DOT BELOW
                AddToTable(ToUnichar("1EDA"), "O");	// LATIN CAPITAL LETTER O WITH HORN AND ACUTE
                AddToTable(ToUnichar("1EDB"), "o");	// LATIN SMALL LETTER O WITH HORN AND ACUTE
                AddToTable(ToUnichar("1EDC"), "O");	// LATIN CAPITAL LETTER O WITH HORN AND GRAVE
                AddToTable(ToUnichar("1EDD"), "o");	// LATIN SMALL LETTER O WITH HORN AND GRAVE
                AddToTable(ToUnichar("1EDE"), "O");	// LATIN CAPITAL LETTER O WITH HORN AND HOOK ABOVE
                AddToTable(ToUnichar("1EDF"), "o");	// LATIN SMALL LETTER O WITH HORN AND HOOK ABOVE
                AddToTable(ToUnichar("1EE0"), "O");	// LATIN CAPITAL LETTER O WITH HORN AND TILDE
                AddToTable(ToUnichar("1EE1"), "o");	// LATIN SMALL LETTER O WITH HORN AND TILDE
                AddToTable(ToUnichar("1EE2"), "O");	// LATIN CAPITAL LETTER O WITH HORN AND DOT BELOW
                AddToTable(ToUnichar("1EE3"), "o");	// LATIN SMALL LETTER O WITH HORN AND DOT BELOW
                AddToTable(ToUnichar("1EE4"), "U");	// LATIN CAPITAL LETTER U WITH DOT BELOW
                AddToTable(ToUnichar("1EE5"), "u");	// LATIN SMALL LETTER U WITH DOT BELOW
                AddToTable(ToUnichar("1EE6"), "U");	// LATIN CAPITAL LETTER U WITH HOOK ABOVE
                AddToTable(ToUnichar("1EE7"), "u");	// LATIN SMALL LETTER U WITH HOOK ABOVE
                AddToTable(ToUnichar("1EE8"), "U");	// LATIN CAPITAL LETTER U WITH HORN AND ACUTE
                AddToTable(ToUnichar("1EE9"), "u");	// LATIN SMALL LETTER U WITH HORN AND ACUTE
                AddToTable(ToUnichar("1EEA"), "U");	// LATIN CAPITAL LETTER U WITH HORN AND GRAVE
                AddToTable(ToUnichar("1EEB"), "u");	// LATIN SMALL LETTER U WITH HORN AND GRAVE
                AddToTable(ToUnichar("1EEC"), "U");	// LATIN CAPITAL LETTER U WITH HORN AND HOOK ABOVE
                AddToTable(ToUnichar("1EED"), "u");	// LATIN SMALL LETTER U WITH HORN AND HOOK ABOVE
                AddToTable(ToUnichar("1EEE"), "U");	// LATIN CAPITAL LETTER U WITH HORN AND TILDE
                AddToTable(ToUnichar("1EEF"), "u");	// LATIN SMALL LETTER U WITH HORN AND TILDE
                AddToTable(ToUnichar("1EF0"), "U");	// LATIN CAPITAL LETTER U WITH HORN AND DOT BELOW
                AddToTable(ToUnichar("1EF1"), "u");	// LATIN SMALL LETTER U WITH HORN AND DOT BELOW
                AddToTable(ToUnichar("1EF2"), "Y");	// LATIN CAPITAL LETTER Y WITH GRAVE
                AddToTable(ToUnichar("1EF3"), "y");	// LATIN SMALL LETTER Y WITH GRAVE
                AddToTable(ToUnichar("1EF4"), "Y");	// LATIN CAPITAL LETTER Y WITH DOT BELOW
                AddToTable(ToUnichar("1EF5"), "y");	// LATIN SMALL LETTER Y WITH DOT BELOW
                AddToTable(ToUnichar("1EF6"), "Y");	// LATIN CAPITAL LETTER Y WITH HOOK ABOVE
                AddToTable(ToUnichar("1EF7"), "y");	// LATIN SMALL LETTER Y WITH HOOK ABOVE
                AddToTable(ToUnichar("1EF8"), "Y");	// LATIN CAPITAL LETTER Y WITH TILDE
                AddToTable(ToUnichar("1EF9"), "y");	// LATIN SMALL LETTER Y WITH TILDE
                AddToTable(ToUnichar("2071"), "i");	// SUPERSCRIPT LATIN SMALL LETTER I
                AddToTable(ToUnichar("207F"), "n");	// SUPERSCRIPT LATIN SMALL LETTER N
                AddToTable(ToUnichar("212A"), "K");	// KELVIN SIGN
                AddToTable(ToUnichar("212B"), "A");	// ANGSTROM SIGN
                AddToTable(ToUnichar("212C"), "B");	// SCRIPT CAPITAL B
                AddToTable(ToUnichar("212D"), "C");	// BLACK-LETTER CAPITAL C
                AddToTable(ToUnichar("212F"), "e");	// SCRIPT SMALL E
                AddToTable(ToUnichar("2130"), "E");	// SCRIPT CAPITAL E
                AddToTable(ToUnichar("2131"), "F");	// SCRIPT CAPITAL F
                AddToTable(ToUnichar("2132"), "F");	// TURNED CAPITAL F -- no decomposition
                AddToTable(ToUnichar("2133"), "M");	// SCRIPT CAPITAL M
                AddToTable(ToUnichar("2134"), "0");	// SCRIPT SMALL O
                AddToTable(ToUnichar("213A"), "0");	// ROTATED CAPITAL Q -- no decomposition
                AddToTable(ToUnichar("2141"), "G");	// TURNED SANS-SERIF CAPITAL G -- no decomposition
                AddToTable(ToUnichar("2142"), "L");	// TURNED SANS-SERIF CAPITAL L -- no decomposition
                AddToTable(ToUnichar("2143"), "L");	// REVERSED SANS-SERIF CAPITAL L -- no decomposition
                AddToTable(ToUnichar("2144"), "Y");	// TURNED SANS-SERIF CAPITAL Y -- no decomposition
                AddToTable(ToUnichar("2145"), "D");	// DOUBLE-STRUCK ITALIC CAPITAL D
                AddToTable(ToUnichar("2146"), "d");	// DOUBLE-STRUCK ITALIC SMALL D
                AddToTable(ToUnichar("2147"), "e");	// DOUBLE-STRUCK ITALIC SMALL E
                AddToTable(ToUnichar("2148"), "i");	// DOUBLE-STRUCK ITALIC SMALL I
                AddToTable(ToUnichar("2149"), "j");	// DOUBLE-STRUCK ITALIC SMALL J
                AddToTable(ToUnichar("FB00"), "ff");	// LATIN SMALL LIGATURE FF
                AddToTable(ToUnichar("FB01"), "fi");	// LATIN SMALL LIGATURE FI
                AddToTable(ToUnichar("FB02"), "fl");	// LATIN SMALL LIGATURE FL
                AddToTable(ToUnichar("FB03"), "ffi");	// LATIN SMALL LIGATURE FFI
                AddToTable(ToUnichar("FB04"), "ffl");	// LATIN SMALL LIGATURE FFL
                AddToTable(ToUnichar("FB05"), "st");	// LATIN SMALL LIGATURE LONG S T
                AddToTable(ToUnichar("FB06"), "st");	// LATIN SMALL LIGATURE ST
                AddToTable(ToUnichar("FF21"), "A");	// FULLWIDTH LATIN CAPITAL LETTER B
                AddToTable(ToUnichar("FF22"), "B");	// FULLWIDTH LATIN CAPITAL LETTER B
                AddToTable(ToUnichar("FF23"), "C");	// FULLWIDTH LATIN CAPITAL LETTER C
                AddToTable(ToUnichar("FF24"), "D");	// FULLWIDTH LATIN CAPITAL LETTER D
                AddToTable(ToUnichar("FF25"), "E");	// FULLWIDTH LATIN CAPITAL LETTER E
                AddToTable(ToUnichar("FF26"), "F");	// FULLWIDTH LATIN CAPITAL LETTER F
                AddToTable(ToUnichar("FF27"), "G");	// FULLWIDTH LATIN CAPITAL LETTER G
                AddToTable(ToUnichar("FF28"), "H");	// FULLWIDTH LATIN CAPITAL LETTER H
                AddToTable(ToUnichar("FF29"), "I");	// FULLWIDTH LATIN CAPITAL LETTER I
                AddToTable(ToUnichar("FF2A"), "J");	// FULLWIDTH LATIN CAPITAL LETTER J
                AddToTable(ToUnichar("FF2B"), "K");	// FULLWIDTH LATIN CAPITAL LETTER K
                AddToTable(ToUnichar("FF2C"), "L");	// FULLWIDTH LATIN CAPITAL LETTER L
                AddToTable(ToUnichar("FF2D"), "M");	// FULLWIDTH LATIN CAPITAL LETTER M
                AddToTable(ToUnichar("FF2E"), "N");	// FULLWIDTH LATIN CAPITAL LETTER N
                AddToTable(ToUnichar("FF2F"), "O");	// FULLWIDTH LATIN CAPITAL LETTER O
                AddToTable(ToUnichar("FF30"), "P");	// FULLWIDTH LATIN CAPITAL LETTER P
                AddToTable(ToUnichar("FF31"), "Q");	// FULLWIDTH LATIN CAPITAL LETTER Q
                AddToTable(ToUnichar("FF32"), "R");	// FULLWIDTH LATIN CAPITAL LETTER R
                AddToTable(ToUnichar("FF33"), "S");	// FULLWIDTH LATIN CAPITAL LETTER S
                AddToTable(ToUnichar("FF34"), "T");	// FULLWIDTH LATIN CAPITAL LETTER T
                AddToTable(ToUnichar("FF35"), "U");	// FULLWIDTH LATIN CAPITAL LETTER U
                AddToTable(ToUnichar("FF36"), "V");	// FULLWIDTH LATIN CAPITAL LETTER V
                AddToTable(ToUnichar("FF37"), "W");	// FULLWIDTH LATIN CAPITAL LETTER W
                AddToTable(ToUnichar("FF38"), "X");	// FULLWIDTH LATIN CAPITAL LETTER X
                AddToTable(ToUnichar("FF39"), "Y");	// FULLWIDTH LATIN CAPITAL LETTER Y
                AddToTable(ToUnichar("FF3A"), "Z");	// FULLWIDTH LATIN CAPITAL LETTER Z
                AddToTable(ToUnichar("FF41"), "a");	// FULLWIDTH LATIN SMALL LETTER A
                AddToTable(ToUnichar("FF42"), "b");	// FULLWIDTH LATIN SMALL LETTER B
                AddToTable(ToUnichar("FF43"), "c");	// FULLWIDTH LATIN SMALL LETTER C
                AddToTable(ToUnichar("FF44"), "d");	// FULLWIDTH LATIN SMALL LETTER D
                AddToTable(ToUnichar("FF45"), "e");	// FULLWIDTH LATIN SMALL LETTER E
                AddToTable(ToUnichar("FF46"), "f");	// FULLWIDTH LATIN SMALL LETTER F
                AddToTable(ToUnichar("FF47"), "g");	// FULLWIDTH LATIN SMALL LETTER G
                AddToTable(ToUnichar("FF48"), "h");	// FULLWIDTH LATIN SMALL LETTER H
                AddToTable(ToUnichar("FF49"), "i");	// FULLWIDTH LATIN SMALL LETTER I
                AddToTable(ToUnichar("FF4A"), "j");	// FULLWIDTH LATIN SMALL LETTER J
                AddToTable(ToUnichar("FF4B"), "k");	// FULLWIDTH LATIN SMALL LETTER K
                AddToTable(ToUnichar("FF4C"), "l");	// FULLWIDTH LATIN SMALL LETTER L
                AddToTable(ToUnichar("FF4D"), "m");	// FULLWIDTH LATIN SMALL LETTER M
                AddToTable(ToUnichar("FF4E"), "n");	// FULLWIDTH LATIN SMALL LETTER N
                AddToTable(ToUnichar("FF4F"), "o");	// FULLWIDTH LATIN SMALL LETTER O
                AddToTable(ToUnichar("FF50"), "p");	// FULLWIDTH LATIN SMALL LETTER P
                AddToTable(ToUnichar("FF51"), "q");	// FULLWIDTH LATIN SMALL LETTER Q
                AddToTable(ToUnichar("FF52"), "r");	// FULLWIDTH LATIN SMALL LETTER R
                AddToTable(ToUnichar("FF53"), "s");	// FULLWIDTH LATIN SMALL LETTER S
                AddToTable(ToUnichar("FF54"), "t");	// FULLWIDTH LATIN SMALL LETTER T
                AddToTable(ToUnichar("FF55"), "u");	// FULLWIDTH LATIN SMALL LETTER U
                AddToTable(ToUnichar("FF56"), "v");	// FULLWIDTH LATIN SMALL LETTER V
                AddToTable(ToUnichar("FF57"), "w");	// FULLWIDTH LATIN SMALL LETTER W
                AddToTable(ToUnichar("FF58"), "x");	// FULLWIDTH LATIN SMALL LETTER X
                AddToTable(ToUnichar("FF59"), "y");	// FULLWIDTH LATIN SMALL LETTER Y
                AddToTable(ToUnichar("FF5A"), "z");	// FULLWIDTH LATIN SMALL LETTER Z
                AddToTable(ToUnichar("0300"), "");	// FULLWIDTH LATIN SMALL LETTER \
                AddToTable(ToUnichar("0301"), "");	// FULLWIDTH LATIN SMALL LETTER /
                AddToTable(ToUnichar("0303"), "");	// FULLWIDTH LATIN SMALL LETTER ~
                AddToTable(ToUnichar("0309"), "");	// FULLWIDTH LATIN SMALL LETTER ?
                AddToTable(ToUnichar("0323"), "");	// FULLWIDTH LATIN SMALL LETTER .
            }
            finally { }
            string[] array = new string[mCharacterTable.Count];
            mCharacterTable.Keys.CopyTo(array, 0);
            mPattern = "[" + string.Join("", array) + "]";
        } // end addValues
        private static void AddToTable(string code, string latin)
        {
            try 
            {
                if (!mCharacterTable.ContainsKey(code))
                    mCharacterTable.Add(code, latin);
            }
            finally { }
        }
        public static string UrlString(string s)
        {
            var special = new Regex(@"[!@#$%^&*\(\)+_<>,\.\?\\\/'""\s-]+");
            s = Regex.Replace(RemoveDiacritics(s), @"^[!@#$%^&*\(\)+_<>,\.\?\\\/'""\s-]+", "");
            s = Regex.Replace(s, @"[!@#$%^&*\(\)+_<>,\.\?\\\/'""\s-]+$", "");
            s = special.Replace(s, "-");
            return s.Replace(";","").Replace(":","");
        }

        public static string RemoveDiacritics(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string stFormD = input.Normalize(NormalizationForm.FormD);
                int len = stFormD.Length;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < len; i++)
                {
                    System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                    if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(stFormD[i]);
                    }
                }
                var ret = sb.ToString().Normalize(NormalizationForm.FormC);
                ret = ret.Replace(UnicodeEncoding.Unicode.GetString(new byte[] { 16, 1 }), "D").Replace(UnicodeEncoding.Unicode.GetString(new byte[] { 17, 1 }), "d");
                return ret;
            }
            else return string.Empty;
        }
    }
}
