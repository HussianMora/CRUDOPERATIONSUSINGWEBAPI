using CRUDWithWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace CRUDWithWebAPI.Controllers
{
    
    public class EmployeesWebAPIController : ApiController
    {
        private SqlConnection Connection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"Data Source=(LocalDb)\LocalDBDemo;Initial Catalog=EmployeeDB;Integrated Security=True");
                return connection;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<EmployeeModel> GetEmployees()
        {
            DataTable dataTable = new DataTable();
            try
            {
                var con = Connection();
                con.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("Select * from Employee", con);
                dataAdapter.Fill(dataTable);
                List<EmployeeModel> employees = new List<EmployeeModel>();
                employees = (from DataRow dr in dataTable.Rows
                             select new EmployeeModel()
                             {
                                 EmployeeID = (int)(dr["EmployeeID"]),
                                 FullName = (String)(dr["FullName"]),
                                 Country = (String)(dr["Country"]),
                                 State = (String)(dr["State"]),
                                 City = (String)(dr["City"]),
                                 DOB = (DateTime)(dr["DOB"]),
                             }).ToList();
                con.Close();
                return employees;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public IHttpActionResult PostEmployee(EmployeeModel employee)
        {
            try
            {
                    int result = 0;
                    string message = "";
                    var con = Connection();
                    con.Open();                
                    SqlCommand sqlCommand = new SqlCommand("Insert INTO Employee VALUES(@FullName,@DOB,@Country,@State,@City)", con);
                    sqlCommand.Parameters.AddWithValue("@FullName", employee.FullName);
                    sqlCommand.Parameters.AddWithValue("@Country", employee.Country);
                    sqlCommand.Parameters.AddWithValue("@State", employee.State);
                    sqlCommand.Parameters.AddWithValue("@City", employee.City);
                    sqlCommand.Parameters.AddWithValue("@DOB", employee.DOB);
                    result= sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                    {
                        message = "Success";
                    }
                    else
                    {
                        message = "Error";
                    }
                    if (message == "Success")
                    {
                        return Created<EmployeeModel>("Sucess",employee);
                    }
                    else
                    {
                        return StatusCode(HttpStatusCode.BadRequest);
                    }
                    
            }
            catch (Exception)
            {

                throw;
            }

        }
        
        public IHttpActionResult DeleteEmployee(int id)
        {
            try
            {
                int result = 0;
                string message = "";
                var con = Connection();
                con.Open();
                SqlCommand sqlCommand = new SqlCommand("DELETE FROM EMPLOYEE WHERE EmployeeID=@EmployeeId", con);
                sqlCommand.Parameters.AddWithValue("@EmployeeId", id);
                result = sqlCommand.ExecuteNonQuery();
                if (result > 0)
                {
                    message = "Success";
                }
                else
                {
                    message = "Error";
                }
                if(message=="Success")
                {
                    return Ok("Success");
                }
                else
                {
                    return StatusCode(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
       
       public IHttpActionResult GetEmployeeById(int id)
       {
            try
            {
                DataTable dataTable = new DataTable();
                var con = Connection();
                con.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("Select * from Employee WHERE EmployeeID=" + id, con);
                dataAdapter.Fill(dataTable);
                con.Close();
                var employeeObj = dataTable.Rows[0];
                EmployeeModel employee = new EmployeeModel();
                if (dataTable != null && employeeObj != null)
                {
                    employee.EmployeeID = !string.IsNullOrEmpty(Convert.ToString(employeeObj["EmployeeID"])) ? Convert.ToInt32(employeeObj["EmployeeID"]) : 0;
                    employee.FullName = (string)employeeObj["FullName"];
                    employee.Country = (string)employeeObj["Country"];
                    employee.State = (string)employeeObj["State"];
                    employee.City = (string)employeeObj["City"];
                    employee.DOB = (DateTime)employeeObj["DOB"];
                }
                return Json(employee);
            }
            catch (Exception)
            {

                throw;
            }
       }

        public IHttpActionResult PutEmployee(EmployeeModel employee)
        {
            int result = 0;
            string message = "";
            var con = Connection();
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("UPDATE EMPLOYEE SET FullName=@FullName,Country=@Country,State=@State,City=@City,DOB=@DOB WHERE EmployeeID=@EmployeeId", con);
            sqlCommand.Parameters.AddWithValue("@FullName", employee.FullName);
            sqlCommand.Parameters.AddWithValue("@Country", employee.Country);
            sqlCommand.Parameters.AddWithValue("@State", employee.State);
            sqlCommand.Parameters.AddWithValue("@City", employee.City);
            sqlCommand.Parameters.AddWithValue("@DOB", employee.DOB);
            sqlCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeID);
            result=sqlCommand.ExecuteNonQuery();
            if (result > 0)
            {
                message = "Success";
            }
            else
            {
                message = "Error";
            }
            if (message == "Success")
            {
                return Ok("Success");
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        [Route("api/EmployeesWebAPIController/Countries")]
        public IEnumerable<Country> GetCountries()
        {
            DataTable dataTable = new DataTable();
            try
            {
                var con = Connection();
                con.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("Select * from Country", con);
                dataAdapter.Fill(dataTable);
                List<Country> country = new List<Country>();
                country = (from DataRow dr in dataTable.Rows
                           select new Country()
                           {
                               CountryId = Convert.ToInt32(dr["CountryId"]),
                               Name = dr["Name"].ToString()
                           }).ToList();
                con.Close();
                return country;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("api/EmployeesWebAPIController/{id}/States")]
        public IEnumerable<State> GetStates(int id)
        {
            try
            {
                DataTable dataTable = new DataTable();
                var con = Connection();
                con.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM State WHERE CountryId=" + id, con);
                dataAdapter.Fill(dataTable);
                List<State> states = new List<State>();
                states = (from DataRow dr in dataTable.Rows
                           select new State()
                           {
                               StateId = Convert.ToInt32(dr["CountryId"]),
                               Name=(dr["Name"]).ToString()
                           }).ToList();
                con.Close();
                return states;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Route("api/EmployeesWebAPIController/{id}/Cities")]
        public IEnumerable<City> GetCities(int id)
        {
            try
            {
                DataTable dataTable = new DataTable();
                var con = Connection();
                con.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM City WHERE CityId=" + id, con);
                dataAdapter.Fill(dataTable);
                List<City> cities = new List<City>();
                cities = (from DataRow dr in dataTable.Rows
                          select new City()
                          {
                              CityId = Convert.ToInt32(dr["CityId"]),
                              Name = (dr["Name"]).ToString()
                          }).ToList();
                con.Close();
                return cities;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
