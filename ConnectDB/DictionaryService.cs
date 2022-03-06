using MongoDB.Driver;
using TranslatorWebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB;

namespace TranslatorWebAPI.Services
{
    public class DictionaryService
    {
        private readonly IMongoCollection<Dictionary> _dictionnary;
        public DictionaryService(IDictionaryDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _dictionnary = database.GetCollection<Dictionary>(settings.DictionaryCollectionName);

        }

        public List<Dictionary> Get()
        {
            List<Dictionary> dictionaries;
            dictionaries = _dictionnary.Find(emp => true).ToList();
            return dictionaries;
        }

        public List<Dictionary> Get(string name) {
            var filter = new BsonDocument { { "name", name }};
            //tốc độ xử lý quá kém
            return  _dictionnary.Find<Dictionary>(filter).ToList<Dictionary>();
        }

        

    }

}
