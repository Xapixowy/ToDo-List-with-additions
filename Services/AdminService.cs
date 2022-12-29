using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;
using ToDo_List_with_additions.Models;

namespace ToDo_List_with_additions.Services
{
    public class AdminService
    {
        private readonly IMongoCollection<StatisticsModel> admin;
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
            admin = database.GetCollection<StatisticsModel>("users");
        }
    }
}
