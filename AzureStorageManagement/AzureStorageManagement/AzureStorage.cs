using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureStorageManagement
{
    class AzureStorage
    {

        public bool ListAll(string containerName)
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString")); //Coming from the web.config
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            foreach (IListBlobItem item in container.ListBlobs(null,true))//Returning a flat folder structure. Everything listed in a flat structure, no directories or pages
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    string blobName = blob.Name;
                    if (blob.Parent.Prefix != String.Empty)
                    {
                        blobName = blob.Name.Trim().Replace(blob.Parent.Prefix, "");
                    }
                    else
                    {
                        blobName = blob.Name;
                    }
                    Console.WriteLine("Block blob of length {0}: {1} \nName: {2}\n", blob.Properties.Length, blob.Uri, blobName);
                }
            }
            Console.Out.WriteLine("Processed at: " + DateTime.Now);
            return false;
        }
    }
}
