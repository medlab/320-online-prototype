using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWithZipKin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var httpClient = new HttpClient();
            
            var result=await httpClient.GetStringAsync("https://github.com");

            await DoSomeWork();
            
            return Ok($"hello from github.com: {result.Substring(0,255)}");
        }
        
        static ActivitySource s_source = new ActivitySource("Sample");

        // static async Task Main(string[] args)
        // {
        //     await DoSomeWork();
        //     Console.WriteLine("Example work done");
        // }

        static async Task DoSomeWork()
        {
            // Activity? activity = s_source.StartActivity("SomeWork");
            // if (activity is not null)
            // {
            //     activity.IsAllDataRequested = true;
            // }
            
            using (Activity? activity = s_source.StartActivity("SomeWork"))
            {
                //TODO this not work!
                //activity.AddBaggage(); // 后续所有上下文会继承这个值
                activity?.SetBaggage("whoamii", "all from folks");
                activity?.AddBaggage("ok", "en");
                await StepOne();
                await StepTwo();
            }
        }

        static async Task StepOne()
        {
            using (Activity? activity = s_source.StartActivity("StepOne"))
            {
                await Task.Delay(500);
            }
        }

        static async Task StepTwo()
        {
            using (Activity? activity = s_source.StartActivity("StepTwo"))
            {
                activity.SetTag("custom_tag", "kernel step"); //这个tag会成为专属标记
                await Task.Delay(1000);
            }
        }
    }
}