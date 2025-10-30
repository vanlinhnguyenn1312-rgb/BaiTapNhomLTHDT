using System;
using System.Xml.Serialization;

namespace DTO_QLTBN
{
    public abstract class BanhDTO
    {
        #region Attributes

        private string maBanh;
        private string tenBanh;
        private DateTime ngaySanXuat;
        private int hanSuDung;
        private double giaBan;
        public string MaBanh { get => maBanh; set => maBanh = value; }
        public string TenBanh { get => tenBanh; set => tenBanh = value; }
        public DateTime NgaySanXuat { get => ngaySanXuat; set => ngaySanXuat = value; }
        public int HanSuDung { get => hanSuDung; set => hanSuDung = value; }
        public double GiaBan { get => giaBan; set => giaBan = value; }
        #endregion
        #region Methods
        public BanhDTO() {
            this.MaBanh = "";
            this.TenBanh = "";
            this.NgaySanXuat = new DateTime();
            this.HanSuDung = 0;
            this.GiaBan = 0;
        }
        public BanhDTO(string maBanh, string tenBanh, DateTime ngaySanXuat, int hanSuDung, double giaBan)
        {
            MaBanh = maBanh;
            TenBanh = tenBanh;
            NgaySanXuat = ngaySanXuat;
            HanSuDung = hanSuDung;
            GiaBan = giaBan;
        }
        public DateTime NgayHetHan()
        {
            return NgaySanXuat.AddDays(HanSuDung);
        }
        public int SoNgayConLai()
        {
            return (int)(NgayHetHan() - DateTime.Now).TotalDays;
        }
        public abstract double TinhGiamGia();
        public virtual void XuatThongTin()
        {
            Console.Write("│ {0,-25} │ {1,-12:dd/MM/yyyy} │ {2,-12:dd/MM/yyyy} │ {3,15:N0} │ {4,15:N0} │",
                TenBanh, NgaySanXuat, HanSuDung, GiaBan, TinhGiamGia());
        }
        #endregion
    }
}