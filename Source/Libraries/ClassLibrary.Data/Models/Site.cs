using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Data.Models
{
    public class Site : DataModel<Site>
    {
        #region properties

        [DataType(DataType.Text)]
        [MinLength(3, ErrorMessage = "Description value must contain at least 3 characters.")]
        [MaxLength(256, ErrorMessage = "Description value cannot exceed 256 characters.")]
        public string Description { get; set; } = string.Empty;

        #endregion

        #region relationships

        public Guid MenuId { get; set; }
        public Menu Menu { get; set; } = new();

        #endregion
    }
}
