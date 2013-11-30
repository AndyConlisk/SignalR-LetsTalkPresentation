using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using LetsLearnSignalRPart2.Models;

namespace LetsLearnSignalRPart2
{
    public class BaconNotifier
    {
        //uses Lazy initialization for thread safty, since everytime a client connects a new instance of the BaconHub is running in another thread
        private readonly static Lazy<BaconNotifier> _instance = new Lazy<BaconNotifier>(() => new BaconNotifier(GlobalHost.ConnectionManager.GetHubContext<BaconHub>().Clients));

        private readonly ConcurrentDictionary<string, BaconModel> _baconCollection = new ConcurrentDictionary<string, BaconModel>();

        private readonly object _updateBaconStockLock = new object();

        //how fast my bacon stock changes
        private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(2);

        private readonly Timer _timer;
        private volatile bool _updatingBaconStock = false;

        private readonly Random random = new Random();

        private BaconNotifier(IHubConnectionContext clients)
        {
            Clients = clients;

            _baconCollection.Clear();
            var baconCollection = new List<BaconModel>
            {
                new BaconModel { BaconName = "Hickory Smoked Vegan", BaconStockQty = 100 },
                new BaconModel { BaconName = "Honey Smothered", BaconStockQty = 20 },
                new BaconModel { BaconName = "Thick Cut", BaconStockQty = 0}
            };
            baconCollection.ForEach(bacon => _baconCollection.TryAdd(bacon.BaconName, bacon));

            _timer = new Timer(UpdateBaconStock, null, _updateInterval, _updateInterval);
        }

        public static BaconNotifier Instance
        {
            get { return _instance.Value; }
        }

        private IHubConnectionContext Clients { get; set; }

        public IEnumerable<BaconModel> GetAllBaconStock()
        {
            return _baconCollection.Values;
        }

        private void UpdateBaconStock(object state)
        {
            lock(_updateBaconStockLock)
            {
                if(!_updatingBaconStock)
                {
                    _updatingBaconStock = true;

                    foreach(var bacon in _baconCollection.Values)
                    {
                        if(TryUpdateBaconStock(bacon))
                        {
                            BroadcastBaconStock(bacon);
                        }
                    }

                    _updatingBaconStock = false;
                }
            }
        }

        private bool TryUpdateBaconStock(BaconModel bacon)
        {            
            var qtyChange = random.Next(-100, 100);
            bacon.BaconStockQty += qtyChange;

            if(bacon.BaconStockQty < 0)
            {
                bacon.BaconStockQty = 0;
            }

            return true;
        }

        private void BroadcastBaconStock(BaconModel bacon)
        {
            Clients.All.updateBaconQty(bacon);
        }
    }
}