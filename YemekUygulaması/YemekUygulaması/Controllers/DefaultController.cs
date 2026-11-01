using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using YemekUygulaması.Helpers;
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
            var bilgiler = string.IsNullOrWhiteSpace(p.EMail)
                ? null
                : db.Kullanıcı.FirstOrDefault(x => x.EMail == p.EMail);

            bool needsUpgrade;
            if (bilgiler != null && PasswordHasher.Verify(p.Şifre, bilgiler.Şifre, out needsUpgrade))
            {
                // Eski düz metin şifreyi ilk başarılı girişte hash'e çevir
                if (needsUpgrade)
                {
                    bilgiler.Şifre = PasswordHasher.Hash(p.Şifre);
                    db.SaveChanges();
                }

                FormsAuthentication.SetAuthCookie(bilgiler.EMail, false);
                Session["Mail"] = bilgiler.EMail;
                Session["Ad"] = bilgiler.KullanıcıAdı;
                Session.Remove("Role"); // Normal kullanıcı admin yetkisi taşımaz

                return RedirectToAction("Index", "Default");
            }

            ViewBag.hata = "E-posta veya şifre hatalı";
            return View();
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Kullanıcı model)
        {
            // Bu form yalnızca normal kullanıcı hesabı oluşturur.
            // Admin hesapları internetten kayıt olunarak değil, veritabanından eklenir.
            if (string.IsNullOrWhiteSpace(model.KullanıcıAdı) ||
                string.IsNullOrWhiteSpace(model.EMail) ||
                string.IsNullOrEmpty(model.Şifre))
            {
                ViewBag.hata = "Tüm alanları doldurun.";
                return View(model);
            }

            if (!IsValidEmail(model.EMail))
            {
                ViewBag.hata = "Geçerli bir e-posta adresi girin.";
                return View(model);
            }

            if (model.Şifre.Length < 8)
            {
                ViewBag.hata = "Şifre en az 8 karakter olmalıdır.";
                return View(model);
            }

            if (db.Kullanıcı.Any(x => x.EMail == model.EMail))
            {
                ViewBag.hata = "Bu e-posta adresiyle zaten bir hesap var.";
                return View(model);
            }

            var kullanıcı = new Kullanıcı
            {
                EMail = model.EMail.Trim(),
                Şifre = PasswordHasher.Hash(model.Şifre),
                KullanıcıAdı = model.KullanıcıAdı.Trim()
            };
            db.Kullanıcı.Add(kullanıcı);
            db.SaveChanges();

            return RedirectToAction("Giriş");
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email.Trim());
                return addr.Address == email.Trim();
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool IsAdmin()
        {
            return (Session["Role"] as string) == "Admin";
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
            if (!IsAdmin()) return RedirectToAction("AdminLogin");
            return View();
        }

        public ActionResult AdminPage()
        {
            if (!IsAdmin()) return RedirectToAction("AdminLogin");
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
            var bilgiler = string.IsNullOrWhiteSpace(admin.E_posta)
                ? null
                : db.Admin.FirstOrDefault(x => x.E_posta == admin.E_posta);

            bool needsUpgrade;
            if (bilgiler != null && PasswordHasher.Verify(admin.Şifre, bilgiler.Şifre, out needsUpgrade))
            {
                if (needsUpgrade)
                {
                    bilgiler.Şifre = PasswordHasher.Hash(admin.Şifre);
                    db.SaveChanges();
                }

                FormsAuthentication.SetAuthCookie(bilgiler.E_posta, false);
                Session["Mail"] = bilgiler.E_posta;
                Session["Ad"] = bilgiler.İsim;
                Session["Role"] = "Admin";
                return RedirectToAction("AdminPage", "Default");
            }

            ViewBag.hata = "E-posta veya şifre hatalı";
            return View();
        }
    }

}
