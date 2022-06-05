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
            File.AppendLine("MANUFACTURER");
            File.AppendLine($"{company.company_name.CSVSafe()},");
            File.Append($"{company.addr_line_1.CSVSafe()},");
            File.Append($"{company.addr_line_2.CSVSafe()},");
            File.Append($"{company.addr_line_3.CSVSafe()},");
            File.AppendLine($"COMPANY ID: {company.company_id.ToString().CSVSafe()}");
            File.AppendLine();
            File.AppendLine($"COMPANY ID: {company.company_id.CSVSafe()}");
            File.AppendLine();
        }

        public void AddLicene(KeyValuePair<Licence, List<Device>> detail)
        {
            File.AppendLine($"License No.: {detail.Key.original_licence_no} ('{detail.Key.licence_type_cd}')");
            File.AppendLine($"Type: {detail.Key.licence_type_desc}");
            File.AppendLine($"Device class: {detail.Key.appl_risk_class}");
            File.AppendLine($"Device First Issue Date: {detail.Key.first_licence_status_dt}");
            File.AppendLine($"Licence Name: {detail.Key.licence_name}");
            File.AppendLine("Device Details:");
            File.AppendLine("Device First Issue Date, Device Name, Identifier First Issue Date, Device Identifier");

            foreach (var device in detail.Value)
            {
                File.Append($"{device.first_licence_dt},");
                File.Append($"{device.trade_name},");

                File.AppendLine();
            }

            File.AppendLine();
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
        }


    }
}
