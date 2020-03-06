using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.Web.Models
{
    public class PermissionViewModel
    {

        public int ID { get; set; }


        public string RoleId { get; set; }

        public string FunctionId { get; set; }

        public bool CanCreate { set; get; }

        public bool CanRead { set; get; }

        public bool CanReadAll { set; get; }

        public bool CanUpdate { set; get; }

        public bool CanDelete { set; get; }

        public bool CanCancel { set; get; }

        public ApplicationRoleViewModel AppRole { get; set; }

        public FunctionViewModel Function { get; set; }
    }
}