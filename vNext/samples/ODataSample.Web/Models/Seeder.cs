using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ODataSample.Web.Controllers;

namespace ODataSample.Web.Models
{
	public class Seeder
	{
		public static void EnsureDatabase(IApplicationBuilder app)
		{
			using (var context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>())
			{
				context.Database.EnsureCreated();
				// Add Mvc.Client to the known applications.
				var productsCrud = new CrudBase<Product, int>(
					context, context.Products, p => p.ProductId);
				var customersCrud = new CrudBase<Customer, int>(
					context, context.Customers, p => p.CustomerId);

				var productId = 0;
				Action<string, double, int> prod = (name, price, customerId) =>
				{
					productsCrud.EnsureEntity(
						++productId, product =>
						{
							product.Name = name;
							product.Price = price;
							product.CustomerId = customerId;
						});
				};
				var currentCustomerId = 0;
				Action<string, string> cust = (firstName, lastName) =>
				{
					customersCrud.EnsureEntity(
						++currentCustomerId, customer =>
						{
							customer.FirstName = firstName;
							customer.LastName = lastName;
						});
				};

				cust("Harry", "Whitburn");
				cust("Nick", "Lawden");
				context.SaveChanges();
				prod("Apple number1", 10, 1);
				prod("Apple number1", 10, 1);
				prod("Orange number1", 20, 2);
				prod("Peanut butter number1", 25, 2);
				prod("xApple number2", 10, 1);
				prod("xOrange number2", 20, 2);
				prod("xPeanut butter number2", 25, 2);
				prod("xApple number2", 10, 1);
				prod("xOrange number2", 20, 2);
				prod("xPeanut butter number2", 25, 2);
				prod("xApple number2", 10, 1);
				prod("xOrange number2", 20, 2);
				prod("xPeanut butter number2", 25, 2);
				prod("xApple number2", 10, 1);
				prod("xOrange number2", 20, 2);
				prod("xPeanut butter number2", 25, 2);
				prod("Apple number3", 10, 1);
				prod("Orange number3", 20, 2);
				prod("Peanut butter number3", 25, 2);
				prod("Apple number4", 10, 1);
				prod("Orange number4", 20, 2);
				prod("Peanut butter number4", 25, 2);
				prod("Apple number5", 10, 1);
				prod("Orange number5", 20, 2);
				prod("Peanut butter number5", 25, 2);
				prod("Apple number6", 10, 1);
				prod("Orange number6", 20, 2);
				prod("Peanut butter number6", 25, 2);
				context.SaveChanges();
			}
		}
	}
}