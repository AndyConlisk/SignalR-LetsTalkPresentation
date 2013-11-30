using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using LetsLearnSignalRPart2.Models;

namespace LetsLearnSignalRPart2
{
    [HubName("baconNotifier")]
    public class BaconHub : Hub
    {
        private readonly BaconNotifier _baconNotifier;

        public BaconHub() : this(BaconNotifier.Instance) { }

        public BaconHub(BaconNotifier baconNotifier)
        {
            _baconNotifier = baconNotifier;
        }

        // If code required database lookup, set the return type to a Task to make async; (Task<IEnumerable<BaconModel>>)
        public IEnumerable<BaconModel> GetBaconStock()
        {
            return _baconNotifier.GetAllBaconStock();
        }
    }
}