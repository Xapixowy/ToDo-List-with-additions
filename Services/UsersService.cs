using ToDo_List_with_additions.Models;
using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;

namespace ToDo_List_with_additions.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMongoCollection<UserModel> Users;
        private readonly IMongoCollection<ToDoModel> ToDos;
        private readonly IMongoCollection<StatisticsModel> Stats;
        public UsersService(IConfiguration config)
        {
            var settings = MongoClientSettings.FromConnectionString(config.GetValue<string>("Database:ConnectionString"));
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var cert = new X509Certificate2(config.GetValue<string>("Database:CertificateLocation"), config.GetValue<string>("Database:CertificatePassword"));
            settings.SslSettings = new SslSettings
            {
                ClientCertificates = new List<X509Certificate>() { cert },
            };
            MongoClient client = new MongoClient(settings);
            IMongoDatabase database = client.GetDatabase(config.GetValue<string>("Database:DatabaseName"));
            Users = database.GetCollection<UserModel>("users");
            ToDos = database.GetCollection<ToDoModel>("todos");
            Stats = database.GetCollection<StatisticsModel>("stats");
        }
        public List<UserModel> Get()
        {
            return Users.Find(User => true).ToList();
        }
        public UserModel GetUser(string id)
        {
            UserModel user = Users.Find<UserModel>(User => User.Id == id).FirstOrDefault();
            return user;
        }
        public UserModel Register(UserModel user)
        {
            
            Users.InsertOne(user);
            
            return user;
        }
        public UserModel CheckLogin(string login)
        {
            UserModel user = Users.Find<UserModel>(User => User.Login == login).FirstOrDefault();
            return user;
        }
        public UserModel Edit(UserModel user)
        {
            Users.ReplaceOne(User => User.Id == user.Id, user);
            return user;
        }
        public UserModel Delete(string id)
        {
            UserModel user = Users.Find<UserModel>(User => User.Id == id).FirstOrDefault();
            Users.DeleteOne(User => User.Id == id);
            Stats.DeleteOne(Stat => Stat.userId == id);
            var deleteToDos = ToDos.Find<ToDoModel>(Todo => Todo.UserId == id).ToList();
            foreach(var toDo in deleteToDos)
            {
                ToDos.DeleteOne(toDo => true);
            }
            return user;
        }
        public UserModel Login(string login, string password)
        {
            UserModel user = Users.Find<UserModel>(User => User.Login == login && User.Password == password).FirstOrDefault();
            return user;
        }
    }
}
