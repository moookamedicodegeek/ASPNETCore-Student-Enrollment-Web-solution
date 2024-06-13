using ASPNETCore_DB.Data;
using ASPNETCore_DB.Interfaces;
using ASPNETCore_DB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;

namespace ASPNETCore_DB.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly SQLiteDBContext _context;
        private readonly IDBInitializer _seedDatabase;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, SQLiteDBContext context, IDBInitializer seedDatabase,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _seedDatabase = seedDatabase;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            ViewData["UserName"] = _userManager.GetUserName(this.User);
            if (this.User.IsInRole("Admin"))
            {
                ViewData["UserRole"] = "Admin";
            }
            if (this.User.IsInRole("User")) 
            {
                ViewData["UserRole"] = "User";
            }
            return View();
        }

        public IActionResult SeedDatabase()
        {
            //_seedDatabase.Initialize(_context);
            ViewBag.SeedDbFeedback = "Database created and Student Table populated with Data. Check Database folder.";
            return View("SeedDatabase");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Contact(Email model)
        //{
        //    MailMessage mailMessage = new MailMessage(model.From, "cutadminbfn2024@gmail.com");
        //    mailMessage.Subject = model.Subject;
        //    mailMessage.Body = model.Body;
        //    mailMessage.IsBodyHtml = false;

        //    SmtpClient smtpClient = new SmtpClient();
        //    smtpClient.Host = "smtp.gmail.com";
        //    smtpClient.Port = 587;
        //    smtpClient.EnableSsl = true;

        //    NetworkCredential networkCredential = new NetworkCredential("kurochin051@gmail.com", "Masabata");
        //    smtpClient.UseDefaultCredentials = true;
        //    smtpClient.Credentials = networkCredential;
        //    smtpClient.Send(mailMessage);
        //    ViewBag.Message = "Message has been sent";
        //    return View();
        //}

        [HttpPost]
        [HttpPost]
        public IActionResult Contact(Email model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse("lebakengkgauhelo2@gmail.com"));
                    email.To.Add(MailboxAddress.Parse("CUTAdminBFN2024@gmail.com"));
                    email.Subject = model.Subject;
                    email.Body = new TextPart(TextFormat.Text) { Text = model.Body };

                    using var smtp = new SmtpClient();
                    smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("lebakengkgauhelo2@gmail.com", "bR2+U_4N*4-*wg_");
                    smtp.Send(email);
                    smtp.Disconnect(true);

                    ViewBag.Message = "Message sent successfully";
                }
                catch (Exception ex)
                {
                    // Log the error
                    ViewBag.ErrorMessage = "An error occurred while sending the message. Please try again later.";
                    _logger.LogError(ex, "Error occurred while sending email.");
                }
            }
            // If model state is invalid, return the view with validation errors
            return View(model);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}