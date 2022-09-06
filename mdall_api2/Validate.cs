using mdall_api2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdall_api2
{
    public static class Validator
    {
        public static async Task<Company> GetValidCompany(string companyId, MDALL_Client client)
        {
            if (string.IsNullOrEmpty(companyId))
            {
                Console.WriteLine("Invalid. Try again: ");
                companyId = Console.ReadLine();
            }

            if (!int.TryParse(companyId, out int id))
            {
                Console.WriteLine("Invalid. Try again: ");
                companyId = Console.ReadLine();
            }

            return await client.getCompany(companyId);
        }
    }
}
