using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;               // (User, UserDetails, Complaint, Address, Wishlist, etc.)
using Domain.Interfaces;            // IUnitOfWork
using SahaflarPazari.Security;      // MyAuthorization vb. attributeler
using SahaflarPazari.Models;
using Microsoft.AspNet.Identity;
using Infrastructure.Identity;
using Microsoft.AspNet.Identity.Owin;        // ValidateControl vs. eğer varsa

namespace SahaflarPazari.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationUserManager _userManager;
        public ProfileController(IUnitOfWork unitOfWork, ApplicationUserManager applicationUserManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = applicationUserManager;
        }

       

        // ------------------------------------------------------------------
        // [Sikayet POST] -> Bir kitap için şikayet oluşturur
        // ------------------------------------------------------------------
        [MyAuthorization(Roles = "User")]
        [HttpPost]
        public async Task<ActionResult> Sikayet(int id, string options)
        {
            string userId = User.Identity.GetUserId();
            
            // Şikayet entity oluştur
            var newComplaint = new Complaint
            {
                BookId = id,
                UserId = userId,
                ComplaintContent = options,
                Date = DateTime.Now
            };

      
            _unitOfWork.Complaints.AddAsync(newComplaint);
            await _unitOfWork.CommitAsync();
             
            // ya da direkt olarak kullanıcı mesajı yollama işlemi yapılabilir
            return View();
        }

        // ------------------------------------------------------------------
        // [Index GET] -> Profil anasayfası, istek listesi vb.
        // ------------------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        public async Task<ActionResult> Index()
        {
            var user= _userManager.FindById(User.Identity.GetUserId());

            var UserModel = new UserModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Wishlists = (await _unitOfWork.Wishlists.GetWishlistsByUserIdAsync(user.Id)).ToList(),
                ShoppingCarts= (await _unitOfWork.ShoppingCarts.GetShoppingCartsByUserIdAsync(user.Id)).ToList(),
                Addresses= (await _unitOfWork.Addresses.GetAddressesByUserIdAsync(user.Id)).ToList(),
            };
            
            return View(UserModel);
        }

        //------------------------------------------------------------------
        //[UpdateUserInfo] -> Kullanıcı detaylarını(Ad, Soyad, Email, Telefon) günceller
        //------------------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        public async Task<ActionResult> UpdateUserInfo(UserInfoModel userDetails)
        {
            if (!ModelState.IsValid)
            {
                var fieldeErrors = GetFieldErrors(ModelState);
                return Json(
                    new
                    {
                        success = false,
                        fieldeErrors,
                        message = "Update failed"
                    });
            }
            var userId= User.Identity.GetUserId();
            var user= _userManager.FindById(userId);

            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

            user.PhoneNumber = userDetails.Phone;
            user.Email = userDetails.Email;
            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Json(new { success = false, message = "Güncelleme Başarısız" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
        }

         //------------------------------------------------------------------
         //[UpdateLoginInfo] -> Kullanıcı adı + Şifre güncelleme
         //------------------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        public async Task<ActionResult> UpdateLoginInfo(LoginModel userModel)
        {

            if (!ModelState.IsValid) 
            {
                var fieldErrors = GetFieldErrors(ModelState);
                return Json(
                    new
                    {
                        success= false,
                        fieldErrors,
                        message = "Update failed"
                    });
            }

            // Kullanıcı adı aynı olan degerler kontrolu
            var duplicateControle = await _userManager.FindByNameAsync(userModel.userName);
            if (duplicateControle != null && duplicateControle.Id != User.Identity.GetUserId())
            {
                return Json(new
                {
                    success = false,
                    message = "Kullanici Adi Zaten Alınmış",

                }, JsonRequestBehavior.AllowGet);

            }
            // Şu an oturumdaki user
            string userId= User.Identity.GetUserId();
            var dbUser = await _userManager.FindByIdAsync(userId);
            if (dbUser == null)
            {
                return Json(new { success = false, message = "Mevcut kullanıcı bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

            // Update
            dbUser.UserName = userModel.userName;
            var passwordHasher= new PasswordHasher();   
            var hashedPassword= passwordHasher.HashPassword(userModel.password);
            dbUser.PasswordHash = hashedPassword;

            var updateResult= await _userManager.UpdateAsync(dbUser);
            if (!updateResult.Succeeded)
            {
                return Json(new { success = true, message = "Güncelleme Başarısız" }, JsonRequestBehavior.AllowGet);

            }

            var signInManager= HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            await signInManager.SignInAsync(dbUser, isPersistent: false, rememberBrowser: false);

            return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
        }

        // ------------------------------------------------------------------
        // [UpdateList POST] -> İstek listesinden kitap ekle/çıkar
        // ------------------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        [HttpPost]
        public async Task<ActionResult> UpdateList(int kitapId)
        {
           

            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

            // O kitap istek listesinde var mı kontrol et
            var existingWishlist = await _unitOfWork.Wishlists.FindAsync(w => w.BookId == kitapId && w.UserId == user.Id);
            var item = existingWishlist.FirstOrDefault();
            if (item != null)
            {
                // Sildik
                await _unitOfWork.Wishlists.DeleteAsync(item.ListId);
                await _unitOfWork.CommitAsync();
                return Json(new { success = true, message = "Kitap listeden kaldırıldı" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Ekle
                var newWishlist = new Wishlist
                {
                    BookId = kitapId,
                    UserId = user.Id
                };
                _unitOfWork.Wishlists.AddAsync(newWishlist);
                await _unitOfWork.CommitAsync();

                return Json(new { success = true, message = "Kitap istek listesine eklendi" }, JsonRequestBehavior.AllowGet);
            }
        }

        // ------------------------------------------------------------------
        // [CalculateFavorCount] -> İstek listesi sayısı
        // ------------------------------------------------------------------
        [MyAuthorization(Roles = "User")]
        public async Task<int> CalculateFavorCount(string id)
        {
            var wishlist = await _unitOfWork.Wishlists.FindAsync(w => w.UserId == id);
            return wishlist.Count();
        }

        // ------------------------------------------------------------------
        // [UpdateAdressInfo] -> Adres güncelleme
        // ------------------------------------------------------------------
        [MyAuthorization(Roles = "User")]
        public async Task<ActionResult> UpdateAddressInfo(AddressViewModel addressModel, int AddressId)
        {
            // Adres var mı?
            var dbAddress = await _unitOfWork.Addresses.GetByIdAsync(AddressId);
            if (dbAddress == null)
            {
                return Json(new { success = false, message = "Adres Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

            // Güncelle
            dbAddress.AddressName = addressModel.AddressName;
            dbAddress.AddressArea = addressModel.AddressArea;
            dbAddress.City = addressModel.City;
            dbAddress.District = addressModel.District;
            dbAddress.Neighborhood = addressModel.Neighborhood;
            dbAddress.PostalCode = addressModel.PostalCode;

            _unitOfWork.Addresses.UpdateAsync(dbAddress);
            await _unitOfWork.CommitAsync();

            return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
        }

        // ------------------------------------------------------------------
        // [DeleteAdress] -> Adres silme
        // ------------------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        public async Task<ActionResult> DeleteAddress(int id)
        {
            var dbAddress = await _unitOfWork.Addresses.GetByIdAsync(id);
            if (dbAddress == null)
            {
                return Json(new { success = false, message = "Adres Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

            await _unitOfWork.Addresses.DeleteAsync(dbAddress.AddressId);
            await _unitOfWork.CommitAsync();

            return Json(new { success = true, message = "Adres Silindi" }, JsonRequestBehavior.AllowGet);
        }

        // ------------------------------------------------------------------
        // [AddAdress] -> Yeni Adres ekleme
        // ------------------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        public async Task<ActionResult> AddAddress(AddressViewModel addressModel)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return Json(new { success = false, message = "Kullanıcı Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

            // Adres ekle
            var addressEntity = new Address
            {
                UserId = userId,
                AddressName = addressModel.AddressName,
                City = addressModel.City,
                District = addressModel.District,
                Neighborhood = addressModel.Neighborhood,
                PostalCode = addressModel.PostalCode,
                AddressArea = addressModel.AddressArea
            };
            _unitOfWork.Addresses.AddAsync(addressEntity);
            await _unitOfWork.CommitAsync();

            return Json(new { success = true, message = "Adres Eklendi" }, JsonRequestBehavior.AllowGet);
        }

        private Dictionary<string, List<string>> GetFieldErrors(ModelStateDictionary modelState)
        {
            var errorsDict = new Dictionary<string, List<string>>();

            foreach (var states in modelState)
            {
                // key usually matches the property, e.g. "UserName", "Email", etc.
                var state = states.Value;
                var stateKey = states.Key;
                var fieldErrors = state.Errors.Select(e => e.ErrorMessage).
                    Where(msg => !string.IsNullOrEmpty(msg)).
                    ToList();

                if (fieldErrors.Any())
                    errorsDict[states.Key] = fieldErrors;
            }
            return errorsDict;
        }
    }
}
