using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using YemekUygulaması.Models;

namespace YemekUygulaması.Controllers
{
    public class SepetController : Controller
    {
        private yemekdbEntities1 db = new yemekdbEntities1();

        public ActionResult Index()
        {
            var cart = GetCart();
            decimal totalPrice = cart.Sum(item => item.Fiyat ?? 0);
            ViewBag.TotalPrice = totalPrice;
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int itemId, string itemType, int quantity = 1)
        {
            Siparişler item = GetItemById(itemId, itemType);

            if (item != null)
            {
                var cart = GetCart();
                var existingItem = cart.FirstOrDefault(i => i.SiparişID == item.SiparişID);

                if (existingItem != null)
                {
                    existingItem.Miktar += quantity;
                }
                else
                {
                    item.Miktar = quantity;
                    cart.Add(item);
                }

                Session["Cart"] = cart;
                SaveCartToDatabase(cart);
            }

            return RedirectToAction("Index");
        }

        private Siparişler GetItemById(int id, string itemType)
        {
            object item = null;

            switch (itemType)
            {
                case "Hamburger":
                    item = db.Hamburger.Find(id);
                    break;
                case "Pizza":
                    item = db.Pizza.Find(id);
                    break;
                case "Balık":
                    item = db.Balık.Find(id);
                    break;
                case "Sushi":
                    item = db.Sushi.Find(id);
                    break;
                case "Tatlı":
                    item = db.Tatlı.Find(id);
                    break;
                case "İçecekler":
                    item = db.İçecekler.Find(id);
                    break;
                default:
                    return null;
            }

            if (item != null)
            {
                if (item is Hamburger hamburger)
                {
                    return new Siparişler
                    {
                        HamburgerId = hamburger.ID,
                        Adı = hamburger.Adı,
                        Fiyat = hamburger.Fiyat,
                        ResimURL = hamburger.ResimURL,
                        Açıklama = hamburger.Açıklama
                    };
                }
                else if (item is Pizza pizza)
                {
                    return new Siparişler
                    {
                        PizzaId = pizza.ID,
                        Adı = pizza.Adı,
                        Fiyat = pizza.Fiyat,
                        ResimURL = pizza.ResimURL,
                        Açıklama = pizza.Açıklama
                    };
                }
                else if (item is Balık balık)
                {
                    return new Siparişler
                    {
                        BalıkId = balık.ID,
                        Adı = balık.Adı,
                        Fiyat = balık.Fiyat,
                        ResimURL = balık.ResimURL,
                        Açıklama = balık.Açıklama
                    };
                }
                else if (item is Sushi sushi)
                {
                    return new Siparişler
                    {
                        SushiId = sushi.ID,
                        Adı = sushi.Adı,
                        Fiyat = sushi.Fiyat,
                        ResimURL = sushi.ResimURL,
                        Açıklama = sushi.Açıklama
                    };
                }
                else if (item is Tatlı tatlı)
                {
                    return new Siparişler
                    {
                        TatlıId = tatlı.ID,
                        Adı = tatlı.Adı,
                        Fiyat = tatlı.Fiyat,
                        ResimURL = tatlı.ResimURL,
                        Açıklama = tatlı.Açıklama
                    };
                }
                else if (item is İçecekler içecekler)
                {
                    return new Siparişler
                    {
                        İçeceklerId = içecekler.İçecekID,
                        Adı = içecekler.İçecekAdı,
                        Fiyat = içecekler.Fiyat,
                        ResimURL = içecekler.ResimURL,
                        Açıklama = içecekler.Açıklama
                    };
                }
            }

            return null;
        }

        private List<Siparişler> GetCart()
        {
            return Session["Cart"] as List<Siparişler> ?? new List<Siparişler>();
        }

        private void SaveCartToDatabase(List<Siparişler> cart)
        {
            var existingOrderIds = cart.Select(i => i.SiparişID).ToList();
            var ordersInDb = db.Siparişler.Where(o => existingOrderIds.Contains(o.SiparişID)).ToList();

            foreach (var item in cart)
            {
                var existingOrder = ordersInDb.FirstOrDefault(o => o.SiparişID == item.SiparişID);

                if (existingOrder != null)
                {
                    existingOrder.Miktar = item.Miktar;
                    existingOrder.Adı = item.Adı;
                    existingOrder.Fiyat = item.Fiyat;
                    existingOrder.ResimURL = item.ResimURL;
                    existingOrder.Açıklama = item.Açıklama;
                }
                else
                {
                    db.Siparişler.Add(item);
                }
            }

            db.SaveChanges();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromCart(int remove)
        {
            try
            {
                var cart = GetCart();
                var itemToRemove = cart.FirstOrDefault(i => i.SiparişID == remove);

                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    Session["Cart"] = cart;

                    var itemInDb = db.Siparişler.FirstOrDefault(i => i.SiparişID == remove);
                    if (itemInDb != null)
                    {
                        db.Siparişler.Remove(itemInDb);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public JsonResult GetBasketCount()
        {
            var cart = GetCart();
            var itemCount = cart.Count;
            return Json(new { success = true, count = itemCount }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Checkout()
        {
            var cart = GetCart();
            decimal totalPrice = cart.Sum(item => item.Fiyat ?? 0);
            ViewBag.TotalPrice = totalPrice;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteOrder(string paymentMethod, string holdername, string cardno, string cvcpwd, string exp)
        {
            try
            {
                var cart = GetCart();

                if (cart.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Cart is empty. Redirecting to error page.");
                    return RedirectToAction("Error", "Default");
                }

                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    foreach (var item in cart)
                    {
                        if (item != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Adding item to database: {item.Adı}");
                            db.Siparişler.Add(item);
                        }
                    }

                    db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Database changes saved successfully.");
                    dbTransaction.Commit();

                    Session["OrderDetails"] = cart.FirstOrDefault();
                    Session["Cart"] = null;
                    System.Diagnostics.Debug.WriteLine("Cart cleared successfully.");
                }

                return RedirectToAction("OrderConfirmation");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        public ActionResult OrderConfirmation()
        {
            var model = Session["OrderDetails"] as Siparişler;

            if (model == null)
            {
                // Sipariş detayları mevcut değilse kullanıcıyı bir hata sayfasına yönlendir
                return RedirectToAction("Error", "Default");
            }

            return View(model);
        }

        public ActionResult ClearCartAndRedirectHome()
        {
            Session["Cart"] = null;
            return RedirectToAction("Index", "Default");
        }
    }
}
