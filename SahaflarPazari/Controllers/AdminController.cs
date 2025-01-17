using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Identity;
using Microsoft.Ajax.Utilities;
using SahaflarPazari.Models;
using SahaflarPazari.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace SahaflarPazari.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationUserManager _userManager;
        public AdminController(IUnitOfWork unitOfWork, ApplicationUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager; 
        }

        [MyAuthorization(Roles = "Admin")]
        public async Task<ActionResult> Users()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var users = new List<UserModel>();
            foreach (var user in allUsers) { 
                users.Add(new UserModel
                {
                    UserId=user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                  
                });
            }
            return View(users);
        }

        public void DeletePictureFromFiles(List<BookImage> kitapResimleri)
        {

            string rootFolder = Server.MapPath("~/Content/img/Kitap_Resimleri");

            string fullPath = "";
            foreach (var resim in kitapResimleri)
            {
                fullPath = Path.Combine(rootFolder, resim.ImagePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }


        }

        // sonradan tamamlanması gerekiyor
        [MyAuthorization(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string id)
        {

            if (string.IsNullOrEmpty(id))
                return Json(new { success = false, message = "Kullanici Id Bulunamadi" }, JsonRequestBehavior.AllowGet);

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return Json(new { success = false, message = "Kullanici Bulunamadi" }, JsonRequestBehavior.AllowGet);
            await _userManager.DeleteAsync(user);
           
            return Json(new { success = true, message = "Kullanici Silindi" }, JsonRequestBehavior.AllowGet);

        }


        public async Task<ActionResult> Complaints()
        {
            var copmlaints = await _unitOfWork.Complaints.GetAllAsync();
            if (copmlaints.Any())
            {
                ViewBag.message = "Sikayet Bulunmamakta";
                return View();
            }
            var Complaints = new List<ComplaintsViewModel>();
            foreach(var complaint in copmlaints)
            {
                Complaints.Add(new ComplaintsViewModel
                {
                    ComplaintText = complaint.ComplaintContent,
                    ComplaintDate = complaint.Date.ToString(),
                    BookName = complaint.Book.BookName,
                    BookOwner = (await _userManager.FindByIdAsync(complaint.Book.SellerId)).UserName,
                    ComplaintUserName = (await _userManager.FindByIdAsync(complaint.UserId)).UserName,

                });
            }
           
            return View(copmlaints);
        }
    }
}