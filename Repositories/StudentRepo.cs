using ASPNETCore_DB.Data;
using ASPNETCore_DB.Interfaces;
using ASPNETCore_DB.Models;


namespace ASPNETCore_DB.Repositories
{
    public class StudentRepo : IStudent
    {
        private readonly SQLiteDBContext _context;

        public StudentRepo(SQLiteDBContext context)
        {
            _context = context;
        }

        public Student Create(Student student)
        {
            _context.Add(student);
            _context.SaveChanges();
            return student;
        }

        public bool Delete(Student student)
        {
            _context.Remove(student);
            _context.SaveChanges();
            return IsExist(student.StudentNumber);
        }

        public Student Details(string id)
        {
            var student = _context.Students?.FirstOrDefault(x => x.StudentNumber == id);
            return student;
        }

        public Student ByEmail(string id)
        {
            var student = _context .Students?.FirstOrDefault(x => x.Email == id);
            return student;
        }

        public Student Edit(Student student)
        {
            _context.Update(student);
            _context.SaveChanges();
            return student;
        }

        public IQueryable<Student> GetStudents(string searchString, string sortOrder)
        {
            var student = _context.Students
               .ToList();
            if(!String.IsNullOrEmpty(searchString)) 
            {
                student = student.Where(s => s.StudentNumber.Contains(searchString)).ToList();
            }
            switch (sortOrder)
            {
                case "number_desc":
                    student = student.OrderByDescending(s => s.StudentNumber).ToList();
                    break;
                case "name_desc":
                    student = student.OrderByDescending(s => s.Surname).ToList();
                    break;
                case "Date":
                    student = student.OrderBy(s => s.EnrollmentDate).ToList();
                    break;
                case "date_desc":
                    student = student.OrderByDescending(s => s.EnrollmentDate).ToList();
                    break;
                default:
                    student = student.OrderBy(s => s.Surname).ToList();
                    break;
            }

            return student.AsQueryable();
        }

        public bool IsExist(string id)
        {
            bool isExist = false;
            Student existStudent = Details(id);
            if (existStudent == null)
            {
                isExist = true;
            }
            return isExist;
        }

    }
}
