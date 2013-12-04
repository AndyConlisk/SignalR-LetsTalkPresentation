using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;
using LetsLearnSignalRPart3.Models;
using Newtonsoft.Json;

namespace LetsLearnSignalRPart3
{
    public class Broadcaster
    {
        private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());
        private readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(1000 / 25);
        private readonly IHubContext _hubContext;
        private Timer _broadcastLoop;
        private List<BoxModel> _boxes;
        private bool _boxesUpdated;

        public Broadcaster()
        {
            // Save our hub context so we can easily use it 
            // to send to its connected clients
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<BoxHub>();
            _boxes = new List<BoxModel>();
            _boxesUpdated = false;
            // Start the broadcast loop
            _broadcastLoop = new Timer(
                BroadcastBoxes,
                null,
                BroadcastInterval,
                BroadcastInterval);
        }

        public static Broadcaster Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void BroadcastBoxes(object state)
        {
            if(_boxesUpdated)
            {

                foreach(var box in _boxes)
                {
                    _hubContext.Clients.AllExcept(box.Id).updateBoxes(box);
                }

                _boxesUpdated = false;
            }
        }

        public void UpdateBox(BoxModel clientModel)
        {
            var currentbox = _boxes.Find(x => x.Id == clientModel.Id);
            _boxes.Remove(currentbox);
            _boxes.Add(clientModel);

            _boxesUpdated = true;
        }

        public void AddBox(BoxModel clientModel)
        {
            _boxes.Add(clientModel);
            _boxesUpdated = true;
        }

        public void RemoveBox(string boxId)
        {
            var removeBox = _boxes.Find(x => x.Id == boxId);
            _boxes.Remove(removeBox);
            _boxesUpdated = true;
        }
    }
}