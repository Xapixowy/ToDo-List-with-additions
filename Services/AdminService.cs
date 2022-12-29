using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;
using ToDo_List_with_additions.Models;

namespace ToDo_List_with_additions.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMongoCollection<UserModel> users;
        private readonly IMongoCollection<ToDoModel> todos;
        public AdminService(IConfiguration config)
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
            users = database.GetCollection<UserModel>("users");
            todos = database.GetCollection<ToDoModel>("todos");
        }
        public List<UserModel> GetUsers()
        {
            return users.Find(User => true).ToList();
        }
        
        public List<ToDoModel> GetToDos(string userId)
        {
            return todos.Find(Todo => Todo.UserId == userId).ToList();
        }
    }
}
