using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SahaflarPazari.Security;
using Domain.Interfaces;       // IUnitOfWork
using Domain.Entities;
using Microsoft.AspNet.Identity;        // User, ShoppingCart, Order, Address, Book, etc.

namespace SahaflarPazari.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShopCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // -----------------------------------------------------------
        // [ShopCart GET] -> Alışveriş sepeti görüntüleme
        // -----------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult> ShopCart()
        {
           
            // Sepet itemleri
            var userId = User.Identity.GetUserId();    
            var sepet = (await _unitOfWork.ShoppingCarts.FindAsync(x => x.UserId == userId)).ToList();
            if (sepet==null)
            {
                return Json(new { success = false, message = "Siparis Listesi Boş" }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.SepetItem = sepet;

            int total = sepet.Count();
            Session["ItemCount"] = total;

            return View();
        }

        // -----------------------------------------------------------
        // [AddToCart POST] -> Sepete kitap ekle
        // -----------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        [HttpPost]
        public async Task<ActionResult> AddToCart(int id)
        {


            var userId = User.Identity.GetUserId();
            // Kullanıcının sattığı kitapları çekip, eklenen kitap onlara ait mi kontrol et
            var userBooks = await _unitOfWork.Books.FindAsync(b => b.SellerId == userId);
            bool isUserBook = userBooks.Any(b => b.BookId == id);
            if (isUserBook)
            {
                return Json(new { success = false, message = "Kitap Zaten Size Ait" });
            }

            // Sepete ekle
            var newCartItem = new ShoppingCart
            {
                BookId = id,
                UserId = userId
            };
            _unitOfWork.ShoppingCarts.AddAsync(newCartItem);
            await _unitOfWork.CommitAsync();

            // Sepet sayısını yeniden hesapla
            int newCount = await CalculateBooksCount(userId);

            return Json(new { success = true, message = "Kitap Sepete Eklendi", BookCount = newCount });
        }

        // -----------------------------------------------------------
        // [RemoveFromCart] -> Sepetten kitap çıkart
        // -----------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        public async Task<ActionResult> RemoveFromCart(int? id)
        {
            if (!id.HasValue)
            {
                return Json(new { success = false, message = "Sepet ID Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }
            var userId = User.Identity.GetUserId(); 
            // Toplam kitap sayısı / Toplam fiyat
            int Count = await CalculateBooksCount(userId);
            int Sum = await CalculateSum(userId);

            // AlışverişSepeti kaydını bul
            var cartItem = await _unitOfWork.ShoppingCarts.GetByIdAsync(id.Value);
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Sepet item bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

            // Book fiyatını sum'dan düş
            // Kitabı bul
            var book = await _unitOfWork.Books.GetByIdAsync(cartItem.BookId);
            if (book != null)
            {
                Sum -= book.Price;
            }

            // Kaldır
            await _unitOfWork.ShoppingCarts.DeleteAsync(cartItem.CartId);
            await _unitOfWork.CommitAsync();

            // Güncel sepet sayısı
            int newCount = Count - 1;

            return Json(new
            {
                success = true,
                message = "Kitap Sepetten Kaldırıldı",
                BookCount = newCount,
                sum = Sum
            }, JsonRequestBehavior.AllowGet);
        }

        // -----------------------------------------------------------
        // [ChooseAdres GET] -> Kullanıcının adreslerini seçme
        // -----------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        public async Task<ActionResult> ChooseAdres()
        {
            var userId= User.Identity.GetUserId();
            var addresses = await _unitOfWork.Addresses.FindAsync(a => a.UserId == userId);
            return View(addresses);
        }

        // -----------------------------------------------------------
        // [Orders GET] -> Kullanıcının siparişlerini göster
        // -----------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult> Orders()
        {
            var userId = User.Identity.GetUserId();
            var orders = await _unitOfWork.Orders.FindAsync(o => o.UserId == userId);
            return View(orders);
        }

        // -----------------------------------------------------------
        // [Orders POST] -> Seçilen adresle, sepetteki kitaplardan sipariş oluştur
        // -----------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        [HttpPost]
        public async Task<ActionResult> Orders(int? id)
        {
         

            var address = await _unitOfWork.Addresses.GetByIdAsync(id.GetValueOrDefault());
            if (address == null)
            {
                return new HttpStatusCodeResult(400, "Adres Bulunamadı");
            }
            var userId = User.Identity.GetUserId();

            // Kullanıcının sepeti
            var cartItems = await _unitOfWork.ShoppingCarts.FindAsync(c => c.UserId == userId);
            var orders = new List<Order>();
            DateTime nowLocal = DateTime.Now;

            foreach (var item in cartItems)
            {
                orders.Add(new Order
                {
                    UserId = userId,
                    BookId = item.BookId,
                    Address = address.AddressName, // ya da address.ToString()
                    OrderDate = nowLocal,
                    OrderStatus = "Hazırlanıyor"
                });
            }

            foreach (var order in orders)
            {
                _unitOfWork.Orders.AddAsync(order);
            }

            await _unitOfWork.CommitAsync();

            // Tüm sepeti temizlemek istersen:
            foreach (var item in cartItems)
            {
                await _unitOfWork.ShoppingCarts.DeleteAsync(item.CartId);
            }
            await _unitOfWork.CommitAsync();

            return RedirectToAction("Orders", "ShopCart");
        }

        // -----------------------------------------------------------
        // [SoldItems GET] -> Kullanıcının sattığı kitaplardan gelen siparişler
        // -----------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        public async Task<ActionResult> SoldItems()
        {
            var userId = User.Identity.GetUserId();
            var orders = (await _unitOfWork.Orders.FindAsync(o => o.Book.SellerId == userId)).ToList();
            return View(orders);
        }

        // -----------------------------------------------------------
        // [Durum GET] -> Sipariş durumunu güncelle
        // -----------------------------------------------------------
        public async Task<ActionResult> Durum(int id, string durum)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return new HttpStatusCodeResult(400, "Sipariş Bulunamadı");
            }

            order.OrderStatus = durum;
            _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.CommitAsync();

            return View();
        }

        // -----------------------------------------------------------
        // [CalculateBooksCount] -> Kullanıcının sepetindeki kitap sayısı
        // -----------------------------------------------------------
        [MyAuthorization(Roles = "User,Admin")]
        public async Task<int> CalculateBooksCount(string id)
        {
            var cartItems = await _unitOfWork.ShoppingCarts.FindAsync(x => x.UserId == id);
            return cartItems.Count();
        }

        // -----------------------------------------------------------
        // [CalculateSum] -> Kullanıcının sepetindeki kitapların toplam fiyatı
        // -----------------------------------------------------------
        public async Task<int> CalculateSum(string id)
        {
            // AlışverişSepeti + Book join
            var cartItems = await _unitOfWork.ShoppingCarts.FindAsync(s => s.UserId == id);

            int totalPrice = 0;
            foreach (var item in cartItems)
            {
                var book = await _unitOfWork.Books.GetByIdAsync(item.BookId);
                if (book != null)
                {
                    totalPrice += book.Price;
                }
            }
            return totalPrice;
        }
    }
}
