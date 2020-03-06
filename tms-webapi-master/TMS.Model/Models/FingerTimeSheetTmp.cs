using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Constants;

namespace TMS.Model.Models
{
    [Table(CommonConstants.FingerTimeSheetTmps)]
    public class FingerTimeSheetTmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public string UserNo { set; get; }  
        public DateTime Date { set; get; }    
        public int NumberFinger { set; get; }
        public string AccName { set; get; }
    }
}
