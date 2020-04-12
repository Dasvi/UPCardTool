using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using MU3.Client;
using MU3.Data;
using MU3.Util;

namespace UPCardTool
{
    public class CardManager
    {
        private static List<MU3.Client.UserCard> _cards;
        private static List<MU3.DataStudio.Serialize.CardData> _allCards;
        private static string _filename = "UserCard.json";

        protected CardManager() { }

        public static List<MU3.Client.UserCard> Cards() 
        {
            if (_cards == null)
            {
                GetUserCardResponse userCardResponse = FileSystem.Configuration.LoadJson<GetUserCardResponse>(_filename);
                _cards = new List<MU3.Client.UserCard>();

                foreach (MU3.Client.UserCard card in userCardResponse.userCardList)
                {
                    _cards.Add(card);
                }
            }

            return _cards;
        }

        public static List<MU3.DataStudio.Serialize.CardData> AllCards()
        {
            if (_allCards == null)
            {
                _allCards = new List<MU3.DataStudio.Serialize.CardData>();

                string[] cardFiles = FileSystem.Assets.GetFiles("card", "xml");
                foreach (var cardFile in cardFiles)
                {
                    MU3.DataStudio.Serialize.CardData cardData = new MU3.DataStudio.Serialize.CardData();
                    XmlSerializer xml = new XmlSerializer(cardData.GetType());

                    //Console.WriteLine(cardFile);

                    cardData = (MU3.DataStudio.Serialize.CardData)xml.Deserialize(File.OpenRead(cardFile));
                    _allCards.Add(cardData);
                }
            }

            return _allCards;
        }

        public static void AddCard(MU3.DataStudio.Serialize.CardData cardData)
        {
            bool isAdded = false;
            foreach (var card in CardManager.Cards())
            {
                if (card.cardId == cardData.GetID())
                {
                    card.digitalStock++;
                    card.printCount++;
                    card.skillId = cardData.SkillID.id;
                    isAdded = true;
                    break;
                }
            }

            if (!isAdded)
            {
                var card = new MU3.Client.UserCard()
                {
                    digitalStock = 1,
                    printCount = 1,
                    skillId = 1,
                    analogStock = 1,
                    exp = 1,
                    level = 1,
                    maxLevel = 1,
                    useCount = 1,
                    isNew = true,
                    kaikaDate = "0000-00-00 00:00:00.0",
                    choKaikaDate = "0000-00-00 00:00:00.0",
                    isAcquired = true,
                    created = "",
                    cardId = cardData.GetID()
                };
                _cards.Add(card);
            }
        }

        public static void PrintCard(int cardId)
        {
            foreach (var card in CardManager.Cards())
            {
                if (card.cardId == cardId)
                {
                    card.printCount++;
                    break;
                }
            }
        }

        public static void Save()
        {
            GetUserCardResponse userCardResponse = FileSystem.Configuration.LoadJson<GetUserCardResponse>(_filename);

            foreach (var card in _cards)
            {
                if (card.created == "0000-00-00 00:00:00.0")
                {
                    card.created = NetPacketUtil.toString(NetPacketUtil.LocalNow);
                }
                if (card.kaikaDate == "0000-00-00 00:00:00.0" && card.printCount > 0)
                {
                    card.kaikaDate = NetPacketUtil.toString(NetPacketUtil.LocalNow);
                }
                if (card.choKaikaDate == "0000-00-00 00:00:00.0" && card.printCount > 2 && card.level == card.maxLevel)
                {
                    card.choKaikaDate = NetPacketUtil.toString(NetPacketUtil.LocalNow);
                }
            }

            userCardResponse.userCardList = _cards.ToArray();
            userCardResponse.length = _cards.Count;
            FileSystem.Configuration.SaveJson(_filename, userCardResponse);
        }


    }
}
