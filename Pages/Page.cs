using System;
using System.Collections.Generic;
using System.Text;

namespace UPCardTool.Pages
{
    public enum PageId
    {
        Menu,
        Gacha,
        UnlockAll,
        BackupRestore,
        Print,
        PrintAll,
        SaveQuit,
        Quit
    }

    struct PageOption
    {
        public string key;
        public PageId pageId;
        public string description;
    }

    abstract class Page
    {
        public List<PageOption> options;
        public string title;

        public abstract void Run();

        public PageId Execute()
        {
            Console.WriteLine($"Unity Parrot Console Tool v{UPCardTool.Program.version}\n");
            Console.WriteLine("=========================================");
            Console.WriteLine($"                 {title}");
            Console.WriteLine("=========================================\n");

            this.Run();

            Console.WriteLine();
            foreach (PageOption option in options)
            {
                Console.WriteLine($"[{option.key}] {option.description}");
            }

            while (true)
            {
                Console.Write("Choose: ");
                string input = Console.ReadLine().ToUpper();
                foreach (PageOption option in options)
                {
                    if (input == option.key)
                    {
                        return option.pageId;
                    }
                }

                int currentLineCursor = Console.CursorTop - 1;
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
            }

        }
    }
}
