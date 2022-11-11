using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Xml.Xsl;

namespace ToDo_List_with_additions.Services
{
    class Mongo
    {
        public async Task MainAsync()
        {
            var connectionString = "mongodb+srv://cluster0.btywazj.mongodb.net/?authSource=%24external&authMechanism=MONGODB-X509&retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            // You will need to convert your Atlas-provided PEM containing the cert/private keys into a PFX
            // use openssl and the following line to create a PFX from your PEM:
            // openssl pkcs12 -export -in <x509>.pem -inkey <x509>.pem -out <x509>.pfx
            // and provide a password, which should match the second argument you pass to X509Certificate2
            var cert = new X509Certificate2("./Certificates/Default.perm");

            settings.SslSettings = new SslSettings
            {
                ClientCertificates = new List<X509Certificate>() { cert }
            };

            var client = new MongoClient(settings);
            var database = client.GetDatabase("sample_mflix");
            var collection = database.GetCollection<BsonDocument>("users");

            var document = new BsonDocument
            {
                { "item", "canvas" },
                { "qty", 100 },
                { "tags", new BsonArray { "cotton" } },
                { "size", new BsonDocument { { "h", 28 }, { "w", 35.5 }, { "uom", "cm" } } }
            };
            collection.InsertOne(document);

            var docCount = collection.CountDocuments("{}");
            Console.WriteLine(docCount);
        }
    }
}