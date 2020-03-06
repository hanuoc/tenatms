using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.Model.Abstract;

namespace TMS.Model.Models
{
    public class Holiday:Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [MaxLength(256)]
        public string Note { set; get; }
        public DateTime Date { set; get; }
        public DateTime? Workingday { set; get; }
    }
}
