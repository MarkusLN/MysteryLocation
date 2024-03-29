﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MysteryLocation.Model;
using Newtonsoft.Json;
using Plugin.Toast;
using Xamarin.Forms;

namespace MysteryLocation
{
    public class APIConnection
    {
        private HttpClient client;

        private SemaphoreSlim sem;
        

        public APIConnection()
        {
            client = new HttpClient();
            sem = new SemaphoreSlim(1);
        }

        public async Task<List<Post>> getDataAsync()
        {
            await sem.WaitAsync();
            Uri uri = new Uri(string.Format("https://saabstudent2020.azurewebsites.net/observation", string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            List<Post> posts = new List<Post>();
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                posts = JsonConvert.DeserializeObject<List<Post>>(content);
            }
            sem.Release();
            return posts;
        }

        public async Task<int> testPublishPosts(createPost x)
        {
            await sem.WaitAsync();
            Uri uri = new Uri(string.Format("https://saabstudent2020.azurewebsites.net/observation", string.Empty));
            string json = JsonConvert.SerializeObject(x, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(uri, content);
              if (response.IsSuccessStatusCode)
              {      
                string temp = response.Headers.Location.ToString().Substring(54);
                sem.Release();
                return Int32.Parse(temp);
              }
            sem.Release();
            return -1;
        }

        public async Task<bool> publishAttachment(PostAttachment x)
        {
           await sem.WaitAsync();
            Uri uri = new Uri(string.Format("https://saabstudent2020.azurewebsites.net/observation/" + x.obsID + "/attachment", string.Empty));
            string json = JsonConvert.SerializeObject(x, Formatting.Indented);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                sem.Release();
                return true;
            }
            sem.Release();
            return false;
        }

        public async Task<UnlockedPosts> getPostAttachmentAsync(int obsId)
        {
           await sem.WaitAsync();
            Uri uri = new Uri(string.Format("https://saabstudent2020.azurewebsites.net/observation/" + obsId + "/attachment", string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            UnlockedPosts temp = null;
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                temp = MakeUnlockedPost(content);
                if(temp == null) {
                    switch (Device.RuntimePlatform)
                    {
                        case Device.Android:
                            DependencyService.Get<SnackInterface>().SnackbarShow("Post #" + obsId + " does not have an image");
                            break;
                        case Device.iOS:
                            CrossToastPopUp.Current.ShowToastMessage("Post #" + obsId + " does not have an image");
                            break;
                        default:
                            break;
                    }
                }

                

                else 
                    temp.obsID = obsId;
                sem.Release();
                return temp;
            }

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    DependencyService.Get<SnackInterface>().SnackbarShow("Failed to unlock post #" + obsId);
                    break;
                case Device.iOS:
                    CrossToastPopUp.Current.ShowToastMessage("Failed to unlock post #" + obsId);
                    break;
                default:
                    break;
            }


            sem.Release();
            return temp;
        }

        private UnlockedPosts MakeUnlockedPost(string content)
        {
            if (content.Length < 100)
                return null;
            string id = "";
            int counter = 7;
            while (content[counter] != ',')
            {
                id += content[counter];
                counter++;
            }
            counter = 23 + id.Length;
            content = content.Substring(counter);
            content = content.Remove(content.Length - 3, 3);
            UnlockedPosts temp = new UnlockedPosts(int.Parse(id), content);
            return temp;
        }

        /** Method to delete an observation.
         *  Currently not supported by the API.
         *  The response returns an errorcode 405
         * */
        /*public async void deleteItem(int observationID)
        {
           // Uri uri = new Uri(string.Format("https://saabstudent2020.azurewebsites.net/observation", string.Empty));
            Uri uri = new Uri(string.Format("https://saabstudent2020.azurewebsites.net/observation/", observationID));
            HttpResponseMessage response = await client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(@"\tTodoItem successfully deleted.");
            }
            else
            {
                Console.WriteLine("Delete failed " + response);
            }
        }*/


    }
}
