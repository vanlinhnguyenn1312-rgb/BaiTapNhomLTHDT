using System;
using System.Collections.Generic;
using System.Linq;

namespace DTO_QLTBN
{
    public class KhachHangDTO
    {
        private string maKH;
        private string tenKH;
        private string soDienThoai;
        private List<BanhDTO> danhSachBanh;
        private double tongTien;

        public string MaKH { get => maKH; set => maKH = value; }
        public string TenKH { get => tenKH; set => tenKH = value; }
        public string SoDienThoai { get => soDienThoai; set => soDienThoai = value; }
        public List<BanhDTO> DanhSachBanh { get => danhSachBanh; set => danhSachBanh = value; }
        public double TongTien { get => tongTien; set => tongTien = value; }

        public KhachHangDTO()
        {
            this.MaKH = "";
            this.TenKH = "";
            this.SoDienThoai = "";
            this.DanhSachBanh = new List<BanhDTO>();
            this.TongTien = 0;
        }

        public KhachHangDTO(string maKH, string tenKH, string sdt, List<BanhDTO> dsb, double tt)
        {
            MaKH = maKH;
            TenKH = tenKH;
            SoDienThoai = sdt;
            DanhSachBanh = dsb;
            TongTien = tt;
        }
        public void TinhTongTien()
        {
            TongTien = 0;

            int soLuongHandmade = DanhSachBanh.Count(b => b is BanhHandmadeDTO);

            foreach (var banh in DanhSachBanh)
            {
                double giaThanhToan = banh.GiaBan;

                if (banh is BanhHandmadeDTO banhHandmade)
                {
                    giaThanhToan = soLuongHandmade >= 5 ? banh.GiaBan * 0.95 : banh.GiaBan;
                }
                else
                {
                    giaThanhToan = banh.TinhGiamGia();
                }

                if (banh is ITroGia banhTroGia)
                {
                    giaThanhToan = giaThanhToan - (banh is BanhHandmadeDTO ? 2000 : 1000);
                }

                TongTien += giaThanhToan;
            }
        }

        public void XuatThongTin()
        {
            Console.WriteLine("│ {0} │ {1} │ {2} │ {3} ",
                maKH, tenKH, soDienThoai, danhSachBanh.Count);
        }
    }
}