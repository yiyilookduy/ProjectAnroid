using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceApi2.ModelAPIs;
using FaceApi2.Models;
using Microsoft.AspNetCore.Mvc;

namespace FaceApi2.Controllers
{
    public class ProfileController : Controller
    {
        [HttpGet("/Profile/GetProfile")]
        public IActionResult GetProfile(string username)
        {
            CheckValid valid = new CheckValid();
            try
            {
                valid.IsExistedUsername(username);

                if (valid.IsValid)
                {
                    FaceIOContext context = new FaceIOContext();

                    var user = context.Users.Where(x => x.Username == username).FirstOrDefault();

                    if (user != null)
                    {
                        return Ok(new BaseResponse(user, "Get profile success", true));
                    }

                    return NotFound(new BaseResponse(null, "Not found", false));
                }

                return NotFound(new BaseResponse(null, "Username not found", false));
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }
        }

        [HttpGet("/Profile/GetProfiles")]
        public IActionResult GetProfiles(string name)
        {
            try
            {
                FaceIOContext context = new FaceIOContext();

                var users = context.Users.Where(x => x.Fullname.Contains(name)).ToList();

                if (users.Count > 0)
                {
                    return Ok(new BaseResponse(users, "Get profiles success", true));
                }

                return NotFound(new BaseResponse(null, "Not found", false));
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }
        }

        [HttpPost("/Profile/UpdateProfile")]
        public IActionResult UpdateProfile(Users user)
        {
            CheckValid valid = new CheckValid();
            
            try
            {
                valid.IsExistedUsername(user.Username);
                if (valid.IsValid)
                {
                    FaceIOContext context = new FaceIOContext();

                    context.Users.Update(user);

                    context.SaveChanges();

                    return Ok(new BaseResponse(user, "Update success", true));
                }

                return NotFound(new BaseResponse(null, "User not found", false));

            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }
        }


    }
}