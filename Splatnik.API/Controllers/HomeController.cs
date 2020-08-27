using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Splatnik.API.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet("api/test")]
		public IActionResult Index()
		{
			return Ok(new { name = "elo"});
		}
	}
}
