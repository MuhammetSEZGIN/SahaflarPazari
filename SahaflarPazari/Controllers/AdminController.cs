//using Domain.Entities;
//using Domain.Interfaces;
//using Microsoft.Ajax.Utilities;
//using SahaflarPazari.Models;
//using SahaflarPazari.Security;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Security;
//using System.Web.UI;

//namespace SahaflarPazari.Controllers
//{
//    [Authorize]
//    public class AdminController : Controller
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        public AdminController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        [MyAuthorization(Roles = "Admin")]
//        public async Task<ActionResult> Kullanicilar()
//        {
//            var allUsers = await _unitOfWork.Users.GetAllAsync();
//            ViewBag.Kullanici = allUsers;
//            ViewBag.Kişisel = await _unitOfWork.UserDetails.GetAllAsync();
//            return View();
//        }

//        public void DeletePictureFromFiles(List<BookImage> kitapResimleri)
//        {

//            string rootFolder = Server.MapPath("~/Content/img/Kitap_Resimleri");

//            string fullPath = "";
//            foreach (var resim in kitapResimleri)
//            {
//                fullPath = Path.Combine(rootFolder, resim.ImagePath);
//                if (System.IO.File.Exists(fullPath))
//                {
//                    System.IO.File.Delete(fullPath);
//                }
//            }


//        }

//        // sonradan tamamlanması gerekiyor
//        [MyAuthorization(Roles = "Admin")]
//        public ActionResult KullaniciSil(int? id)
//        {

//            if (!id.HasValue)
//                return Json(new { success = false, message = "Kullanici Id Bulunamadi" }, JsonRequestBehavior.AllowGet);

//            var user = _unitOfWork.Users.GetByIdAsync(id.Value);

//            if (user == null)
//                return Json(new { success = false, message = "Kullanici Bulunamadi" }, JsonRequestBehavior.AllowGet);



//            return Json(new { success = true, message = "Kullanici Silindi" }, JsonRequestBehavior.AllowGet);

//        }


//        public async Task<ActionResult> Sikayetler()
//        {
//            var copmlaints = await _unitOfWork.Complaints.GetAllAsync();

//            if (copmlaints.Any())
//            {
//                ViewBag.message = "Sikayet Bulunmamakta";
//                return View();
//            }
//            return View(copmlaints);
//        }
//    }
//}