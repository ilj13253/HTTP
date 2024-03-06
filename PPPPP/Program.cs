using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;
namespace PPPPP
{
    class Program
    {
        public class Root
        {
            public List<Search> Search { get; set; }
            public string totalResults { get; set; }
            public string Response { get; set; }
        }

        public class Search
        {
            public string Title { get; set; }
            public string Year { get; set; }
            public string imdbID { get; set; }
            public string Type { get; set; }
            public string Poster { get; set; }
        }
        
        async Task<Root> Scargb(string title)
        {
            var httpclient = new HttpClient();
            //ждёт ответа
           var resp=await httpclient.GetAsync($"https://www.omdbapi.com/?apikey=2c9d65d5&s={title}");
            //Console.WriteLine(resp.res);
            var res =await resp.Content.ReadAsStringAsync();
           var Res= JsonSerializer.Deserialize<Root>(res);
            Console.WriteLine(res+"\n");
            return Res;

        }
        //C:\Users\Илья\source\repos\PPPPP\bin\Debug\movies.txt
        async Task Start()
        {
            var lines = File.ReadAllLines("movies.txt");
            var random = new Random();
            var selectedTitles = lines.OrderBy(x => random.Next()).Take(10).ToList();

            var tasks = selectedTitles.Select(title => Scargb(title));
            var results = await Task.WhenAll(tasks);

            // Печать информации о фильмах
            foreach (var result in results)
            {
                foreach (var item in result.Search)
                {
                    Console.WriteLine(item.Title + " (" + item.Year + ")");
                }
            }
        }
        static async Task Main(string[] args)
        {
            var pro = new Program();
            await pro.Start();
            Console.ReadKey();
        
        }
    }
}