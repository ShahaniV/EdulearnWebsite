using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EdulearnWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Home()
        {

            return View();
        }

        public ActionResult AdminHome()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        public ActionResult Library()
        {
            return View();
        }

        public ActionResult AdminLibrary()
        {
            return View();
        }
        public ActionResult Game()
        {
            return View();
        }
        //======================================================= Manage Accounts
        public ActionResult ManageAdmin()
        {
            return View("Accounts/ManageAdmin");
        }
        public ActionResult ManageUser()
        {
            return View("Accounts/ManageUser");
        }
        //======================================================= Directory Pages
        public ActionResult EnglishD()
        {
            return View("LibraryPages/DIrectoryPages/EnglishD");
        }
        public ActionResult MathD()
        {
            return View("LibraryPages/DIrectoryPages/MathD");
        }
        public ActionResult FilipinoD()
        {
            return View("LibraryPages/DIrectoryPages/FilipinoD");
        }
        public ActionResult ScienceD()
        {
            return View("LibraryPages/DIrectoryPages/ScienceD");
        }
        public ActionResult eBooksD()
        {
            return View("LibraryPages/DIrectoryPages/eBooksD");
        }

        //=========================================================== Modules
        public ActionResult Kindergarten()
        {
            return View("LibraryPages/Kindergarten");
        }
        public ActionResult Elementary()
        {
            return View("LibraryPages/Elementary");
        }
        public ActionResult Junior_high()
        {
            return View("LibraryPages/Junior_high");
        }

        //===================================================== Tables and Charts
        public ActionResult Flags()
        {
            return View("LibraryPages/ChartsAndTables/Flags");
        }
        public ActionResult NumberChart()
        {
            return View("LibraryPages/ChartsAndTables/NumberChart");
        }
        public ActionResult LetterChart()
        {
            return View("LibraryPages/ChartsAndTables/LetterChart");
        }
        public ActionResult Shapes()
        {
            return View("LibraryPages/ChartsAndTables/Shapes");
        }
        public ActionResult Color()
        {
            return View("LibraryPages/ChartsAndTables/Color");
        }
        public ActionResult BodyParts()
        {
            return View("LibraryPages/ChartsAndTables/BodyParts");
        }
        public ActionResult MultiplicationTable()
        {
            return View("LibraryPages/ChartsAndTables/MultiplicationTable");
        }
        //========================================================== Maps
        public ActionResult Country()
        {
            return View("LibraryPages/Maps/Country");
        }
        public ActionResult Google()
        {
            return View("LibraryPages/Maps/Google");
        }
        public ActionResult World()
        {
            return View("LibraryPages/Maps/World");
        }
        //================================================== Videos
        public ActionResult Youtube()
        {
            return View("LibraryPages/Youtube");
        }
        public ActionResult Channels()
        {
            return View("LibraryPages/Channels");
        }

        //===================================== Ebooks, trivia, dictionary, studytips
        public ActionResult Ebook()
        {
            return View("LibraryPages/Ebook");
        }
        public ActionResult Trivia()
        {
            return View("LibraryPages/Trivia");
        }
        public ActionResult Dictionary()
        {
            return View("LibraryPages/Dictionary");
        }
        public ActionResult StudyTips()
        {
            return View("LibraryPages/StudyTips");
        }

        //====================================================== Games Pages
        public ActionResult Bubbleshooter()
        {
            return View("GamePages/Bubbleshooter");
        }
        
        public ActionResult BabyHazel()
        {
            return View("GamePages/BabyHazel");
        }
        public ActionResult BlockMonsters()
        {
            return View("GamePages/BlockMonsters");
        }
        public ActionResult Checkers()
        {
            return View("GamePages/Checkers");
        }
        public ActionResult Clickplay5()
        {
            return View("GamePages/Clickplay5");
        }
        public ActionResult DinoColor()
        {
            return View("GamePages/DinoColor");
        }
        public ActionResult Drawingforkids()
        {
            return View("GamePages/Drawingforkids");
        }
        public ActionResult Goo()
        {
            return View("GamePages/Goo");
        }
        public ActionResult Happyglass()
        {
            return View("GamePages/Happyglass");
        }
        public ActionResult Masterchess()
        {
            return View("GamePages/Masterchess");
        }
        public ActionResult Mathbat()
        {
            return View("GamePages/Mathbat");
        }
        public ActionResult PuzzleAndMatchingGame()
        {
            return View("GamePages/PuzzleAndMatchingGame");
        }
        public ActionResult RollTheBall()
        {
            return View("GamePages/RollTheBall");
        }
        public ActionResult SolarMax2()
        {
            return View("GamePages/SolarMax2");
        }
        public ActionResult WildAnimals()
        {
            return View("GamePages/WildAnimals");
        }
        public ActionResult WordSearch()
        {
            return View("GamePages/WordSearch");
        }
    }
}