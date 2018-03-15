using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DbConnection;
using wall.Models;

namespace login.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            string name = HttpContext.Session.GetString("user");
            ViewBag.name = name;
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                string checkEmail = $"SELECT * FROM users WHERE(email = '{newUser.Email}')";
                var emailExists = DbConnector.Query(checkEmail);
                if (emailExists.Count == 0)
                {
                    string query = $"INSERT INTO users (FirstName, LastName, Email, Password, Created_at, Updated_at) VALUES ('{newUser.FirstName}', '{newUser.LastName}', '{newUser.Email}', '{newUser.Password}', NOW(), NOW());";
                    DbConnector.Execute(query);
                    HttpContext.Session.SetString("user", newUser.FirstName);
                    var sessionQuery = DbConnector.Query(checkEmail);
                    int sessionId = (int)sessionQuery[0]["id"];
                    return RedirectToAction("Success"); 
                }
                else
                {
                    ViewBag.Email = "This email already exists.";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.Email = "";
                return View("Index");
            }
        }

        [HttpPost]
        [Route("logUser")]
        public IActionResult LogUser(LogUser user)
        {
            if(ModelState.IsValid)
            {
                string query = $"SELECT * FROM users where (Email = '{user.Email}')";
                var exist = DbConnector.Query(query);
                if (exist.Count == 0)
                {
                    ViewBag.Email = "Email not found!";
                    return View ("Login");
                }
                else
                {
                    string Password = (exist[0]["Password"]).ToString();
                    if (Password != user.Password)
                    {
                        ViewBag.Password = "Email or password does not match. Please try again.";
                        return View("Login");
                    }
                    else
                    {
                        int id = (int) exist[0]["id"];
                        HttpContext.Session.SetInt32("id", id);
                        string name = (exist[0]["FirstName"]).ToString();
                        HttpContext.Session.SetString("user", name);
                        return RedirectToAction("Success");
                    }
                }
            }
            else
            {
                ViewBag.Email = "";
                ViewBag.Password = "";
                return View("Login");
            }
        }

        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            string name = HttpContext.Session.GetString("user");
            ViewBag.name = name;
            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            string name = HttpContext.Session.GetString("user");
            ViewBag.name = name;
            return View("Login");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
