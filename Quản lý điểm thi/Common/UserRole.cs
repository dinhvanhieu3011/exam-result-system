using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quản_lý_điểm_thi.Common
{
    public class UserRole
    {
        public const int Read = 1;
        public const int Edit = 2;
        public const int Delete = 3;
        public const int Create = 4;

        public static List<SelectOption> ListRole = new List<SelectOption>{
            new SelectOption { Value = Edit, Name = "Sửa" },
            new SelectOption { Value = Delete, Name = "Xóa" },
            new SelectOption { Value = Create, Name = "Tạo" }

        };
    }

    public class SelectOption
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }
}