﻿using System;
using System.Collections.Generic;
using System.Data;
using ASPNET.Models;
using Dapper;

namespace ASPNET
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _conn;

        public ProductRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _conn.Query<Product>("SELECT * FROM Products;");
        }

        public Product GetProduct(int id)
        {
            return _conn.QuerySingle<Product>("SELECT * from Products " +
                "WHERE ProductID = @id;", new {id});
        }

        public void UpdateProduct(Product product)
        {
            _conn.Execute("UPDATE Products SET Name=@name, Price=@price, StockLevel=@stockLevel WHERE ProductID=@productID",
                new
                {
                    productID = product.ProductID,
                    name = product.Name,
                    price = product.Price,
                    stockLevel = product.StockLevel,
                }); 
        }

        public void InsertProduct(Product productToInsert)
        {
            _conn.Execute("INSERT INTO Products (Name, Price, CategoryID) VALUES (@name, @price, @categoryID);",
            new
            {
                name = productToInsert.Name,
                price = productToInsert.Price,
                categoryID = productToInsert.CategoryID,
                onSale = productToInsert.OnSale,
                stockLevel = productToInsert.StockLevel,
            });
        }

        public IEnumerable<Category> GetCategories()
        {
            return _conn.Query<Category>("SELECT * FROM Categories;");
        }

        public Product AssignCategory()
        {
            var categoryList = GetCategories();
            var product = new Product();
            product.Categories = categoryList;

            return product;
        }

        public void DeleteProduct(Product product)
        {
            _conn.Execute("DELETE FROM Reviews WHERE ProductID = @id;",new {id = product.ProductID});
            _conn.Execute("DELETE FROM Sales WHERE ProductID = @id;", new { id = product.ProductID });
            _conn.Execute("DELETE FROM Products WHERE ProductID = @id;", new { id = product.ProductID });
        }
    }
}
