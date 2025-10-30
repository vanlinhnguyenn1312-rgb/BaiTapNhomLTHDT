using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QLTBN
{
    public class BanhNgotDTO : BanhDTO, ITroGia
    {
        #region Attributes
        private string cachBaoQuan;
        public string CachBaoQuan { get => cachBaoQuan; set => cachBaoQuan = value; }
        #endregion
        #region Methods
        public BanhNgotDTO() : base()
        {
            this.CachBaoQuan = "";
        }
        public BanhNgotDTO(string maBanh, string tenBanh, DateTime ngaySanXuat, int hanSuDung, double giaBan, string cachBaoQuan) : base(maBanh, tenBanh, ngaySanXuat, hanSuDung, giaBan)
        {
            CachBaoQuan = cachBaoQuan;
        }
        public override double TinhGiamGia()
        {
            int soNgayConLai = SoNgayConLai();
            return soNgayConLai <= 2 ? GiaBan * 0.7 : GiaBan;
        }
        public double TinhTroGia()
        {
            return GiaBan - 1000;
        }
        public override void XuatThongTin()
        {
            base.XuatThongTin();
            Console.WriteLine(" {0,-15} │ {1,-12} │", CachBaoQuan, "Bánh Ngọt");
        }
        #endregion
    }
}
