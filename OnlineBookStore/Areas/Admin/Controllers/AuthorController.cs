﻿using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Administrator")]
    public class AuthorController : Controller
    {
        private ApplicationDbContext _context;  
  
        public AuthorController(ApplicationDbContext context)  
        {  
            _context = context;  
        }  
        // GET: /<controller>/  
        public IActionResult Index()  
        {  
            return View();  
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var author = _context.Authors.FirstOrDefault(o => o.Id == id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                _context.SaveChanges();
            }
            return Ok(true);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Details(Author author)
        {
            try
            {
                _context.Update(author); 
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "There is something wrong while saving item.");
                return View();
            }
            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            ViewBag.IsDetail = true;
            return View(_context.Authors.Find(id));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.IsDetail = false;
            return View("Details",_context.Authors.Find(id));
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.IsDetail = false;
            return View("Details",new Author());
        }
        
        [HttpPost]
        public IActionResult LoadData()  
        {  
            // try  
            // {  
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();  
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();  
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();  
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();  
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();  
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();  
  
                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;  
                int skip = start != null ? Convert.ToInt32(start) : 0;  
                int recordsTotal = 0;  
  
                // Getting all Author data  
                var authorData = (from tempaAuthor in _context.Authors
                    select new{Id=tempaAuthor.Id.ToString(),Name=tempaAuthor.Name,CreationDate=tempaAuthor.CreationDate.ToString(CultureInfo.InvariantCulture)} );  
  
                //Sorting  
                // if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))  
                // {  
                //     authorData = authorData.OrderBy(sortColumn , sortColumnDirection);  
                // }  
                //Search  
                if (!string.IsNullOrEmpty(searchValue))  
                {  
                    authorData = authorData.Where(m => m.Name.Contains(searchValue));  
                }  
  
                //total number of rows count   
                recordsTotal = authorData.Count();  
                //Paging   
                var data = authorData.Skip(skip).Take(pageSize).ToList();  
                //Returning Json Data  
                var x =Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }),"application/json");
                return x;

                // }  
                // catch (Exception)  
                // {  
                //     throw;  
                // }  

        }  
    }  
    
}
