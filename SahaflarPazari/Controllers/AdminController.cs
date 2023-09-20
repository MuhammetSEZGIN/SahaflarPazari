using SahaflarPazari.Models;
using SahaflarPazari.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace SahaflarPazari.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
      
        private SahaflarPazariEntities db = new SahaflarPazariEntities();

        [MyAuthorization(Roles = "Admin")]
        public ActionResult Kullanicilar()
        {

            string username= User.Identity.Name;
            List<Kullanici> kullanicilar= db.Kullanici.Where(x=>x.KullaniciAdi!=username).ToList();
            List<KullaniciBilgileri> kullaniciBilgileri= db.KullaniciBilgileri.ToList();
            ViewBag.Kullanici=kullanicilar;
            ViewBag.Kişisel = kullaniciBilgileri;
            
            return View();

            
        }

        public void DeletePicture(List<KitapResimleri> kitapResimleri)
        {

            //KitapResimleri kitapResimleri= db.KitapResimleri.FirstOrDefault(x=>x.ResimId== id);
            string rootFolder = Server.MapPath("~/Content/img/Kitap_Resimleri");
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

        [MyAuthorization(Roles = "Admin")]
        public ActionResult KullaniciSil(int ?id)
        {
            Kullanici kullanici= db.Kullanici.FirstOrDefault(x=>x.KullaniciId==id);
            if (kullanici != null)
            {
                KullaniciBilgileri kullaniciBilgileri = db.KullaniciBilgileri.FirstOrDefault(x => x.KullaniciId == id);
                List<Adres> Adresler= db.Adres.Where(x=>x.KullaniciId==id).ToList();
                List<IstekListesi> ıstekListesi = db.IstekListesi.Where(x => x.KullaniciId == id).ToList();
                List<AlisverisSepeti> alisverisSepeti=db.AlisverisSepeti.Where(x => x.KullaniciId == id).ToList();
                Roller roller = db.Roller.FirstOrDefault(x => x.KullaniciId == id);
                List<Siparisler> siparisListesi= db.Siparisler.Where(x=>x.SiparisId == id).ToList();              
                var kitapContext = db.Kitap.Where(x => x.SaticiId == id);
                List<Kitap> kitaps= kitapContext.ToList();
                List<KitapResimleri> kitapResimleri = new List<KitapResimleri>();

                List<Sikayet>sikayets= db.Sikayet.Where(x=>x.KullaniciId==id).ToList() ;

                foreach (var kitap in kitapContext)
                {
                    foreach (var resim in kitap.KitapResimleri)
                    {
                       
                        kitapResimleri.Add(db.KitapResimleri.Find(resim.ResimId));

                    }
                }

                if (sikayets != null)
                {
                    db.Sikayet.RemoveRange(sikayets);
                }


                if (kitapResimleri != null)
                {
                    DeletePicture(kitapResimleri);
                }


                if (kullaniciBilgileri != null)
                {
                    db.KullaniciBilgileri.Remove(kullaniciBilgileri);
                }
                if (ıstekListesi != null)
                {
                    db.IstekListesi.RemoveRange(ıstekListesi);
                }
                if (alisverisSepeti != null)
                {
                    db.AlisverisSepeti.RemoveRange(alisverisSepeti);
                }
                if (roller != null)
                {
                    db.Roller.Remove(roller);
                }
                if (Adresler != null)
                {
                    db.Adres.RemoveRange(Adresler);                
                
                }
                if (kullanici != null)
                {
                    db.Kullanici.Remove(kullanici);
                }
                if (siparisListesi != null)
                {
                    db.Siparisler.RemoveRange(siparisListesi);
                }

               

                if (kitaps !=null)
                {
                    db.Kitap.RemoveRange(kitaps);
                }

               
                db.SaveChanges();
                return Json(new { success = true, message = "Kullanici Silindi" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Kullanici Silinemedi" }, JsonRequestBehavior.AllowGet);
            }
           
        }


        public ActionResult Sikayetler()
        {
           List <Sikayet>sikayet = db.Sikayet.ToList();
           if(sikayet.Count > 0)
            {
                return View(sikayet);
            }
            else
            {
                ViewBag.message = "Sikayet Bulunmamakta";
                return View();
            }
        }
    }
}