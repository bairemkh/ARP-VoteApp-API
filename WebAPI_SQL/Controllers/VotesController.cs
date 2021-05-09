using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using WebAPI_SQL.Useful_Stuff;
using WebAPI_SQL.Entities;

namespace WebAPI_SQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        
       

        [HttpPost("Vote")]
        public string Vote(UserVote vote)

        {
            var session = Useful_Methodes.getLastSession();
            if (session.SessionState.ToLower().Equals("closed"))
                return "Time Up";
            SqlCommand command = new SqlCommand("INSERT INTO [dbo].[Vote](SessionId, UserId, Vote) VALUES(" + Useful_Stuff.Useful_Methodes.getLastSessionId() + ", '" + vote.UserId + "', '" + vote.Vote + "')", DataBaseManager.connect); ;

            if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                DataBaseManager.connect.Close();
            DataBaseManager.connect.Open();
            try
            {
                int x = command.ExecuteNonQuery();
                if (x == 1)
                {
                    return "Inserted";
                }
                return "not inserted";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key"))
                {
                    return "User Alredy Voted";
                }
                if (ex.Message.Contains("The conflict occurred in database "))
                    return "User Not In the database";
                    
                return "Unknown Error ==>  "+ex.Message;
            }

        }

        [HttpGet("Get YesVote")]
        public int YesVotes()
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
             return int.Parse(datatable.Rows[0][0].ToString()); }
            catch (Exception)
            {
                return -1;
            }
           
        }

        [HttpGet("Get NoVote")]
        public int NoVotes()
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

        [HttpGet("Get ReservedVote")]
        public int ReservedVotes()
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

        [HttpGet("RetriveUserVote")]
        public UserVote RetriveUserVote(string UserId)
        {
            SqlCommand command = new SqlCommand("Select UserId, Vote  from [dbo].[Vote] WHERE SessionId=" + Useful_Stuff.Useful_Methodes.getLastSessionId() + " AND UserId='"+UserId+"';", DataBaseManager.connect);
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
                UserVote vote = new UserVote(UserId,"Didn't Vote");
                return vote;
            }
            
        }

        [HttpGet("VotersNumber")]
        public int VotersNumber()
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
        [HttpGet("MustVoted")]
        public string MustVoted()
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
                return "Retained";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }




    }
}
