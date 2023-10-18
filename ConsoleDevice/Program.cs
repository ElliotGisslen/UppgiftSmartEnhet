using ConsoleDevice.Contexts;
using DataAccess.Devices.Services;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.EntityFrameworkCore;


var macAddress = "8D-2E-4C-68-2C-17";
var twinCollection = new TwinCollection();
twinCollection["deviceType"] = "console";
twinCollection["location"] = "virtual";

var deviceManager = new DeviceManager(macAddress, "https://iotdevicefunctions.azurewebsites.net/api/devices?code=iuAxEhRfVJ2JfniSEy-Y0pQguaMhbWVEqeEEcm8UdYpQAzFungHWPw==");
deviceManager.IsConfiguredChanged += async () => await OnConfiguredAsync();

async Task OnConfiguredAsync()
{
    if (deviceManager!.IsConfigured)
    {
        Console.WriteLine("Is Configured");
        await deviceManager.UpdateTwinPropsAsync(twinCollection);
        Console.WriteLine("Device Twin Updated");

        await deviceManager.SetDirectMethodAsync("start", StartMethod);
        await deviceManager.SetDirectMethodAsync("stop", StopMethod);
        Console.WriteLine("Direct Method(s) Registered");

        await deviceManager.SendMessageAsync("[\"testar\"]");
    }

}

async Task<MethodResponse> StartMethod(MethodRequest methodRequest, object userContext)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"{methodRequest.Name} triggered.");
    Console.ForegroundColor = ConsoleColor.White;

    if (methodRequest.Name.ToLower() == "start")
    {
        return new MethodResponse(200);
    }
    return new MethodResponse(404);
}

async Task<MethodResponse> StopMethod(MethodRequest methodRequest, object userContext)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"{methodRequest.Name} triggered.");
    Console.ForegroundColor = ConsoleColor.White;
    

    if (methodRequest.Name.ToLower() == "stop")
    {
        return new MethodResponse(200);
    }

    return new MethodResponse(404);
}

Console.ReadKey();