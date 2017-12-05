using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vyayaam.Data;
using Vyayaam.Services;
using Vyayaam.ViewModels;

namespace Vyayaam.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IVyayaamRepository repository;
        //private readonly VyayaamContext _context;

        //Changed to use repository instead of _context
        //public AppController(IMailService mailService, VyayaamContext context)
        //{
        //    this._mailService = mailService;
        //    this._context = context;
        //}
        public AppController(IMailService mailService, IVyayaamRepository repository)
        {
            this._mailService = mailService;
            this.repository = repository;
            //this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            //throw new InvalidOperationException("Throwing an error!");
            return View();
        }
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            ViewBag.Title = "Contact Us";
            //throw new InvalidOperationException("Throwing an error!");
            if (ModelState.IsValid)
            {
                _mailService.SendMessage(model.Name, model.Subject, model.Message);
                ViewBag.UserMessage = "Mail sent";
                ModelState.Clear();
            }
            else
            {
                _mailService.AlertMessage(model.Name, model.Subject, model.Message);
            }
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }

        public IActionResult Shop()
        {
            //var results = from p in _context.Products
            //              orderby p.Category
            //              select p;
            //var results = _context.Products
            //    .OrderBy(p=>p.Category)
            //    .ToList();
            var results = repository.GetAllProducts();
            return View(results);
        }
    }
}
