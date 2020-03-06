using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.Common.Constants;

namespace TMS.Model.Models
{
    [Table(CommonConstants.Permissions)]
    public class Permission
    {
        [Key]
        public int ID { get; set; }

        [StringLength(128)]
        public string RoleId { get; set; }

        [StringLength(50)]
        [Column(TypeName ="varchar")]
        public string FunctionId { get; set; }

        public bool CanCreate { set; get; } 

        public bool CanRead { set; get; }

        public bool CanReadAll { set; get; }

        public bool CanUpdate { set; get; }

        public bool CanDelete { set; get; }

        public bool CanCancel { set; get; }

        [ForeignKey("RoleId")]
        public AppRole AppRole { get; set; }

        [ForeignKey("FunctionId")]
        public Function Function { get; set; }
    }
}