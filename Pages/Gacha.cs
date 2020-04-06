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
    class Gacha : Page
    {
        public static int SSRChance = 3;
        public static int SRChance = 10;
        public static Dictionary<RarityType, string> rarityNewString = new Dictionary<RarityType, string> {
            {RarityType.N,   "       NEW       "},
            {RarityType.R,   "       NEW       "},
            {RarityType.SR,  "   ･:* NEW *:･   "},
            {RarityType.SSR, "･✧･ﾟ:* NEW *:･ﾟ✧･"}
        };

        public Gacha()
        {
            title = "Gacha";
            options = new List<PageOption>
                {
                    new PageOption{ key = "R", description = "Roll again", pageId = PageId.Gacha},
                    new PageOption{ key = "M", description = "Return to menu", pageId = PageId.Menu},
                };
        }

        public override void Run()
        {
            Console.WriteLine("Searching for cards...\n");
            bool gotGuaranteedSR = false;
            for (int i = 0; i < 10; ++i)
            {
                var rand = new Random();
                var lottery = rand.Next(100);
                RarityType rarity = RarityType.R;

                if (lottery < SSRChance)
                {
                    rarity = RarityType.SSR;
                } else if (lottery < SRChance)
                {
                    rarity = RarityType.SR;
                }

                if (i == 9 && rarity == RarityType.R && !gotGuaranteedSR)
                {
                    rarity = RarityType.SR;
                }

                while(true)
                {
                    var cardData = CardManager.AllCards().ToArray()[rand.Next(CardManager.AllCards().Count)];

                    if (cardData.Rarity == rarity)
                    {
                        bool isDupe = CardManager.Cards().Any(item => item.cardId == cardData.GetID());
                        string dupeText = isDupe ? "                 " : rarityNewString[rarity];
                        var cardText = $"    {dupeText}{cardData.Name.str} ({cardData.Attribute})";
                        foreach (var c in cardText) {
                            Console.Write(c);
                            Thread.Sleep(2);
                        }
                        Console.Write("\n");

                        CardManager.AddCard(cardData);   

                        if (cardData.Rarity != RarityType.R)
                        {
                            gotGuaranteedSR = true;
                        }

                        Thread.Sleep(50);
                        break;
                    }
                }
            }
            
        }
    }
}
