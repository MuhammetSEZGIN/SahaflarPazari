using SahaflarPazari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace SahaflarPazari.Security
{
    public class ValidateControl
    {
        public string PasswordControl(string sifre)
        {
            if (string.IsNullOrEmpty(sifre))
            {
                return "Şifre Boş Olamamaz";
            }
            if (sifre.Length < 6)
            {
                return "Şifre 6 karakterden kısa olamaz";
            }
            if (!sifre.Any(char.IsUpper) || !sifre.Any(char.IsLower))
            {
                return "Şifre en az bir büyük ve küçük harf içermeli.";
            }
            if (!sifre.Any(char.IsDigit))
            {
                return "Şifre en az bir sayı içermeli.";
            }
            if (!sifre.Any(c => !char.IsLetterOrDigit(c)))
            {
                return "Şifre en az bir özel karakter içermeli.";
            }
            return string.Empty;
        }
       
        /*
        public bool UserNameControl(string userName, SahaflarPazariEntities db)
        {

            List<string> KullaniciAdi = db.Kullanici.Where(x => x.KullaniciAdi == userName).Select(x=>x.KullaniciAdi).ToList();
            if(KullaniciAdi.Contains(userName))
            {
                return false;
            }
            else
            {
                return true;
            }
                

        }
        */
       
    }
}