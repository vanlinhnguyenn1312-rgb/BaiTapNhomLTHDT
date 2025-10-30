using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QLTBN
{
    public class BanhHandmadeDTO : BanhDTO, ITroGia
    {
        #region Attributes
        private string dongBanh;
        public string DongBanh { get => dongBanh; set => dongBanh = value; }
        #endregion
        #region Methods
        public BanhHandmadeDTO() : base()
        {
            this.DongBanh = "";
        }
        public BanhHandmadeDTO(string maBanh, string tenBanh, DateTime ngaySanXuat, int hanSuDung, double giaBan, string dongBanh) : base(maBanh, tenBanh, ngaySanXuat, hanSuDung, giaBan)
        {
            DongBanh = dongBanh;
        }
        public override double TinhGiamGia()
        {
            return GiaBan;
        }
        public double TinhGiamGia(int soLuong)
        {
            return soLuong >= 5 ? GiaBan * 0.95 : GiaBan;
        }
        public double TinhTroGia()
        {
            return GiaBan - 2000;
        }
        public override void XuatThongTin()
        {
            base.XuatThongTin();
            Console.WriteLine(" {0,-15} │ {1,-12} │", dongBanh, "Handmade");
        }
        #endregion
    }
}
