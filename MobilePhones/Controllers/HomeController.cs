using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MobilePhones.Models;
using MobilePhones.Services;
using Stripe;

namespace MobilePhones.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MobileContext _context;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ViewToStringRendererService _viewToStringRendererService;


        public HomeController(ILogger<HomeController> logger, MobileContext context, 
            IStringLocalizer<HomeController> localizer, ViewToStringRendererService viewToStringRendererService)
        {
            _logger = logger;
            _context = context;
            _localizer = localizer;
            _viewToStringRendererService = viewToStringRendererService;

        }

        public async Task<IActionResult> Index()
        {
            

            ViewData["Title"] = _localizer["Header"];
            ViewData["Message"] = _localizer["Message"];

            string html = "";

            if (true)
            {
                html = _viewToStringRendererService.RenderViewToString("/Views/Samples/LoginForm.cshtml");
            }
            else
            {
                html = _viewToStringRendererService.RenderViewToString("/Views/Samples/EmptyBlock.cshtml");
            }

            ViewBag.MyHTML = html;


            return View(await _context.Phones.ToListAsync());
        }
        [HttpPost]
        public IActionResult Processing(string stripeToken, string stripeEmail)
        {
            Dictionary<string, string> Metadata = new Dictionary<string, string>();
            Metadata.Add("Product", "RubberDuck");
            Metadata.Add("Quantity", "10");
            var options = new ChargeCreateOptions
            {
                Amount = 456,
                Currency = "USD",
                Description = "Buying 10 rubber ducks",
                Source = stripeToken,
                ReceiptEmail = stripeEmail,
                Metadata = Metadata
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string GetCulture(string code = "")
        {
            if (!String.IsNullOrEmpty(code))
            {
                CultureInfo.CurrentCulture = new CultureInfo(code);
                CultureInfo.CurrentUICulture = new CultureInfo(code);
            }
            return $"CurrentCulture: {CultureInfo.CurrentCulture.Name}, CurrentUICulture: {CultureInfo.CurrentUICulture.Name}";
        }

    }
}
