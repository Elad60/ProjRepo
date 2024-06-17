namespace ServerSideEx4.BL
{
    public class User
    {
        int id;
        string name;
        string email;
        string password;
        bool isAdmin;
        bool isActive;

        public User()
        {
        }

        public User(int id,string name, string email, string password, bool isAdmin, bool isActive)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            IsAdmin = isAdmin;
            IsActive = isActive;

        }
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public static List<User> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadUsers();
        }
        public User Register()
        {
            DBservices dbs = new DBservices();
            if (dbs.ReadUsers().Contains(this) == false)
            {
              return  dbs.RegisterUser(this);
                
            }
            throw new Exception("The email is registered in the system");
        }
        public static User Login(string email, string password)
        {
            DBservices dbs = new DBservices();
            return dbs.LogInUser(email, password);
        }
        public override bool Equals(object? obj)
        {
            return obj is User user && Email == user.Email;
        }
        public static bool Delete(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteUser(id);
        }
    }
}
