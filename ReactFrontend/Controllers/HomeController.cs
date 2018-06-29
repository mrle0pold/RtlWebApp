using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace ReactFrontend.Controllers
{
    public class HomeController : Controller
    {
        public QueryHandler QueryHandler { get; set; }

        public HomeController(QueryHandler queryHandler)
        {
            QueryHandler = queryHandler;
        }

        public async Task<IActionResult> GetShows(Query query)
        {
            var result = await QueryHandler.Handle(query);
            // to json
            return Json(result);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}

