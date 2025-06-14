using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using ChatApplicationCoreANDReact.Common;
using ChatApplicationCoreANDReact.Models;
using Domain.Entities;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplicationCoreANDReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly IUserMessagesService _UserMessagesService;

        public UserController(IUserService UserService, IUnitOfWorkAsync unitOfWorkAsync, IConfiguration config, IWebHostEnvironment env, IUserMessagesService userMessagesService)
        {
            _UserService = UserService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _config = config;
            _env = env;
            _UserMessagesService = userMessagesService;
        }

        //Authorization: Bearer YOUR_TOKEN

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            try
            {
                // Check if user already exists
                var existingUser = _UserService.Queryable().FirstOrDefault(e => e.Email == model.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "User already registered." });
                }

                var newUser = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Image = model.Image,
                    Password = model.Password,
                    CreatedDate = DateTime.Now
                };

                _UserService.Insert(newUser);
                _unitOfWorkAsync.SaveChanges();

                var userData = new JWTModel
                {
                    UserID = newUser.UserID,
                    Image = newUser.Image,
                    UserName = newUser.UserName,
                    Email = newUser.Email
                };

                var role = new List<string>();
                var token = JWTManager.GenerateJwtToken(userData, role, _config);

                return Ok(new { message = "Registration successful", token , userData });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

                //return BadRequest(new ProblemDetails
                //{
                //    Title = "Validation Failed",
                //    Status = 400,
                //    Detail = "Check the input fields.",
                //    Instance = HttpContext.Request.Path
                //});
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO Model)
        {
            var user = _UserService.Queryable().Where(e => e.Email == Model.Email && e.Password == Model.Password).FirstOrDefault();
            if (user != null)
            {
                var userData = new JWTModel()
                {
                    UserID = user.UserID,
                    Image = user.Image,
                    UserName = user.UserName,
                    Email = user.Email
                };

                var role = new List<string>();
                var token = JWTManager.GenerateJwtToken(userData, role, _config);
                return Ok(new { token, userData });
            }
            return Unauthorized();
        }


        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            //Ensure you have a wwwroot folder in your project root If you're not using wwwroot, set it manually in Program.cs -- builder.WebHost.UseWebRoot("wwwroot");
            var webRootPath = _env.WebRootPath ?? _env.ContentRootPath;
            var uploadsPath = Path.Combine(webRootPath, "Files", "UserProfileIcon");

           // var uploadsPath = _config["ProfilefilePath"];

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var originalFileName = file.FileName;

            var FileName = $"{timestamp}_{originalFileName}";
            var filePath = Path.Combine(uploadsPath, FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { filePath, fileName = FileName });
        }


        [Authorize]
        [HttpGet("getCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var UserID = User.GetUserId();
            
            if (!string.IsNullOrEmpty(UserID) && int.TryParse(UserID, out int userId))
            {
                var userData = _UserService.GetCurrentUser(userId);
                return Ok(new { userData });
            }
            else
            {
                // Handle invalid or missing user ID
                return Unauthorized("Invalid or missing User ID");
            }
        }


        [Authorize]
        [HttpGet("getAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = _UserService.GetALLUser();
            return Ok(new { users });
        }


        [Authorize]
        [HttpGet("getChatMessages")]
        public ActionResult GetChatMessages(long senderId, long receiverId)
        {

            var messages = _UserMessagesService.Queryable()
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) || (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.Timestamp)
                .ToList();

            var Sender = _UserService.Find(senderId);
            var recipient = _UserService.Find(receiverId);

            var messageList = messages.Select(m => new MessageViewDTO
            (
                Id : m.Id,
                SenderId : m.SenderId,
                SenderName : m.SenderId == senderId ? "You" : Sender.UserName,
                ReceiverName : m.ReceiverId == receiverId ? "You" : recipient.UserName,
                Message : m.Content,
                IsSender : m.SenderId == senderId,
                Timestamp : m.Timestamp.ToLocalTime().ToString()
            )).ToList();

            return Ok(new {messageList});
        }

    }
}
