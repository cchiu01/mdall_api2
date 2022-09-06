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

            var company = await Validator.GetValidCompany(companyID, client);

            while (company.company_id == "0")
                company = await Validator.GetValidCompany("", client);
            

            var activeFile = new MDALL_File(FileType.CSV);
            activeFile.AddCompany(company);

            activeFile.AddLine("Device Identifier, Identifier First Issue Date, Identifier End Date, Device Name, Device Number, Device Class, Device First Issue Date, Device End Date,  Licence Number,  Licence Name, Licence Type, Licence Status, Licence End Date, Licence Basis Decision Summary");
            
            var licenceDetails = await GatherData(company, true, client);

            foreach (var licenceDetail in licenceDetails)
                activeFile.AddLicene(licenceDetail);

            Console.WriteLine("Exporting csv file for active licences");
            activeFile.SaveAs($"ACTIVE_MDALL_{companyId}_{DateTime.Now.Ticks}.csv");


            var archivedFile = new MDALL_File(FileType.CSV);
            archivedFile.AddCompany(company);

            archivedFile.AddLine("Device Identifier, Identifier First Issue Date, Identifier End Date, Device Name, Device Number, Device Class, Device First Issue Date, Device End Date,  Licence Number,  Licence Name, Licence Type, Licence Status, Licence End Date,  Licence Basis Decision Summary");
            
            var archivedDetails = await GatherData(company, false, client);
            
            foreach (var licenceDetail in archivedDetails)
                archivedFile.AddLicene(licenceDetail);

            Console.WriteLine("Exporting csv file for inactive licences");
            archivedFile.SaveAs($"INACTIVE_MDALL_{companyId}_{DateTime.Now.Ticks}.csv");

            client.Timer.Stop();

            return 0;

        }

        private static async Task<Dictionary<Licence, List<Device>>> GatherData(Company company, bool active, MDALL_Client client)
        {
            var debugLog = new StringBuilder();
            debugLog.Append("Gathering ");
            if (active)
                debugLog.Append($"active licence data for company: {company.company_name} ");
            else
                debugLog.Append($"archived licence data for company: {company.company_name} ");

            Console.WriteLine(debugLog.ToString());


            // get all licences for company
            var licences = await client.getLicence(int.Parse(company.company_id), active);

            Console.WriteLine($"Found {licences.Where(x => active ? string.IsNullOrEmpty(x.end_date) : !string.IsNullOrEmpty(x.end_date)).Count()} licences");

            // get all devices on record in MDALL db because their API has no filters
            var devices = await client.getDevices(active);

            Console.WriteLine("Gathering device information");
            Dictionary<Licence, List<Device>> details = new Dictionary<Licence, List<Device>>();

            // loop over all licences for company
            foreach (var licence in licences.Where(x => (active ? string.IsNullOrEmpty(x.end_date) : !string.IsNullOrEmpty(x.end_date))))
            {
                var licenceSummary = await client.getSbdLocation(licence.original_licence_no);
                licence.SummaryLocation = licenceSummary?.sbd_web_loc;


                // filter devices based on current licence 
                var foundDevices = devices.Where(x => x.original_licence_no == licence.original_licence_no &&
                                                      (active ? string.IsNullOrWhiteSpace(x.end_date) : !string.IsNullOrWhiteSpace(x.end_date)));
              

                foreach (var device in foundDevices)
                {
                    // get identifiers for current device
                    var deviceidentifier = await client.getDeviceIdentifier(device.device_id, active);
                    // filter identifiers which match current licence
                    device.identifiers = deviceidentifier.Where(x => (active ? string.IsNullOrWhiteSpace(x.end_date) : !string.IsNullOrWhiteSpace(x.end_date)) && x.original_licence_no == licence.original_licence_no).ToList();
                }

                Console.WriteLine($"Found {foundDevices.Count()} devices for license no. {licence.original_licence_no}");

                details.Add(licence, foundDevices.ToList());
            }

            return details;
        }
    }
}
