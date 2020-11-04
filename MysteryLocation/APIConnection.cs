﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MysteryLocation
{
    public class APIConnection 
    {
        private HttpClient client;
        private Label label;

        public APIConnection(Label label)
        {
            client = new HttpClient();
            this.label = label;
        }

        public async void RefreshDataAsync() // Get 
        {
            Console.WriteLine("Trying to connect to API");
        Uri uri = new Uri(string.Format("https://saabstudent2020.azurewebsites.net/observation", string.Empty));
           // ActivityIndicator activityIndicator = new ActivityIndicator { Color = Color.Orange };
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                Console.WriteLine("In if statement in RefreshDataAsync");
                var posts = ConvertJsonToPosts(content);
                label.Text = posts.ToString();
                Console.WriteLine(posts);

            }
           

        }

        private String ConvertJsonToPosts(string JsonString) // This should return the whole list instead of a String. Todo
        {
            List<Post> posts;
            posts = JsonConvert.DeserializeObject<List<Post>>(JsonString); // Converts the Json to Posts
            String everything = "";
            foreach(Post p in posts){ 
                Console.WriteLine(p.toString());
                everything += p.toString();
            }
            return everything;

        }


    }
}