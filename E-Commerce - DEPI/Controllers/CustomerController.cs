using Microsoft.AspNetCore.Mvc;
using E_Commerce___DEPI.Models;
using E_Commerce___DEPI.Session;
using GP.Controllers;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce___DEPI.Controllers
{
    public class CustomerController : Controller
    {
		DbIntities context;
		private readonly ILogger<CustomerController> _logger;

		public CustomerController(DbIntities _context, ILogger<CustomerController> logger)
		{
			context = _context;
			_logger = logger;
		}
        public IActionResult AddToCart(int productId)
        {
			User? user = SessionHelper.GetUser(this, context);
			if (user == null)
				return View(HomeController.LoggedInView);

			var selectedCustomer = context.Customers.FirstOrDefault(c => c.Id == user.Id);
            var selectedProduct = context.Products.FirstOrDefault(c => c.Id == user.Id);
            CartItem cartItem = new CartItem
            {
                Customer = selectedCustomer,
                Product = selectedProduct,
                Quantity = 1

            };
            context.CartItems.Add(cartItem);
            context.SaveChanges();
            return RedirectToAction("Cart", new { customerId = user.Id });

        }
        public IActionResult Cart()
        {
            User? user = SessionHelper.GetUser(this, context);
			if (user == null)
				return View(HomeController.LoggedInView);

			List<CartItem> cartItems = context.CartItems.Where(x => x.Customer.Id == user.Id).ToList();
            ViewData["cartItems"] = cartItems;
            ViewData["customerId"] = user.Id;
            return View();
        }

        public IActionResult DeleteCartItem(int cartItemId)
        {
			User? user = SessionHelper.GetUser(this, context);
			if (user == null)
				return View(HomeController.LoggedInView);

			// Retrieve the cart item by cartItemId
			var cartItem = context.CartItems.SingleOrDefault(x => x.Id == cartItemId);

            if (cartItem != null)
            {
                // Remove the item from the CartItems table
                context.CartItems.Remove(cartItem);

                // Save changes to the database
                context.SaveChanges();
            }

            // Redirect back to the Cart action with the customerId as a parameter
            return RedirectToAction("Cart", new { customerId = user.Id });
        }

        public IActionResult IncreaseItemQuantity(int cartItemId, int customerId)
        {
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

			// Retrieve the cart item by cartItemId
			var cartItem = context.CartItems.SingleOrDefault(x => x.Id == cartItemId);

            if (cartItem != null)
            {
                cartItem.Quantity += 1;

                // Save changes to the database
                context.SaveChanges();
            }

            // Redirect back to the Cart action with the customerId as a parameter
            return RedirectToAction("Cart", new { customerId = customerId });
        }

        public IActionResult DecreaseItemQuantity(int cartItemId)
        {
			User? user = SessionHelper.GetUser(this, context);
			if (user == null)
				return View(HomeController.LoggedInView);

			// Retrieve the cart item by cartItemId
			var cartItem = context.CartItems.SingleOrDefault(x => x.Id == cartItemId);

            if (cartItem != null)
            {
                cartItem.Quantity -= 1;

                // Save changes to the database
                context.SaveChanges();

                if (cartItem.Quantity == 0)
                {
                    // Redirect back to the DeleteCartItem action with the customerId as a parameter
                    return RedirectToAction("DeleteCartItem", new { customerId = user.Id, cartItemId = cartItemId });
                }
                else
                {
                    // Redirect back to the Cart action with the customerId as a parameter
                    return RedirectToAction("Cart", new { customerId = user.Id });
                }
            }
            return RedirectToAction("Cart", new { customerId = user.Id });

        }

        public IActionResult Checkout()
        {
			User? user = SessionHelper.GetUser(this, context);
			if (user == null)
				return View(HomeController.LoggedInView);

			List<CartItem> cartItems = context.CartItems.Where(x => x.Customer.Id == user.Id).ToList();
            List<ShippmentCity> shippmentCities = context.ShippmentCities.ToList();
            ViewData["shippmentCities"] = shippmentCities;
            ViewData["customerId"] = user.Id;
            double subTotal = 0;
            foreach (var item in cartItems)
            {
                subTotal += item.Product.Price * item.Quantity;
            }
            ViewData["subTotal"] = subTotal;
            return View();
        }

        public IActionResult SubmitCheckout(Address address, int subtotal)
        {
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

			var selectedCity = context.ShippmentCities.FirstOrDefault(c => c.Id == address.ShippmentCitiesId);
            var selectedCustomer = SessionHelper.GetUser(this, context);

            // get the cart items to fill the order items
            var cartItems = context.CartItems.Where(ci=>ci.Customer == selectedCustomer).ToList();

            if (selectedCity != null && selectedCustomer != null && cartItems != null && cartItems.Any())
            {
                // Save The Address
                address.ShippmentCities = selectedCity;
                context.Addresses.Add(address);
                
                // Save The Order
                var order = new Order
                {
                    CustomerId = selectedCustomer.Id,
                    Address = address,
                    Status = E_Commerce___DEPI.Models.OrderState.Pending,
                    Date= DateTime.Now,
                    total = subtotal + selectedCity.ShppmentFee
                };
                context.Orders.Add(order);
                

                // Save The Address Items
                var orderedItems = new List<OrderdItem>();
                foreach (var item in cartItems)
                {
                    OrderdItem orderdItem = new OrderdItem
                    {
                        Amount = item.Quantity,
                        Order = order,
                        Product = item.Product
                    };
                    orderedItems.Add(orderdItem);
                };
                context.OrderdItems.AddRange(orderedItems);
                // Remove the cart items from the database
                context.CartItems.RemoveRange(cartItems);
                // Commit everything to the database
                context.SaveChanges();
            }
            // Redirect to the Cart action in the desired controller
            return RedirectToAction("Index", "Home", new { page = 1, categoryPage = 1 });
        }

        public IActionResult viewCustomerProfile(int id )
        {
			if (!SessionHelper.IsLoggedIn(this, context))
				return View(HomeController.LoggedInView);

			var customer = context.Customers.FirstOrDefault(x => x.Id == id);
            return View(customer);
        }  
    }
}
