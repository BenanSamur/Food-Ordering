using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using YemekUygulaması.Models;

namespace YemekUygulaması.Controllers
{
    public class DefaultController : Controller
    {
        private readonly yemekdbEntities1 db = new yemekdbEntities1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menü()
        {
            return View();
        }

        public ActionResult Giriş()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Giriş(Kullanıcı p)
        {
            var bilgiler = db.Kullanıcı.FirstOrDefault(x => x.EMail == p.EMail && x.Şifre == p.Şifre);
            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.EMail, false);
                Session["Mail"] = bilgiler.EMail;
                Session["Ad"] = bilgiler.KullanıcıAdı;

                // Rol kontrolü
                if (bilgiler.RolAdı == "Admin")
                {
                    return RedirectToAction("AdminPage", "Default");
                }

                // Kullanıcı rolü ise anasayfaya dön
                return RedirectToAction("Index", "Default");
            }
            else
            {
                ViewBag.hata = "Kullanıcı Adı ve şifre hatalı";
            }
            return View();
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Kullanıcı model, string userType)
        {
            if (ModelState.IsValid)
            {
                if (userType == "Admin")
                {
                    var admin = new Admin
                    {
                        E_posta = model.EMail,
                        Şifre = model.Şifre,
                        İsim = model.KullanıcıAdı
                    };
                    db.Admin.Add(admin);
                    db.SaveChanges();

                    return RedirectToAction("AdminLogin");
                }
                else
                {
                    var kullanıcı = new Kullanıcı
                    {
                        EMail = model.EMail,
                        Şifre = model.Şifre,
                        KullanıcıAdı = model.KullanıcıAdı
                    };
                    db.Kullanıcı.Add(kullanıcı);
                    db.SaveChanges();

                    return RedirectToAction("Giriş");
                }
            }
            ViewBag.hata = "Kayıt sırasında bir hata oluştu.";
            return View(model);
        }

        public ActionResult Sepet()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult İletişim()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult İletişim(Firma model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Mesajınız başarıyla gönderildi!";
                ModelState.Clear();
                return View();
            }
            return View(model);
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult AdminPage()
        {
            return View();
        }
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(Admin admin)
        {
            var bilgiler = db.Admin.FirstOrDefault(x => x.E_posta == admin.E_posta && x.Şifre == admin.Şifre);
            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.E_posta, false);
                Session["Mail"] = bilgiler.E_posta;
                Session["Ad"] = bilgiler.İsim;
                Session["Role"] = "Admin";
                return RedirectToAction("AdminPage", "Default");
            }
            else
            {
                ViewBag.hata = "Kullanıcı Adı ve şifre hatalı";
            }
            return View();
        }
    }

}
