﻿using Milkyfie.AppCode.Helper;
using System.Collections.Generic;

namespace Milkyfie.Models
{
    public class PageSetting
    {
        public int TotoalRows { get; set; }
        public int CurrentPage { get; set; }
        public int Draw { get; set; }
    }

    public class JDataTable<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> Data { get; set; }
        public PageSetting PageSetting { get; set; }
    }
    public class jsonAOData
    {
        public int draw { get; set; }
        public int start { get; set; } = 0;
        public int length { get; set; } = 100;
        public jsonAODataSearch search { get; set; }
        public List<Order> order { get; set; }
        public object param { get; set; }
     
    }

    public class Order
    {
        public int column { get; set; } 
        public string dir { get; set; } 
    }

    //public class jsonAOData<T> : jsonAOData
    //{
    //    public T param { get; set; }
    //}

    public class jsonAODataSearch
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }
}
