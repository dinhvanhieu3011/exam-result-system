using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quản_lý_điểm_thi.Models
{
    public class StudentModel
    {

        public StudentModel() { }

        public StudentModel(int Id, string tt, string sbd, string ho_ten, string ngay_sinh,
         string gioi_tinh, string coquan_congtac, string truong_hoc, string xeploai_hanhkiem, string xeploai_hocluc,
         string dien_uudai, string tong_so, string ketqua_thi, string xeploai_totnghiep, string ghichu, int ID_Exam_Room,
         string Name_Exam_Room, int ID_Exam, int ID_HD_Thi, string Name_HD_Thi, string create_date, string create_user,
         string pdf, string dantoc)
        {
            this.Id = Id;
            this.tt = tt;
            this.sbd = sbd;
            this.ho_ten = ho_ten;
            this.ngay_sinh = ngay_sinh;
            this.gioi_tinh = gioi_tinh;
            this.coquan_congtac = coquan_congtac;
            this.truong_hoc = truong_hoc;
            this.xeploai_hanhkiem = xeploai_hanhkiem;
            this.xeploai_hocluc = xeploai_hocluc;
            this.dien_uudai = dien_uudai;
            this.tong_so = tong_so;
            this.ketqua_thi = ketqua_thi;
            this.xeploai_totnghiep = xeploai_totnghiep;
            this.ghichu = ghichu;
            this.ID_Exam_Room = ID_Exam_Room;
            this.Name_Exam_Room = Name_Exam_Room;
            this.ID_Exam = ID_Exam;
            this.ID_HD_Thi = ID_HD_Thi;
            this.Name_HD_Thi = Name_HD_Thi;
            this.create_date = create_date;
            this.create_user = create_user;
            this.pdf = pdf;
            this.dantoc = dantoc;
        }

        public int Id { get; set; }
        public string tt { get; set; }
        public string sbd { get; set; }
        public string ho_ten { get; set; }
        public string ngay_sinh { get; set; }
        public string gioi_tinh { get; set; }
        public string coquan_congtac { get; set; }
        public string truong_hoc { get; set; }
        public string xeploai_hanhkiem { get; set; }
        public string xeploai_hocluc { get; set; }
        public string dien_uudai { get; set; }
        public string tong_so { get; set; }
        public string ketqua_thi { get; set; }
        public string xeploai_totnghiep { get; set; }
        public string ghichu { get; set; }
        public int ID_Exam_Room { get; set; }
        public string Name_Exam_Room { get; set; }
        public int ID_Exam { get; set; }
        public int ID_HD_Thi { get; set; }
        public string Name_HD_Thi { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string pdf { get; set; }
        public string dantoc { get; set; }
    }
}