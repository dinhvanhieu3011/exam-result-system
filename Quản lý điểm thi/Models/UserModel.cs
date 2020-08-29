using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quản_lý_điểm_thi.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ComfirmPassword { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string CMND { get; set; }

        public string OfficeRoom { get; set; }

        public string WorkPlace { get; set; }

        public HttpPostedFileBase Image { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        public int[] Role { get; set; }
        public int UnitId { get; set; }
        public HttpPostedFileBase Avatar { get; set; }
    }
}