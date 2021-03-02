using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateEmployee_Returns_Created()
        {
            // Arrange
            var compensationRequest = new CompensationRequest()
            {
                EffectiveDate = "04-01-2021", 
                EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3", 
                Salary = 100000
            };

            var requestContent = new JsonSerialization().ToJson(compensationRequest);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<CompensationResponse>();
            Assert.IsNotNull(newCompensation.Employee);
            Assert.AreEqual(compensationRequest.EmployeeId, newCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensationRequest.Salary, newCompensation.Salary);
            Assert.AreEqual(compensationRequest.EffectiveDate, newCompensation.EffectiveDate.ToString("MM-dd-yyyy"));
        }

        [TestMethod]
        public void Create_Returns_BadRequest_InvalidEmpId()
        {
            // Arrange
            var compensationRequest = new CompensationRequest()
            {
                EffectiveDate = "04-01-2021",
                EmployeeId = "b7839309-3348-463b-a7e3",
                Salary = 100000
            };

            var requestContent = new JsonSerialization().ToJson(compensationRequest);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Create_Returns_BadRequest_InvalidSalary()
        {
            // Arrange
            var compensationRequest = new CompensationRequest()
            {
                EffectiveDate = "04-01-2021",
                EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
                Salary = -100000
            };

            var requestContent = new JsonSerialization().ToJson(compensationRequest);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Create_Returns_BadRequest_InvalidDate()
        {
            // Arrange
            var compensationRequest = new CompensationRequest()
            {
                EffectiveDate = "04-202021",
                EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
                Salary = 100000
            };

            var requestContent = new JsonSerialization().ToJson(compensationRequest);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_Returns_Empty()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var compensations = response.DeserializeContent<IEnumerable<CompensationResponse>>();
            Assert.IsTrue(!compensations.Any());
        }

    }
}
