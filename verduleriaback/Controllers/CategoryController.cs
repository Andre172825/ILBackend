using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace verduleriaback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetCategory()
        {
            string query = @"SELECT IDCATEGORY
                                   ,NAMECATEGORY
                                   ,STATUSCATEGORY
                                   FROM CATEGORY
                                   WHERE STATUSCATEGORY = 1";

            DataTable table = new DataTable();
            string MySqlDataSource = _configuration.GetConnectionString("VerduleriaContext");
            MySqlDataReader RDR;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MySqlDataSource))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection))
                {
                    RDR = mySqlCommand.ExecuteReader();

                    table.Load(RDR);

                    RDR.Close();
                    mySqlConnection.Close();
                }
            }

            return new JsonResult(table);
        }


    }
}
