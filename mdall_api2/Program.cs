
using mdall_api2;
using mdall_api2.Models;
using Newtonsoft.Json;
using mdall_api2_file;

Console.WriteLine("*******************");
Console.WriteLine("**** MDALL API ****");
Console.WriteLine("*******************");

Console.WriteLine(" ");
Console.WriteLine("Enter Company ID: ");
var companyID = Console.ReadLine();

if (string.IsNullOrEmpty(companyID))
    Console.WriteLine("Invalid. Try again: ");

if (!int.TryParse(companyID, out int companyId))
    Console.WriteLine("Invalid. Try again: ");

var client = new MDALL_Client();

var company = await client.getCompany(companyId);
var licences = await client.getLicence(companyId);
var devices  = await client.getDevice();

Dictionary<Licence, List<Device>> details = new Dictionary<Licence, List<Device>>();

foreach (var license in licences) 
{
    var foundDevices = devices.Where(x => x.original_licence_no == license.original_licence_no).ToList();
    foreach (var device in foundDevices)
    {

    }
    details.Add(license, foundDevices);
}

var file = new MDALL_File(FileType.CSV);
file.AddCompany(company);

foreach (var licenceDetail in details)
    file.AddLicene(licenceDetail);

file.SaveAs($"MDALL_{companyId}_{DateTime.Now.ToLongDateString()}.csv");

Console.ReadLine();



