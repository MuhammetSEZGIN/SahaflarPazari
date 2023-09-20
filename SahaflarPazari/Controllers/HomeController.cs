using SahaflarPazari.Models;
using SahaflarPazari.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SahaflarPazari.Controllers
{
    public class HomeController : Controller
    {
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


        [HttpGet]
        public ActionResult Details(int? id)
        {
            
            
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Kitap kitap = db.Kitap.Find(id);
                if (kitap == null)
                {
                    return HttpNotFound();
                }
                return View(kitap);
            
        }
      
        // GET: Kitap
        [HttpGet]
        public ActionResult Index()
        {
            string username= GetCurrentUserName();
            List<Kitap> kitap = db.Kitap.ToList();
            List<Kitap> enSonEklenenler = db.Kitap.OrderByDescending(k => k.EklenmeTarihi).ToList();
            MainPageModel model = new MainPageModel();
            if (username != String.Empty)
            {
                Kullanici kullanici = new Kullanici();               
                kullanici =db.Kullanici.FirstOrDefault(x=>x.KullaniciAdi==username);
               
                List<int?> istekListesiKitaplari = db.IstekListesi.Where(x => x.KullaniciId == kullanici.KullaniciId).Select(x => x.KitapId).ToList();              
                model.istekListesiKitaplari=istekListesiKitaplari;
                model.kitap= kitap;
                model.enSonEklenenler = enSonEklenenler;
                Session["ItemCount"] = db.AlisverisSepeti.Count(x => x.KullaniciId == kullanici.KullaniciId);
                return View(model);
            }
            else
            {
                model.kitap = kitap;
                model.enSonEklenenler= enSonEklenenler;
                return View(model); 
            }
        }

      

    }
}