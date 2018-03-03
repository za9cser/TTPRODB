using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TTPRODB.BuisnessLogic;
using System.Linq;
using TTPRODB.BuisnessLogic.Entities;
using System.Collections.Generic;

namespace TTPRODB.UnitTests
{
    [TestClass]
    public class ParseTTDbUnitTests
    {
        private string url = "http://www.tabletennisdb.com/blade/";

        [TestMethod]
        public void TestItemsCount()
        {
            ParseTTDb parse = new ParseTTDb();
            var root = parse.GetPageRootNode(url);
            // tables with items urls
            var itemsTables = root.Descendants()
                .Where(x => x.GetAttributeValue("class", "").Equals("specscompare no-side-border-xxs")).ToList();

            // items count
            int itemsCount = parse.GetItemsCount(itemsTables);
            Assert.AreNotEqual(itemsCount, 0);
        }

        [TestMethod]
        public void TestInitializeProducers()
        {
            ParseTTDb parse = new ParseTTDb();
            var root = parse.GetPageRootNode(url);
            // divs with the names of producers
            var producersDivs = root.Descendants().Where(x =>
                x.GetAttributeValue("class", "").Equals("dbcat max-xxs no-radius-xxs no-side-border-xxs")).ToList();
            foreach (var producersDiv in producersDivs)
            {
                parse.InitializeProducer(producersDiv);
            }
            Assert.AreNotEqual(parse.ProducersList.Count, 0);
        }

        [TestMethod]
        public void TestParseRubber()
        {
            string iurl = "http://www.tabletennisdb.com/rubber/darker-vlon-s-blue-v.html";
            ParseTTDb parse = new ParseTTDb
            {
                currentItemType = typeof(Rubber)
            };
            parse.constructor = parse.currentItemType.GetConstructor(new Type[0]);
            parse.itemRatingsProperties = parse.currentItemType.GetProperties().Where(x => x.PropertyType == typeof(double)).ToArray();
            //var tmp = parse.itemRatingsProperties.
            List<Rubber> rubbers = new List<Rubber>
            {
                parse.ParseItem(iurl, 2)
            };
            Assert.AreNotEqual(rubbers.Count, 0);
        }

        [TestMethod]
        public void TestParsePips()
        {
            string iurl = "http://www.tabletennisdb.com/pips/andro-blowfish-plus.html";
            ParseTTDb parse = new ParseTTDb();
            parse.currentItemType = typeof(Pips);
            parse.constructor = parse.currentItemType.GetConstructor(new Type[0]);
            parse.itemRatingsProperties = parse.currentItemType.GetProperties().Where(x => x.PropertyType == typeof(double)).ToArray();
            //var tmp = parse.itemRatingsProperties.
            List<Pips> pipses = new List<Pips>();
            parse.pipsFlag = true;
            pipses.Add(parse.ParseItem(iurl, 2));
            Assert.AreNotEqual(pipses.Count, 0);
        }
    }
}
