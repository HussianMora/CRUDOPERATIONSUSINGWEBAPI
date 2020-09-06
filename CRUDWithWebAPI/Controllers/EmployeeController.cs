using CRUDWithWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace CRUDWithWebAPI.Controllers
{
    public class EmployeeController : Controller
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

        public ActionResult Index()
        {
            List<EmployeeModel> employees = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44370/api/");
                var responseTask = client.GetAsync("EmployeesWebAPI");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<EmployeeModel>>();
                    readTask.Wait();
                    employees = (List<EmployeeModel>)readTask.Result;
                }
                else
                {
                    //employees = List<EmployeeModel>.Empty();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(employees);
        }

        public ActionResult Home()
        {
            List<Country> countries = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44370/api/");
                var responseTask = client.GetAsync("EmployeesWebAPIController/Countries");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Country>>();
                    readTask.Wait();
                    countries = (List<Country>)readTask.Result;
                    ViewBag.CountryList = new SelectList(countries, "CountryId", "Name");
                }
                else
                {
                    //employees = List<EmployeeModel>.Empty();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

                //}
                //try
                //{
                //    DataTable dataTable = new DataTable();
                //    var con = Connection();
                //    con.Open();
                //    SqlDataAdapter dataAdapter = new SqlDataAdapter("Select * from Country", con);
                //    dataAdapter.Fill(dataTable);
                //    List<Country> country = new List<Country>();
                //    country = (from DataRow dr in dataTable.Rows
                //               select new Country()
                //               {
                //                   CountryId = Convert.ToInt32(dr["CountryId"]),
                //                   Name = dr["Name"].ToString()
                //               }).ToList();
                //    ViewBag.CountryList = new SelectList(country, "CountryId", "Name");
                //}
                //catch (Exception)
                //{ }
            }
                return View();
        }

        public string getstates(int country)
        {
            try
            {
                List<State> states = null;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44370/api/");
                    var responseTask = client.GetAsync("EmployeesWebAPIController/"+country.ToString()+ "/States");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<State>>();
                        readTask.Wait();
                        states = (List<State>)readTask.Result;
                        var info = states.AsEnumerable().Select(c => new SelectListItem { Text = c.Name, Value = Convert.ToString(c.StateId) }).ToList();
                        //.Select(c => new SelectListItem { Text = c.["Name"], Value = c["Name"] }).ToList();
                        return Newtonsoft.Json.JsonConvert.SerializeObject(info);
                    }
                    else
                    {
                        return "Error";
                    }
                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        public string GetCities(int state)
        {
            try
            {
                List<City> cities = null;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44370/api/");
                    var responseTask = client.GetAsync("EmployeesWebAPIController/" + state.ToString() + "/Cities");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<City>>();
                        readTask.Wait();
                        cities = (List<City>)readTask.Result;
                        var info = cities.AsEnumerable().Select(c => new SelectListItem { Text = c.Name, Value = Convert.ToString(c.CityId) }).ToList();
                        //.Select(c => new SelectListItem { Text = c.["Name"], Value = c["Name"] }).ToList();
                        return Newtonsoft.Json.JsonConvert.SerializeObject(info);
                    }
                    else
                    {
                        return "Error";
                    }
                }
            }

            catch (Exception)
            {

                throw;
            }
        }

        public string Insert(EmployeeModel employeeModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44370/api/");
                var postTask = client.PostAsJsonAsync<EmployeeModel>("EmployeesWebAPI", employeeModel);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return "AddSuccess";
                }
                else
                {
                    return "Error";
                }
            }
        }

        public JsonResult GetEmployeeById(int id)
        {
            EmployeeModel employeeModel = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44370/api/");

                var responseTask = client.GetAsync("EmployeesWebAPI?id=" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<EmployeeModel>();
                    readTask.Wait();
                    employeeModel = readTask.Result;
                }
            }
            return Json(employeeModel, JsonRequestBehavior.AllowGet);
        }

        public string DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44370/api/");
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("EmployeesWebAPI/" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return "Success";
                }
                else
                {
                    return "Error";
                }
            }
        }

        public string Update(EmployeeModel employeeModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44370/api/");
                var putTask = client.PutAsJsonAsync<EmployeeModel>("EmployeesWebAPI", employeeModel);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return "Success";
                }
                else
                {
                    return "Error";
                }
            }
        }
    }
}
