using DatabaseBusinessDLL;
using DatabaseBusinessDLL.Interfaces;
using FinanceProjectDLL;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceWebProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(DatabaseContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.Users.Select(u => new User
            {
                Id = u.Id,
                UserName = u.UserName,
                Password = u.Password
            }).ToListAsync();
        }

        [HttpGet("GetUsersByCount/{count}")]
        public async Task<ActionResult<List<User>>> GetUsersByCount(int count)
        {
            var toReturnList = await _context.Users.ToListAsync();

            return toReturnList.Take(count).Adapt<List<User>>();
        }

        // [HttpGet("SearchSongsByTitle/{title},{count}")]
        // public async Task<List<User>> SearchSongsByTitle(string title, int count)
        // {
        //     var toSortList = await _context.Users.ToListAsync();

        //     var output = _controllerFunctions.SearchByTitle(toSortList, title, count);

        //     return output.Adapt<List<User>>();
        // }

        // [HttpGet("GetSongsByProperty/{count}")]
        // public async Task<List<User>> GetSongsByProperty(int count, bool sortByTitle, bool sortByLength, bool sortByLocation, bool sortByInterpretName)
        // {
        //     var toSortList = await _context.Users.ToListAsync();

        //     if (sortByTitle)
        //     {
        //         toSortList = toSortList.OrderBy(s => s.Title).ToList();
        //     }
        //     else if (sortByLength)
        //     {
        //         toSortList = toSortList.OrderBy(s => s.Length).ToList();
        //     }
        //     else if (sortByLocation)
        //     {
        //         toSortList = toSortList.OrderBy(s => s.Location).ToList();
        //     }
        //     else if (sortByInterpretName)
        //     {
        //         toSortList = toSortList.OrderBy(s => s.InterpretFullName).ToList();
        //     }

        //     return toSortList.Take(count).Adapt<List<User>>();
        // }

        // [HttpGet("UserPagination/{page},{itemsPerPage}")]
        // public async Task<ActionResult<List<User>>> UserPagination(int page, int itemsPerPage)
        // {
        //     var toSortList = await _context.Users.ToListAsync();

        //     var output = _controllerFunctions.Pagination(toSortList, itemsPerPage, page);

        //     return output.Adapt<List<User>>();
        // }

        // [HttpGet("IsPageLastPage/{page},{pageSize}")]
        // public async Task<ActionResult<bool>> IsPageLastPage(int page, int pageSize)
        // {
        //     var songs = await _context.Users.ToListAsync();
        //     var toReturnBool = false;

        //     if (songs.Count / pageSize <= page)
        //     {
        //         toReturnBool = true;
        //     }

        //     return toReturnBool;
        // }

        // [HttpGet("GetPageCount/{pageSize}")]
        // public async Task<ActionResult<int>> GetPageCount(int pageSize)
        // {
        //     var songList = await _context.Users.ToListAsync();

        //     return songList.Count / pageSize;
        // }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user.Adapt<User>();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user.Adapt<User>()).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _unitOfWork.Add(user);
            await _unitOfWork.BeginTransactionAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _unitOfWork.Remove(user);
            await _unitOfWork.BeginTransactionAsync();
            System.Console.WriteLine("User Deleted");

            return NoContent();
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
