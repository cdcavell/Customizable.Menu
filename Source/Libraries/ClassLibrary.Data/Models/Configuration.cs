using ClassLibrary.Common.Encryption;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Data.Models
{
    [Table("Configuration")]
    public class Configuration : DataModel<Configuration>
    {
        #region properties

        public byte[] EncryptionKey { get; set; } = AESGCM.NewKey();

        #endregion
    }
}
