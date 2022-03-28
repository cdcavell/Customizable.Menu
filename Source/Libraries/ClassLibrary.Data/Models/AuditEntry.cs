using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Data.Models
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        [DataType(DataType.Text)]
        public string TableName { get; set; } = String.Empty;
        [DataType(DataType.Text)]
        public string State { get; set; } = String.Empty;
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OriginalValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> CurrentValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public AuditHistory ToAuditHistory()
        {
            return new AuditHistory
            {
                Entity = TableName,
                State = State,
                ModifiedOn = ModifiedOn,
                KeyValues = JsonConvert.SerializeObject(KeyValues),
                OriginalValues = OriginalValues.Count == 0 ? string.Empty : JsonConvert.SerializeObject(OriginalValues),
                CurrentValues = CurrentValues.Count == 0 ? string.Empty : JsonConvert.SerializeObject(CurrentValues)
            };
        }
    }
}
