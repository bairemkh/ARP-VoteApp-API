using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI_SQL.Entities
{
    public class User
    {
        public string UserId { get; set; }
        public string UserPass { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserType { get; set; }
        public string UserPoliticalParty { get; set; }

        public byte[] UserImage { get; set; }

       public User(string userId, string userPass, string userName, string userLastName, string userType, string userPoliticalParty,byte[] userImage)
        {
            UserId = userId;
            UserPass = userPass;
            UserName = userName;
            UserLastName = userLastName;
            UserType = userType;
            UserPoliticalParty = userPoliticalParty;
            UserImage = userImage;
        }
    }
}
