using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Data.Models
{
    public abstract partial class DataModel<T> : IDataModel<DataModel<T>> where T : DataModel<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; } = Guid.Empty;

        [NotMapped]
        public bool IsNew
        {
            get
            {
                if (this.Guid == Guid.Empty)
                    return true;

                return false;
            }
        }

        public virtual void AddUpdate(ApplicationDbContext dbContext)
        {
            if (this.IsNew)
            {
                this.Guid = Guid.NewGuid();
                dbContext.Add<DataModel<T>>(this);
            }
            else
                dbContext.Update<DataModel<T>>(this);

            dbContext.SaveChanges();
        }

        public virtual void Delete(ApplicationDbContext dbContext)
        {
            if (!this.IsNew)
            {
                dbContext.Attach<DataModel<T>>(this);
                dbContext.Remove<DataModel<T>>(this);
                dbContext.SaveChanges();
            }
        }

        public virtual bool Equals(DataModel<T> obj)
        {
            return (this == obj);
        }
    }
}
