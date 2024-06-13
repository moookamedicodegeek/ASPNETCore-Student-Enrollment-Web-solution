using ASPNETCore_DB.Interfaces;
using ASPNETCore_DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCore_DB.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class StudentController : Controller
    {
        private readonly IStudent _studentRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentController(IStudent studentRepo, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                _studentRepo = studentRepo;
                _httpContextAccessor = httpContextAccessor;
                _webHostEnvironment = webHostEnvironment;

            }
            catch (Exception ex)
            {
                throw new Exception("Constructor not initialized - IStudent studentRepo");
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            pageNumber = pageNumber ?? 1;
            int pageSize = 3;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["StudentNumberSortParm"] = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            ViewResult viewResult = View();

            try
            {
                viewResult = View(PaginatedList<Student>.Create(_studentRepo.GetStudents(searchString, sortOrder).AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                throw new Exception("No student records detected");
            }

            return viewResult;
        }
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var student = _studentRepo.ByEmail(this.User.Identity.Name.ToString());
                return View(student);
            }
            else
            {
                var student = _studentRepo.Details(id);
                return View(student);
            }
        }


        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Create()
        {
            var studentExist = _studentRepo.ByEmail(this.User.Identity.Name.ToString());
            if (studentExist != null) 
            {
                return RedirectToAction("Details", "Student", studentExist.StudentNumber);
            }
            else
            {
                Student student = new Student();
                string fileName = "default.PNG";
                student.Photo = fileName;
                return View(student);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult Create(Student student)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WebConstants.ImagePath;
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);
            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension),
            FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }
            student.Photo = fileName + extension;
            try
            {
                if (ModelState.IsValid)
                {
                    _studentRepo.Create(student);

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Student record not saved.");
            }

            var studentExist = _studentRepo.ByEmail(this.User.Identity.Name.ToString());

            if(studentExist != null)
            {
                return RedirectToAction("Details", "Student", new { id = studentExist.StudentNumber });
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            ViewResult viewDetail = View();
            try
            {
                viewDetail = View(_studentRepo.Details(id));
            }
            catch (Exception ex)
            {
                throw new Exception("Student detail not found");
            }
            return viewDetail;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            string photoName = "";
            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                string upload = webRootPath + WebConstants.ImagePath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);
                var oldFile = Path.Combine(upload, photoName);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension),
                FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                student.Photo = fileName + extension;
            }
            else
            {
                student.Photo = photoName;
            }
            try
            {
                _studentRepo.Edit(student);
            }
            catch (Exception ex)
            {
                throw new Exception("Student record not saved.");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            ViewResult viewDetail = View();
            try
            {
                viewDetail = View(_studentRepo.Details(id));
            }
            catch (Exception ex)
            {
                throw new Exception("Student detail not found");
            }
            return viewDetail;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([Bind("StudentNumber, FirstName, Surname, EnrollmentDate")] Student student)
        {
            try
            {
                _studentRepo.Delete(student);
            }
            catch (Exception ex)
            {
                throw new Exception("Student could not be deleted");
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
