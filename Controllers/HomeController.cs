using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {

        private int? UserSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }

        private MyContext dbContext;

        public HomeController(MyContext context)

        {
            dbContext = context;
        }


        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            {
                if (ModelState.IsValid)
                {
                var existingUser = dbContext.Users.FirstOrDefault(u => u.Email == newUser.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "This Username already exists, check yo facts bro!");
                    return View("Index");
                }


                PasswordHasher<User> Hasher = new PasswordHasher<User>();

                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

                dbContext.Users.Add(newUser);

                dbContext.SaveChanges();

                UserSession = newUser.UserId;
                // SessionSeller = newUser.Username;

                return RedirectToAction("Dashboard");
            }
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(User userSubmission)
        {

            

            var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
            if (userInDb == null)
            {
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("Index");
            }
            
            


            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

            Console.WriteLine(userInDb.Password + "     Break em Up     " + userSubmission.Password);


            Console.WriteLine("********************************************************************");
            Console.WriteLine(result);
            Console.WriteLine("********************************************************************");

            if (result != 0)
            {
                UserSession = userInDb.UserId;
                Console.WriteLine(userInDb.UserId);

                return RedirectToAction("Dashboard");


            }
            Console.WriteLine("********************************************************************");
            Console.WriteLine(userInDb.UserId);

            
            // Console.WriteLine(result);
            Console.WriteLine("********************************************************************");
            ModelState.AddModelError("Password", "Git ta know ur hashword dummy!");
            return View("Index");
        }




        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if (UserSession == null)
                return RedirectToAction("Index");

            var userInDb = dbContext.Users.FirstOrDefault(u => u.UserId == UserSession);
            ViewBag.thisUser = userInDb;


            var AllHappenings = dbContext.Happenings
                .Include(w => w.RSVPs)
                .Include(c => c.Creator)
                .OrderBy(d => d.Date)
                .ToList();



            ViewBag.UserId = UserSession;
            return View("dashboard", AllHappenings);
        }


        [HttpGet("newhappening")]
        public IActionResult NewHappening()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(Happening newHappening)
        {
            if (UserSession == null)
                return RedirectToAction("Index");
            if (ModelState.IsValid)
            {
                newHappening.UserId = (int)UserSession;
                dbContext.Happenings.Add(newHappening);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("newhappening");
        }

        [HttpGet("rsvp/{happeningId}")]
        public IActionResult RSVP(int happeningId)
        {
            if (UserSession == null)
                return RedirectToAction("Index");

            // RSVP toDelete = dbContext.RSVPs.FirstOrDefault(r => r.HappeningId == happeningId && r.UserId == UserSession);



            // RSVP thisRSVP = dbContext.RSVPs.FirstOrDefault(r => r.HappeningId == happeningId && r.UserId == UserSession);

            // if (thisRSVP == null)
            // {
            //     return RedirectToAction("Dashboard");
            // }
            // else
            // {
            var existingRSVP = dbContext.RSVPs.FirstOrDefault(r => r.HappeningId == happeningId && r.UserId == UserSession);
            if (existingRSVP != null)
            {
                return RedirectToAction("Dashboard");
            }


            RSVP newRSVP = new RSVP()
            {
                HappeningId = happeningId,
                UserId = (int)UserSession
            };



            dbContext.RSVPs.Add(newRSVP);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");



        }

        [HttpGet("unrsvp/{happeningId}")]
        public IActionResult UnRSVP(int happeningId)
        {
            if (UserSession == null)
                return RedirectToAction("Index");

            RSVP toDelete = dbContext.RSVPs.FirstOrDefault(r => r.HappeningId == happeningId && r.UserId == UserSession);

            if (toDelete == null)
                return RedirectToAction("Dashboard");

            dbContext.RSVPs.Remove(toDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }


        [HttpGet("show/{happeningId}")]
        public IActionResult Show(int happeningId)
        {
            var thisHappening = dbContext.Happenings
            .Include(w => w.RSVPs)
            .ThenInclude(r => r.Guest)
            .Include(c => c.Creator)
            .FirstOrDefault(w => w.HappeningId == happeningId);

            ViewBag.UserId = UserSession;

            return View("Show", thisHappening);
        }

        [HttpGet("delete/{happeningId}")]
        public IActionResult Delete(int happeningId)
        {
            if (UserSession == null)
                return RedirectToAction("Index");

            Happening toDelete = dbContext.Happenings.FirstOrDefault(w => w.HappeningId == happeningId);

            if (toDelete == null)
                return RedirectToAction("Dashboard");
            // Redirect to dashboard if user trying to delete isn't the wedding creator
            if (toDelete.UserId != UserSession)
                return RedirectToAction("Dashboard");

            dbContext.Happenings.Remove(toDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }



        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
