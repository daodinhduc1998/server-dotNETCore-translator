namespace TranslatorWebAPI.Models
{
    public class DictionaryDatabaseSettings : IDictionaryDatabaseSettings
    {
        public string DictionaryCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDictionaryDatabaseSettings
    {
        public string DictionaryCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
