using ASPNETCore_DB.Data;

namespace ASPNETCore_DB.Interfaces
{
    public interface IDBInitializer
    {
        void Initialize(SQLiteDBContext context);
    }
}
