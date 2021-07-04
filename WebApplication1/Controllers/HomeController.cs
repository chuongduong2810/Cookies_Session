using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.IO;
namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public class Session
        {
            public string ID { get; set; }
            public string Username { get; set; }
        }

        public IActionResult Index()
        {

            //Lay sessionID da luu tu Cookies
            var sessionID = Request.Cookies["sessionID"];
            //Doc file ~ Lay du lieu tu db
            var allSession = new List<Session>();
            allSession = JsonConvert.DeserializeObject<List<Session>>(System.IO.File.ReadAllText(@"F:\MWG\Session and cookie\Session and cookie\WebApplication1\wwwroot\lib\json\session.json"));

            if (sessionID != null)
            {
                var existedSession = allSession.Find(item => item.ID == sessionID);
                if (existedSession != null)
                {
                    ViewBag.username = existedSession.Username;
                    return View();
                }
                else return RedirectToAction("Login", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        public IActionResult Login()
        {

            //Lay sessionID da luu tu Cookies
            var sessionID = Request.Cookies["sessionID"];
            var allSession = new List<Session>();
            allSession = JsonConvert.DeserializeObject<List<Session>>(System.IO.File.ReadAllText(@"F:\MWG\Session and cookie\Session and cookie\WebApplication1\wwwroot\lib\json\session.json"));

            if (sessionID != null)
            {
                var existedSession = allSession.Find(item => item.ID == sessionID);
                if (existedSession != null)
                {
                    ViewBag.username = existedSession.Username;
                    return View();
                }
                else return View();
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Login(IFormCollection formCollection)
        {
            //Lay sessionID da luu tu Cookies
            var sessionID = Request.Cookies["sessionID"];
            //Doc file ~ Lay du lieu tu db
            var allSession = new List<Session>();
            allSession = JsonConvert.DeserializeObject<List<Session>>(System.IO.File.ReadAllText(@"F:\MWG\Session and cookie\Session and cookie\WebApplication1\wwwroot\lib\json\session.json"));

            if (sessionID != null)
            {
                var existedSession = allSession.Find(item => item.ID == sessionID);
                if (existedSession != null)
                {
                    ViewBag.username = existedSession.Username;
                    return View();
                }
                else return RedirectToAction("Login", "Home");
            }
            else
            {
                Guid id = Guid.NewGuid();
                CookieOptions option = new CookieOptions();
                ViewBag.username = formCollection["name"];
                //Doc file ~ Lay du lieu tu db
                allSession = JsonConvert.DeserializeObject<List<Session>>(System.IO.File.ReadAllText(@"F:\MWG\Session and cookie\Session and cookie\WebApplication1\wwwroot\lib\json\session.json"));

                Session session = new Session
                {
                    ID = id.ToString(),
                    Username = formCollection["name"]
                };
                //Luu thong tin session vo file json ~ luu session vo DB
                allSession.Insert(allSession.Count(), session);
                System.IO.File.WriteAllText(@"F:\MWG\Session and cookie\Session and cookie\WebApplication1\wwwroot\lib\json\session.json", JsonConvert.SerializeObject(allSession));

                //Set thoi han cookies
                option.Expires = DateTime.Now.AddHours(3);

                //Luu sessionID vao cookies.
                Response.Cookies.Append("sessionID", id.ToString(), option);

                return RedirectToAction("Index", "Home");
            }
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
