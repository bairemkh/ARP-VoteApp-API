using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;
using WebAPI_SQL.Services;

namespace WebAPI_SQL.Useful_Stuff
{
    public static class Useful_Methodes
    {
        #region Voting Room
        public static int getLastSessionId()
        {
            SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM [dbo].[Voting Session] ORDER BY SessionId DESC", DataBaseManager.connect);

            if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                DataBaseManager.connect.Close();
            DataBaseManager.connect.Open();
            SqlDataReader reader = command.ExecuteReader();
            var datatable = new DataTable();
            datatable.Load(reader);
            DataBaseManager.connect.Close();
            try
            {
                return int.Parse(datatable.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region GetLastSession
        public static VotingRoom getLastSession()
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT TOP 1 Duration,Date,Time,Subject,SessionState FROM [dbo].[Voting Session] ORDER BY SessionId DESC", DataBaseManager.connect);

                if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                    DataBaseManager.connect.Close();
                DataBaseManager.connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                var datatable = new DataTable();
                datatable.Load(reader);
                DataBaseManager.connect.Close();
                VotingRoom votingRoom = new VotingRoom(Convert.ToInt32(datatable.Rows[0][0]), datatable.Rows[0][1].ToString(), datatable.Rows[0][2].ToString(), datatable.Rows[0][3].ToString(), datatable.Rows[0][4].ToString().Trim(' '));


                return votingRoom;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public static void CloseTheRoom()
        {
            try
            {

                SqlCommand command = new SqlCommand("UPDATE [dbo].[Voting Session] SET SessionState='closed', TotalVotes=" + VotersNumber() + ",YesVotes=" + YesVotes() + ",NoVotes=" + NoVotes() + ",RetainedVotes=" + ReservedVotes() + ", FinalVote='" + MustVoted()+ "', SessionFile="+GeneratePdf() + " where SessionId=" + Useful_Stuff.Useful_Methodes.getLastSessionId(), DataBaseManager.connect);

                if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                    DataBaseManager.connect.Close();
                DataBaseManager.connect.Open();
                int x = command.ExecuteNonQuery();


            }
            catch (Exception)
            {

            }
        }

        public static int YesVotes()
        {

            SqlCommand command = new SqlCommand("SELECT COUNT(Vote) FROM[dbo].[Vote] WHERE SessionId=" + Useful_Stuff.Useful_Methodes.getLastSessionId() + " AND Vote='yes';", DataBaseManager.connect);
            if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                DataBaseManager.connect.Close();
            DataBaseManager.connect.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                var datatable = new DataTable();
                datatable.Load(reader);
                DataBaseManager.connect.Close();
                return int.Parse(datatable.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return -1;
            }

        }


        public static int NoVotes()
        {

            SqlCommand command = new SqlCommand("SELECT COUNT(Vote) FROM[dbo].[Vote] WHERE SessionId=" + Useful_Stuff.Useful_Methodes.getLastSessionId() + " AND Vote='no';", DataBaseManager.connect);
            if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                DataBaseManager.connect.Close();
            DataBaseManager.connect.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                var datatable = new DataTable();
                datatable.Load(reader);
                DataBaseManager.connect.Close();
                return int.Parse(datatable.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return -1;
            }

        }


        public static int ReservedVotes()
        {

            SqlCommand command = new SqlCommand("SELECT COUNT(Vote) FROM[dbo].[Vote] WHERE SessionId=" + Useful_Stuff.Useful_Methodes.getLastSessionId() + " AND Vote='Retained';", DataBaseManager.connect);
            if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                DataBaseManager.connect.Close();
            DataBaseManager.connect.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                var datatable = new DataTable();
                datatable.Load(reader);
                DataBaseManager.connect.Close();
                return int.Parse(datatable.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return -1;
            }

        }


        public static UserVote RetriveUserVote(string UserId)
        {
            SqlCommand command = new SqlCommand("Select UserId, Vote  from [dbo].[Vote] WHERE SessionId=" + Useful_Stuff.Useful_Methodes.getLastSessionId() + " AND UserId='" + UserId + "';", DataBaseManager.connect);
            if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                DataBaseManager.connect.Close();
            DataBaseManager.connect.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                var datatable = new DataTable();
                datatable.Load(reader);
                DataBaseManager.connect.Close();
                UserVote vote = new UserVote(datatable.Rows[0][0].ToString().Trim(' '), datatable.Rows[0][1].ToString().Trim(' '));
                return vote;

            }
            catch (Exception)
            {
                UserVote vote = new UserVote(UserId, "Didn't Vote");
                return vote;
            }

        }


        public static int VotersNumber()
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(Vote) FROM[dbo].[Vote] WHERE SessionId=" + Useful_Stuff.Useful_Methodes.getLastSessionId() + ";", DataBaseManager.connect);
            if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                DataBaseManager.connect.Close();
            DataBaseManager.connect.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                var datatable = new DataTable();
                datatable.Load(reader);
                DataBaseManager.connect.Close();
                return int.Parse(datatable.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static string MustVoted()
        {

            try
            {
                var yes = YesVotes();
                var no = NoVotes();
                var retained = ReservedVotes();
                var max = Math.Max(yes, no);
                max = Math.Max(max, retained);
                if (max == yes)
                    return "Yes";
                if (max == no)
                    return "No";
                if ((max == no) && (max == yes) && (max == retained))
                    return "All votes are equal";
                if ((max == no) && (max == yes))
                    return "equal votes";
                if ((max == retained) && (max == yes))
                    return "equal votes";
                if ((max == no) && (max == retained))
                    return "equal votes";
                return "Retained";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            string query = "Select UserId, UserPass, UserName,UserLastName,UserType,UserPoliticalParty,UserImage  from [dbo].[User] WHERE UserType='user';";
            SqlCommand command = new SqlCommand(query, DataBaseManager.connect);
            if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                DataBaseManager.connect.Close();
            DataBaseManager.connect.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                var datatable = new DataTable();
                datatable.Load(reader);
                DataBaseManager.connect.Close();
                for (int i=0; i<datatable.Rows.Count; i++)
                {
                    User user = new User(datatable.Rows[i][0].ToString().Trim(' '), datatable.Rows[i][1].ToString().Trim(' '), datatable.Rows[i][2].ToString().Trim(' '), datatable.Rows[i][3].ToString().Trim(' '), datatable.Rows[i][4].ToString().Trim(' '), datatable.Rows[i][5].ToString().Trim(' '), (byte[])datatable.Rows[i][6]);
                    users.Add(user);
                }    

              return users;
            }
            catch (Exception e)
            {
                throw;
            }


        }
       
        public static VoteSessionResult GetResult()
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT TOP 1 TotalVotes,YesVotes,NoVotes,RetainedVotes,FinalVote,SessionFile FROM [dbo].[Voting Session] ORDER BY SessionId DESC", DataBaseManager.connect);

                if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                    DataBaseManager.connect.Close();
                DataBaseManager.connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                var datatable = new DataTable();
                datatable.Load(reader);
                DataBaseManager.connect.Close();
                var x = datatable.Rows[0][0].ToString();
                VoteSessionResult sessionResult = new VoteSessionResult(Convert.ToInt32(datatable.Rows[0][0]), Convert.ToInt32(datatable.Rows[0][1]), Convert.ToInt32(datatable.Rows[0][2]), Convert.ToInt32(datatable.Rows[0][3]), datatable.Rows[0][4].ToString().Trim(' '), (byte[])datatable.Rows[0][5]);


                return sessionResult;
            }
            catch (Exception ex)
            {

                VoteSessionResult res = new VoteSessionResult(0, 0, 0, 0, ex.Message,null);
                return res;
            }
            finally
            {
                DataBaseManager.connect.Close();
                
            }
        }
        public static byte[] GeneratePdf()
        {
            var html = PDFTemplateGenerator.GeneratePdf();
            var votesession = Useful_Methodes.getLastSession();
            HtmlToPdf htmlToPdf = new HtmlToPdf();
            var Pdf = htmlToPdf.ConvertHtmlString(html);
            var file = Pdf.Save();
            Pdf.Close();
            return file;
        }
        
    }
}
