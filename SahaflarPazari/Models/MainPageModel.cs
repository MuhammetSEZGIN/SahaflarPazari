using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SahaflarPazari.Models
{
    public class MainPageModel
    {
        
        public  List<Kitap>  kitap{ get; set; } 
        public List<int?> istekListesiKitaplari { get; set; }
        
        public List<Kitap> enSonEklenenler { get; set; }

    }
    public class LoginModel
    {
        public string Ad { get; set; }
        public string Sifre { get; set; }
    }

    public class RegisterModel
    {
        public string KullaniciAdi { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }

        public string Sifre { get; set; }
        public string Telefon { get; set; }
        public string Eposta { get; set; }

    }

    public class UsersBook
    {
        public Kullanici kullanici { get; set; }
        public List<Kitap> kitaplar { get; set; }

    }
    public class AddBookModel
    {
        
        public int KitapId { get; set; }    
        public string KitapAdi { get; set; }
        public int SaticiId { get; set; }
        public int Fiyat { get; set; }
        public int YayineviId { get; set; }
        public int KategoriId { get; set; }
        public string Detay { get; set; }
        public string Yazar { get; set; }

    
    }
    
}