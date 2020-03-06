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
    //[Table(CommonConstants.ListOT)]
    class ListOT { }
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int ID { set; get; }

    //    [Required]
    //    [MaxLength(128)]
    //    public string IDOTRequest { set; get; }

    //    [Required]
    //    [MaxLength(128)]
    //    public string IDTimeSheet { set; get; }

    //    [ForeignKey("IDOTRequest")]
    //    public virtual OTRequest OTRequest { set; get; }

    //    [ForeignKey("IDTimeSheet")]
    //    public virtual TimeSheet TimeSheet { set; get; }
    //}
}
