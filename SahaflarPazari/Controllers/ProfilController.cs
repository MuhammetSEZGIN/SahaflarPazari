using Antlr.Runtime.Tree;
using SahaflarPazari.Models;
using SahaflarPazari.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace SahaflarPazari.Controllers
{
    [Authorize]
    public class ProfilController : Controller
    {
        // GET: Profil

       
        private SahaflarPazariEntities db = new SahaflarPazariEntities();

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

        [MyAuthorization(Roles = "User")]
        [HttpPost]
        public ActionResult Sikayet(int id, string options)
        {
            Sikayet sikayet = new Sikayet();
            DateTime dateTime = DateTime.Now;
            sikayet.Tarih=dateTime; 
            sikayet.KitapId = id;
            sikayet.SikayetIcerigi = options;
            string username = User.Identity.Name;
            Kullanici kullanici = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == username);
            sikayet.KullaniciId = kullanici.KullaniciId;
            db.Sikayet.AddOrUpdate(sikayet);
            db.SaveChanges();
            return View();
        }


        [MyAuthorization(Roles = "User,Admin")]
        public ActionResult Index()
        {
            
            string userName = GetCurrentUserName();
            if (userName != String.Empty)
            {
                Kullanici kullanici = new Kullanici();
                kullanici = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == userName);
                List<IstekListesi> sepet = db.IstekListesi.Where(x => x.KullaniciId == kullanici.KullaniciId).ToList();
                ViewBag.sepet = sepet;
                return View(kullanici);
            }          
            return Json(new { success = false, message = "İstek Listesi Boş" }, JsonRequestBehavior.AllowGet);


        }

      

        [MyAuthorization(Roles = "User,Admin")]
        public ActionResult UpdateUserInfo(KullaniciBilgileri kullaniciBilgileri)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
               
                HttpCookie authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                {

                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {

                        string userName = authTicket.Name;
                        Kullanici kullanici = db.Kullanici.FirstOrDefault(k => k.KullaniciAdi == userName);
                        
                        if (kullanici != null)
                        {
                           
                            kullanici.KullaniciBilgileri.Ad = kullaniciBilgileri.Ad;
                            kullanici.KullaniciBilgileri.Soyad=kullaniciBilgileri.Soyad;
                            kullanici.KullaniciBilgileri.Eposta = kullaniciBilgileri.Eposta;
                            kullanici.KullaniciBilgileri.Telefon = kullaniciBilgileri.Telefon;
                            db.Kullanici.AddOrUpdate(kullanici);
                            db.SaveChanges();
                            return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, message = "Güncelleme Başarısız" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [MyAuthorization(Roles = "User,Admin")]
        public ActionResult UpdateLoginInfo(Kullanici kullanici) {

            ValidateControl VControl=new ValidateControl();
            string Control = VControl.PasswordControl(kullanici.Sifre);

            if (Control == String.Empty)
            {
               
                Kullanici dbKullanici = db.Kullanici.FirstOrDefault(x=>x.KullaniciAdi==kullanici.KullaniciAdi);
                dbKullanici.Sifre = kullanici.Sifre;
                dbKullanici.KullaniciAdi = kullanici.KullaniciAdi;
                if (VControl.UserNameControl(dbKullanici.KullaniciAdi, db))
                {
                    
                    db.Kullanici.AddOrUpdate(dbKullanici);
                    db.SaveChanges();
                    FormsAuthentication.SetAuthCookie(dbKullanici.KullaniciAdi, false);
                    return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Kullanici Adi Zaten Alınmış " }, JsonRequestBehavior.AllowGet);
                }
            }
            else {

                return Json(new { success = false , message= Control});
                   
            }
        }

       
        [MyAuthorization(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult UpdateList(int kitapId)
        {
            string userName = GetCurrentUserName();
            if (userName != String.Empty)
            {
                Kullanici kullanici = db.Kullanici.FirstOrDefault(k => k.KullaniciAdi == userName);
                IstekListesi istekListesi = new IstekListesi();
                istekListesi = db.IstekListesi.FirstOrDefault(x => x.KitapId == kitapId && x.KullaniciId == kullanici.KullaniciId); 
                if(istekListesi != null)
                {
                    db.IstekListesi.Remove(istekListesi);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Kitap listeden kaldırıldı" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // kitap sahibini kontrol etme
                    //List<int> kitap = db.Kitap.Where(x => x.SaticiId == kullanici.KullaniciId).Select(x => x.KitapId).ToList();

                    //if (kitap.Contains(kitapId))
                    //{
                    //    return Json(new { success = false, message = "Kitap Zaten Size Ait" });
                    //}
                    IstekListesi istekListesi1 = new IstekListesi();
                    istekListesi1.KitapId = kitapId;
                    istekListesi1.KullaniciId = kullanici.KullaniciId;
                    db.IstekListesi.AddOrUpdate(istekListesi1);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Kitap istek listesinden kaldırıldı." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Login", "Index");
            }

               
        }


        [MyAuthorization(Roles = "User")]
        public int CalculateFavorCount(int id)
        {
            int count = db.IstekListesi.Count(x => x.KullaniciId == id);
            return count;

        }

        [MyAuthorization(Roles = "User")]
        public ActionResult UpdateAdressInfo(Adres adres)
        {
           
            Adres DbAdres = db.Adres.FirstOrDefault(k => k.AdresId == adres.AdresId);
            
            if (DbAdres != null)
            {
                DbAdres.AdresAlani=adres.AdresAlani;
                DbAdres.AdresAdi=adres.AdresAdi;
                DbAdres.PostaKodu=adres.PostaKodu;
                DbAdres.Sehir=adres.Sehir;
                DbAdres.Mahalle=adres.Mahalle;
                DbAdres.Ilce=adres.Ilce;

                db.Adres.AddOrUpdate(DbAdres);
                db.SaveChanges();
                return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Güncelleme Başarısız" }, JsonRequestBehavior.AllowGet);
            }
                              
        }
        [MyAuthorization(Roles ="User,Admin")]
        public ActionResult DeleteAdress(int id)
        {
            Adres DbAdres = db.Adres.FirstOrDefault(k => k.AdresId == id);
            if (DbAdres != null)
            {
                db.Adres.Remove(DbAdres);
                db.SaveChanges();
                return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Güncelleme Başarısız" }, JsonRequestBehavior.AllowGet);
            }
        }


        [MyAuthorization(Roles = "User,Admin")]
        public ActionResult AddAdress(Adres adres)
        {
            string username= GetCurrentUserName();
            if(username!=null)
            {
                Kullanici kullanici = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == username);
                adres.KullaniciId = kullanici.KullaniciId;
                db.Adres.AddOrUpdate(adres);
                db.SaveChanges();
                return Json(new { success = true, message = "Ekleme Başarılı" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        
    
    }
}