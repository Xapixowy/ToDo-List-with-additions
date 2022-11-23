using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;

namespace ToDo_List_with_additions.Services
{
    public class Database
    {
        static void Main(string[] args)
        {
            var data = new BsonDocument {
                {"login", "default"},
                {"password", "deafult"},
                {"firstName", "John"},
                {"lastName", "Doe"},
                {"nickname", "FunnyDoe"},
                {"toDos", new BsonArray {
                    { new BsonDocument {
                        {"id",0},
                        {"date","2022-11-11"},
                        {"content","Buy a drink!"},
                        {"importance",2},
                        {"done",true},
                    }},
                }},
                {"stats", new BsonDocument {
                    {"done", new BsonDocument {
                        {"0", 0},
                        {"1", 0},
                        {"2", 0},
                        {"3", 0},
                    }},
                    {"notDone", new BsonDocument {
                        {"0", 0},
                        {"1", 0},
                        {"2", 0},
                        {"3", 0},
                    }},
                    {"postponed", new BsonDocument {
                        {"0", 0},
                        {"1", 0},
                        {"2", 0},
                        {"3", 0},
                    }},
                }}
            };
            MainAsync("users",data).Wait();
        }
        static async Task MainAsync(string collectionName, BsonDocument data)
        {
            var connectionString = "mongodb+srv://cluster0.btywazj.mongodb.net/?authSource=%24external&authMechanism=MONGODB-X509&retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var cert = new X509Certificate2("./Certificates/Default.pfx", "default");
            settings.SslSettings = new SslSettings
            {
                ClientCertificates = new List<X509Certificate>() { cert }
            };

            var client = new MongoClient(settings);
            var database = client.GetDatabase("todo-list");
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(data);
        }
    }
}
