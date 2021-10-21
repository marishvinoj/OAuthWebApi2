using Newtonsoft.Json;
using OAuthWebApi2.Filters;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace OAuthWebApi2.Controllers
{
    [CustomAuthorize]
    public class ValuesController : ApiController
    {
        private UnitOfWork<CustomerDbEntities> unitOfWork = new UnitOfWork<CustomerDbEntities>();
        private GenericRepository<Employee> repository;

        public ValuesController()
        {
            repository = new GenericRepository<Employee>(unitOfWork);
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //This resource is For all types of role
        //[CustomAuthorize("User")]
        //[Authorize(Roles = "SuperAdmin, Admin, User")]

        //[CustomAuthorize]
        [HttpGet]
        [Route("api/values/getvalues")]
        public IHttpActionResult GetValues()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            var LogTime = identity.Claims
                      .FirstOrDefault(c => c.Type == "LoggedOn").Value;
            return Ok("Hello: " + identity.Name + ", " +
                "Your Role(s) are: " + string.Join(",", roles.ToList()) +
                "Your Login time is :" + LogTime);
        }


        //This resource is For all types of role
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet]
        [Route("api/values/getvalues1")]
        public IHttpActionResult GetValues1()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            var LogTime = identity.Claims
                      .FirstOrDefault(c => c.Type == "LoggedOn").Value;
            return Ok("Hello: " + identity.Name + ", " +
                "Your Role(s) are: " + string.Join(",", roles.ToList()) +
                "Your Login time is :" + LogTime);
        }

        //This resource is For all types of role
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/values/AddEmployee")]
        public IHttpActionResult AddEmployee(object emp)
        {
            try
            {
                var objEmployee = JsonConvert.DeserializeObject<Employee>(JsonConvert.SerializeObject(emp));
                unitOfWork.CreateTransaction();
                repository.Insert(objEmployee);
                unitOfWork.Save();
                //Do Some Other Task with the Database
                //If everything is working then commit the transaction else rollback the transaction
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw;
            }
            return Ok("Hello: ");
        }

        //This resource is For all types of role
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        [Route("api/values/AddBulkEmployee")]
        public IHttpActionResult AddBulkEmployee(object emp)
        {
            try
            {
                var objEmployees = (List<Employee>)emp;
                unitOfWork.CreateTransaction();
                foreach (var item in objEmployees)
                {
                    repository.Insert(item);
                }
                unitOfWork.Save();
                //Do Some Other Task with the Database
                //If everything is working then commit the transaction else rollback the transaction
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw;
            }
            return Ok("Hello: ");
        }

    }


}
