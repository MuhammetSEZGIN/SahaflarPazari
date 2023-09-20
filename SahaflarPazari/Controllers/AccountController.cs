using SahaflarPazari.Models;
using SahaflarPazari.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace SahaflarPazari.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        public ActionResult Logout()
        {
            // Oturumu sonlandırma
            FormsAuthentication.SignOut();

            // Çıkış işleminden sonra ana sayfaya yönlendirme
            return RedirectToAction("Login", "Account");
        }

        // GET: Account
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Kullanici kullanici)
        {

            try
            {
                SahaflarPazariEntities db = new SahaflarPazariEntities();
                Kullanici kb = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == kullanici.KullaniciAdi && x.Sifre == kullanici.Sifre);
                if (kb == null)
                {
                    ViewBag.hata = "Kullanıcı Adı veya Sifre Hatalı";
                    return View();
                }
                else
                {

                    FormsAuthentication.SetAuthCookie(kullanici.KullaniciAdi,false);                   
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)

            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }


        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel registerModel)
        {


            using (ValidateControl VControl = new ValidateControl())
            {
                try
                {
                    String Control = VControl.PasswordControl(registerModel.Sifre);
                    if (Control==String.Empty) 
                    {
                        SahaflarPazariEntities model = new SahaflarPazariEntities();
                        if (VControl.UserNameControl(registerModel.KullaniciAdi, model))
                        {
                            Kullanici kullanici = new Kullanici();
                            KullaniciBilgileri kullaniciBilgileri = new KullaniciBilgileri();

                            kullanici.KullaniciAdi = registerModel.KullaniciAdi;
                            kullanici.Sifre = registerModel.Sifre;
                            kullaniciBilgileri.Ad = registerModel.Ad;
                            kullaniciBilgileri.Soyad = registerModel.Soyad;
                            kullaniciBilgileri.Eposta = registerModel.Eposta;
                            kullaniciBilgileri.Telefon = registerModel.Telefon;
                            kullanici.KullaniciBilgileri = kullaniciBilgileri;

                            Roller roller = new Roller();
                            roller.KullaniciId = kullanici.KullaniciId;
                            roller.RolAdi = "User";    
                            model.Roller.AddOrUpdate(roller);
                            model.Kullanici.AddOrUpdate(kullanici);
                           
                            model.SaveChanges();
                            return RedirectToAction("Login", "Account");
                        }
                        else
                        {
                            return Json(new{ success = false, type = "kullaniciAdi", message = "Kullanici Adi Zaten Alınmış " }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = Control, JsonRequestBehavior.AllowGet });

                    }


                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes (optional).
                    // You can use a logging framework like log4net, NLog, or Serilog for this purpose.


                    // Return HTTP Bad Request (400) with an error message.
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
                }
            }
        }
     
    }
}