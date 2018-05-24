using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DbConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using the_wall.Models;

namespace the_wall.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("/user/register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string emailCheck = $"SELECT * FROM users WHERE Email='{model.Email}'";
                List<Dictionary<string, object>> similarEmails = DbConnector.Query(emailCheck);
                if(similarEmails.Count > 0)
                {
                    ViewBag.messages = "This email already exists. Please Log In";
                    return View("Index");
                }
                else
                {
                    User NewUser = new User
                    {
                        First_name = model.First_name,
                        Last_name = model.Last_name,
                        Email = model.Email,
                        Password = model.Password,
                    };
                    string query = $"INSERT INTO users (first_name, last_name, email, password, created_at, updated_at) VALUES('{NewUser.First_name}', '{NewUser.Last_name}', '{NewUser.Email}', '{NewUser.Password}', NOW(), NOW());";
                    DbConnector.Execute(query);
                    string getNewUser = $"SELECT * FROM users WHERE Email='{NewUser.Email}'";
                    List<Dictionary<string, object>> activeUser = DbConnector.Query(getNewUser);
                    HttpContext.Session.SetInt32("activeUser", (int)activeUser[0]["id"]);
                    return RedirectToAction("Dashboard", "Message");
                }
            }
            else
            {
                return View("Index");
            }
        }
        [Route("/login")]
        public IActionResult Login(string email, string password)
        {
            string emailCheck = $"SELECT * FROM users WHERE Email='{email}'";
            List<Dictionary<string, object>> similarEmails = DbConnector.Query(emailCheck);
            if(similarEmails.Count < 1)
            {
                ViewBag.messages = "This email is no registered!";
                return View("Index");
            }
            else
            {
                if ((string)similarEmails[0]["password"] == password)
                {
                    HttpContext.Session.SetInt32("activeUser", (int)similarEmails[0]["id"]);
                    return RedirectToAction("Dashboard", "Message");
                }
                else {
                    ViewBag.messages = "Password does not match!";
                    return View("Index");
                }
            }
        }
    }
}
