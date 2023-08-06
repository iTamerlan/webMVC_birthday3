using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using WebBirthdayMVC.Models;

namespace WebBirthdayMVC.Controllers
{
    public class ApiController : Controller
    {
        // "/api/users" GET // весь список
        // "/api/users/{id:int}" GET // конкретный ID
        // "/api/users/{id:int}" DELETE // Удалить ID
        // "/api/users" POST // Добавить ID
        // "/api/users" PUT // Изменить ID

        // await Response.WriteAsync(content); JsonResult
        [HttpGet]
        [Route("{id?:int}")]
        public async Task<IActionResult> Users(int? id, ApplicationContext db)
        {
            if (id is null)
            {
                //return View();
                DbSet<User> UsersTable = db.Users;
                // Json(await UsersTable.OrderBy(p => p.Birthday).ToListAsync())
                return (IActionResult)Task.FromResult(Json(await UsersTable.OrderBy(p => p.Birthday).ToListAsync()));
                //.Take(5).OrderBy(p => p.DayOfYear) OrderBy(p => p.Birthday)
            }
            else
            {
                // получаем пользователя по id
                User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

                // если не найден, отправляем статусный код и сообщение об ошибке
                if (user == null) return (IActionResult)Results.NotFound(new { message = "Пользователь не найден" });

                // если пользователь найден, отправляем его
                return (IActionResult)Results.Json(user);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        async public Task<IActionResult> Users(int id, ApplicationContext db) 
        {
            // получаем пользователя по id
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

            // если не найден, отправляем статусный код и сообщение об ошибке
            if (user == null) return (IActionResult)Results.NotFound(new { message = "Пользователь не найден" });

            // если пользователь найден, удаляем его
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return (IActionResult)Results.Json(user);
        }

        [HttpPost]
        [Route("add")]
        async public Task<IActionResult> Users(User user, ApplicationContext db)
        {
            // добавляем пользователя в массив
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return (IActionResult)user;
        }
        [HttpPut]
        async public Task<IActionResult> Users(User userData, ApplicationContext db)
        {
            // получаем пользователя по id
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

            // если не найден, отправляем статусный код и сообщение об ошибке
            if (user == null) return (IActionResult)Results.NotFound(new { message = "Пользователь не найден" });

            // если пользователь найден, изменяем его данные и отправляем обратно клиенту
            user.Name = userData.Name;
            user.Birthday = userData.Birthday;
            user.Type = userData.Type;
            user.Photo = userData.Photo;
            //user.Photo = Convert.FromBase64String(userData.Photo);

            await db.SaveChangesAsync();
            return (IActionResult)Results.Json(user);
        }

        // 


        /*[HttpGet]
        public ActionResult users()
        {
            return View();
        }*/

    }
}
