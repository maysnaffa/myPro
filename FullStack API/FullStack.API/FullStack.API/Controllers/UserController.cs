using Docker.DotNet.Models;
using FullStack.API.Data;
using FullStack.API.Helper;
using FullStack.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace FullStack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EmployeesSiteDbContext _authDbContext;   
        public UserController(EmployeesSiteDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        { 
         
            if (userObj == null)
              return BadRequest();
           
            var user = await _authDbContext.Users
                .FirstOrDefaultAsync(x => x.UserName == userObj.UserName);

           
            
            if(user == null) 
            return NotFound(new { Message = "user is not found !" });


            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "password is Incorrect "});            
            
            }

            return Ok(new { Message = "LogIn Success !" });




        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User userObj)
        { 
         
            if(userObj==null) return BadRequest();


            //check username 
            if (await CheckUserNameExistAsync(userObj.UserName))
             
              return BadRequest(new {message =" Username is Already Exist"});



            // check email

            if (await CheckUserEmailExistAsync(userObj.Email))

                return BadRequest(new { message = " Email is Already Exist" });



            // checkpasswordStrength

            var pass = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass.ToString() });


            userObj.Password=PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";

            await  _authDbContext.Users.AddAsync(userObj);
            await _authDbContext.SaveChangesAsync();
            return Ok( new
            {
                message = "User Register"
            }  );
          
        
        }


        //this 
      /*  private async  Task<bool> CheckUserNameExistAsync(string username)
        {
            return await  _authDbContext.Users.AnyAsync(x => x.UserName == username);

        
        } */

        //or this
        private Task<bool> CheckUserNameExistAsync(string username)

           => _authDbContext.Users.AnyAsync(x => x.UserName == username);


        private Task<bool> CheckUserEmailExistAsync(string email)

           => _authDbContext.Users.AnyAsync(x => x.Email == email);


        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
              sb.Append("Minimum Password lenght should be 8 " + Environment.NewLine);


            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") &&
                Regex.IsMatch(password, "[0-9]")))
                sb.Append("password should be Alphanumeric" + Environment.NewLine);

            if (!(Regex.IsMatch(password, "[<,>,@,!,#,$,^,&,*,(,),_,+,-,=,|.,{,},~,`,?,:]")))
                sb.Append(" password shoul contain  epecial Chars" + Environment.NewLine);
         
                return sb.ToString();
        }
    }
}
