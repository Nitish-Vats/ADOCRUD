using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Entities;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        IConfiguration _configuration;
        ProductRepository _productRepository;
        CategoryRepository _categoryRepository;
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            _productRepository = new ProductRepository(_configuration.GetConnectionString("DbConnection")); 
            _categoryRepository = new CategoryRepository(_configuration.GetConnectionString("DbConnection"));
        }
        public IActionResult Index()
        {
            var products = _productRepository.GetProducts();
            return View(products);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryRepository.GetCategories(); 
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product model)
        {
            ModelState.Remove("ProductId");//Itisanoptionalfieldforcreation,soremovedit
            if(ModelState.IsValid) 
            { 
                _productRepository.AddProduct(model); 
                return RedirectToAction("Index");//productHomepage
            } 
            ViewBag.Categories=_categoryRepository.GetCategories(); 
            return View(); 
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _categoryRepository.GetCategories(); 
            Product model = _productRepository.GetProductById(id); 
            return View("Create", model);//CallCreateViewandpassmodel
        } 
        [HttpPost] 
        public IActionResult Edit(Product model) 
        { 
            if(ModelState.IsValid) 
            { 
                _productRepository.UpdateProduct(model); 
                return RedirectToAction("Index");//productHomepage
            } 
            ViewBag.Categories=_categoryRepository.GetCategories(); 
            return View(); 
        }
        public IActionResult Delete(int id) 
        { 
            _productRepository.DeleteProduct(id); 
            return RedirectToAction("Index");//GototheListingPage
        }
    }
}
