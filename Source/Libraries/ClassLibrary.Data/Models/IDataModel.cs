namespace ClassLibrary.Data.Models
{
    public interface IDataModel<T>
    {
        Guid Guid { get; set; }

        bool IsNew { get; }

        void AddUpdate(ApplicationDbContext dbContext);
        void Delete(ApplicationDbContext dbContext);
        bool Equals(T obj);
    }
}
