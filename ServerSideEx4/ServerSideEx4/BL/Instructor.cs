namespace ServerSideEx4.BL
{
    public class Instructor
    {
        int id;
        string title;
        string name;
        string image;
        string jobTitle;
        public Instructor()
        {

        }
        public Instructor(int id, string title, string name, string image, string jobTitle)
        {
            Id = id;
            Title = title;
            Name = name;
            Image = image;
            JobTitle = jobTitle;
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Name { get => name; set => name = value; }
        public string Image { get => image; set => image = value; }
        public string JobTitle { get => jobTitle; set => jobTitle = value; }
        public static List<Instructor> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadInstructors();
        }
        public bool AddInstructor()
        {
            DBservices dbs = new DBservices();
           return dbs.InsertInstructor(this);
        }
        public static bool Delete(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteInstructor(id);
        }

    }
}
