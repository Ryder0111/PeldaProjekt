using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Tanulokezelo_MVC_API.Models;

namespace Tanulokezelo_MVC_API.Controllers
{
    [Route("api/studentapi")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private static List<Student> _students = new List<Student>
{
    new Student(1, "Kovács Anna", 17, 3.8, 123456, 123456789),
    new Student(2, "Nagy Péter", 16, 4.2, 234567, 234567891),
    new Student(3, "Szabó Júlia", 18, 4.7, 345678, 345678912),
    new Student(4, "Tóth Bence", 17, 3.5, 456789, 456789123),
    new Student(5, "Varga Lili", 16, 4.1, 567890, 567890234),
    new Student(6, "Kiss Márton", 17, 3.9, 678901, 678901345),
    new Student(7, "Horváth Réka", 18, 4.6, 789012, 789012456),
    new Student(8, "Balogh Dániel", 17, 3.4, 890123, 890123567),
    new Student(9, "Farkas Zsófia", 16, 4.3, 901234, 901234678),
    new Student(10, "Molnár Kristóf", 18, 3.7, 112233, 112233445),

    new Student(11, "Németh Eszter", 17, 4.5, 223344, 223344556),
    new Student(12, "Juhász Áron", 18, 3.6, 334455, 334455667),
    new Student(13, "Kerekes Luca", 16, 4.0, 445566, 445566778),
    new Student(14, "Lakatos Máté", 17, 3.3, 556677, 556677889),
    new Student(15, "Papp László", 18, 4.4, 667788, 667788990),
    new Student(16, "Takács Dorina", 16, 3.8, 778899, 778899001),
    new Student(17, "Fülöp Ádám", 17, 4.6, 889900, 889900112),
    new Student(18, "Gulyás Kitti", 18, 3.9, 990011, 990011223),
    new Student(19, "Barta Kornél", 16, 4.2, 101112, 101112334),
    new Student(20, "Simon Nóra", 17, 3.7, 121314, 121314556)
};
        private static int nextId = 21;

        [HttpPost("create")]
        public ActionResult<Student> addStudent([FromBody] StudentDTO s)
        {
            Random rnd = new Random();
            int randomTaj = rnd.Next(100000000, 1000000000);
            try
            {
                bool vaneOM = false;
                foreach(var item in _students)
                {
                    if(item.OMidentifier == s.OMidentifier)
                    {
                        vaneOM = true;
                    }
                }

                if (vaneOM == false)
                {
                    Student nStudent = new Student(nextId, s.Name, s.Age, s.GPA, s.OMidentifier, randomTaj);
                    _students.Add(nStudent);
                    return Ok(nStudent);
                }
                else return StatusCode(409,"Ilyen OM azonosító már létezik");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult getAll()
        {
            return Ok(convertToDTO(_students));
        }

        [HttpGet("{om}")]
        public ActionResult<Student> getById(int om)
        {
            var tanulo = _students.Find(x => x.OMidentifier == om);
            if (tanulo != null)
            {
                return Ok(tanulo);
            }
            return BadRequest("A tanuló nem található!");
        }

        [HttpDelete("delete/{om}")]
        public ActionResult<Student> deleteStudent(int om)
        { 
            var keresett = _students.Find(x=>x.OMidentifier == om);
            if (keresett != null)
            { 
                _students.Remove(keresett);
                return NoContent(); //sikeres törlés, de nem ad vissza értéket
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPut("modify/{om}")]
        public ActionResult<Student> modifyById(int om,[FromBody]StudentDTO modositando)
        {
            try
            {
                if (modositando == null) return BadRequest();
                var keres = _students.Find(x => x.OMidentifier == om);
                if (keres != null)
                {
                    keres.Name = modositando.Name;
                    keres.Age = modositando.Age;
                    keres.OMidentifier = modositando.OMidentifier;
                    keres.GPA = modositando.GPA;
                    return Ok(keres);
                }
                else return BadRequest("Nem található a tanuló!");
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);    
            }
        }

        [HttpGet("filter/{gpa}")]
        public ActionResult<FilterStudent> Filter(double gpa)
        {
            FilterStudent atadando = new FilterStudent();
            List<StudentDTO> megfelelok = new List<StudentDTO>();
            foreach (var student in _students)
            {
                if (student.GPA > gpa)
                {
                    megfelelok.Add(convertToDtoOne(student));
                }
            }
            atadando.GPA = gpa;
            atadando.filteredList = megfelelok;
            return Ok(atadando);
        }

        public StudentDTO convertToDtoOne (Student student)
        {
            StudentDTO visszaAd = new StudentDTO() { 
                Age= student.Age,
                OMidentifier=student.OMidentifier,
                Name = student.Name,
                GPA= student.GPA
            };
            return visszaAd;
        }

        public List<StudentDTO> convertToDTO(List<Student> lista)
        { 
            List<StudentDTO > DTO = new List<StudentDTO>();
            foreach (var student in lista)
            { 
                StudentDTO uj = new StudentDTO
                {
                    OMidentifier = student.OMidentifier,
                    Name = student.Name,
                    Age = student.Age,
                    GPA = student.GPA
                };
                DTO.Add(uj);
            }
            return DTO;
        }
    }
}
