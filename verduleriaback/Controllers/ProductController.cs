  using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using verduleriaback.Models;

namespace verduleriaback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetProducts")]
        public JsonResult GetProducts()
        {
            string query = @"SELECT IDPRODUCT id
                                   ,NAMEPRODUCT name
                                   ,PRICEPRODUCT price
                                   ,STOCKPRODUCT status
                                   ,STATUSPRODUCT stat
                                   ,IMAGEPRODUCT picture
                                   FROM PRODUCT
                                   WHERE STATUSPRODUCT = 1";

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

        [HttpPost]
        [Route("BuyProduct")]
        public ProductoResponse UpdateProduct(ProductoRequest ProductoRequest)
        {
            ProductoResponse ProductoResponse = new ProductoResponse();
            try
            {
                string query = @"SET @STOCK = SELECT STOCKPRODUCT - "+ ProductoRequest.STOCKPRODUCT.ToString()  + " FROM PRODUCT WHERE IDPRODUCT = " + ProductoRequest.IDPRODUCT.ToString() + ";"
                                + " UPDATE PRODUCT"
                                + " SET STOCKPRODUCT = @STOCK"
                                + " WHERE IDPRODUCT = " + ProductoRequest.IDPRODUCT.ToString() + ";"
                                + " SELECT ROW_COUNT(); ";


                int RowsAffected = 0;

                string MySqlDataSource = _configuration.GetConnectionString("VerduleriaContext");
                MySqlDataReader RDR;
                using (MySqlConnection mySqlConnection = new MySqlConnection(MySqlDataSource))
                {
                    mySqlConnection.Open();
                    using (MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        RDR = mySqlCommand.ExecuteReader();

                        RowsAffected = RDR.GetInt32(0);

                        RDR.Close();
                        mySqlConnection.Close();
                    }
                }

                if (RowsAffected > 1)
                {
                    ProductoResponse.CODE = 1;
                    ProductoResponse.MESSAGE = "Transacción Exitosa!";
                }
                else
                {
                    ProductoResponse.CODE = 2;
                    ProductoResponse.MESSAGE = "Hubo un error al realizar la transacción.";
                }
            }
            catch(Exception ex)
            {
                ProductoResponse.CODE = 3;
                ProductoResponse.MESSAGE = "Error : " + ex.ToString();
            }

            return ProductoResponse;
        }


    }
}
