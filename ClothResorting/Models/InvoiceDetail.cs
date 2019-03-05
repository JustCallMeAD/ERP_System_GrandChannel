﻿using ClothResorting.Models.FBAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothResorting.Models
{
    public class InvoiceDetail
    {
        public int Id { get; set; }

        public string Activity { get; set; }

        public string OrderType { get; set; }

        public string ChargingType { get; set; }

        public string Unit { get; set; }

        public double Quantity { get; set; }

        public double Rate { get; set; }

        public double Amount { get; set; }

        public DateTime DateOfCost { get; set; }

        public string Memo { get; set; }

        public FBAMasterOrder FBAMasterOrder { get; set; }

        public FBAShipOrder FBAShipOrder { get; set; }

        public Invoice Invoice { get; set; }
    }
}