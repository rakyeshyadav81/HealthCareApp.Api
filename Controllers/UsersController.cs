using HeakthCareApp.DataServices.Data;
using HealthCareApp.Entities.DbSet;
using HealthCareApp.Entities.DbSet.Incoming;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace HealthCareApp.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        //Get
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _db.Users.Where(x => x.Status == 1).ToList();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto user)
        {
            
            var _user = new User();
            _user.FirstName = user.FirstName;
            _user.LastName = user.LastName;
            _user.Email = user.Email;
            _user.Phone = user.Phone;
            _user.DateOfBirth = Convert.ToDateTime(user.DateOfBirth);
            _user.Country = user.Country;
            _user.Status = 1;

            _db.Users.Add(_user);
            _db.SaveChanges();

            var response = new
            {
                sucess = true,
                StatusCode = 201,
                message = "user added successfully",
                result = user
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(Guid id)
        {
            //var getUser = _db.Users.FirstOrDefault(x => x.Id == id);
            var getUser = from x in _db.Users where x.Id == id && x.Status == 1 select x;
            return Ok(getUser);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(Guid id, UserDto user)
        {
            var findUser = _db.Users.FirstOrDefault(x => x.Id == id);
            
            if(findUser == null)
            {
                return BadRequest("User Not Found");
            }
            else
            {
                findUser.FirstName = user.FirstName;
                findUser.LastName = user.LastName;
                findUser.Email = user.Email;
                findUser.Phone = user.Phone;
                findUser.DateOfBirth = Convert.ToDateTime(user.DateOfBirth);
                findUser.Country = user.Country;

                _db.SaveChanges();

                var response = new
                {
                    sucess = true,
                    StatusCode = 202,
                    message = "user updated successfully",
                    result = user
                };
                return Ok(response);

            }
            
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public IActionResult DeleteUser(Guid id)
        {
            var findUser = _db.Users.FirstOrDefault(x => x.Id == id);
            if (findUser == null) return BadRequest("User Not Found");

            else
            {
                
               // user.Status = 2;
                _db.Users.Remove(findUser);
                _db.SaveChanges();

                return Ok("User Deleted Sucessfully"); 
            }


        }
    }
}
