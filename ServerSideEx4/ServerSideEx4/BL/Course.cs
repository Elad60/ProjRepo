namespace ServerSideEx4.BL
{
    public class Course
    {
        int id;
        string title;
        string url;
        double rating;
        int numberOfReviews;
        int instructorsId;
        string imageReference;
        double duration;
        string lastUpdate;
        bool isActive;

        public Course()
        {
        }
        public Course(int id, string title, string url, double rating, int numberOfReviews, int instructorsId, string imageReference, double duration, string lastUpdate)
        {
            Id = id;
            Title = title;
            Url = url;
            Rating = rating;
            NumberOfReviews = numberOfReviews;
            InstructorsId = instructorsId;
            ImageReference = imageReference;
            Duration = duration;
            LastUpdate = lastUpdate;
            this.isActive = true;
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Url { get => url; set => url = value; }
        public double Rating { get => rating; set => rating = value; }
        public int NumberOfReviews { get => numberOfReviews; set => numberOfReviews = value; }
        public int InstructorsId { get => instructorsId; set => instructorsId = value; }
        public string ImageReference { get => imageReference; set => imageReference = value; }
        public double Duration { get => duration; set => duration = value; }
        public string LastUpdate { get => lastUpdate; set => lastUpdate = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public static List<Course> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadCourse();
        }
        public Course AddCourse()
        {
            DBservices dbs = new DBservices();
            if (dbs.ReadCourse().Contains(this) == false)
            {
             return  dbs.InsertCourse(this);
            }
            throw new Exception("error");

        }
        public bool Update()
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateCourse(this);
        }
        public static bool UpdateIsActive(int courseID,bool updatedIsActive)
        {
            DBservices dbs = new DBservices();
            return dbs.updateIsActive(courseID,updatedIsActive);
        }
        public static bool Delete(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteCourse(id);
        }
        public List<Course> InstructorCourses(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.GetInstructorCourses(id);
        }
        public static Object ReadTop5Courses()
        {
            DBservices dbs = new DBservices();
            return dbs.GetTop5Courses();
        }
        
        public  static Course CourseByTitle(string title) {
            DBservices dbs = new DBservices();
            return dbs.GetCourseByTitle(title);
        }
        public override bool Equals(object? obj)
        {
            return obj is Course course && (Id == course.Id ||Title ==course.Title) ;
        }
    }
}
