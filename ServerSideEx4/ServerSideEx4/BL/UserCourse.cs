using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace ServerSideEx4.BL
{
    public class UserCourse
    {
        int userId;
        int courseId;
        public UserCourse()
        {
        }
        public UserCourse(int userId, int courseId)
        {
            this.UserId = userId;
            this.CourseId = courseId;
        }

        public int UserId { get => userId; set => userId = value; }
        public int CourseId { get => courseId; set => courseId = value; }
        public static List<UserCourse> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadUserCourse();
        }
        public static List<Course> ReadAllUserCourses(int id) {
            DBservices dbs = new DBservices();
            return dbs.GetAllUserCourses(id);
        }
        public List<Course> RatingRange(int id,float minRating, float maxRating)
        {
            DBservices dbs = new DBservices();
            return dbs.GetByRatingRange(id,minRating, maxRating);
        }
        public List<Course> DurationRange(int id,float minDuration, float maxDuration)
        {
            DBservices dbs = new DBservices();
            return dbs.GetByDurationRange(id, minDuration, maxDuration);
        }
        public bool Insert()
        {
            DBservices dbs = new DBservices();
           return dbs.InsertUserCourse(this);
        }



    }
}
