using System;
using System.Collections.Generic;
using System.Text;

namespace UPCardTool.Pages
{
    class PrintAll : Page
    {
        public PrintAll()
        {
            title = "Print";
            options = new List<PageOption>
                {
                    new PageOption{ key = "M", description = "Return to menu", pageId = PageId.Menu},
                };
        }

        public override void Run()
        {
            foreach (var card in CardManager.Cards())
            {
                CardManager.PrintCard(card.cardId);
                foreach (var cardData in CardManager.AllCards())
                {
                    if (card.cardId == cardData.GetID())
                    {
                        Console.WriteLine($"    Printed [{cardData.Rarity}] {cardData.Name.str}");
                        break;
                    }
                }
            }
        }
    }
}
