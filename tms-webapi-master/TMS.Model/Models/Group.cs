using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Model.Abstract;

namespace TMS.Model.Models
{
    [Table(CommonConstants.Groups)]
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        public IEnumerable<AppUser> AppUsers { set; get; }
        [MaxLength((128))]
        public string DelegateId { set; get; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }

    }
}
