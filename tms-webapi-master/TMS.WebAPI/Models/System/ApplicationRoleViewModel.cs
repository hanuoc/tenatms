﻿using System.ComponentModel.DataAnnotations;

namespace TMS.Web.Models
{
    public class ApplicationRoleViewModel
    {
        public string Id { set; get; }

        [Required(ErrorMessage = "Bạn phải nhập tên")]
        public string Name { set; get; }

        public string Description { set; get; }

    }
}