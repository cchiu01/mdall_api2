﻿using mdall_api2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdall_api2
{
    public class MDALL_Client : HttpClient 
    {
        public MDALL_Client() { 
        DefaultRequestHeaders.Add("user-key", "8f9d5d7deef1c93d908bf793a62e7ade");
        BaseAddress = new Uri("https://mdall-hc-sc-apicast-production.api.canada.ca/v1/");
        }

        public async Task<Company> getCompany() {
            var response = await GetAsync("company?lang=en&type=json&id=113585");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Company>(content);
        }

        public async Task<List<Licence>> getLicence() {
            var response = await GetAsync("licence?lang=en&type=json&company_id=113585");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Licence>>(content);
        }

        public async Task<List<Device>> getDevice() {
            var response = await GetAsync("device");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Device>>(content);
        }
    }
}
