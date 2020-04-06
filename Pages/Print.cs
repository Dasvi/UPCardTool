using System;
using System.Collections.Generic;
using System.Text;

namespace UPCardTool.Pages
{
    class Print : Page
    {
        public Print()
        {
            title = "Print";
            options = new List<PageOption>
                {
                    new PageOption{ key = "A", description = "Print all cards in deck", pageId = PageId.PrintAll},
                    new PageOption{ key = "M", description = "Return to menu", pageId = PageId.Menu},
                };
        }

        public override void Run()
        {
            Console.WriteLine(
                "Printing cards allows the cards' max level to be raised by 40.\n" +
                "This does not actually physically print anything."
            );
        }
    }
}
