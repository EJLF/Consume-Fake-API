using Consume_API.Models;
using Consume_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Consume_API.Controllers
{
    public class TodoController : Controller
    {
        string baseURL = "https://jsonplaceholder.typicode.com";

        HttpClient httpClient = new HttpClient();

        public IActionResult Index()
        {
            var response = httpClient.GetAsync(baseURL + "/todos").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result; 
                List<Todo> todos = JsonConvert.DeserializeObject<List<Todo>>(data);
                return View(todos);
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddTodo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTodo(AddTodoViewModel todoVM)
        {
            string newBaseURl = baseURL + "/todos";
            var stringContent = new StringContent(JsonConvert.SerializeObject(todoVM), Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(newBaseURl, stringContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var responsecontent = response.Content.ReadAsStringAsync().Result;
                Todo todo = JsonConvert.DeserializeObject<Todo>(responsecontent);
               
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpGet]
        public IActionResult Details(int todoId)
        {
            var response = httpClient.GetAsync(baseURL + "/todos/" + todoId).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                Todo todos = JsonConvert.DeserializeObject<Todo>(data);
                return View(todos);
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int todoId)
        {
            var response = httpClient.GetAsync(baseURL + "/todos/" + todoId).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                AddTodoViewModel todos = JsonConvert.DeserializeObject<AddTodoViewModel>(data);
                return View(todos);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(AddTodoViewModel todoVM, int todoId)
        {
            string data = JsonConvert.SerializeObject(todoVM);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = httpClient.PutAsync(baseURL + "/todos/" + todoId, content).Result;
            if (response.IsSuccessStatusCode)
            {
                var responsecontent = response.Content.ReadAsStringAsync().Result;
                Todo todo = JsonConvert.DeserializeObject<Todo>(responsecontent);
                return View(todo);
            }
            return View();
        }
        public IActionResult Delete(int todoId)
        {
            var response = httpClient.DeleteAsync(baseURL + "/todos/" + todoId).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return BadRequest(response);
        }
    }
}
