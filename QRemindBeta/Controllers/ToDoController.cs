using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization.Metadata;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication2.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        public string PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+ @"\ToDo.txt";

        private readonly ILogger<ToDoController> _logger;

        public ToDoController(ILogger<ToDoController> logger)
        {
            _logger = logger;
            if(!System.IO.File.Exists(PATH))
            {
                System.IO.File.Create(PATH).Close();
            }
        }

        [HttpGet("getall")]
        public String GetAll()
        {
            Console.WriteLine("Loaded all ToDo Items & sent it over JSON");
            List<ToDo> ToDoList = new List<ToDo>();
            foreach( string line in System.IO.File.ReadAllLines     (PATH))
            {
                String[] data = line.Split("#");
                if (data.Length == 3)
                {
                    ToDoList.Add(new ToDo(data[0], data[1], data[2].ToLower() == "true"));
                }
            }
            return JsonSerializer.Serialize<List<ToDo>>(ToDoList);
        }

        [HttpGet("add")]
        public IActionResult Add(String Title, String Desc)
        {
            Console.WriteLine("Added "+ Title + " to ToDo Items");
            List<String> lines = System.IO.File.ReadAllLines(PATH).ToList();
            lines.Add(Title + "#" + Desc + "#false");
            System.IO.File.WriteAllLines(PATH,lines.Where(s => s != ""));
            return Ok();
        }

        [HttpGet("del")]
        public IActionResult Delete(int Index)
        {

            Console.WriteLine("Deleted " + Index + "-th ToDo Item");

            List<String> data = System.IO.File.ReadAllLines(PATH).ToList();

            data.RemoveAt(Index);

            System.IO.File.WriteAllLines(PATH, data.ToArray());
            return Ok();
        }

        [HttpGet("setdone")]
        public IActionResult SetDone(int Index, bool IsDone)
        {

            Console.WriteLine(Index + "-th Item was set as "+ (IsDone?"Done":"Not Done"));

            List<String> data = System.IO.File.ReadAllLines(PATH).ToList();


          //  int lastHash = data[Index].LastIndexOf('#');
          //  string result = lastHash != -1 ? data[Index].Substring(0, lastHash) : data[Index];

            String[] u = data[Index].Split("#");

            data[Index] = u[0] + "#" + u[1]+"#"+IsDone.ToString().ToLower();

            System.IO.File.WriteAllLines(PATH, data.ToArray());
            return Ok();
        }
    }

    public class ToDo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }

        public ToDo(string Title, string Description, bool IsDone)
        {
            this.Title = Title;
            this.Description = Description;
            this.IsDone = IsDone;
        }
    }

}
