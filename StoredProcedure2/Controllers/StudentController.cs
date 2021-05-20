using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StoredProcedure2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private readonly string _connectionString;

        public StudentController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("connection_string");
        }

        [HttpGet]
        public IActionResult Get(int pageNumber)
        {
            List<StudentList> studentList = new List<StudentList>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("student_store", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@PageNumber", pageNumber);
                command.Parameters.AddWithValue("@PageSize", 10);

                SqlParameter outParameter1 = new SqlParameter
                {
                    ParameterName = "@FirstName",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 255,
                };

                SqlParameter outParameter2 = new SqlParameter
                {
                    ParameterName = "@LastName",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 255,

                };

                SqlParameter outParameter3 = new SqlParameter
                {
                    ParameterName = "@Age",
                    SqlDbType = System.Data.SqlDbType.Int,
                };

                SqlParameter outParameter4 = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = System.Data.SqlDbType.Int
                };

                outParameter1.Direction = System.Data.ParameterDirection.Output;
                outParameter2.Direction = System.Data.ParameterDirection.Output;
                outParameter3.Direction = System.Data.ParameterDirection.Output;
                outParameter4.Direction = System.Data.ParameterDirection.Output;

                SqlParameter[] parameters = { outParameter1, outParameter2, outParameter3, outParameter4 };

                command.Parameters.AddRange(parameters);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentList list = new StudentList()
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Age = reader.GetInt32(3)
                        };
                        studentList.Add(list);
                    };
                };
            }

            return Ok(studentList);
        }
    }

    public class StudentList
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
