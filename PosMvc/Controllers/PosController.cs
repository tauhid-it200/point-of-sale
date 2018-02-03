using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PosMvc.Models;

namespace PosMvc.Controllers
{
    
    public class PosController : Controller
    {
        private ApplicationDbContext _context;

        public PosController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult Index()
        {
            return View();
        }

        [Route("pos/admin-login")]
        public ActionResult AdminLogin()
        {
            if (Session["loggedIn"] != null)
            {
                return RedirectToAction("Dashboard", "Pos");
            }

            return View();
        }

        [HttpPost]
        public ActionResult AdminLoginAttempt(string userName, string password)
        {
            if (userName != "admin")
            {
                Session["message"] = "Invalid username!";

                return RedirectToAction("AdminLogin", "Pos");
            }
            else
            {
                if (password == "12341234")
                {
                    Session["loggedIn"] = true;
                    Session["message"] = "You are now logged in!";

                    return RedirectToAction("Dashboard", "Pos");
                }
                else
                {
                    Session["message"] = "Incorrect password!";

                    return RedirectToAction("AdminLogin", "Pos");
                }
            }
        }

        [Route("pos/dashboard")]
        public ActionResult Dashboard()
        {
            return View();
        }

        [Route("pos/add-item")]
        public ActionResult AddItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewItem(Item item)
        {
            var newItem = new Item();
            newItem.Name = item.Name;
            newItem.Price = item.Price;
            newItem.Stock = item.Stock;

            _context.Items.Add(newItem);
            _context.SaveChanges();

            Session["message"] = "Item added successfully!";

            return RedirectToAction("ItemList", "Pos");
        }

        [Route("pos/items")]
        public ActionResult ItemList()
        {
            var items = _context.Items.ToList();

            return View(items);
        }

        [Route("pos/orders")]
        public ActionResult ShowOrders()
        {
            var orders = _context.BoughtItems.Include(c => c.Item).ToList();

            return View(orders);
        }

        [Route("pos/edit-item/{id}")]
        public ActionResult EditItem(int id)
        {
            var item = _context.Items.Single(c => c.Id == id);

            return View(item);
        }

        [HttpPost]
        public ActionResult UpdateItem(Item item)
        {
            var itemToUpdate = _context.Items.Single(c => c.Id == item.Id);
            if (itemToUpdate != null)
            {
                itemToUpdate.Name = item.Name;
                itemToUpdate.Price = item.Price;
                itemToUpdate.Stock = item.Stock;
                _context.SaveChanges();

                Session["message"] = "Item updated successfully!";

                return RedirectToAction("ItemList", "Pos");
            }
            else
            {
                Session["message"] = "Invalid item to update!";

                return RedirectToAction("ItemList", "Pos");
            }
        }

        [Route("pos/delete-item/{id}")]
        public ActionResult DeleteItem(int id)
        {

            var itemToDelete = _context.Items.Single(c => c.Id == id);

            return View(itemToDelete);
        }

        [Route("pos/confirm-delete/{id}")]
        public ActionResult ConfirmDelete(int id, int choice)
        {
            if (choice == 1)
            {
                var itemToDelete = _context.Items.Single(c => c.Id == id);
                _context.Items.Remove(itemToDelete);
                _context.SaveChanges();

                Session["message"] = "Item deleted successfully!";

                return RedirectToAction("ItemList", "Pos");
            }
            else
            {
                return RedirectToAction("ItemList", "Pos");
            }
        }

        [Route("pos/logout")]
        public ActionResult Logout()
        {
            Session["loggedIn"] = null;
            Session["message"] = "You have successfully logged out!";

            return RedirectToAction("Index", "Pos");
        }

        [Route("pos/home")]
        public ActionResult Home()
        {
            if (Session["loggedIn"] == null)
            {
                return RedirectToAction("ItemList", "Pos");
            }
            else
            {
                Session["message"] = "Currently you are logged in as Admin. Please logout to start shopping!";

                return RedirectToAction("Index", "Pos");
            }

        }

        [Route("pos/buy-item/{id}")]
        public ActionResult BuyItem(int id)
        {
            var item = _context.Items.SingleOrDefault(c => c.Id == id);

            return View(item);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var item = _context.Items.Single(c => c.Id == id);

            if (quantity > item.Stock)
            {
                Session["message"] = "Not enough items in the stock!";

                return RedirectToAction("BuyItem", "Pos", new {id = id});
            }

            if (Session["customerGuid"] == null)
            {
                var customerGuid = Guid.NewGuid();
                Session["customerGuid"] = customerGuid;
            }

            var cart = new List<BoughtItem>();

            if (Session["cart"] == null)
            {
                Session["cart"] = cart;
            }

            cart = (List<BoughtItem>)Session["cart"];

            if (cart.Count != 0)
            {
                foreach (var boughtItem in cart)
                {
                    if (id == boughtItem.ItemId)
                    {
                        boughtItem.Quantity = boughtItem.Quantity + quantity;

                        Session["message"] = "Item added to the cart successfully!";

                        return RedirectToAction("ItemList", "Pos");
                    }
                }

                var itemToAdd = new BoughtItem();
                itemToAdd.ItemId = id;
                itemToAdd.Item = item;
                itemToAdd.Quantity = quantity;
                itemToAdd.CustomerGuid = (Guid)Session["customerGuid"];

                cart.Add(itemToAdd);
                Session["cart"] = cart;
                Session["message"] = "Item added to the cart successfully!";

                return RedirectToAction("ItemList", "Pos");
            }
            else
            {
                var itemToAdd = new BoughtItem();
                itemToAdd.ItemId = id;
                itemToAdd.Item = item;
                itemToAdd.Quantity = quantity;
                itemToAdd.CustomerGuid = (Guid)Session["customerGuid"];

                cart.Add(itemToAdd);
                Session["cart"] = cart;
                Session["message"] = "Item added tothe cart successfully!";

                return RedirectToAction("ItemList", "Pos");
            }
        }

        [Route("pos/cart")]
        public ActionResult ShowCart()
        {
            var cart = (List<BoughtItem>)Session["cart"];

            return View(cart);
        }

        [Route("pos/checkout")]
        public ActionResult Checkout()
        {
            if (Session["cart"] == null)
            {
                Session["message"] = "Nothing to checkout!";

                return RedirectToAction("ItemList", "Pos");
            }

            var cart = (List<BoughtItem>)Session["cart"];

            if (Session["customerGuid"] == null)
            {
                Session["message"] = "You have to buy something before checking out!";

                return RedirectToAction("ItemList", "Pos");
            }

            var customerGuid = (Guid)Session["customerGuid"];

            foreach (var boughtitem in cart)
            {
                var newBoughtItem = new BoughtItem();
                newBoughtItem.CustomerGuid = customerGuid;
                newBoughtItem.ItemId = boughtitem.ItemId;
                newBoughtItem.Quantity = boughtitem.Quantity;

                var item = _context.Items.Single(c => c.Id == newBoughtItem.ItemId);
                item.Stock = item.Stock - newBoughtItem.Quantity;
                _context.BoughtItems.Add(newBoughtItem);
                _context.SaveChanges();

                Session["cart"] = null;
                Session["customerGuid"] = null;
                Session["message"] = "Your order has been placed successfully!";
            }

            return RedirectToAction("ItemList", "Pos");
        }
	}
}