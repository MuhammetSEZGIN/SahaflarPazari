using SahaflarPazari.Models;
using SahaflarPazari.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SahaflarPazari.Controllers
{
    public class ShopCartController : Controller
    {
        // GET: ShopCart
        private SahaflarPazariEntities db= new SahaflarPazariEntities();
        public string GetCurrentUserName()
        {
            if (HttpContext.Request.IsAuthenticated)
            {

                HttpCookie authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                {

                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {

                        return authTicket.Name;

                    }
                }
            }

            return String.Empty;
        }

        [MyAuthorization(Roles = "User,Admin")]
        [HttpGet]
        public ActionResult ShopCart()
        {
            String username=GetCurrentUserName();
            if(username != String.Empty)
            {
                Kullanici kullanici = new Kullanici();
                kullanici = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == username);
                List<AlisverisSepeti> sepet = db.AlisverisSepeti.Where(x => x.KullaniciId == kullanici.KullaniciId).ToList();
                ViewBag.SepetItem = sepet;
                int total=CalculateBooksCount(kullanici.KullaniciId);
               
                Session["ItemCount"] = total;
                return View();
            
            }
            return Json(new { success = false, message = "Siparis Listesi Boş" }, JsonRequestBehavior.AllowGet);
        }


        [MyAuthorization(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
           
            String username = GetCurrentUserName();
            if (username != String.Empty)
            {
                Kullanici kullanici = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == username);
                List<int> kitap = db.Kitap.Where(x => x.SaticiId == kullanici.KullaniciId).Select(x=>x.KitapId).ToList();

                if (kitap.Contains(id))
                {
                    return  Json(new { success = false, message = "Kitap Zaten Size Ait" });
                }
                AlisverisSepeti alisverisSepeti = new AlisverisSepeti();
                alisverisSepeti.KitapId = id;
               
                alisverisSepeti.KullaniciId=kullanici.KullaniciId;
                int Count=CalculateBooksCount(kullanici.KullaniciId);
                
                db.AlisverisSepeti.Add(alisverisSepeti);
                db.SaveChanges();
                return Json(new { success = true, message = "Kitap Sepete Eklendi", BookCount= Count+1 });
            }
          
            return Json(new { success = false, message = "Kitap Sepete Eklenemedi" });
          
        }

        [MyAuthorization(Roles = "User,Admin")]
        public ActionResult RemoveFromCart(int ? id)
        {
            String username = GetCurrentUserName();
            int Count = 0;
            int Sum = 0;
            if (username != String.Empty)
            {
                Kullanici kullanici = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == username);
                Count = CalculateBooksCount(kullanici.KullaniciId);
                Sum = CalculateSum(kullanici.KullaniciId);
            }

            AlisverisSepeti alisverisSepeti= new AlisverisSepeti();
            alisverisSepeti = db.AlisverisSepeti.Find(id);
            Sum -= alisverisSepeti.Kitap.Fiyat;
            if (alisverisSepeti != null)
            {
                db.AlisverisSepeti.Remove(alisverisSepeti);
                db.SaveChanges();
            }

            return Json(new { success = true, message = "Kitap Sepetten Kaldırıldı", BookCount = Count-1, sum = Sum }, JsonRequestBehavior.AllowGet);
            //return Json(new { success = true, message = "Kitap Sepetten Kaldırıldı" , BookCount = 0} , JsonRequestBehavior.AllowGet);
        }

        [MyAuthorization(Roles = "User,Admin")]
        public ActionResult ChooseAdres()
        {
            string username = GetCurrentUserName(); 
            if(username != String.Empty)
            {
                Kullanici kullanici= db.Kullanici.FirstOrDefault(x=>x.KullaniciAdi==username);
                List<Adres> list= new List<Adres>();
                list= db.Adres.Where(x=>x.KullaniciId==kullanici.KullaniciId).ToList();
                return View(list);  
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [MyAuthorization(Roles = "User,Admin")]
        [HttpGet]
        public ActionResult Orders()
        {
            string username = GetCurrentUserName();
            if (username != String.Empty)
            {
                Kullanici kullanici = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == username);
                List<Siparisler> siparisler = db.Siparisler.Where(x=>x.KullaniciId== kullanici.KullaniciId).ToList() ;
                return View(siparisler);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [MyAuthorization(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult Orders(int ? id)
        {
            string username = GetCurrentUserName();
            if(username != String.Empty)
            {
                Kullanici kullanici = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == username);
                Adres adres = db.Adres.FirstOrDefault(x => x.AdresId == id);
                List<Siparisler> siparisler = new List<Siparisler>();
                DateTime nowLocal = DateTime.Now;
                foreach (var item in kullanici.AlisverisSepeti.ToList())
                {
                    siparisler.Add(new Siparisler
                    {
                        KullaniciId = kullanici.KullaniciId,
                        KitapId = item.KitapId,
                        Adres = adres.ToString(),
                        SiparisTarihi = nowLocal,
                        SiparisDurumu = "Hazırlanıyor"
                    });
                        
                       
                }
                db.Siparisler.AddRange(siparisler);
                db.SaveChanges();

                return RedirectToAction("Orders", "ShopCart");
               

            }
            else
            {
                return RedirectToAction("Login","Account" );
            }
        }

        [MyAuthorization(Roles = "User,Admin")]
        public ActionResult SoldItems()
        {
            string username = GetCurrentUserName();
            if (username != String.Empty)
            {
                Kullanici kullanici = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == username);
                List <Siparisler> siparisler = db.Siparisler.Where(x=>x.Kitap.SaticiId==kullanici.KullaniciId).ToList();
                return View(siparisler);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public ActionResult Durum(int id, string durum)
        {
            Siparisler siparisler = db.Siparisler.FirstOrDefault(x=>x.SiparisId == id);
            siparisler.SiparisDurumu=durum;
            db.SaveChanges();
            return View();
        }

        [MyAuthorization(Roles = "User,Admin")]
        public int CalculateBooksCount(int id)
        {  
            int count = db.AlisverisSepeti.Count(x => x.KullaniciId == id);
            return count;
               
        }

        public int CalculateSum(int id)
        {

            int toplamFiyat = db.AlisverisSepeti.Where(s => s.KullaniciId == id).Sum(s => s.Kitap.Fiyat);           
            return toplamFiyat;
        }
        
    }
}