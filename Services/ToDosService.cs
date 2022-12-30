using ToDo_List_with_additions.Models;
using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;

namespace ToDo_List_with_additions.Services
{
    public class ToDosService : IToDosService
    {
        private readonly IMongoCollection<ToDoModel> toDos;
        public ToDosService(IConfiguration config)
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
            toDos = database.GetCollection<ToDoModel>("todos");
        }
        public List<ToDoModel> GetToday(string userId)
        {
            var today = DateTime.Now.Date;
            var filter = Builders<ToDoModel>.Filter.Eq(t => t.UserId, userId) & Builders<ToDoModel>.Filter.Eq(t => t.Done, false) & Builders<ToDoModel>.Filter.Gte(t => t.Date, today) & Builders<ToDoModel>.Filter.Lt(t => t.Date, today.AddDays(1));
            var toDoList = toDos.Find(filter).ToList();
            toDoList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            toDoList.Reverse();
			return toDoList;
        }
        public List<ToDoModel> GetOthers(string userId)
        {
            var today = DateTime.Now.Date;
            var filter = Builders<ToDoModel>.Filter.Eq(t => t.UserId, userId) & Builders<ToDoModel>.Filter.Eq(t => t.Done, false) & Builders<ToDoModel>.Filter.Gt(t => t.Date, today);
            var toDoList = toDos.Find(filter).ToList();
            toDoList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            toDoList.Reverse();
            return toDoList;
        }
        public List<ToDoModel> GetDone(string userId)
        {
            var filter = Builders<ToDoModel>.Filter.Eq(t => t.UserId, userId) & Builders<ToDoModel>.Filter.Eq(t => t.Done, true);
            var toDoList = toDos.Find(filter).ToList();
            toDoList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            toDoList.Reverse();
            return toDoList;
        }
        public ToDoModel GetToDo(string id)
        {
            ToDoModel toDo = toDos.Find<ToDoModel>(ToDo => ToDo.Id == id).FirstOrDefault();
            return toDo;
        }
        public ToDoModel Create(ToDoModel toDo)
        {
            toDos.InsertOne(toDo);
            return toDo;
        }
        public ToDoModel Edit(ToDoModel toDo)
        {
            toDos.ReplaceOne(ToDo => ToDo.Id == toDo.Id, toDo);
            return toDo;
        }
        public ToDoModel Delete(string id)
        {
            ToDoModel toDo = toDos.Find<ToDoModel>(ToDo => ToDo.Id == id).FirstOrDefault();
            toDos.DeleteOne(ToDo => ToDo.Id == id);
            return toDo;
        }
        public ToDoModel Done(string id)
        {
            ToDoModel toDo = toDos.Find<ToDoModel>(ToDo => ToDo.Id == id).FirstOrDefault();
            toDo.Done = true;
            toDos.ReplaceOne(ToDo => ToDo.Id == toDo.Id, toDo);
            return toDo;
        }
    }
}
