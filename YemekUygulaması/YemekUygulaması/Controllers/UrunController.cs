using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YemekUygulaması.Models;

namespace YemekUygulaması.Controllers
{
    public class UrunController : Controller
    {
        yemekdbEntities1 db = new yemekdbEntities1();
        public ActionResult Hamburger()
        {


            var list = db.Hamburger.ToList();
            return View(list);

        }
        public ActionResult Pizza()
        {
            var list = db.Pizza.ToList();
            return View(list);
        }
        public ActionResult Sushi()
        {

            var list = db.Sushi.ToList();
            return View(list);
        }
        public ActionResult Balık()
        {
            var list = db.Balık.ToList();
            return View(list);
        }
        public ActionResult İçecekler()
        {
            var list = db.İçecekler.ToList();
            return View(list);
        }
        public ActionResult Tatlı()
        {
            var list = db.Tatlı.ToList();
            return View(list);
        }
        
    }
}