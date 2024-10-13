using Microsoft.AspNetCore.Mvc;
using E_Commerce___DEPI.Models;

namespace E_Commerce___DEPI.Controllers
{
    public class CustomerController : Controller
    {
        DbIntities context = new DbIntities();
        public IActionResult Cart(int customerId)
        {
            List<CartItem> cartItems = context.CartItems.Where(x => x.Customer.Id == customerId).ToList();
            ViewData["cartItems"] = cartItems;
            ViewData["customerId"] = customerId;
            return View();
        }

        public IActionResult DeleteCartItem(int cartItemId, int customerId)
        {
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
            return RedirectToAction("Cart", new { customerId = customerId });
        }

        public IActionResult IncreaseItemQuantity(int cartItemId, int customerId)
        {
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

        public IActionResult DecreaseItemQuantity(int cartItemId, int customerId)
        {
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
                    return RedirectToAction("DeleteCartItem", new { customerId = customerId, cartItemId = cartItemId });
                }
                else
                {
                    // Redirect back to the Cart action with the customerId as a parameter
                    return RedirectToAction("Cart", new { customerId = customerId });
                }
            }
            return RedirectToAction("Cart", new { customerId = customerId });

        }

        public IActionResult Checkout(int customerId)
        {
            List<CartItem> cartItems = context.CartItems.Where(x => x.Customer.Id == customerId).ToList();
            List<ShippmentCity> shippmentCities = context.ShippmentCities.ToList();
            ViewData["shippmentCities"] = shippmentCities;
            ViewData["customerId"] = customerId;
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
            var selectedCity = context.ShippmentCities.FirstOrDefault(c => c.Id == address.ShippmentCitiesId);
            var selectedCustomer = context.Customers.FirstOrDefault(c => c.Id == address.CustomerId);
            // get the cart items to fill the order items
            var cartItems = context.CartItems.Where(ci=>ci.Customer == selectedCustomer).ToList();

            if (selectedCity != null && selectedCustomer != null && cartItems != null && cartItems.Any())
            {
                // Save The Address
                address.ShippmentCities = selectedCity;
                address.Customer = selectedCustomer;
                context.Addresses.Add(address);
                
                // Save The Order
                var order = new Order
                {
                    CustomerId = selectedCustomer.Id,
                    Address = address,
                    Status = E_Commerce___DEPI.Models.OrderState.Pending,
                    Date= DateTime.Now,
                    total= subtotal + selectedCity.ShppmentFee
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
            var customer = context.Customers.FirstOrDefault(x => x.Id == id);
            return View(customer);
        }  
    }
}
