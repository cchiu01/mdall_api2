using mdall_api2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdall_api2
{
    public class MDALL_Client : HttpClient 
    {
        private int count { get; set; }
        private int keyIdx { get; set; } = 0;
        public Stopwatch Timer { get; set; }

        public List<string> apiKeys = new List<string>()
        {
            "8f9d5d7deef1c93d908bf793a62e7ade",
            "924b07664768b899c22f08f6c73c84d8",
            "a9710d3252dad2f93ae619fe0f863de5",
            "b1e68264e50f4a30c78822a9f6311d42",
            "bb9eef60e0c6dbbe0c9597461b84b0c7",
            "7614c02ac5e47d6c10448810bd9e1e93",
            "8d527d006a8e602dcbaac53167c41f9c",
            "bd56bbbe8f3c80a5094ee8ed13230128",
            "6b2d972a7909e4501d8d72eba84b21db",
            "8ba3498cf07b1b537ca8c67ba057c996"
        };

        public MDALL_Client() {
            DefaultRequestHeaders.Add("user-key", apiKeys[keyIdx]);
            BaseAddress = new Uri("https://mdall-hc-sc-apicast-production.api.canada.ca/v1/");
            Timer = new Stopwatch();
        }

        public async Task<Company> getCompany(string id) {
            HandleApiKey();
            var response = await GetAsync($"company?lang=en&type=json&id={id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Company>(content);
        }

        public async Task<List<Licence>> getLicence(int companyId) {
            HandleApiKey();
            var response = await GetAsync($"licence?lang=en&type=json&company_id={companyId}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Licence>>(content);
        }

        public async Task<List<Device>> getDevice() {
            HandleApiKey();
            var response = await GetAsync("device");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Device>>(content);
        }

        public async Task<List<DeviceIdentifier>> getDeviceIdentifier(int deviceID) {
            HandleApiKey();
            var response = await GetAsync($"deviceidentifier?id={deviceID}");
            var content = await response.Content.ReadAsStringAsync();
            var result = new List<DeviceIdentifier>();
            try { result = JsonConvert.DeserializeObject<List<DeviceIdentifier>>(content); }
            catch (Exception e) {
                result.Add(JsonConvert.DeserializeObject<DeviceIdentifier>(content));
            }
            return result;
        }

        private void HandleApiKey()
        {
            count += 1;

            // we have gone further than 60 per minute:
            if (count % 60 == 0 && Timer.ElapsedMilliseconds >= 6000)
            {
                if (keyIdx > 9)
                    keyIdx = 0;
                else
                    keyIdx += 1;

                DefaultRequestHeaders.Remove("user-key");
                DefaultRequestHeaders.Add("user-key", apiKeys[keyIdx]);
            }
        }
    }
}
