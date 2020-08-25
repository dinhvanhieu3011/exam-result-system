using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quản_lý_điểm_thi.Models
{

    /// <summary>
    /// Lop trung gian map menu
    /// </summary>
    public class MnuItem
    {
        public string Id { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string ParentId { get; set; }
        public string OrderIndex { get; set; }
        public string Icon { get; set; }

        /// <summary>
        /// Danh dau menu co duoc chon khong
        /// </summary>
        public bool Checked { get; set; }
    }
}