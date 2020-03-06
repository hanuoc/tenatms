using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using TMS.Common.Constants;
using TMS.Model.Abstract;

namespace TMS.Model.Models
{
    [Table(CommonConstants.AppUsers)]
    public class AppUser : IdentityUser 
    {
        [MaxLength(50)]
        public string FullName { set; get; }
        [MaxLength(20)]
        public string EmployeeID { set; get; }
        public DateTime? BirthDay { set; get; }
        public bool? Status { get; set; }
        public bool? Gender { get; set; }
        public int? GroupId { set; get; }
        public DateTime? ResignationDate { set; get; }
        [ForeignKey("GroupId")]
        public virtual Group Group { set; get; }
        public int? ChildcareLeaveID { set; get; }
        [ForeignKey("ChildcareLeaveID")]
        public virtual ChildcareLeave ChildcareLeave { set; get; }
        //[MaxLength(15)]
        //public string EmployeeNo { set; get; }
        //[MaxLength(50)]
        //public string AccNameInMachineFinger { set; get; }
        public DateTime StartWorkingDay { set; get; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        //public virtual IEnumerable<Order> Orders { set; get; }
    }
}