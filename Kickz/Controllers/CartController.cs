using Kickz.Infrastructure;
using Kickz.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kickz.Controllers
{
    public class CartController : Controller
    {

        private readonly ApplicationDbContext context;

        public CartController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //Index Method
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };

            return View(cartVM);
        }
        //Add Method
        public async Task<IActionResult> Add(int id)
        {
            Product product =await context.Products.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartitem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if(cartitem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartitem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            if(HttpContext.Request.Headers["x-Requested-With"] != "XMLHttpRequest")
            {
                return RedirectToAction("Index");
            }

            return ViewComponent("SmallCart");
        }

        //Decrease Method
        public IActionResult Decrease(int id)
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartitem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartitem.Quantity > 1)
            {
                --cartitem.Quantity;
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }
            
            if(cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        //Remove Method
        public IActionResult Remove(int id)
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            
            cart.RemoveAll(x => x.ProductId == id);
            

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        //Clear Cart Method
        public IActionResult Clear()
        {

            HttpContext.Session.Remove("Cart");

            if (HttpContext.Request.Headers["x-Requested-With"] != "XMLHttpRequest")
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return Ok();                
        }
    }
}
