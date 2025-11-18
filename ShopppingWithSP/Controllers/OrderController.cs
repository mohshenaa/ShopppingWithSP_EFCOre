using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Selfcare_meets_Beautify.Services;
using ShopppingWithSP.Models;
using System;
using System.Data;

namespace ShopppingWithSP.Controllers
{
	[Authorize]
	public class OrderController : Controller
	{
		private readonly ShoppingWithSP_DB db;
		private readonly IUploadService _uploadService;

		public OrderController(ShoppingWithSP_DB context, IUploadService uploadService)
		{
			db = context;
			_uploadService = uploadService;
		}

		// GET: Order/Index
		public async Task<IActionResult> Index()
		{
			var orders = await db.Order
				.Include(o => o.Details)
				.ToListAsync();
			return View(orders);
		}

		// POST: Order/Index
		[HttpPost]
		public async Task<IActionResult> Index(int orderId, Status status)
		{
			
			var order = await db.Order.FindAsync(orderId);
		
			if (order != null)
			{
				if (order.OrderStatus == Status.Cancel)
				{
					TempData["Success"] = "Cancelled Order can't be updated!";
					return RedirectToAction(nameof(Index));
				}
                if (order.OrderStatus == Status.Delivered)
                {
                    TempData["Success"] = "Delivered Order can't be updated!";
                    return RedirectToAction(nameof(Index));
                }


                order.OrderStatus = status;
				await db.SaveChangesAsync();
				TempData["Success"] = "Order status updated successfully!";
			}
			else
			{
				TempData["Error"] = "Order not found!";
			}

		
			return RedirectToAction(nameof(Index));
		}
		// GET: Order/Create
		public IActionResult Create()
		{
			var order = new Order
			{
				Details = new List<Details> { new Details() }
			};

			return View(order);
		}

		// POST: Order/Create
		[HttpPost]
		public async Task<IActionResult> Create(Order order, string operation = "")
		{

			if (operation == "AddItem")
			{
				//order.OrderDetails ??= new List<OrderDetails>();
				order.Details.Add(new Details());

				ModelState.Clear();
				return View(order);
			}


			if (operation.StartsWith("DeleteItem"))
			{
				var index = int.Parse(operation.Replace("DeleteItem-", ""));

				order.Details?.RemoveAt(index);
				ModelState.Clear();

				return View(order);
			}

			if (ModelState.IsValid)
			{

				foreach (var item in order.Details)
				{
					if (item.ImageFile?.Length > 0)
						item.ImageUrl = await _uploadService.FileSave(item.ImageFile);
				}


				//if (!order.OrderDetails.Any())
				//{
				//    ModelState.AddModelError("", "Please add at least one order item.");
				//    return View(order);
				//}


				await ExecuteAddOrderSP(order);
				return RedirectToAction("Index");
			}

			return View(order);
		}


		// GET: Order/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var order = await db.Order
				.Include(o => o.Details)
				.FirstOrDefaultAsync(o => o.OrderId == id);

			if (order == null)
				return NotFound();

			return View(order);
		}

		// POST: Order/Edit/5
		[HttpPost]
		public async Task<IActionResult> Edit(int id, Order order, string operation = "")
		{
			if (id != order.OrderId)
				return BadRequest();

			if (operation == "AddItem")
			{
				//order.OrderDetails ??= new List<OrderDetails>();
				order.Details.Add(new Details());

				ModelState.Clear();
				return View(order);
			}


			if (operation.StartsWith("DeleteItem"))
			{
				var index = int.Parse(operation.Replace("DeleteItem-", ""));

				order.Details?.RemoveAt(index);
				ModelState.Clear();

				return View(order);
			}

			if (ModelState.IsValid)
			{
				try
				{
					foreach (var item in order.Details)
					{
						if (item.ImageFile?.Length > 0)
						{

							item.ImageUrl = await _uploadService.FileSave(item.ImageFile);
						}
						else if (item.ItemCode > 0)
						{

							var existingDetail = await db.OrderDetails
								.AsNoTracking()
								.FirstOrDefaultAsync(d => d.ItemCode == item.ItemCode);

							if (existingDetail != null)
							{
								item.ImageUrl = existingDetail.ImageUrl;
							}
						}
					}

					await db.Database.ExecuteSqlRawAsync(
				  "delete from OrderDetails where OrderId = {0}", order.OrderId);

					db.Order.Update(order);
					await db.SaveChangesAsync();

					return RedirectToAction("Index");
				}
				catch
				{
					ModelState.AddModelError("", "Unable to save changes.");
				}
			}

			return View(order);
		}


		//Get:Order/Details/5
		public async Task<IActionResult> Details(int id)
		{
         

         var order =await db.Order.Include(e => e.Details).FirstOrDefaultAsync
                (e => e.OrderId == id);

			if (order == null)
			{
				return NotFound();
			}
            foreach (var detail in order.Details)
            {
                Console.WriteLine($"Detail: {detail.Item}, ImageUrl: {detail.ImageUrl}");
            }
            return View(order);
		}

		// GET: Order/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var order = await db.Order
				.Include(o => o.Details)
				.FirstOrDefaultAsync(o => o.OrderId == id);

			if (order == null)
				return NotFound();

			return View(order);
		}

		// POST: Order/Delete/5
		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var order = await db.Order
				.Include(o => o.Details)
				.FirstOrDefaultAsync(o => o.OrderId == id);

			if (order == null)
			{
				return NotFound();
			}

			
			db.Order.Remove(order);
			await db.SaveChangesAsync();

			return RedirectToAction("Index");
		}

		// Helper: Execute AddOrderWithDetails SP
		private async Task ExecuteAddOrderSP(Order order)
		{
			var orderDetailsTable = new DataTable();
			orderDetailsTable.Columns.Add("Item", typeof(string));
			orderDetailsTable.Columns.Add("Size", typeof(string));
			orderDetailsTable.Columns.Add("Price", typeof(decimal));
			orderDetailsTable.Columns.Add("Description", typeof(string));
			orderDetailsTable.Columns.Add("ImageUrl", typeof(string));

			foreach (var detail in order.Details)
			{
				orderDetailsTable.Rows.Add(detail.Item, detail.Size, detail.Price, detail.Description, detail.ImageUrl);
			}

			var parameters = new[]
			{
				new SqlParameter("@CustomerName", order.CustomerName),
				new SqlParameter("@CustomerContact", order.CustomerContact),
				new SqlParameter("@CustomerEmail", order.CustomerEmail),
				new SqlParameter("@CustomerAddress", order.CustomerAddress),
				new SqlParameter("@OrderStatus", (int)order.OrderStatus),
				new SqlParameter("@OrderDetails", orderDetailsTable)
				{
					SqlDbType = SqlDbType.Structured,
					TypeName = "dbo.OrderDetailsType"
				}
			};

			await db.Database.ExecuteSqlRawAsync(
				"EXEC dbo.sp_AddOrderWithDetails @CustomerName, @CustomerContact, @CustomerEmail, @CustomerAddress, @OrderStatus, @OrderDetails",
				parameters);
		}
	}
}
