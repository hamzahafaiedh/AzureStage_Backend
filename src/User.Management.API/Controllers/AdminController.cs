using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using User.Management.API.Models;
using User.Management.API.Models.Authentication.SignUp;
using User.Management.API.Models.CRUD;

namespace User.Management.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]   
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Test authorisation
        [HttpGet("Users")]
        public IEnumerable<string> Get()
        {
            return new List<string> { "Rayen", "Hamza", "Ali" };
        }

        /*[HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "User deleted successfully" });
                else
                    StatusCode(StatusCodes.Status404NotFound,
                new Response { Status = "Error", Message = "User is not deleted" });
            }
            else
            return StatusCode(StatusCodes.Status404NotFound,
                new Response { Status = "Error", Message = "User not found" });
        }*/

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var Getuser = new GetUserModel
                {
                    UserName = user.UserName,
                    Email = user.Email
                };
                return Ok(Getuser);
            }
            else
            {
                // User not found, return 404 Not Found
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var allUsers = _userManager.Users.Select(user => new GetUserModel
            {
                UserName = user.UserName,
                Email = user.Email
            }).ToList();

            return Ok(allUsers);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.UserName = model.NewUsername;
                user.Email = model.NewEmail; 
                user.TwoFactorEnabled = model.NewTwoFactorEnabled;

                // Change password if provided
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resultP = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                    if (!resultP.Succeeded)
                    {
                        return BadRequest(resultP.Errors);
                    }
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // User updated successfully
                    // You can add additional logic here if needed
                    return Ok();
                }
                else
                {
                    // Handle the errors
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                // User not found, return 404 Not Found
                return NotFound();
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                // User not found
                return NotFound();
            }
        }








    }
    
}
