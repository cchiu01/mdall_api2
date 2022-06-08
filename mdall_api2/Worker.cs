using mdall_api2.Models;
using mdall_api2_file;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdall_api2
{
    public static class Worker
    {
        public static async Task<int> DoWork()
        {
            Console.WriteLine("Enter Company ID: ");
            var companyID = Console.ReadLine();

            if (string.IsNullOrEmpty(companyID))
            {
                Console.WriteLine("Invalid. Try again: ");
                companyID = Console.ReadLine();
            }

            if (!int.TryParse(companyID, out int companyId))
            {
                Console.WriteLine("Invalid. Try again: ");
                companyID = Console.ReadLine();
            }
            var client = new MDALL_Client();
            // can only make 60 requests per minute
            // start stop watch so we can swap api keys when necessary
            client.Timer.Start();
            var company = await Validator.Validate(companyID, client);

            while (company.company_id == "0")
            {
                company = await Validator.Validate("", client);
            }

            Console.WriteLine($"Gathering data for company: {company.company_name}");

            var licences = await client.getLicence(int.Parse(company.company_id));

            Console.WriteLine($"Found {licences.Count()} licences");
            var devices = await client.getDevice();

            Dictionary<Licence, List<Device>> details = new Dictionary<Licence, List<Device>>();

            Console.WriteLine("Gathering device information");
            foreach (var license in licences)
            {
                var foundDevices = devices.Where(x => x.original_licence_no == license.original_licence_no).ToList();
                foreach (var device in foundDevices)
                {
                    var deviceidentifier = await client.getDeviceIdentifier(device.device_id);
                    device.identifiers = deviceidentifier;
                }
                Console.WriteLine($"Found {foundDevices.Count()} devices for license no. {license.original_licence_no}");
                details.Add(license, foundDevices);
            }

            Console.WriteLine("Exporting csv file data");
            var file = new MDALL_File(FileType.CSV);
            file.AddCompany(company);

            foreach (var licenceDetail in details)
                file.AddLicene(licenceDetail);

            file.SaveAs($"MDALL_{companyId}_{DateTime.Now.Ticks}.csv");

            client.Timer.Stop();

            return 0;

        }
    }
}
