using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceApi2.ModelAPIs;
using FaceApi2.Models;
using Microsoft.AspNetCore.Mvc;


namespace FaceApi2.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost("/Home/Login")]
        public IActionResult Login(string username, string password)
        {
            try
            {
                var context = new FaceIOContext();
                var result = context.Users.Where(q => q.Username == username && q.Password == password).FirstOrDefault();
                if (result != null)
                {

                    return Ok(new BaseResponse(result, "", true));
                }
                else
                {
                    return NotFound(new BaseResponse(result, "Invalid username or password", false));
                }
            }
            catch (Exception e)
            {
                return Ok();
            }
        }

        [HttpPost("/Home/Register")]
        public IActionResult Register(Users user)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return Ok();
            }
        }

    }
}