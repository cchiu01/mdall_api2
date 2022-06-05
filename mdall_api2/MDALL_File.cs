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
        }

        public void AddLicene(KeyValuePair<Licence, List<Device>> detail)
        {

        }

        public void SaveAs(string fileName)
        {

            if (!System.IO.Directory.Exists(@"C:\MDALL"))
                Directory.CreateDirectory(@"C:\MDALL");

            using (var fileStream = new FileStream(@$"C:\MDALL\{DateTime.UtcNow.ToLongDateString()}-{fileName}", FileMode.OpenOrCreate, FileAccess.Write))
            using (var stream = new StreamWriter(fileStream))
            {
                stream.Write(File.ToString());
            }
        }


    }
}
