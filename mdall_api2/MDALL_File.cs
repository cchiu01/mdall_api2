using mdall_api2.Models;
using mdall_extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdall_api2_file
{
    public enum FileType
    {
        CSV,
        EXCEL
    }
    public class MDALL_File
    {
        public FileType Type { get; set; }
        public StringBuilder File { get; set; }

        public MDALL_File(FileType type)
        {
            this.Type = type;
            this.File = new StringBuilder();
        }

        public void AddCompany(Company company)
        {
            switch (Type) {
                case FileType.CSV:
                    AddCompanyCSV(company);
                    break;
                default:
                    break;
            }
        }

        private void AddCompanyCSV(Company company)
        {
            File.AppendLine("MANUFACTURER, COMPANY ID");
            File.AppendLine($"{company.company_name.CSVSafe()}, {company.company_id.CSVSafe()}");
            File.AppendLine($"{company.addr_line_1.CSVSafe()} {company.addr_line_2.CSVSafe()} {company.addr_line_3.CSVSafe()}");
            File.AppendLine();

            File.AppendLine("Device Identifier, Identifier First Issue Date, Device Name, Licence Number, Device Class, Licence Name, Type, Device First Issue Date");

        }

        public void AddLicene(KeyValuePair<Licence, List<Device>> detail)
        {
          
            foreach (var device in detail.Value)
            {
                foreach (var identifier in device.identifiers)
                {
                    File.Append($"{identifier.device_identifier},");
                    File.Append($"{identifier.first_licence_dt},");
                    File.Append($"{device.trade_name},");
                    File.Append($"{device.original_licence_no},");
                    File.Append($"{detail.Key.appl_risk_class},");
                    File.Append($"{detail.Key.licence_name},");
                    File.Append($"{detail.Key.licence_type_desc},");
                    File.Append($"{device.first_licence_dt}");
                    File.AppendLine();
                }
            }
        }

        public void SaveAs(string fileName)
        {

            if (!System.IO.Directory.Exists(@"C:\MDALL"))
                Directory.CreateDirectory(@"C:\MDALL");

            using (var fileStream = new FileStream(@$"C:\MDALL\{fileName}", FileMode.OpenOrCreate, FileAccess.Write))
            using (var stream = new StreamWriter(fileStream))
            {
                stream.Write(File.ToString());
            }

            Console.WriteLine($"Successfully exported data to: {@$"C:\MDALL\{fileName}"}");
        }


    }
}
