using ServerSideEx4.BL;
using System;
using System.Data;
using System.Data.SqlClient;

public class DBservices
{

    public DBservices()
    {

    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }


    //--------------------------------------------------------------------------------------------------
    // Courses DB Methods
    //--------------------------------------------------------------------------------------------------


    // insert course
    public Course InsertCourse(Course course)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        //create Dictionary for sp
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Title", course.Title);
        paramDic.Add("@Url", course.Url);
        paramDic.Add("@Duration", course.Duration);
        paramDic.Add("@InstructorId", course.InstructorsId);
        paramDic.Add("@Image", course.ImageReference);
        cmd = CreateCommandWithStoredProcedure("SP_CreateCourse", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected > 0)
            {
                return ReadSpecificCourse();
            }
            return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    // delete course
    public bool DeleteCourse(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@CourseId", id);

        cmd = CreateCommandWithStoredProcedure("SP_DeleteCourse", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    
    public bool updateIsActive(int courseID, bool updatedIsActive)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@CourseId", courseID);
        paramDic.Add("@IsActive", updatedIsActive);
        cmd = CreateCommandWithStoredProcedure("SP_UpdateCourseIsActive", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected == 0) return false;
            else return true;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //update course
    public bool UpdateCourse(Course course)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@CourseId", course.Id);
        paramDic.Add("@Title", course.Title);
        paramDic.Add("@Url", course.Url);
        paramDic.Add("@Duration", course.Duration);
        paramDic.Add("@Image", course.ImageReference);
        cmd = CreateCommandWithStoredProcedure("SP_EditCourse", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if(numEffected == 0) return false;
            else return true;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    //read all courses
    public List<Course> ReadCourse()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("SP_ReadCourse", con, null);             // create the command
        List<Course> CoursesList = new List<Course>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Course course = new Course();
                course.Id = Convert.ToInt32(dataReader["id"]);
                course.Title = dataReader["title"].ToString();
                course.Url = dataReader["url"].ToString();
                course.Rating = Convert.ToSingle(dataReader["rating"]);
                course.NumberOfReviews = Convert.ToInt32(dataReader["num_reviews"]);
                course.InstructorsId = Convert.ToInt32(dataReader["instructors_id"]);
                course.ImageReference = dataReader["image"].ToString();
                course.Duration = Convert.ToSingle(dataReader["duration"]);
                DateTime Date = Convert.ToDateTime(dataReader["last_update_date"]);
                course.LastUpdate = Date.ToString("dd/MM/yyyy");
                CoursesList.Add(course);
            }
            return CoursesList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public Object GetTop5Courses()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("SP_GetTop5Courses", con, null);             // create the command
        List<Object> objectsList = new List<Object>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                objectsList.Add(new
                {
                    Id = Convert.ToInt32(dataReader["id"]),
                    Title = dataReader["name"].ToString(),
                    Rating = Convert.ToSingle(dataReader["rating"]),
                    Num_registered_users = Convert.ToInt32(dataReader["num_registered_users"])
                });
            }
            return objectsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //return currect course
    public Course ReadSpecificCourse()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("SP_GetLastInsertedCourse", con, null);             // create the command
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read())
            {
                Course course = new Course();
                course.Id = Convert.ToInt32(dataReader["id"]);
                course.Title = dataReader["title"].ToString();
                course.Url = dataReader["url"].ToString();
                course.Rating = Convert.ToSingle(dataReader["rating"]);
                course.NumberOfReviews = Convert.ToInt32(dataReader["num_reviews"]);
                course.InstructorsId = Convert.ToInt32(dataReader["instructors_id"]);
                course.ImageReference = dataReader["image"].ToString();
                course.Duration = Convert.ToSingle(dataReader["duration"]);
                DateTime Date = Convert.ToDateTime(dataReader["last_update_date"]);
                course.LastUpdate = Date.ToString("dd/MM/yyyy");
                return course;
            }
            return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    public Course GetCourseByTitle(string title)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@title", title);
        cmd = CreateCommandWithStoredProcedure("SP_GetCourseByTitle", con, paramDic);             // create the command
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read())
            {
                Course course = new Course();
                course.Id = Convert.ToInt32(dataReader["id"]);
                course.Title = dataReader["title"].ToString();
                course.Url = dataReader["url"].ToString();
                course.Rating = Convert.ToSingle(dataReader["rating"]);
                course.NumberOfReviews = Convert.ToInt32(dataReader["num_reviews"]);
                course.InstructorsId = Convert.ToInt32(dataReader["instructors_id"]);
                course.ImageReference = dataReader["image"].ToString();
                course.Duration = Convert.ToSingle(dataReader["duration"]);
                DateTime Date = Convert.ToDateTime(dataReader["last_update_date"]);
                course.LastUpdate = Date.ToString("dd/MM/yyyy");
                return course;
            }
            return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    // instructor DB Methods
    //--------------------------------------------------------------------------------------------------

    //read all Instructors

    public List<Instructor> ReadInstructors()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("SP_ReadInstructors", con, null);             // create the command
        List<Instructor> InstructorsList = new List<Instructor>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Instructor instructor = new Instructor();
                instructor.Id = Convert.ToInt32(dataReader["id"]);
                instructor.Title = dataReader["title"].ToString();
                instructor.Name = dataReader["name"].ToString();
                instructor.JobTitle = dataReader["jobTitle"].ToString();
                instructor.Image = dataReader["image"].ToString();
                InstructorsList.Add(instructor);
            }
            return InstructorsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    // insert Instructor
    public bool InsertInstructor(Instructor instructor)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Title", instructor.Title);
        paramDic.Add("@Name", instructor.Name);
        paramDic.Add("@jobTitle", instructor.JobTitle);
        paramDic.Add("@Image", instructor.Image);

        cmd = CreateCommandWithStoredProcedure("SP_CreateInstructor", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected == 0) return false;
            else return true;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
   
    // delete Instructor by id

    public bool DeleteInstructor(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@InstructorId", id);

        cmd = CreateCommandWithStoredProcedure("SP_DeleteInstructor", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected == 0) return false;
            else return true;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    //--------------------------------------------------------------------------------------------------
    // get instructor courses DB Method
    //--------------------------------------------------------------------------------------------------

    public List<Course> GetInstructorCourses(int id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);

        cmd = CreateCommandWithStoredProcedure("SP_GetInstructorCourses", con, paramDic);             // create the command
        List<Course> CoursesList = new List<Course>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Course course = new Course();
                course.Id = Convert.ToInt32(dataReader["id"]);
                course.Title = dataReader["title"].ToString();
                course.Url = dataReader["url"].ToString();
                course.Rating = Convert.ToSingle(dataReader["rating"]);
                course.NumberOfReviews = Convert.ToInt32(dataReader["num_reviews"]);
                course.InstructorsId = Convert.ToInt32(dataReader["instructors_id"]);
                course.ImageReference = dataReader["image"].ToString();
                course.Duration = Convert.ToSingle(dataReader["duration"]);
                DateTime Date = Convert.ToDateTime(dataReader["last_update_date"]);
                course.LastUpdate = Date.ToString("dd/MM/yyyy");
                CoursesList.Add(course);
            }
            return CoursesList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    // Users DB Method
    //--------------------------------------------------------------------------------------------------
    
    //User Read
    public List<User> ReadUsers()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("SP_ReadUsers", con, null);             // create the command
        List<User> UsersList = new List<User>();
        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                User user = new User();
                user.Id = Convert.ToInt32(dataReader["id"]);
                user.Name = dataReader["name"].ToString();
                user.Email = dataReader["email"].ToString();
                user.Password = dataReader["password"].ToString();
                user.IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]);
                user.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
                UsersList.Add(user);
            }
            return UsersList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //register user
    public User RegisterUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Name", user.Name);
        paramDic.Add("@Email", user.Email);
        paramDic.Add("@Password", user.Password);
        cmd = CreateCommandWithStoredProcedure("SP_RegisterUser", con, paramDic);             // create the command
       
        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected > 0) {
                return LogInUser(user.Email, user.Password);

            }
            //SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            //if (dataReader.Read())
            //{
            //    User newUser = new User();
            //    newUser.Id = Convert.ToInt32(dataReader["id"]);
            //    newUser.Name = dataReader["name"].ToString();
            //    newUser.Email = dataReader["email"].ToString();
            //    newUser.Password = dataReader["password"].ToString();
            //    newUser.IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]);
            //    newUser.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
            //    return newUser;
            //}
            else return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //login user
    public User LogInUser(string email, string password)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Email", email);
        paramDic.Add("@Password", password);


        cmd = CreateCommandWithStoredProcedure("SP_LoginUser", con, paramDic);             // create the command

        try
        {
            object result = cmd.ExecuteScalar(); // execute the command and get the result
            int returnValue = Convert.ToInt32(result);
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read())
            {
                User user = new User();
                user.Id = Convert.ToInt32(dataReader["id"]);
                user.Name = dataReader["name"].ToString();
                user.Email = dataReader["email"].ToString();
                user.Password = dataReader["password"].ToString();
                user.IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]);
                user.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
                return user;
            }
            else return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //delete user
    public bool DeleteUser(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", id);

        cmd = CreateCommandWithStoredProcedure("SP_DeleteUser", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected == 0) return false;
            else return true;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // UsersCourses DB Method
    //--------------------------------------------------------------------------------------------------
    public List<UserCourse> ReadUserCourse()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("SP_ReadAllUserCourse", con, null);             // create the command
        List<UserCourse> UserCourseList = new List<UserCourse>();
        try
        {

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                UserCourse userCourse = new UserCourse();
                userCourse.UserId = Convert.ToInt32(dataReader["UserId"]);
                userCourse.CourseId = Convert.ToInt32(dataReader["CourseId"]);
                UserCourseList.Add(userCourse);
            }
            return UserCourseList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public List<Course> GetAllUserCourses(int userId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        //create Dictionary for sp
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", userId);
        cmd = CreateCommandWithStoredProcedure("SP_GetAllUserCourses", con, paramDic);     // create the command

        List<Course> CoursesList = new List<Course>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Course course = new Course();
                course.Id = Convert.ToInt32(dataReader["id"]);
                course.Title = dataReader["title"].ToString();
                course.Url = dataReader["url"].ToString();
                course.Rating = Convert.ToSingle(dataReader["rating"]);
                course.NumberOfReviews = Convert.ToInt32(dataReader["num_reviews"]);
                course.InstructorsId = Convert.ToInt32(dataReader["instructors_id"]);
                course.ImageReference = dataReader["image"].ToString();
                course.Duration = Convert.ToSingle(dataReader["duration"]);
                DateTime Date = Convert.ToDateTime(dataReader["last_update_date"]);
                course.LastUpdate = Date.ToString("dd/MM/yyyy");
                CoursesList.Add(course);
            }
            return CoursesList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    public bool InsertUserCourse(UserCourse userCourse)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        //create Dictionary for sp
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", userCourse.UserId);
        paramDic.Add("@CourseId", userCourse.CourseId);
        cmd = CreateCommandWithStoredProcedure("SP_InsertUserCourse", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //get all courses by rating range
    public List<Course> GetByRatingRange(int id,float minRating, float maxRating)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", id);
        paramDic.Add("@minRating", minRating);
        paramDic.Add("@maxRating", maxRating);

        cmd = CreateCommandWithStoredProcedure("SP_GetAllUserCoursesRatingRange", con, paramDic);             // create the command
        List<Course> CoursesList = new List<Course>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Course course = new Course();
                course.Id = Convert.ToInt32(dataReader["id"]);
                course.Title = dataReader["title"].ToString();
                course.Url = dataReader["url"].ToString();
                course.Rating = Convert.ToSingle(dataReader["rating"]);
                course.NumberOfReviews = Convert.ToInt32(dataReader["num_reviews"]);
                course.InstructorsId = Convert.ToInt32(dataReader["instructors_id"]);
                course.ImageReference = dataReader["image"].ToString();
                course.Duration = Convert.ToSingle(dataReader["duration"]);
                DateTime Date = Convert.ToDateTime(dataReader["last_update_date"]);
                course.LastUpdate = Date.ToString("dd/MM/yyyy");
                CoursesList.Add(course);
            }
            return CoursesList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    //get all courses by duration range
    public List<Course> GetByDurationRange(int id,float minDuration, float maxDuration)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", id);
        paramDic.Add("@minDuration", minDuration);
        paramDic.Add("@maxDuration", maxDuration);

        cmd = CreateCommandWithStoredProcedure("SP_GetAllUserCoursesDurationRange", con, paramDic);             // create the command
        List<Course> CoursesList = new List<Course>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Course course = new Course();
                course.Id = Convert.ToInt32(dataReader["id"]);
                course.Title = dataReader["title"].ToString();
                course.Url = dataReader["url"].ToString();
                course.Rating = Convert.ToSingle(dataReader["rating"]);
                course.NumberOfReviews = Convert.ToInt32(dataReader["num_reviews"]);
                course.InstructorsId = Convert.ToInt32(dataReader["instructors_id"]);
                course.ImageReference = dataReader["image"].ToString();
                course.Duration = Convert.ToSingle(dataReader["duration"]);
                DateTime Date = Convert.ToDateTime(dataReader["last_update_date"]);
                course.LastUpdate = Date.ToString("dd/MM/yyyy");
                CoursesList.Add(course);
            }
            return CoursesList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }



    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        //check if Dictionary not null and add to cmd
        if (paramDic != null)
            foreach (KeyValuePair<string, object> param in paramDic)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);

            }


        return cmd;
    }
}
