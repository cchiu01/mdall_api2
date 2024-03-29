﻿using mdall_api2.Models;
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

            File.AppendLine("Device Identifier, Identifier First Issue Date, Identifier End Date, Device Name, Device Number, Device Class, Device First Issue Date, Device End Date,  Licence Number,  Licence Name, Licence Type, Licence Status, Licence End Date");

        }

        public void AddLicene(KeyValuePair<Licence, List<Device>> detail)
        {
          
            foreach (var device in detail.Value)
            {
                foreach (var identifier in device.identifiers)
                {
                    var licence = detail.Key;

                    File.Append($"{identifier.device_identifier},");
                    File.Append($"{identifier.first_licence_dt},");
                    File.Append($"{identifier.end_date},");
                    File.Append($"{device.trade_name},");
                    File.Append($"{device.device_id},");
                    File.Append($"{licence.appl_risk_class},");
                    File.Append($"{device.first_licence_dt},");
                    File.Append($"{device.end_date},");
                    File.Append($"{device.original_licence_no},");
                    File.Append($"{licence.licence_name},");
                    File.Append($"{licence.licence_type_desc},");
                    File.Append($"{licence.licence_status},");
                    File.Append($"{licence.end_date}");

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
