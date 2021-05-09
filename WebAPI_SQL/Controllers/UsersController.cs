using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_SQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
       

        #region Login
        [HttpGet("Login")]
        public User Auth(string id, string password)
        {
            try
            {
                string Query = "Select UserId, UserPass, UserName,UserLastName,UserType,UserPoliticalParty,UserImage  from [dbo].[User] where UserId = '" + id + "' AND UserPass = '" + password + "';";
                SqlCommand command = new SqlCommand(Query, DataBaseManager.connect);
                if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                    DataBaseManager.connect.Close();
                DataBaseManager.connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                var datatable = new DataTable();
                datatable.Load(reader);
                DataBaseManager.connect.Close();

                User user = new User(datatable.Rows[0][0].ToString().Trim(' '), datatable.Rows[0][1].ToString().Trim(' '), datatable.Rows[0][2].ToString().Trim(' '), datatable.Rows[0][3].ToString().Trim(' '), datatable.Rows[0][4].ToString().Trim(' '), datatable.Rows[0][5].ToString().Trim(' '), (byte[])datatable.Rows[0][6]); 
                return user;
            }
            catch (Exception)
            {
                
                return null;
            }


        }
        #endregion
        [HttpGet("GetAllUsers")]
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            string query = "Select UserId, UserPass, UserName,UserLastName,UserType,UserPoliticalParty,UserImage  from [dbo].[User] WHERE UserType='user'";
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
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    User user = new User(datatable.Rows[i][0].ToString().Trim(' '), datatable.Rows[i][1].ToString().Trim(' '), datatable.Rows[i][2].ToString().Trim(' '), datatable.Rows[i][3].ToString().Trim(' '), datatable.Rows[i][4].ToString().Trim(' '), datatable.Rows[i][5].ToString().Trim(' '), (byte[])datatable.Rows[i][6]);
                    users.Add(user);
                }

                return users;
            }
            catch (Exception)
            {
                throw;
            }


        }




    }
}

