﻿using DataAccess.Devices.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Devices.Services
{
    public class DeviceManager
    {
        public bool IsConfigured {  get; private set; }
        private DeviceClient? _deviceClient;
        public event Action? IsConfiguredChanged;



        public DeviceManager()
        {

        }
        public DeviceManager(string deviceId, string url)
        {
            Task.Run(() => InitializeAsync(deviceId, url));
        }
        public async Task InitializeAsync(string deviceId, string url)
        {


            var deviceItem = new DeviceItem { DeviceId = deviceId };

            using var http = new HttpClient();
            var result = await http.PostAsync(url, new StringContent(JsonConvert.SerializeObject(deviceItem)));
            if (result.IsSuccessStatusCode)
            {
                var connectionString = await result.Content.ReadAsStringAsync();
                FileHandler.SaveToFile(@$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\config_{deviceItem.DeviceId}.txt", connectionString);

                _deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
                IsConfigured = true;

                IsConfiguredChanged?.Invoke();
            }
            else
            {
                IsConfigured = false;
            }



        }

        public async Task UpdateTwinPropsAsync(TwinCollection twinCollection)
        {
            if (IsConfigured)
                await _deviceClient!.UpdateReportedPropertiesAsync(twinCollection);
        }

        public async Task SetDirectMethodAsync(string methodName, MethodCallback methodCallback)
        {
            if(IsConfigured)
                await _deviceClient!.SetMethodHandlerAsync(methodName, methodCallback, null!);

        }

        public async Task SendMessageAsync(string content)
        {
            var message = new Message(Encoding.UTF8.GetBytes(content));
            await _deviceClient!.SendEventAsync(message);
        }



    }
}
