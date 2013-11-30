using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetsLearnSignalRPart2.Models
{
    public class BaconModel
    {
        public string BaconId { get { return BaconName.Replace(" ", ""); } }

        public string BaconName { get; set; }

        public int BaconStockQty { get; set; }

        public bool BaconInStock { get { return BaconStockQty > 0; } }
    }
}