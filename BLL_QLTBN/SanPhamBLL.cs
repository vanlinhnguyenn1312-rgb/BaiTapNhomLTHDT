using System;
using System.Collections.Generic;
using System.Linq;
using DAL_QLTBN;
using DTO_QLTBN;

namespace BLL_QLTBN
{
    public class BanhBLL
    {
        private SanPhamDAL _banhDAL;

        public BanhBLL()
        {
            _banhDAL = new SanPhamDAL();
        }

        public List<BanhDTO> LayTatCaBanh(string filename)
        {
            return _banhDAL.LayTatCaBanh(filename);
        }

        public List<BanhDTO> TimKiemTheoTen(string filename, string tenBanh)
        {
            return _banhDAL.TimBanhTheoTen(filename, tenBanh);
        }

        public List<BanhDTO> LayBanhTheoGia(string filename, double giaMin)
        {
            var tatCaBanh = LayTatCaBanh(filename);
            return tatCaBanh.Where(b => b.GiaBan >= giaMin).ToList();
        }

        public List<BanhDTO> LayBanhTheoLoai(string filename, string loaiBanh)
        {
            return _banhDAL.LayBanhTheoLoai(filename, loaiBanh);
        }

        public List<BanhDTO> LayBanhNgaySXHon3Thang(string filename)
        {
            var tatCaBanh = LayTatCaBanh(filename);
            return tatCaBanh.Where(b => (DateTime.Now - b.NgaySanXuat).TotalDays > 90).ToList();
        }

        public List<BanhDTO> LayBanhGiaCaoNhat(string filename, int soLuong = 5)
        {
            var tatCaBanh = LayTatCaBanh(filename);
            return tatCaBanh.OrderByDescending(b => b.GiaBan).Take(soLuong).ToList();
        }

        public double TinhGiaBanhSauGiam(BanhDTO banh)
        {
            double giaSauGiam = banh.TinhGiamGia();
            if (banh is ITroGia banhTroGia)
            {
                giaSauGiam = banhTroGia.TinhTroGia();
            }
            return giaSauGiam;
        }
        public bool CapNhatGiaBanhHandmade(string filename)
        {
            try
            {
                var khachHangDAL = new DSKhachHangDAL();
                var danhSachKH = khachHangDAL.DocDanhSachKH(filename);

                foreach (var kh in danhSachKH)
                {
                    foreach (var banh in kh.DanhSachBanh)
                    {
                        if (banh is BanhHandmadeDTO)
                        {
                            banh.GiaBan *= 1.03; 
                        }
                    }
                    kh.TinhTongTien();
                }

                return khachHangDAL.GhiDanhSachKH(filename, danhSachKH);
            }
            catch
            {
                return false;
            }
        }
    }
}