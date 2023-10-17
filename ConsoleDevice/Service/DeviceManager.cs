//using ConsoleDevice.Contexts;
//using Microsoft.Azure.Devices.Client;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleDevice.Service
//{
//    internal class DeviceManager
//    {
//        private readonly string _url = "https://deviceregistryfunctions.azurewebsites.net/api/RegisterDevice?code=BhD1kXtn_2LXovXcuKhhzH16q3e7cQso3JtMSqJwdHG4AzFuTMh27Q==";
//        private DeviceClient _deviceClient = null!;
//        private DataContext _context;

//        public DeviceManager(DataContext context)
//        {
//            _context = context;
//        }

//        public DeviceManager()
//        {
            
//        }

//        public async Task InitializeAsync()
//        {
//            var config = await _context.Configuration.FirstOrDefaultAsync();
//            if(config != null)
//            {
//                if(!string.IsNullOrEmpty(config.ConnectionString))
//                {
//                    using var http = new HttpClient();
//                    var result = await http.PostAsync(_url,
//                        new StringContent(JsonConvert.SerializeObject(new { deviceId = config.DeviceId})));

//                    if(result.IsSuccessStatusCode)
//                    {
//                        var connectionString = await result.Content.ReadAsStringAsync();
//                        config.ConnectionString = connectionString;
//                        _context.Configuration.Update(config);
//                        await _context.SaveChangesAsync();
//                    }
//                }

//                _deviceClient = DeviceClient.CreateFromConnectionString(config.ConnectionString);
//            }

//        }


//    }
//}
