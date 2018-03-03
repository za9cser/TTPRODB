using HtmlAgilityPackExtensions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using TTPRODB.BuisnessLogic.Entities;

namespace TTPRODB.BuisnessLogic
{
    public class ParseTTDb
    {
        public string[] pages = { "blade", "rubber", "pips" };
        private string site = "http://www.revspin.net/";
        public List<Producer> ProducersList = new List<Producer>();

        private int itemsCount;     // count of items on page
        private int currentItemId;  // id of item in current type
        private int allItemId;      // id of item in whole list

        public dynamic constructor;    // constructor for item's current type
        public Type currentItemType;          // item's current type 
        public PropertyInfo[] itemRatingsProperties;   // array of current type item's properties 
        public bool pipsFlag;

        #region ParseItemsRegion

        // parsing items page
        public List<dynamic> ParseItems(string page, Type itemType, BackgroundWorker bw)
        {
            string url = site + page;
            var root = PageData.GetPageRootNode(url);

            // divs with the names of producers
            var producersDivs = root.Descendants().Where(x => x.GetAttributeValue("class", "").Equals("dbcat max-xxs no-radius-xxs no-side-border-xxs")).ToList();

            // tables with items urls
            var itemsTables = root.Descendants().Where(x => x.GetAttributeValue("class", "").Equals("specscompare no-side-border-xxs")).ToList();

            // items count
            itemsCount = GetItemsCount(itemsTables);

            // initialize constructor, type and properties of currect items
            constructor = itemType.GetConstructor(new Type[0]);
            currentItemType = itemType;
            itemRatingsProperties = itemType.GetProperties().Where(x => x.GetType() == typeof(double)).ToArray();
            if (currentItemType.Name == "Pips")
            {
                pipsFlag = true;
            }

            currentItemId = 0;
            List<dynamic> itemsList = new List<dynamic>();
            for (int i = 0; i < producersDivs.Count; i++)
            {
                var producerId = InitializeProducer(producersDivs[i]);
                // get items "a" tags of the producer
                var itemsUrls = itemsTables[i].Descendants("a").ToList();
                foreach (var itemUrl in itemsUrls)
                {
                    // get full items's url
                    url = site + itemUrl.Attributes["href"].Value;
                    itemsList.Add(ParseItem(url, producerId));
                }
            }

            return itemsList;
        }        

        // get item count on the page
        public int GetItemsCount(List<HtmlNode> itemsTables)
        {
            return itemsTables.Sum(itemsTable => itemsTable.Descendants("a").Count());
        }

        // initialize producer from html
        public int InitializeProducer(HtmlNode producerDiv)
        {
            // get link in producer's div containing producer's name
            HtmlNode link = producerDiv.Descendants("a").First();
            // get producer's name 
            string name = GetCleanString(link.InnerText);
            var producer = ProducersList.FirstOrDefault(x => x.Name == name);
            // check if the producer is on the producersList
            if (producer != null)
            {
                return producer.Id;
            }
            // else add producer to list
            producer = new Producer(ProducersList.Count + 1, name);
            ProducersList.Add(producer);

            return producer.Id;
        }

        // parse specific item
        public dynamic ParseItem(string url, int producerId)
        {
            // create instance of item
            dynamic item = constructor.Invoke(new object[0]);
            // save starting item's info 
            item.ProducerId = producerId;
            item.Id = ++currentItemId;
            item.ItemId = ++allItemId;
            item.Url = url;

            // get data from first table on page
            double[] ratings;
            HtmlNode pageRoot;
            (item.Name, ratings, item.Ratings, pageRoot) = ParseItemMainData(url);

            // save item's ratings
            for (int i = 0; i < ratings.Length; i++)
            {
                itemRatingsProperties[i].SetValue(item, ratings[i]);
            }

            // save data from second table
            switch (currentItemType.Name)
            {
                case "Rubber":
                    (item.Tensor, item.Anti) = ParseRubbersSecondTable(pageRoot);
                    break;
                case "Pips":
                    item.PipsType = ParsePipsSecondTable(pageRoot);
                    break;
            }
            return item;
        }

        // return Name, ratings and ratings' count of item 
        private Tuple<string, double[], int, HtmlNode> ParseItemMainData(string url)
        {
            // get html document of current items page
            var root = PageData.GetPageRootNode(url);
            // get name and ratings count of item 
            string name = root.Descendants("h1").First().InnerText;
            int ratingsCount = Convert.ToInt32(root.SelectSingleNode(".//span[@itemprop='reviewCount']").InnerText);

            // get first table
            var ratingsTable = root.Descendants("table").FirstOrDefault();
            // get all ratings including overall(average) rating
            var ratingsList = ratingsTable.Descendants().Where(x =>
                x.GetAttributeValue("class", "").Equals("cell_rating") ||
                x.GetAttributeValue("class", "").Equals("average")).ToList();

            // if pips is short, add reversal parametr equals 0,0
            if (pipsFlag && ratingsList.Count != 10)
            {
                ratingsList.Insert(4, null);
            }

            // get item's ratings
            double[] ratings = new double[ratingsList.Count];

            for (int i = 0; i < ratingsList.Count; i++)
            {
                try
                {
                    ratings[i] = GetRating(ratingsList[i].InnerText);
                }
                catch (NullReferenceException)
                {
                    ratings[i] = 0;
                }
            }
            return new Tuple<string, double[], int, HtmlNode>(name, ratings, ratingsCount, root);
        }

        // get rating from text
        private double GetRating(string ratingText)
        {
            // clean rating
            ratingText = GetCleanString(ratingText);
            // delete text after space
            ratingText = RemoveAllAfterSpace(ratingText);
            try
            {
                return Convert.ToDouble(ratingText.Replace('.', ','));
            }
            catch (FormatException)
            {
                // if rating isn't rated return 0.0
                return 0.0;
            }
        }

        // remove all chars after first space if exist
        private string RemoveAllAfterSpace(string inputString)
        {
            int index = inputString.IndexOf(" ", StringComparison.Ordinal);
            if (index != -1)
            {
                inputString = inputString.Remove(index);
            }

            return inputString;
        }

        // remove excess symbols from string
        private string GetCleanString(string inputString)
        {
            return RemoveAllAfterSpace(inputString.Replace("\t", "").Replace(" \n", "").Replace("\n", "").Replace("\r", "").Replace("\"", ""));
        }

        // parse second table for rubber
        private Tuple<bool, bool> ParseRubbersSecondTable(HtmlNode pageRoot)
        {
            HtmlNode[] ratingCells = GetRatingCellsFromSecondTable(pageRoot);
            bool tensor = ratingCells[ratingCells.Length - 2].InnerText.Contains("Yes");
            bool anti = ratingCells[ratingCells.Length - 1].InnerText.Contains("Yes");
            return new Tuple<bool, bool>(tensor, anti);
        }

        // parse second table for rubber
        private string ParsePipsSecondTable(HtmlNode pageRoot)
        {
            HtmlNode[] ratingCells = GetRatingCellsFromSecondTable(pageRoot);
            return GetCleanString(ratingCells[ratingCells.Length - 1].InnerText);
        }

        // parse second item table
        private HtmlNode[] GetRatingCellsFromSecondTable(HtmlNode pageRoot)
        {
            var secondTable = pageRoot.Descendants("table").Where(x => x.GetAttributeValue("class", "").Equals("ProductRatingTable ratingtable")).ElementAt(1);
            var ratingCell = secondTable.Descendants("td").Where(x => x.GetAttributeValue("class", "") == "cell_rating").ToArray();
            return ratingCell;
        }

        #endregion
    }
}