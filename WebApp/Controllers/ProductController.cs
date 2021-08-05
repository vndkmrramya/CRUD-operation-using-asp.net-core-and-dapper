using Dapper;
using Dapper.CRUD.DAL.Interface;
using Dapper.CRUD.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDatabaseStrategy _databaseStrategy;
        public ProductController(IDatabaseStrategy databaseStrategy)
        {
            _databaseStrategy = databaseStrategy;
        }
        // GET: ProductController
        public ActionResult Index()
        {
            using(var connection = _databaseStrategy.Connection)
            {
                //***using inline query***
                //var productModel = connection.Query<ProductModel>(@"SELECT prod.*, cat.Name as [Category]
                //                   FROM Products prod
                //                   INNER JOIN Categories cat
                //                   ON prod.CategoryId = cat.CategoryId
                //                   ORDER BY prod.ProductId");
                
                //using stored procedure
                var procedure = "[spGetAllProduct]";
                var productModel = connection.Query<ProductModel>(procedure, null, commandType: CommandType.StoredProcedure).ToList();
                return View(productModel);
            }
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                using (var connection = _databaseStrategy.Connection)
                {
                    //***using inline query***
                    //var productModel = connection.QuerySingleOrDefault<ProductModel>(@"SELECT prod.*, cat.Name as [Category]
                    //               FROM Products prod
                    //               INNER JOIN Categories cat
                    //               ON prod.CategoryId = cat.CategoryId AND prod.ProductId = @ProductId
                    //               ORDER BY prod.ProductId", new { ProductId = id });

                    //***using stored procedure****
                    var procedure = "[spGetProductDetailsByProductId]";
                    var values = new { ProductId = id };
                    var productModel = connection.QuerySingleOrDefault<ProductModel>(procedure, values, commandType: CommandType.StoredProcedure);
                    return View(productModel);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        // GET: ProductController/Create
        public ActionResult Create()
        {
            using(var connection = _databaseStrategy.Connection)
            {
                ViewBag.Categories = connection.Query<Category>("SELECT CategoryId, Name FROM Categories");
            }
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //***using inline query***
                    //using (var connection = _databaseStrategy.Connection)
                    //{
                    //    _ = connection.Execute(@"INSERT INTO Products([Name], [Description], [UnitPrice], 
                    //                            [CategoryId])
                    //        Values(@Name, @Description, @UnitPrice, @CategoryId)", model);
                    //}

                    //***using stored procedure****
                    using (var connection = _databaseStrategy.Connection)
                    {
                        var procedure = "[spAddProduct]";
                        var values = new { Name = model.Name, Description = model.Description, UnitPrice = model.UnitPrice, CategoryId = model.CategoryId };
                        connection.Execute(procedure, values, commandType: CommandType.StoredProcedure);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                throw;                
            }
            using (var connection2 = _databaseStrategy.Connection)
            {
                ViewBag.Categories = connection2.Query<Category>("SELECT CategoryId, Name FROM Categories");
            }
            return View();
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            using (var connection = _databaseStrategy.Connection)
            {
                //***using inline query***
                //var data = connection.QuerySingleOrDefault<Product>(@"Select * from Products 
                //                            where ProductId=@ProductId", new { ProductId = id });

                //***using function***
                var data = connection.QuerySingleOrDefault<Product>(@"Select * from dbo.[fn_GetProductsByProductId](@ProductId)", new { ProductId = id });
                ViewBag.Categories = connection.Query<Category>("SELECT CategoryId, Name FROM Categories");
                return View("Create", data);
            }
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var connection = _databaseStrategy.Connection)
                    {
                        //***using inline query***
                        _ = connection.Execute(@"Update Products SET [Name] = @Name,
                                        [Description] = @Description,
                                        [UnitPrice] = @UnitPrice,
                                        [CategoryId] = @CategoryId
                                  WHERE ProductId = @ProductId", model);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                
            }
            using (var connection = _databaseStrategy.Connection)
            {
                ViewBag.Categories = connection.Query<Category>("SELECT CategoryId, Name FROM Categories");
            }
            return View("Create", model);
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                using (var connection = _databaseStrategy.Connection)
                {
                    //***using inline query***
                    var product = connection.Query<Product>(@"select * from Products where ProductId=@ProductId", new { ProductId = id });
                    if (product != null)
                    {
                        _ = connection.Execute(@"delete from Products 
                                                where ProductId=@ProductId", new { ProductId = id });

                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

    }
}
