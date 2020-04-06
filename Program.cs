using System;
using System.Collections.Generic;
using UPCardTool.Pages;
using System.Runtime.InteropServices;

namespace UPCardTool
{
    class Program
    {
        public static readonly string version = "1";
        public static readonly string cardsFileName = "UnityParrot\\Configuration\\UserCards.json";

        static readonly Dictionary<PageId, Page> pageDictionary = new Dictionary<PageId, Page>() {
            {PageId.Gacha, new Gacha()},
            {PageId.Menu, new Menu()},
            {PageId.Print, new Print()},
            {PageId.PrintAll, new PrintAll()},
            {PageId.UnlockAll, new UnlockAll()},
            {PageId.SaveQuit, null},
            {PageId.Quit, null},
        };

        // Change console font
        private const int STD_OUTPUT_HANDLE = -11;
        private const int TMPF_TRUETYPE = 4;
        private const int LF_FACESIZE = 32;
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;

            internal COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct CONSOLE_FONT_INFO_EX
        {
            internal uint cbSize;
            internal uint nFont;
            internal COORD dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            internal fixed char FaceName[LF_FACESIZE];
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool GetCurrentConsoleFontEx(
               IntPtr consoleOutput,
               bool maximumWindow,
               ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(
               IntPtr consoleOutput,
               bool maximumWindow,
               CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        public static unsafe void ChangeFont()
        {
            string fontName = "MS Gothic";
            IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
            if (hnd != INVALID_HANDLE_VALUE)
            {
                CONSOLE_FONT_INFO_EX info = new CONSOLE_FONT_INFO_EX();
                info.cbSize = (uint)Marshal.SizeOf(info);
                bool tt = false;

                // First determine whether there's already a TrueType font.
                if (GetCurrentConsoleFontEx(hnd, false, ref info))
                {
                    tt = (info.FontFamily & TMPF_TRUETYPE) == TMPF_TRUETYPE;
                    if (tt)
                    {
                        return;
                    }

                    CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
                    newInfo.cbSize = (uint)Marshal.SizeOf(newInfo);
                    newInfo.FontFamily = TMPF_TRUETYPE;
                    IntPtr ptr = new IntPtr(newInfo.FaceName);
                    Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);

                    // Get some settings from current font.
                    newInfo.dwFontSize = new COORD(info.dwFontSize.X, info.dwFontSize.Y);
                    newInfo.FontWeight = info.FontWeight;
                    SetCurrentConsoleFontEx(hnd, false, newInfo);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ChangeFont();

            Page currentPage = pageDictionary[PageId.Menu];
            while (true)
            {
                PageId nextPageId = currentPage.Execute();
                currentPage = pageDictionary[nextPageId];
                Console.Clear();

                switch (nextPageId)
                {
                    case PageId.SaveQuit:
                        CardManager.Save();
                        return;
                    case PageId.Quit:
                        return;
                }
            }
        }
    }
}
