using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using TTPRODB.BuisnessLogic.Entities;
using System.Reflection;

namespace TTPRODB.BuisnessLogic
{
    public class ParseTTDb
    {
        public Type currentItemType;
        public ConstructorInfo constructor;
        public PropertyInfo[] itemRatingsProperties;
        public bool pipsFlag;

        public List<Producer> ProducersList { get; set; }

        public HtmlNode GetPageRootNode(string url)
        {
            throw new NotImplementedException();
        }

        public int GetItemsCount(List<HtmlNode> itemsTables)
        {
            throw new NotImplementedException();
        }

        public void InitializeProducer(HtmlNode producersDiv)
        {
            throw new NotImplementedException();
        }

        public dynamic ParseItem(string iurl, int v)
        {
            throw new NotImplementedException();
        }
    }
}