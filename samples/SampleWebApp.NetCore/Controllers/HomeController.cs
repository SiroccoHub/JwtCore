﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using JwtCore;

namespace SampleWebApp.NetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var payload = new Dictionary<string, object>
            {
                { "claim1", 0 },
                { "claim2", "claim2-value" }
            };

            var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
            string token = JsonWebToken.Encode(payload, secretKey, JwtHashAlgorithm.HS256);

            ViewData["JWT"] = token;

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
