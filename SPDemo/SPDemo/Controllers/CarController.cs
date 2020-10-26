using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SPDemo.Models;

namespace SPDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly IDbConnection con;
        public CarController()
        {
            con = new SqlConnection("Server=DSK-992\\SQL2019;Database=DbCar;Trusted_Connection=True;");
            if (con.State == ConnectionState.Closed)
                con.Open();
        }

        [Route("GetCars")]
        [HttpGet]
        public IEnumerable<TblCar> GetAllCars()
        {
            List<TblCar> cars = new List<TblCar>();
            cars = con.Query<TblCar>("USP_GetAllCars").ToList();
            return cars;
        }

        [Route("AddCar")]
        [HttpPost]
        public IActionResult AddCar(TblCar car)
        {
            int rowsAffected;

            //DynamicParameters parameters = new DynamicParameters();                   //normal way
            //parameters.Add("@Brand",car.Brand);
            //parameters.Add("@Model",car.Model);
            //parameters.Add("@Price",car.Price);
            DataTable data = new DataTable();                               // using Datatable
            data.Columns.Add("Brand", typeof(string));
            data.Columns.Add("Model", typeof(string));
            data.Columns.Add("Price", typeof(int));

            data.Rows.Add(car.Brand, car.Model, car.Price);

            //rowsAffected = con.Execute("USP_AddCar", parameters , commandType: CommandType.StoredProcedure);
            rowsAffected = con.Execute("USP_AddCar", new { car = data.AsTableValuedParameter("dbo.UDT_Car") }, commandType: CommandType.StoredProcedure);

            if (rowsAffected != 0)
                return Ok();
            else
                return BadRequest();
        }

        [Route("UpdateCar")]
        [HttpPut]
        public IActionResult UpdateCar([FromQuery] int id, [FromBody] TblCar car)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            int rowsAffected;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Brand", car.Brand);
            parameters.Add("@Model", car.Model);
            parameters.Add("@Price", car.Price);

            rowsAffected = con.Execute("USP_EditCar", parameters, commandType: CommandType.StoredProcedure);

            if (rowsAffected != 0)
                return Ok();
            else
                return BadRequest();
        }

        [Route("DeleteCar")]
        [HttpDelete]
        public IActionResult DeleteCar([FromQuery] int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            int rowsAffected;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            rowsAffected = con.Execute("USP_DeleteCar", parameters, commandType: CommandType.StoredProcedure);

            if (rowsAffected != 0)
                return Ok();
            else
                return BadRequest();
        }



    }
}
