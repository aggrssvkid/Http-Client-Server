using Client;
namespace Server
{
    class DataBaseImitator
    {
        public List<RequestJsonModel> DataBase { get; } // our datas collection

        public DataBaseImitator() => DataBase = new List<RequestJsonModel>(); // constructor

        public DataBaseImitator(List<RequestJsonModel> dataBase) => DataBase = dataBase; // constructor
        public void AddNote(RequestJsonModel model) => DataBase.Add(model); // add datas to DataBase
        public RequestJsonModel? GetNote(RequestJsonModel model) => DataBase.Find(x => x.Id == model.Id); // get datas from DB
    }
}
