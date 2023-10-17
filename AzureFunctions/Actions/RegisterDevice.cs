using System.Net;
using Azure;
using AzureFunctions.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions.Actions
{
    public class RegisterDevice
    {
        private static readonly string _connectionString = "HostName=elliot-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=HVjAs0+TdO15cXjtpQj4dZobiOVJaWIxDAIoTHqyU50=";
        private readonly RegistryManager _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);

        [Function("RegisterDevice")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {

            var response = req.CreateResponse(HttpStatusCode.BadRequest);

            var data = JsonConvert.DeserializeObject<DeviceItem>( new StreamReader(req.Body).ReadToEnd());
            if(!string.IsNullOrEmpty(data?.DeviceId))
            {
                var device = await _registryManager.GetDeviceAsync(data.DeviceId);
                device ??= await _registryManager.AddDeviceAsync(new Device(data.DeviceId));

                var connectionString = $"{_connectionString.Split(";")[0]};DeviceId=;SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";

                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; application/json");
                response.WriteString(connectionString);
               

            }
            return response;

        }
    }
}
