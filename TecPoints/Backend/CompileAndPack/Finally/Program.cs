using System;
namespace Finally
{
    class Program
    {
        static void Main(string[] args)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject("Hello .NET 5!");
            Console.WriteLine(msg);
            Console.ReadKey();
        }
    }
}