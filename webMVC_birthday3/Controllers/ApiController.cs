﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Net.NetworkInformation;
using WebBirthdayMVC.Models;
using webMVC_birthday3.Controllers;

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
            return (IActionResult)Json(await db.Users.OrderBy(p => p.Birthday).ToListAsync());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Users(int id)
        {
            // получаем пользователя по id
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

            // если не найден, отправляем статусный код и сообщение об ошибке
            if (user == null) return (IActionResult)NotFound(new { message = "Пользователь не найден" });

            // если пользователь найден, отправляем его
            return (IActionResult)Json(user);
        }

        [HttpDelete]
        [Route("{id:int}/{status:int}")]
        async public Task<IActionResult> Users(int status, int id) 
        {
            // получаем пользователя по id
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

            // если не найден, отправляем статусный код и сообщение об ошибке
            if (user == null) return (IActionResult)NotFound(new { message = "Пользователь не найден" });

            // если пользователь найден, удаляем его
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return (IActionResult)Json(user);
        }

        [HttpPost]
        async public Task<IActionResult> Users(User user)
        {
            // добавляем пользователя в массив
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return (IActionResult)user;
        }
        [HttpPut]
        [Route("{id:int}")]
        async public Task<IActionResult> Users(int id, User userData)
        {
            // получаем пользователя по id
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

            // если не найден, отправляем статусный код и сообщение об ошибке
            if (user == null) return (IActionResult)NotFound(new { message = "Пользователь не найден" });

            // если пользователь найден, изменяем его данные и отправляем обратно клиенту
            user.Name = userData.Name;
            user.Birthday = userData.Birthday;
            user.Type = userData.Type;
            user.Photo = userData.Photo;
            //user.Photo = Convert.FromBase64String(userData.Photo);

            await db.SaveChangesAsync();
            return (IActionResult)Json(user);
        }

        // 


        /*[HttpGet]
        public ActionResult users()
        {
            return View();
        }*/

    }
}
