using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class LawsController : ControllerBase
    {
        [HttpGet("GetLaw")]
        public Law GetLaw(int Chapter,int Number)
        {
           

            try
            {
                string Query = "Select *  from [dbo].[Law] where LawChapter = " + Chapter + " and LawNumber = " + Number + " ;";
                SqlCommand command = new SqlCommand(Query, DataBaseManager.connect);
                if (DataBaseManager.connect.State == System.Data.ConnectionState.Open)
                    DataBaseManager.connect.Close();
                DataBaseManager.connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                var datatable = new DataTable();
                datatable.Load(reader);
                DataBaseManager.connect.Close();
                Law law = new Law(datatable.Rows[0][0].ToString().Trim(' '), int.Parse( datatable.Rows[0][1].ToString().Trim(' ')), int.Parse(datatable.Rows[0][2].ToString().Trim(' ')), datatable.Rows[0][3].ToString().Trim(' '));
                return law;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
