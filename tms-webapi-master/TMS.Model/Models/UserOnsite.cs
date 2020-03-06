using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model.Models
{
    public class UserOnsite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [MaxLength(128)]
        public string UserID { get; set; }
        [MaxLength(100)]
        public string OnsitePlace { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { set; get; }
        [ForeignKey("UserID")]
        public virtual AppUser AppUser { set; get; }
    }
}
