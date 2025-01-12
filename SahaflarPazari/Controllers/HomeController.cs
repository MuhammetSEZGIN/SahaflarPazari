using Domain.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SahaflarPazari.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SahaflarPazari.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
       

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {


            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var book = await _unitOfWork.Books.GetByIdAsync(id.Value);

            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);

        }

        // GET: Kitap
        [HttpGet]

        public async Task<ActionResult> Index()
        {
            string username = User.Identity.GetUserName();

            var allBooks = await _unitOfWork.Books.GetAllAsync();
            var latestBooks = allBooks
                .OrderByDescending(k => k.AddedDate)  // EF property => Book.AddedDate
                .ToList();

            var model = new MainPageModel
            {
                books = allBooks.ToList(),
                lastAddedBooks = latestBooks
            };

            if (!string.IsNullOrEmpty(username))
            {
                var userId = User.Identity.GetUserId(); 
                var wishListBooks = await _unitOfWork.Wishlists.FindAsync(x => x.UserId == userId);
                model.wishListBooks = wishListBooks.Select(x => x.BookId ?? 0).ToList();

                // Alışveriş sepetindeki ürün sayısı
                var cartItems = await _unitOfWork.ShoppingCarts.FindAsync(c => c.UserId == userId);
                Session["ItemCount"] = cartItems.Count();
                
            }

            return View(model);
        }



    }
}