using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;
using ToDo_List_with_additions.Models;

namespace ToDo_List_with_additions.Services
{
    public class StatisticsService : IStatisticsService
    {
        
        private readonly IMongoCollection<StatisticsModel> stats;
        public StatisticsService(IConfiguration config)
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
            stats = database.GetCollection<StatisticsModel>("stats");
        }

        public StatisticsModel Create(string userId)
        {
            var statistics = new StatisticsModel();
            statistics.userId = userId;
            statistics.Done = new Dictionary<string, int>
            {
                { "0", 0 },
                { "1", 0 },
                { "2", 0 },
                { "3", 0 }
            };
            statistics.NotDone = new Dictionary<string, int>
            {
                { "0", 0 },
                { "1", 0 },
                { "2", 0 },
                { "3", 0 }
            };
            statistics.Postponed = new Dictionary<string, int>
            {
                { "0", 0 },
                { "1", 0 },
                { "2", 0 },
                { "3", 0 }
            };

            stats.InsertOne(statistics);
            return statistics;
            
        }

        public StatisticsModel IncrementDone(string userId, int importance)
        {
            var statistics = new StatisticsModel();

            statistics = stats.Find<StatisticsModel>(User => User.userId == userId).FirstOrDefault();
            statistics.Done[importance.ToString()] += 1;
            stats.ReplaceOne(stat => stat.Id == statistics.Id, statistics);

            return statistics;
        }
        public StatisticsModel IncrementNotDone(string userId, int importance)
        {
            var statistics = new StatisticsModel();
            statistics = stats.Find<StatisticsModel>(User => User.userId == userId).FirstOrDefault();
            statistics.NotDone[importance.ToString()] += 1;
            stats.ReplaceOne(stat => stat.Id == statistics.Id, statistics);

            return statistics;
        }

        public StatisticsModel IncrementPostponed(string userId, int importance)
        {
            var statistics = new StatisticsModel();
            statistics = stats.Find<StatisticsModel>(User => User.userId == userId).FirstOrDefault();
            statistics.Postponed[importance.ToString()] += 1;
            stats.ReplaceOne(stat => stat.Id == statistics.Id, statistics);

            return statistics;
        }
        public StatisticsModel DecrementNotDone(string userId, int importance)
        {
            var statistics = new StatisticsModel();

            statistics = stats.Find<StatisticsModel>(User => User.userId == userId).FirstOrDefault();
            statistics.NotDone[importance.ToString()] -= 1;
            stats.ReplaceOne(stat => stat.Id == statistics.Id, statistics);

            return statistics;
        }
        
        public StatisticsModel Get(string userId)
        {
            var statistics = new StatisticsModel();

            statistics = stats.Find<StatisticsModel>(User => User.userId == userId).FirstOrDefault();

            return statistics;
        }
    }
}
