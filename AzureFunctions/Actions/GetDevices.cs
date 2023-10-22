using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace AzureFunctions.Actions
{
    public class GetDevices
    {
        private readonly IConfiguration _configuration;
        private RegistryManager _registryManager;

        public GetDevices(IConfiguration configuration)
        {
            _configuration = configuration;
            try
            {
                var iotHubConnectionString = _configuration.GetConnectionString("IotHub");
                Console.WriteLine($"IoT Hub Connection String: {iotHubConnectionString}");

                _registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            }
            catch (Exception ex)
            {
                // Log or print the exception details to understand the issue.
                Console.WriteLine($"Error creating RegistryManager: {ex.Message}");
            }
        }

        [Function("GetDevices")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "devices")] HttpRequestData req)
        {
            var devices = new List<Twin>();

            var result = _registryManager.CreateQuery("select * from devices");
            foreach (var device in await result.GetNextAsTwinAsync())
                devices.Add(device);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(JsonConvert.SerializeObject(devices));
            return response;
        }
    }
}
