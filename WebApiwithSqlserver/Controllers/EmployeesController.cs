using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace WebApiwithSqlserver.Controllers
{
    public class EmployeesController : ApiController
    {
        //public IEnumerable<Employee> Get()   // if we fix the prefix word Get then it will aumatically work as a Get
        //public IEnumerable<Employee> GetSomething()
        //{
        //    using (EmployeeApiDBEntities entities = new EmployeeApiDBEntities())
        //    {
        //        return entities.Employees.ToList();
        //    }
        //}

        [HttpGet]
        public IEnumerable<Employee> LoadAllEmployees()
        {
            using (EmployeeApiDBEntities entities = new EmployeeApiDBEntities())
            {
                return entities.Employees.ToList();
            }
        }


        //public HttpResponseMessage Get(int id)
        //{
        //    using (EmployeeApiDBEntities entities = new EmployeeApiDBEntities())
        //    {
        //        var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

        //        if (entity != null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, entity);
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
        //                "Employee with Id = " + id.ToString() + " not found");
        //        }
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage LoadEmployeeById(int id)
        {
            using (EmployeeApiDBEntities entities = new EmployeeApiDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with Id = " + id.ToString() + " not found");
                }
            }
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (EmployeeApiDBEntities entities = new EmployeeApiDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeApiDBEntities entities = new EmployeeApiDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id = " + id.ToString() + "not found to delete");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }

        public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        {
            using (EmployeeApiDBEntities entities = new EmployeeApiDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with id = " + id.ToString() + " No found to update");
                }
                else
                {
                    entity.FirstName = employee.FirstName;
                    entity.LastName = employee.LastName;
                    entity.Gender = employee.Gender;
                    entity.Salary = employee.Salary;
                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK,entity);
                }
                

            }
        }
    }
}
