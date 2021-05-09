using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI_SQL.Hubs
{
    public class MyHub:Hub
    {
        public async Task RaiseTimer(int Duration)
        {
            
            for (int i = Duration; i >= 0; i--)
            {
               
                bool IsOver = (i == 0);
                await Clients.All.SendAsync("Timer", i, IsOver);
                Thread.Sleep(1000);
            }

        }
    }
}
