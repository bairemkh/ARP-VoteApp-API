using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebAPI_SQL
{
    public class DataBaseManager
    {
        public static SqlConnection connect = new SqlConnection("Server=tcp:voteapplication.database.windows.net,1433;Initial Catalog=ApplicationDataBase;Persist Security Info=False;User ID=bairemDali;Password=BAIREMpairem123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
       
       /* public static void VotingRoomCreation()
        {
           
                string Query = "CREATE TABLE [dbo].[Voting Session] (UserId    NCHAR(10) NOT NULL," +
                "LawId     NCHAR(10) NOT NULL," +
                "Vote      NCHAR(10) NULL," +
                "CONSTRAINT[PK_Voting Session] PRIMARY KEY CLUSTERED(UserId ASC, [LawId] ASC, SessionID ASC)," +
                "CONSTRAINT[FK_Voting Session_Law] FOREIGN KEY(LawId) REFERENCES[dbo].[Law](LawId)," +
                "CONSTRAINT[FK_Voting Session_User] FOREIGN KEY(UserId) REFERENCES[dbo].[User](UserId));";

            connect.Open();

            SqlCommand command = new SqlCommand(Query, connect);
            connect.Close();

        }*/
        public static void VotingRoomRemoval()
        {

            string Query = "Drop Table [dbo].[Voting Session] ";



            SqlCommand command = new SqlCommand(Query, connect);
            connect.Close();
        }

    }
}
