
using mdall_api2;
using mdall_api2.Models;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var client = new MDALL_Client();

var company = await client.getCompany();
var licences = await client.getLicence();
var devices  = await client.getDevice();

Dictionary<Licence, List<Device>> thingsWeWant = new Dictionary<Licence, List<Device>>();
foreach (var license in licences) {
    var foundDevices = devices.Where(x => x.original_licence_no == license.original_licence_no).ToList();
    thingsWeWant.Add(license, foundDevices);
}
Console.ReadLine();



