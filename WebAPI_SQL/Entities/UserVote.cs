using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_SQL.Entities
{
    public class UserVote
    {
        public string UserId { get; set; }
        public string Vote { get; set; }

        public UserVote(string userId, string vote)
        {
            UserId = userId;
            this.Vote = vote;
        }
    }
}
