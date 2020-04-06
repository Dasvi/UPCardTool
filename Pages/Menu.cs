using System;
using System.Collections.Generic;
using System.Text;

namespace UPCardTool.Pages
{
    class Menu : Page
    {
        public Menu()
        {
            title = "Menu";
            options = new List<PageOption>
                {
                    new PageOption{ key = "G", description = "Gacha", pageId = PageId.Gacha},
                    new PageOption{ key = "U", description = "Unlock all cards", pageId = PageId.UnlockAll},
                    new PageOption{ key = "P", description = "Print cards", pageId = PageId.Print},
                    new PageOption{ key = "S", description = "Save cards and quit", pageId = PageId.SaveQuit},
                    new PageOption{ key = "Q", description = "Quit without saving", pageId = PageId.Quit},
                    // new PageOption{ key = "B", description = "Backup or restore cards", pageId = PageId.BackupRestore}
                };
        }

        public override void Run()
        {
            if (CardManager.Cards() == null)
            {
                Console.WriteLine("Could not load cards file. It may be missing or corrupt.");
                options.RemoveRange(0, options.Count - 1);
            }
            else
            {
                Console.WriteLine("In order to use this program, your terminal font needs to \n" +
                                  "support Unicode characters. Please make sure to back up your \n" +
                                  "save before using any of these features.");
            }
        }
    }
}
