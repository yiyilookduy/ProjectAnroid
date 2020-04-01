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
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    var context = new FaceIOContext();
                    var result = context.Users.Where(q => q.Username == username && q.Password == password).FirstOrDefault();
                    if (result != null)
                    {

                        return Ok(new BaseResponse(result, "", true));
                    }

                    return NotFound(new BaseResponse(result, "Invalid username or password", false));
                }

                return NotFound(new BaseResponse(null, "Username and password cannot be blank", false));


            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
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