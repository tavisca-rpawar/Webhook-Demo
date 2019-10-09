using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using WebApplication3.Models;

namespace WebApplication3.Services
{
    public class AppServices
    {
        public async Task ProcessWebhookPayload(string pullCommentUrl)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com");
            var token = "bc16c1a174477d0adf99ddb1312a0ee1909d6e88";
            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

            var response = await client.GetAsync(new Uri(pullCommentUrl).LocalPath);
            string commentForFile = "";
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<IEnumerable<CommentPayload>>(responseString).ToList<CommentPayload>();
                comments.ForEach(comment => commentForFile += "Usename: " + comment.User.Login + "\n" + "Comment: " + comment.Body + "\n\n");
                commentForFile += $"Total Comments :{comments.Count}";
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                commentForFile = $"Error In Fetching Data fro api call {response.ReasonPhrase}";
            }
            System.IO.File.WriteAllText(@"C:\Users\rupawar\source\repos\WebApplication3\response.txt", commentForFile);
        }
    }
}
