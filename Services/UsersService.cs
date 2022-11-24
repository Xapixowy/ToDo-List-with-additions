using ToDo_List_with_additions.Models;
using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;

namespace ToDo_List_with_additions.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<User> Users;


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
            Users = database.GetCollection<User>("users");
        }
        public List<User> Get()
        {
            return Users.Find(User => true).ToList();
        }
        public User Register(User user)
        {
            Users.InsertOne(user);
            return user;
        }
    }
}
