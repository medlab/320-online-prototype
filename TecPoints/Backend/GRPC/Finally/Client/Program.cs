using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new TestGrpcServices.TestGrpcServicesClient(channel);
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "TestClient" });
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}