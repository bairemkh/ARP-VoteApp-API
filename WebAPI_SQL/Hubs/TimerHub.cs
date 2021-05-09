using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebAPI_SQL.Controllers;

namespace WebAPI_SQL.Hubs
{
    public class TimerHub: Hub
    {
        public async Task RaiseTimer(int Duration)
        {
            TimeSpan time = new TimeSpan(0, Duration, 0);
            for (int i = Convert.ToInt32(time.TotalSeconds); i >= 0; i--)
            {
                
                bool IsOver = (i == 0);                
                await Clients.All.SendAsync("Timer", time.Minutes, time.Seconds, IsOver);
                if (IsOver)
                {
                    Useful_Stuff.Useful_Methodes.CloseTheRoom();
                    //await Clients.All.SendAsync("AddToHistory", IsOver,class vote result);
                }

                time = time.Subtract(TimeSpan.FromSeconds(1));
                Thread.Sleep(1000);
            }

        }
    }
}
