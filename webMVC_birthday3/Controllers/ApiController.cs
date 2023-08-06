using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Xml;
using WebBirthdayMVC.Models;
using webMVC_birthday3.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebBirthdayMVC.Controllers
{
    public class ApiController : Controller
    {
        private readonly ApplicationContext db;
        private readonly ILogger<HomeController> _logger;

        public ApiController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            db = context;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ActionDescriptor actionDescriptor = filterContext.ActionDescriptor;
            string actionName = actionDescriptor.DisplayName;
            //Console.WriteLine("<===== actionName =====>");
            Console.WriteLine(actionName);
            // Now that you have the values, set them somewhere and pass them down with your ViewModel
            // This will keep your view cleaner and the controller will take care of everything that the view needs to do it's job.
        }

        // "/api/users" GET // весь список
        // "/api/users/{id:int}" GET // конкретный ID
        // "/api/users/{id:int}" DELETE // Удалить ID
        // "/api/users" POST // Добавить ID
        // "/api/users" PUT // Изменить ID

        // await Response.WriteAsync(content); JsonResult
        [HttpGet]
        public async Task<IActionResult> Test() // ApplicationContext? db
        {
            return Content("<h2>Test</h2>");
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            return new JsonResult(await db.Users.OrderBy(p => p.Birthday).ToListAsync());

        }

        [HttpGet]
        [Route("api/users/{id:int}")]
        public async Task<IActionResult> Users(int id)
        {
            // получаем пользователя по id
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

            // если не найден, отправляем статусный код и сообщение об ошибке

            if (user == null) return new JsonResult(new { message = "Пользователь не найден" });

            // если пользователь найден, отправляем его
            return new JsonResult(user);
        }

        [HttpDelete]
        [Route("api/users/{id:int}/{status:int}")]
        async public Task<IActionResult> Users(int status, int id) 
        {
            // получаем пользователя по id
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

            // если не найден, отправляем статусный код и сообщение об ошибке
            if (user == null) return new JsonResult(new { message = "Пользователь не найден" });

            // если пользователь найден, удаляем его
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return new JsonResult(user);
        }

        [HttpPost]
        async public Task<IActionResult> Users(User user)
        {
            //var request = context.Request;
            user = await Request.ReadFromJsonAsync<User>();
            // добавляем пользователя в массив
            //var user2 = Request.ReadFromJsonAsync<User>;

            Console.WriteLine("<<<=== user ===>>>");
            
            Console.WriteLine(JsonSerializer.Serialize(user));

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return new JsonResult(user);
        }
        [HttpPut]
        [Route("api/users/{id:int}")]
        async public Task<IActionResult> Users(int id, User userData)
        {
            userData = await Request.ReadFromJsonAsync<User>();
            // получаем пользователя по id
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

            // если не найден, отправляем статусный код и сообщение об ошибке
            if (user == null) return new JsonResult(new { message = "Пользователь не найден" });

            // если пользователь найден, изменяем его данные и отправляем обратно клиенту
            user.Name = userData.Name;
            user.Birthday = userData.Birthday;
            user.Type = userData.Type;
            user.Photo = userData.Photo;
            user.DayOfYear = userData.Birthday.DayOfYear;
            //user.Photo = Convert.FromBase64String(userData.Photo);

            await db.SaveChangesAsync();


            var t = new JsonResult(user); 
            //Console.WriteLine("TEST !!!!!!!!!!!!!!");
            //Console.WriteLine(t);
            return t;
        }

        // 


        /*[HttpGet]
        public ActionResult users()
        {
            return View();
        }*/

    }
}
