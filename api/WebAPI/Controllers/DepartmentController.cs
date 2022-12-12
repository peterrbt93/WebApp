using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        [HttpGet(Name = "GetDepartments")]
        public IActionResult Get()
        {
            string query = @"select DepartmentId, DepartmentName from Department";
            DataTable table = new DataTable();

            MySqlConnection conn = new MySqlConnection();
            MySqlCommand cmd = new MySqlCommand();
            conn.ConnectionString = @"server=localhost;uid=root;pwd=Eipi0314!;database=employeedb";

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Prepare();

                MySqlDataReader rdr = cmd.ExecuteReader();

                table.Load(rdr);

            }
            catch (Exception e)
            {
                return BadRequest("Get failed:" + e.Message);
            }

            conn.Close();
            string JSONString = JsonConvert.SerializeObject(table);
            return Ok(JSONString);

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string query = @"select DepartmentId, DepartmentName from Department WHERE DepartmentId=@id";
            DataTable table = new DataTable();

            MySqlConnection conn = new MySqlConnection();
            MySqlCommand cmd = new MySqlCommand();
            conn.ConnectionString = @"server=localhost;uid=root;pwd=Eipi0314!;database=employeedb";

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                MySqlDataReader rdr = cmd.ExecuteReader();

                table.Load(rdr);

            }
            catch (Exception e)
            {
                return BadRequest("Get failed:" + e.Message);
            }

            conn.Close();
            string JSONString = JsonConvert.SerializeObject(table);
            return Ok(JSONString);

        }

        [HttpPost(Name = "PostDepartments")]
        public IActionResult Post(Department dep)
        {
            try
            {
                string query = @"INSERT INTO Department (DepartmentName) VALUES(@name)";
                DataTable table = new DataTable();

                MySqlConnection conn = new MySqlConnection();
                MySqlCommand cmd = new MySqlCommand();
                conn.ConnectionString = @"server=localhost;uid=root;pwd=Eipi0314!;database=employeedb";
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@name", dep.DepartmentName);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
                conn.Close();

                string JSONString = JsonConvert.SerializeObject("Insert successfull");
                return Ok(JSONString);

            }
            catch (Exception e)
            {
                return BadRequest("Insert failed:" + e.Message);
            }

        }

        [HttpPut(Name = "PutDepartments")]
        public IActionResult Put(Department dep)
        {
            try
            {
                string query = @"UPDATE Department SET DepartmentName=@name WHERE DepartmentId=@id";
                DataTable table = new DataTable();

                MySqlConnection conn = new MySqlConnection();
                MySqlCommand cmd = new MySqlCommand();
                conn.ConnectionString = @"server=localhost;uid=root;pwd=Eipi0314!;database=employeedb";
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@name", dep.DepartmentName);
                cmd.Parameters.AddWithValue("@id", dep.DepartmentId);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
                conn.Close();

                string JSONString = JsonConvert.SerializeObject("Change successfull");
                return Ok(JSONString);

            }
            catch (Exception e)
            {
                return BadRequest("Change failed:" + e.Message);
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM Department WHERE DepartmentId=@id";
                DataTable table = new DataTable();

                MySqlConnection conn = new MySqlConnection();
                MySqlCommand cmd = new MySqlCommand();
                conn.ConnectionString = @"server=localhost;uid=root;pwd=Eipi0314!;database=employeedb";
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@id", id);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
                conn.Close();

                string JSONString = JsonConvert.SerializeObject("Delete successfull");
                return Ok(JSONString);

            }
            catch (Exception e)
            {
                return BadRequest("Delete failed:" + e.Message);
            }

        }
    }
}
