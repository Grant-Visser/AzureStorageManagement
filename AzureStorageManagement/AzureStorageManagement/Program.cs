using System;
using System.Collections.Generic;
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
        public static void Main(string[] args)
        {
            string acs = CloudConfigurationManager.GetSetting("StorageConnectionString");
            JobHostConfiguration config = new JobHostConfiguration(acs);
            JobHost host = new JobHost(config);
            host.RunAndBlock();
        }

        [Singleton]
        public static void SingletonBlobWatcher([BlobTrigger("footage-upload")] Stream input)
        {
            AzureStorage storage = new AzureStorage();
            storage.ListAll("footage-upload");
        }
    }
}
