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
    [Table(CommonConstants.EntitleDay)]
    public class EntitleDay : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [MaxLength(50)]
        public string HolidayType { set; get; }
        [MaxLength(15)]
        public string UnitType { set; get; }
        public float MaxEntitleDay { set; get; }
        [MaxLength(255)]
        public string Description { set; get; }
    }
}
