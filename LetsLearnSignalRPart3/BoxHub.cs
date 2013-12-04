using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using LetsLearnSignalRPart3.Models;
using System.Threading.Tasks;

namespace LetsLearnSignalRPart3
{
    public class BoxHub : Hub
    {
        private Broadcaster _broadcaster;
        private readonly Random _random = new Random();

        public BoxHub() : this(Broadcaster.Instance)
        { }

        public BoxHub(Broadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public void UpdateBox(BoxModel clientModel)
        {
            clientModel.Id = Context.ConnectionId;
            _broadcaster.UpdateBox(clientModel);
        }

        public override Task OnConnected()
        {
            var newBox = new BoxModel
            {
                Id = Context.ConnectionId,
                Left = _random.Next(10, 250),
                Top = _random.Next(10, 250)              
            };

            _broadcaster.AddBox(newBox);

            Clients.Caller.createBox(newBox);
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            _broadcaster.RemoveBox(Context.ConnectionId);

            Clients.AllExcept(Context.ConnectionId).removeBox(Context.ConnectionId);
            return base.OnDisconnected();
        }
    }
}