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
    [Table(CommonConstants.AbnormalReasons)]
    public class AbnormalReason : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public string Name { set; get; }
        public IEnumerable<AbnormalCase> AbnormalCase { set; get; }
    }
}
