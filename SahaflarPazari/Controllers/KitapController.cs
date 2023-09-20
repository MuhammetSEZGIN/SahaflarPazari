using Microsoft.Ajax.Utilities;
using SahaflarPazari.Models;
using SahaflarPazari.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SahaflarPazari.Controllers
{
    [Authorize]
    public class KitapController : Controller
    {
        // GET: Kitap
        private SahaflarPazariEntities db = new SahaflarPazariEntities();


        //public ActionResult AddPicture(HttpPostedFileBase file, int id)
        //{
        //    KitapResimleri kitapResimleri = new KitapResimleri();
        //    kitapResimleri.KitapId = id;
        //    kitapResimleri.DataYolu = Path.GetFileName(file.FileName);

        //    db.KitapResimleri.AddOrUpdate(kitapResimleri);

        //    string path = Server.MapPath("~/Content/img/Kitap_Resimleri/");
        //    bool resimExists = db.KitapResimleri.Any(r => r.DataYolu == file.FileName);

        //    if (resimExists)
        //    {
        //        return Json(new { success = false, message = "Resim adını değiştiriniz " }, JsonRequestBehavior.AllowGet); ;
        //    }


        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }


        //    file.SaveAs(path + Path.GetFileName(file.FileName));
        //    db.SaveChanges();
        //    return View();
        //}

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
        public ActionResult MyBooks()
        {
            string userName = GetCurrentUserName();

            if(userName!=String.Empty)
            {
                List<Kitap> kitap = db.Kitap.ToList();

                Kullanici kullanici = db.Kullanici.FirstOrDefault(k => k.KullaniciAdi == userName);
                kullanici = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == userName);
                List<KitapKategorileri> kitapKategorileri = db.KitapKategorileri.ToList();
                List<Yayinevi> yayinevi = db.Yayinevi.ToList();
                ViewBag.yayinevi = yayinevi;
                ViewBag.kategori=kitapKategorileri;
                return View(kullanici);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }




        [MyAuthorization(Roles = "User,Admin")]
        public ActionResult UpdateBook(Kitap kitap)
        {
            string username= GetCurrentUserName();
            if(username!=String.Empty)
            {
                Kullanici kullanici = db.Kullanici.FirstOrDefault(k => k.KullaniciAdi == username);
                Kitap kitap1 = db.Kitap.FirstOrDefault(x=>x.KitapId == kitap.KitapId);
                if (kitap1 != null)
                {
                    kitap1.KitapAdi = kitap.KitapAdi;
                    kitap1.Kullanici = kitap1.Kullanici;
                    kitap1.SaticiId = kullanici.KullaniciId;
                    kitap1.Fiyat = kitap.Fiyat;
                    kitap1.YayineviId = kitap.YayineviId;
                    kitap1.KategoriId = kitap.KategoriId;
                    kitap1.Detay = kitap.Detay;
                    kitap1.Yazar = kitap.Yazar;
                    db.Kitap.AddOrUpdate(kitap1);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Güncelleme Başarısız" }, JsonRequestBehavior.AllowGet);
                }



            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }




        [MyAuthorization(Roles = "User,Admin")]
        public void DeletePicture(List<KitapResimleri >kitapResimleri)
        {

            //KitapResimleri kitapResimleri= db.KitapResimleri.FirstOrDefault(x=>x.ResimId== id);
            string rootFolder =Server.MapPath( "~/Content/img/Kitap_Resimleri");
            //kitapResimleri.Kitap= kitap;
            db.KitapResimleri.RemoveRange(kitapResimleri);
            string fullPath = "";
            foreach (var resim in kitapResimleri)
            {
                fullPath = Path.Combine(rootFolder, resim.DataYolu);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        

        }




        [MyAuthorization(Roles = "User,Admin")]
        public ActionResult DeleteBook(int? id)
        {

            Kitap kitap = new Kitap();
            kitap=db.Kitap.FirstOrDefault(x => x.KitapId == id);

            if (kitap != null)
            {
                // List<KitapResimleri> kitapResimleri = db.KitapResimleri.Where(x => x.KitapId == id).ToList();
                List<KitapResimleri> kitapResimleri = new List<KitapResimleri>();
                foreach (var resim in kitap.KitapResimleri)
                {

                    kitapResimleri.Add(db.KitapResimleri.Find(resim.ResimId));

                }
                //DeletePicture(kitapResimleri[0]);
                DeletePicture(kitapResimleri);
                List<IstekListesi> istekListesi = db.IstekListesi.Where(x => x.KitapId == id).ToList();
                List<AlisverisSepeti> alisverisSepeti=db.AlisverisSepeti.Where(x => x.KitapId == id).ToList();
                List<Sikayet> sikayet= db.Sikayet.Where(x=>x.KitapId==id).ToList();
                // şu an da siparişlerden de kaldırıyorum.
                List<Siparisler> siparisler= db.Siparisler.Where(x => x.KitapId == id).ToList();
                db.Siparisler.RemoveRange(siparisler);
                db.IstekListesi.RemoveRange(istekListesi);
                db.AlisverisSepeti.RemoveRange(alisverisSepeti);
                db.Sikayet.RemoveRange(sikayet);  

                db.Kitap.Remove(kitap);               
                db.SaveChanges();
                    
            }
            return Json(new { success = true, message = "Kitap Silindi" }, JsonRequestBehavior.AllowGet);

        }





        [MyAuthorization(Roles = "User,Admin")]
        [HttpGet]
        public ActionResult AddBook()
        {
            List<KitapKategorileri> kitapKategorileri = db.KitapKategorileri.ToList();
            List<Yayinevi> yayinevi = db.Yayinevi.ToList();
            ViewBag.yayinevi = yayinevi;
            ViewBag.kategori = kitapKategorileri;
            return View(kitapKategorileri);

        }


       
        [MyAuthorization(Roles = "User")]
        [HttpPost]
        public ActionResult AddBook(Kitap model, HttpPostedFileBase file)
        {
            try
            {
             
                if(model != null && file!=null) 
                {
                    string username = GetCurrentUserName();
                    
                    Kullanici kullanici = db.Kullanici.FirstOrDefault(k => k.KullaniciAdi == username);
                    Kitap kitap1 = new Kitap();

                    kitap1.KitapAdi = model.KitapAdi;
                    kitap1.SaticiId = kullanici.KullaniciId;
                    kitap1.Fiyat = model.Fiyat;
                    kitap1.YayineviId = model.YayineviId;
                    kitap1.KategoriId = model.KategoriId;
                    kitap1.Detay = model.Detay;
                    kitap1.Yazar = model.Yazar;

                    DateTime nowLocal = DateTime.Now;
                    kitap1.EklenmeTarihi = nowLocal;

                    db.Kitap.AddOrUpdate(kitap1);
                    KitapResimleri kitapResimleri = new KitapResimleri();
                    kitapResimleri.KitapId = model.KitapId;
                    kitapResimleri.DataYolu = Path.GetFileName(file.FileName);

                    db.KitapResimleri.AddOrUpdate(kitapResimleri);
                             
                    string path = Server.MapPath("~/Content/img/Kitap_Resimleri/");
                    bool resimExists = db.KitapResimleri.Any(r => r.DataYolu == file.FileName);

                    if (resimExists)
                    {
                        return Json(new { success = false, message = "Resim adını değiştiriniz " }, JsonRequestBehavior.AllowGet);
                    }


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    file.SaveAs(path + Path.GetFileName(file.FileName));
                    ViewBag.Message = "File uploaded successfully.";
                    db.SaveChanges();
                   
                }
                return RedirectToAction("MyBooks", "Kitap");
                //return Json(new { success = true, message = "Kitap başarıyla yüklendi!" }, JsonRequestBehavior.AllowGet);
                //return Json(new { success = false, message = "Resim yüklenemedi!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Resim yükleme hatası: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

   

    }
}