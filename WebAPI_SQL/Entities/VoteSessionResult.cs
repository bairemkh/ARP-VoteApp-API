using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_SQL.Entities
{
    public class VoteSessionResult
    {
        public int TotalVotes { get; set; }
        public int YesVotes { get; set; }
        public int NolVotes { get; set; }
        public int RetainedVotes { get; set; }
        public string FinalVote { get; set; }

        public VoteSessionResult(int totalVotes, int yesVotes, int nolVotes, int retainedVotes, string finalVote)
        {
            TotalVotes = totalVotes;
            YesVotes = yesVotes;
            NolVotes = nolVotes;
            RetainedVotes = retainedVotes;
            FinalVote = finalVote;
        }
    }
}
