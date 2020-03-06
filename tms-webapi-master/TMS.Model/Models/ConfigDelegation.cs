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
    [Table(CommonConstants.ConfigDelegations)]
    public class ConfigDelegation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [MaxLength(128)]
        public string AssignTo { get; set; }
        [MaxLength(128)]
        public string UserId { set; get; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }

    }
}
