using Microsoft.Ajax.Utilities;
using SahaflarPazari.Models;
using SahaflarPazari.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Domain.Entities;           // Book, BookImage, User vs. Domain Entities
using Domain.Interfaces;
using Microsoft.AspNet.Identity;
using System.Runtime.InteropServices;        // IUnitOfWork

namespace SahaflarPazari.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    
        // ---------------------------------------------------------------
        // [MyBooks] -> Kullanıcının kitaplarını listeler
        // ---------------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult> MyBooks()
        {
            string userId = User.Identity.GetUserId();
            
            var MyBooksViewModel = new MyBookPageViewModel
            {
                Books = (await _unitOfWork.Books.FindAsync(b => b.SellerId == userId)).ToList(),
                Categories = (await _unitOfWork.BookCategories.GetAllAsync()).ToList(),
                Publishers = (await _unitOfWork.Publishers.GetAllAsync()).ToList()
            };
            return View(MyBooksViewModel);
                       
        }

        // ---------------------------------------------------------------
        // [UpdateBook] -> Bir kitaba ait bilgileri günceller
        // ---------------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        [HttpPost]
        public async Task<ActionResult> UpdateBook(Book kitap)
        {
            var UserId = User.Identity.GetUserId();
            var existingBook = await _unitOfWork.Books.GetByIdAsync(kitap.BookId);
            if (existingBook == null)
            {
                return Json(new { success = false, message = "Güncellenecek Kitap Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

            existingBook.BookName = kitap.BookName;
            existingBook.Price = kitap.Price;
            existingBook.CategoryId = kitap.CategoryId;
            existingBook.PublisherId = kitap.PublisherId;
            existingBook.Description = kitap.Description;
            existingBook.Author = kitap.Author;
            existingBook.SellerId = UserId;

            _unitOfWork.Books.UpdateAsync(existingBook);
            await _unitOfWork.CommitAsync();

            return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
        }

        // ---------------------------------------------------------------
        // [DeleteBook] -> Belirli bir kitabı siler
        // ---------------------------------------------------------------
        
        [MyAuthorization(Roles = "User,Admin")]
        public async Task<ActionResult> DeleteBook(int? id)
        {
            if (!id.HasValue)
            {
                return Json(new { success = false, message = "Kitap ID Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

            var book = await _unitOfWork.Books.GetByIdAsync(id.Value);
            if (book == null)
            {
                return Json(new { success = false, message = "Kitap Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

            // 1) Kitap Resimleri
            var bookImages = (await _unitOfWork.BookImages.GetBookImagesByBookIdAsync(book.BookId)).ToList();
            // Silmeden önce fiziksel dosyaları da silelim
            DeletePicture(bookImages.ToList());
            // DB'den sil  
             _unitOfWork.BookImages.DeleteBookImagesAsync(bookImages);

            // 2) İstek Listesi
            var wishlistItems = await _unitOfWork.Wishlists.FindAsync(w => w.BookId == book.BookId);
            foreach (var w in wishlistItems)
            {
                await _unitOfWork.Wishlists.DeleteAsync(w.ListId);
            }

            // 3) Alışveriş Sepeti
            var cartItems = await _unitOfWork.ShoppingCarts.FindAsync(c => c.BookId == book.BookId);
            foreach (var c in cartItems)
            {
                await _unitOfWork.ShoppingCarts.DeleteAsync(c.CartId);
            }

            // 4) Şikayetler
            var complaints = await _unitOfWork.Complaints.FindAsync(s => s.BookId == book.BookId);
            foreach (var complaint in complaints)
            {
                await _unitOfWork.Complaints.DeleteAsync(complaint.ComplaintId);
            }

            // 5) Siparişler
            var orders = await _unitOfWork.Orders.FindAsync(o => o.BookId == book.BookId);
            foreach (var o in orders)
            {
                await _unitOfWork.Orders.DeleteAsync(o.OrderId);
            }

            // 6) Son olarak kitabı sil
            await _unitOfWork.Books.DeleteAsync(book.BookId);

            // Değişiklikleri kaydet
            await _unitOfWork.CommitAsync();

            return Json(new { success = true, message = "Kitap Silindi" }, JsonRequestBehavior.AllowGet);
        }
        
        // ---------------------------------------------------------------
        // [AddBook GET] -> Kitap ekleme sayfası
        // ---------------------------------------------------------------


        [MyAuthorization(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult> AddBook()
        {
            var categories = await _unitOfWork.BookCategories.GetAllAsync();
            var publishers = await _unitOfWork.Publishers.GetAllAsync();

            ViewBag.yayinevi = publishers;
            ViewBag.kategori = categories;
            return View(categories);
        }

        // ---------------------------------------------------------------
        // [AddBook POST] -> Kitabı DB'ye kaydeder ve resmi ekler
        // ---------------------------------------------------------------
        [MyAuthorization(Roles = "User")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> AddBook(Book model, HttpPostedFileBase file)
        {
           
            if (model == null || file == null)
            {
                return Json(new { success = false, message = "Eksik veri: Kitap veya dosya boş" }, JsonRequestBehavior.AllowGet);
            }
            var userId = User.Identity.GetUserId(); 
            // Yeni kitap oluştur
            var newBook = new Book
            {
                BookName = model.BookName,
                SellerId = userId,
                Price = model.Price,
                PublisherId = model.PublisherId,  // (YayineviId)
                CategoryId = model.CategoryId,
                Description = model.Description,   // (Detay)
                Author = model.Author,        // (Yazar)
                AddedDate = DateTime.Now         // (EklenmeTarihi)
            };

            // EF ekle
            _unitOfWork.Books.AddAsync(newBook);

            // Resim tabloya ekleme
            var bookImage = new BookImage
            {
                BookId = newBook.BookId, // SaveChanges sonrası ID alabilir, 
                ImagePath = Path.GetFileName(file.FileName)
            };
             _unitOfWork.BookImages.AddBookImages(bookImage);


            // Resim kopyalama
            string path = Server.MapPath("~/Content/img/Kitap_Resimleri/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Fiziksel kaydet
            string savePath = Path.Combine(path, file.FileName);
            file.SaveAs(savePath);

            // Tüm değişiklikleri tek seferde kaydet
            await _unitOfWork.CommitAsync();

            return Json(new 
            { 
                success = true, 
                message = "Kitap Ekleme Başarılı",
                returnUrl= "/Book/MyBooks"

            }, JsonRequestBehavior.AllowGet);



        }

        // ---------------------------------------------------------------
        // [DeletePicture] -> Resimleri diskten siler
        // ---------------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        public void DeletePicture(List<BookImage> kitapResimleri)
        {
            if (kitapResimleri == null || !kitapResimleri.Any())
                return;

            string rootFolder = Server.MapPath("~/Content/img/Kitap_Resimleri");
            foreach (var resim in kitapResimleri)
            {
                string fullPath = Path.Combine(rootFolder, resim.ImagePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }
    }
}
