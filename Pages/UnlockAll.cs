using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using MU3.User;
using MU3.Data;
using MU3.DataStudio;
using MU3.Util;

namespace UPCardTool.Pages
{
    class UnlockAll : Page
    {
        public static Dictionary<RarityType, string> rarityNewString = new Dictionary<RarityType, string> {
            {RarityType.N,   "       NEW       "},
            {RarityType.R,   "       NEW       "},
            {RarityType.SR,  "   ･:* NEW *:･   "},
            {RarityType.SSR, "･✧･ﾟ:* NEW *:･ﾟ✧･"}
        };

        public UnlockAll()
        {
            title = "Unlocking All Cards";
            options = new List<PageOption>
                {
                    new PageOption{ key = "U", description = "Unlock all cards again", pageId = PageId.UnlockAll},
                    new PageOption{ key = "M", description = "Return to menu", pageId = PageId.Menu},
                };
        }

        public override void Run()
        {
            Console.WriteLine("Searching for cards...\n");
            Console.WriteLine("Do you really want all the cards? [yes/no]: ");
            var response = Console.ReadLine();
            if (response == "yes")
            {
                Console.WriteLine("");
                foreach (var cardData in CardManager.AllCards())
                {
                    bool isDupe = CardManager.Cards().Any(item => item.cardId == cardData.GetID());
                    string dupeText = isDupe ? "                 " : rarityNewString[cardData.Rarity];
                    var cardText = $"    {dupeText}{cardData.Name.str} ({cardData.Attribute})";

                    Console.WriteLine(cardText);
                    CardManager.AddCard(cardData);
                    Thread.Sleep(10);
                }
            }
        }
    }
}
