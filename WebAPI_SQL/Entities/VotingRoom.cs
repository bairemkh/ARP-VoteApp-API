using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_SQL.Entities
{
    public class VotingRoom
    {
        
        
        public int duration { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string VoteSubject { get; set; }
        public string SessionState { get; set; }
        

        public VotingRoom(int duration,  string date, string time, string voteSubject, string sessionState)
        {
            
            this.duration = duration;
            this.date = date;
            this.time = time;
            this.VoteSubject = voteSubject;
            this.SessionState = sessionState;
        }
        
    }
}
