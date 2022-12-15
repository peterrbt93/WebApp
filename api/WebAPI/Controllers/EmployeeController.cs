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
    public class EmployeeController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet(Name = "GetEmployees")]
        public IActionResult Get()
        {
            string query = @"select EmployeeId, EmployeeName, Department, 
                            DATE_FORMAT(DateOfJoining, '%Y-%m-%d') as DateOfJoining, 
                            PhotoFileName from Employee";
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
            string query = @"select EmployeeId, EmployeeName, Department, 
                            DATE_FORMAT(DateOfJoining, '%Y-%m-%d') as DateOfJoining, 
                            PhotoFileName from Employee WHERE EmployeeId=@id";
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
                return BadRequest("Change failed:" + e.Message);
            }

            conn.Close();
            string JSONString = JsonConvert.SerializeObject(table);
            return Ok(JSONString);

        }

        [HttpPost(Name = "PostEmployees")]
        public IActionResult Post(Employee emp)
        {
            try
            {
                string query = @"INSERT INTO Employee (EmployeeName,Department,
                                DateOfJoining,PhotoFileName) 
                                VALUES(@name, @department, @date, @file)";
                DataTable table = new DataTable();

                MySqlConnection conn = new MySqlConnection();
                MySqlCommand cmd = new MySqlCommand();
                conn.ConnectionString = @"server=localhost;uid=root;pwd=Eipi0314!;database=employeedb";
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@name", emp.EmployeeName);
                cmd.Parameters.AddWithValue("@department", emp.Department);
                cmd.Parameters.AddWithValue("@date", emp.DateOfJoining);
                cmd.Parameters.AddWithValue("@file", emp.PhotoFileName);


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

        [HttpPut(Name = "PutEmployee")]
        public IActionResult Put(Employee emp)
        {
            try
            {
                string query = @"UPDATE Employee SET 
                                EmployeeName=@name, 
                                Department=@department,
                                DateOfJoining=@date,
                                PhotoFileName=@file
                                WHERE EmployeeId=@id";
                DataTable table = new DataTable();

                MySqlConnection conn = new MySqlConnection();
                MySqlCommand cmd = new MySqlCommand();
                conn.ConnectionString = @"server=localhost;
                                        uid=root;
                                        pwd=Eipi0314!;
                                        database=employeedb";
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@id", emp.EmployeeId);
                cmd.Parameters.AddWithValue("@name", emp.EmployeeName);
                cmd.Parameters.AddWithValue("@department", emp.Department);
                cmd.Parameters.AddWithValue("@date", emp.DateOfJoining);
                cmd.Parameters.AddWithValue("@file", emp.PhotoFileName);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
                conn.Close();

                string JSONString = JsonConvert.SerializeObject("Change successfull");
                return Ok(JSONString);

            }
            catch (Exception e)
            {
                return BadRequest("Change failed:"+e.Message);
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM Employee WHERE EmployeeId=@id";
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

        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public IActionResult GetAllDepartmentNames()
        {
            string query = @"select DepartmentName from Department";
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
                return BadRequest("GetDepartments failed:" + e.Message);
            }

            conn.Close();
            string JSONString = JsonConvert.SerializeObject(table);
            return Ok(JSONString);

        }

        [Route("SaveFile")]
        [HttpPost]
        public IActionResult SaveFile()
        {
            try
            {
                var req = HttpContext.Request;
                var postedFile = req.Form.Files[0];
                string fileName = postedFile.FileName;
                string contentRootPath = _webHostEnvironment.ContentRootPath;

                string physicalPath = "";
                physicalPath = Path.Combine(contentRootPath , "Photos", fileName);
                postedFile.CopyTo(new FileStream(physicalPath, FileMode.Create));

                string JSONString = JsonConvert.SerializeObject(fileName);
                return Ok(JSONString);
            }
            catch (Exception e)
            {

                return BadRequest("An error occured: "+e.Message);
            }
        }

    }
}
