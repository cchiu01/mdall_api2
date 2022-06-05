
using mdall_excel;
using mdall_api2.Models;
using Newtonsoft.Json;
using mdall_api2_file;

Console.WriteLine("Hello, World!");

var client = new HttpClient();
client.DefaultRequestHeaders.Add("user-key", "8f9d5d7deef1c93d908bf793a62e7ade");
client.BaseAddress = new Uri("https://mdall-hc-sc-apicast-production.api.canada.ca/v1/");
var response = await client.GetAsync("company?lang=en&type=json&id=113585");
var content = await response.Content.ReadAsStringAsync();
var company = JsonConvert.DeserializeObject<Company>(content);

var file = new MDALL_File(mdall_api2_file.FileType.CSV);
file.AddCompany(company);
file.SaveAs($"MDALL_{company.company_id}.csv");

//response = await client.GetAsync("licence?lang=en&type=json&company_id=113585");
//content = await response.Content.ReadAsStringAsync();
//var licence = JsonConvert.DeserializeObject<List<Licence>>(content);





