using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_SQL.Entities
{
    public class Law
    {
        public string LawId { get; set; }
        public int LawChapter { get; set; }
        public int LawNumber { get; set; }
        public string LawDesc { get; set; }

        public Law(string lawId, int lawChapter, int lawNumber, string lawDesc)
        {
            LawId = lawId;
            LawChapter = lawChapter;
            LawNumber = lawNumber;
            LawDesc = lawDesc;
        }
    }
}
