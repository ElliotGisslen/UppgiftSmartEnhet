using DataAccess.Devices.Services;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Device_WPF
{
    public partial class MainWindow : Window
    {
        private readonly DeviceManager _deviceManager;
        private TwinCollection twinCollection = new TwinCollection();
        public MainWindow()
        {

            InitializeComponent();

            _deviceManager = new DeviceManager("7B-CE-A3-C2-2C-50", "https://iotdevicefunctions.azurewebsites.net/api/devices?code=iuAxEhRfVJ2JfniSEy-Y0pQguaMhbWVEqeEEcm8UdYpQAzFungHWPw==");
            twinCollection["deviceType"] = "wpf";
            twinCollection["location"] = "virtual";



 
            _deviceManager.IsConfiguredChanged += async () => await OnConfiguredAsync();

        }
        private void UpdateStatus(string message) => Application.Current.Dispatcher.Invoke(()=> StatusActivity.Text = message);

        private async Task OnConfiguredAsync()
        {
            if (_deviceManager!.IsConfigured)
            {

                UpdateStatus("Is Configured");



                await _deviceManager.UpdateTwinPropsAsync(twinCollection);
                UpdateStatus("Device Twin Updated");





                await _deviceManager.SetDirectMethodAsync("start", StartMethod);
                await _deviceManager.SetDirectMethodAsync("stop", StopMethod);
                UpdateStatus("Direct Method(s) Registered");

                await _deviceManager.SendMessageAsync("[\"Alvaro\"]");
                UpdateStatus("DaTruthDT");

            }

        }

        private async Task<MethodResponse> StartMethod(MethodRequest methodRequest, object userContext)
        {

            UpdateStatus($"{methodRequest.Name} triggered.");


            if (methodRequest.Name.ToLower() == "start")
            {
                var twinCollection = new TwinCollection();
                twinCollection["state"] = true;

                await _deviceManager.UpdateTwinPropsAsync(twinCollection);
                return new MethodResponse(200);
            }

            return new MethodResponse(404);
        }

        private async Task<MethodResponse> StopMethod(MethodRequest methodRequest, object userContext)
        {

            UpdateStatus($"{methodRequest.Name} triggered.");

            if (methodRequest.Name.ToLower() == "stop")
            {
                var twinCollection = new TwinCollection();
                twinCollection["state"] = false;

                await _deviceManager.UpdateTwinPropsAsync(twinCollection);
                return new MethodResponse(200);
            }

            return new MethodResponse(404);
        }
    }
}
