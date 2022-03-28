using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ClassLibrary.Data.Models
{
    [Table("AuditHistory")]
    public class AuditHistory : DataModel<AuditHistory>
    {
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; } = DateTime.MinValue;
        [DataType(DataType.Text)]
        public string Entity { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string State { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string KeyValues { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string OriginalValues { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string CurrentValues { get; set; } = string.Empty;
    }
}
