namespace Tanulokezelo_MVC_API.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double GPA { get; set; }
        public int OMidentifier {  get; set; }
        public int  TAJnum { get; set; }
        public Student(int id, string name, int age, double gPA, int oMidentifier, int tAJnum)
        {
            Id = id;
            Name = name;
            Age = age;
            GPA = gPA;
            OMidentifier = oMidentifier;
            TAJnum = tAJnum;
        }
    }
    public class StudentDTO
    {
        public int OMidentifier { get; set; }
        public string Name {  get; set; }
        public int Age { get; set; }
        public double GPA { get; set; }
    }

    public class FilterStudent
    {
        public double GPA { get; set; }
        public List<StudentDTO> filteredList { get; set; }
        
    }
}
