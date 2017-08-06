using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;

namespace AzureStorageManagement
{
    public class Program
    {
        //Requires Microsoft.Azure.WebJobs.Extensions nuget package to be installed.
        //And Microsoft.WindowsAzure.ConfigurationManager
        public static void Main(string[] args)
        {
            string acs = CloudConfigurationManager.GetSetting("StorageConnectionString");
            JobHostConfiguration config = new JobHostConfiguration(acs);
            config.UseTimers();
            JobHost host = new JobHost(config);
            host.RunAndBlock();
            //Nothing runs after this
        }

        [Singleton]
        public static void SingletonBlobWatcher([BlobTrigger("footage-upload")] Stream input)
        {
            Debug.Assert(input.CanRead);
            AzureStorage storage = new AzureStorage();
            storage.ListAll("footage-upload");
        }

        [Singleton]
        public static void SingletonDeleter([TimerTrigger("0 0 0 * * *",RunOnStartup = true)] TimerInfo timer)
        {
            AzureStorage storage = new AzureStorage();
            storage.RemoveOld("footage-upload", 5);
        }

        //[Singleton]
        //public static void TimerJob([TimerTrigger("00:00:30")] TimerInfo timer)
        //{
        //    Console.WriteLine("Timer job fired!");
        //}
    }
}
