
using mdall_api2;
using mdall_api2.Models;
using Newtonsoft.Json;
using mdall_api2_file;

Console.WriteLine("*******************");
Console.WriteLine("**** MDALL API ****");
Console.WriteLine("*******************");

var success = await Worker.DoWork();

while (success == 0)
{
    Console.WriteLine("Would you like to enter another company id? (y/n)");
    var response = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(response) && response.ToLower() == "y")
        success = await Worker.DoWork();
    else
        success = 1;
}



Console.ReadLine();


