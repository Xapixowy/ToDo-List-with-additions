using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;
using ToDo_List_with_additions.Models;

namespace ToDo_List_with_additions.Services
{
    public class APIService : IAPIService
    {
        private readonly IMongoCollection<UserModel> users;
        public APIService(IConfiguration config)
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
        }
        
        public List<UserModel> GetUsers()
        {
            return users.Find(User => true).ToList();

        }

        public UserModel GetUser(string id, string login)
        {
            if (id != null)
            {
                UserModel user = users.Find<UserModel>(User => User.Id == id).FirstOrDefault();
                return user;
            }
            else if (login != null)
            {
                UserModel user = users.Find<UserModel>(User => User.Login == login).FirstOrDefault();
                return user;
            }
            else
            {
                return null;
            }

        }
        public UserModel CheckLogin(string login)
        {
            var user = users.Find(User => User.Login == login).FirstOrDefault();
            return user;
        }
        public UserModel UserCreate(UserModel user)
        {
            users.InsertOne(user);
            return user;
        }
        public UserModel UserEdit(UserModel user)
        {
            users.ReplaceOne(User => User.Id == user.Id, user);
            return user;
        }
        public UserModel UserDelete(string id)
        {
            var user = users.Find(User => User.Id == id).FirstOrDefault();
            users.DeleteOne(User => User.Id == user.Id);
            return user;
        }
    }
}
