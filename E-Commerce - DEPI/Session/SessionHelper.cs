using E_Commerce___DEPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace E_Commerce___DEPI.Session
{
	public static class SessionHelper
	{
		private const string UserInfoKey = "UserId";

		private static DbIntities PrivateContext = new DbIntities();

		public static void SetUser(Controller controller, User user)
		{
			controller.HttpContext.Session.SetInt32(UserInfoKey, user.Id);
		}

		public static void DeleteUser(Controller controller)
		{
			controller.HttpContext.Session.Remove(UserInfoKey);
		}

		public static User? GetUser(Controller controller, DbIntities context = null)
		{
			if (context == null)
				context = PrivateContext;

			if (context != null)
			{
				int? id = controller.HttpContext.Session.GetInt32(UserInfoKey);
				if (id != null && id > 0)
					return context.Customers.FirstOrDefault(x => x.Id == id);
			}
			return null;
		}

		public static User? GetUser(IHttpContextAccessor accessor)
		{
			if (PrivateContext != null)
			{
				int? id = accessor.HttpContext.Session.GetInt32(UserInfoKey);
				if (id != null && id > 0)
					return PrivateContext.Customers.FirstOrDefault(x => x.Id == id);
			}
			return null;
		}

		public static bool IsLoggedIn(Controller controller, DbIntities context = null, bool reqadmin = false)
		{
			return IsLoggedIn(GetUser(controller, context), reqadmin);
		}

		public static bool IsLoggedIn(User? user, bool reqadmin = false)
		{
			if (user != null)
			{
				if (!reqadmin || user.IsAdmin)
					return true;
			}
			return false;
		}
	}
}
