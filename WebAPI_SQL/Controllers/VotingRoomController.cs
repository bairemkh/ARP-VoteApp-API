﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;

namespace WebAPI_SQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingRoomController : ControllerBase
    {
       
        [HttpPost("CreateVotingSession")]
        public string CreateVotingRoom(VotingRoom votingRoom)
        {
            
            try
            {

                
                
                
                //string Query = "INSERT INTO [dbo].[Voting Session] (Duration, Date, Time,Subject,LawId) VALUES(" + duration + ", '" + date + "', '" + time + "','" + subject + "','" + LawRef + "') ";
               SqlCommand command = new SqlCommand("INSERT INTO [dbo].[Voting Session](Duration,Date,Time,Subject) VALUES(" + votingRoom.duration + ",'"+ votingRoom.date+ "','"+ votingRoom.time+ "', N'" + votingRoom.VoteSubject + "');", DataBaseManager.connect);

            if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                    DataBaseManager.connect.Close();
                DataBaseManager.connect.Open();
                int x = command.ExecuteNonQuery();
                if (x == 1)
                {
                    return "Inserted";
                }
                return "Not Inserted";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        [HttpGet("GetVotingSession")]
        public VotingRoom GetVotingRoom()
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
        [HttpGet("CloseRoom")]
        public string CloseRoom()
        {
            try
            {
                var y = Useful_Stuff.Useful_Methodes.GeneratePdf();
                SqlCommand command = new SqlCommand("UPDATE [dbo].[Voting Session] SET SessionState='closed', SessionFile= @file where SessionId="+ Useful_Stuff.Useful_Methodes.getLastSessionId(), DataBaseManager.connect);
                command.Parameters.AddWithValue("@file", y);
                if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                    DataBaseManager.connect.Close();
                DataBaseManager.connect.Open();
                int x = command.ExecuteNonQuery();
                return $"{x} Rows Updated";

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        [HttpGet("GetResult")]
        public VoteSessionResult GetResult()
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
                VoteSessionResult sessionResult = new VoteSessionResult(Convert.ToInt32(datatable.Rows[0][0]), Convert.ToInt32(datatable.Rows[0][1]), Convert.ToInt32(datatable.Rows[0][2]), Convert.ToInt32(datatable.Rows[0][3]), datatable.Rows[0][4].ToString().Trim(' '),(byte[])datatable.Rows[0][5]);


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
        [HttpGet("GetLastId")]
        public string getLastSessionId()
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
               var Json = JsonConvert.SerializeObject(int.Parse(datatable.Rows[0][0].ToString()));
                return Json ;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
