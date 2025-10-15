using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training2Api.models;

namespace Training2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        public FoodFestDbContext db = new FoodFestDbContext();

        [HttpGet("/api/categories")]
        public List<Category> getCategory()
        {
            return db.Categories.ToList();
        }

        //[HttpGet("/api/attendee")]
        //public List<Attendee> getAttendee()
        //{
        //    return db.Attendees.ToList();
        //}

        [HttpGet("/api/attendee")]
        public List<Attendee> getAttendee(string name)
        {
            //contains -> LIKE "%value%"
            return db.Attendees.Where(x => x.FullName.Contains(name))
                .OrderBy(x => x.FullName)

                .ToList();
        }


        [HttpGet("/api/{adminId}/attendee/{id}")]
        public IActionResult getAttendee(int adminId, int id)
        {
            var isAdmin = db.Admins.Any(x => x.IsActive && x.Id == adminId);
            if (!isAdmin)
            {
                return Unauthorized();
            }

            //select a.id, a.fullname, a.email, c.name
            //from Attendees as a join Category as c on a.id = c.id
            //limit 1;


            var attendee = db.Attendees.Include(x => x.Category)
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    AttendeeEmail = x.Email,
                    CategoryName = x.Category.Name,
                })
                .FirstOrDefault(x => x.Id == id);


            return Ok(attendee);


        }



        [HttpPost("/api/attendee")]
        public IActionResult postAttendee([FromBody] Attendee attendee)
        {
            bool isEmailExisit = db.Attendees.Any(x => x.Email == attendee.Email);
            if (isEmailExisit)
            {
                return BadRequest("Your email is taken");
            }
            db.Attendees.Add(attendee);
            db.SaveChanges();
            return Ok(attendee.Id);
        }

        [HttpGet("getSum")]
        public IActionResult getSum()
        {
            var sum = db.Attendees.Sum(x => x.Id);//price
            var avg = db.Attendees.Average(x => x.Id);//price
            var count = db.Attendees.Count();//price
            var max = db.Attendees.Max(x => x.Id);//price
            var min = db.Attendees.Min(x => x.Id);//price

            //var total = 0;
            //for (int i = 0; i < 10; i++)
            //{
            //    total += i;
            //}
            return Ok(new
            {
                sum,
                avg,
                count,
                max,
                min
            });
        }

        [HttpPut("/api/attendee/{id}")]
        public IActionResult updateAttendee([FromBody] Attendee attendee, long id)
        {
            if (attendee.Id != id)
            {
                return BadRequest("Id not same");
            }

            bool isEmailExisit = db.Attendees.Any(x => x.Email == attendee.Email
                                && x.Id != id);

            if (isEmailExisit)
            {
                return BadRequest("Your email is taken");
            }


            db.Entry(attendee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return Ok(attendee);
        }

        public class loginDto
        {
            public string username { get; set; }

            public string password { get; set; }
        }

        [HttpPost("/api/login")]
        public IActionResult login(loginDto req)
        {
            var admin = db.Admins.FirstOrDefault(x => x.Username == req.username
                        && x.Password == req.password
                        );

            if (admin == null)
            {
                return Unauthorized("Wrong username or password");
            }

            if (!admin.IsActive)
            {
                return Unauthorized("Inactive now");

            }

            return Ok(new { message = "Login Success", AdminId = admin.Id });
        }

        [HttpGet("/api/setInactive/{id}")]
        public IActionResult setInactive(long id)
        {
            // var admin = db.Admins.FirstOrDefault(x => x.Id == id);
            var admin = db.Admins.Find(id);

            admin.IsActive = false;
            db.SaveChanges();

            return Ok($"Admin {admin.Username} has been blocked");
        }

        [HttpGet("/api/setActive/{id}")]
        public IActionResult setActive(long id)
        {
            // var admin = db.Admins.FirstOrDefault(x => x.Id == id);
            var admin = db.Admins.Find(id);

            admin.IsActive = true;
            db.SaveChanges();

            return Ok($"Admin {admin.Username} has been blocked");
        }


    }
}
