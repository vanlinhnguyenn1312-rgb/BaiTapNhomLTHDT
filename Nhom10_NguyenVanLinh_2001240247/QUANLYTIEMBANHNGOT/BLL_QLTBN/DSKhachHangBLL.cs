using System;
using System.Collections.Generic;
using System.Linq;
using DAL_QLTBN;
using DTO_QLTBN;

namespace BLL_QLTBN
{
    public class KhachHangBLL
    {
        private DSKhachHangDAL _khachHangDAL;
        public KhachHangBLL()
        {
            _khachHangDAL = new DSKhachHangDAL();
        }

        public List<KhachHangDTO> LayTatCaKhachHang(string filename)
        {
            return _khachHangDAL.DocDanhSachKH(filename);
        }
        public bool GhiDanhSachKH(string filename, List<KhachHangDTO> danhSachKH)
        {
            try
            {
                return _khachHangDAL.GhiDanhSachKH(filename, danhSachKH);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi ghi danh sách khách hàng: {ex.Message}", ex);
            }
        }
        public bool LuuDanhSachKhachHang(string filename)
        {
            try
            {
                var danhSach = _khachHangDAL.LstKH;
                return _khachHangDAL.GhiDanhSachKH(filename, danhSach);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu danh sách khách hàng: {ex.Message}");
            }
        }

        public List<KhachHangDTO> TimKiemKhachHang(string filename, string tuKhoa)
        {
            var danhSach = LayTatCaKhachHang(filename);
            return danhSach.Where(kh =>
                kh.MaKH.ToLower().Contains(tuKhoa.ToLower()) ||
                kh.TenKH.ToLower().Contains(tuKhoa.ToLower()) ||
                kh.SoDienThoai.Contains(tuKhoa)).ToList();
        }

        public List<KhachHangDTO> LayKhachHangMuaNhieuHon3SanPham(string filename)
        {
            var danhSach = LayTatCaKhachHang(filename);
            return danhSach.Where(kh => kh.DanhSachBanh.Count > 3).ToList();
        }

        public KhachHangDTO LayKhachHangMuaNhieuTienNhat(string filename)
        {
            var danhSach = LayTatCaKhachHang(filename);
            return danhSach.OrderByDescending(kh => kh.TongTien).FirstOrDefault();
        }
        public bool ThemDonHangMoi(string filePath, KhachHangDTO khachHangMoi)
        {
            try
            {
                var danhSach = LayTatCaKhachHang(filePath);
                danhSach.Add(khachHangMoi);
                return _khachHangDAL.GhiDanhSachKH(filePath, danhSach);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return false;
            }
        }

        public double TinhTongDoanhThu(string filename)
        {
            var danhSach = LayTatCaKhachHang(filename);
            return danhSach.Sum(kh => kh.TongTien);
        }

        private double TinhTongTienMuaHang(KhachHangDTO khachHang)
        {
            double tongTien = 0;
            foreach (var banh in khachHang.DanhSachBanh)
            {
                double giaSauGiam = banh.TinhGiamGia();

                if (banh is ITroGia banhTroGia)
                {
                    giaSauGiam = banhTroGia.TinhTroGia();
                }

                tongTien += giaSauGiam;
            }
            return tongTien;
        }

        public void ThemKhachHang(KhachHangDTO khachHang)
        {
            _khachHangDAL.ThemKhachHang(khachHang);
        }

        public bool XoaKhachHang(string maKH)
        {
            return _khachHangDAL.XoaKhachHang(maKH);
        }

        public KhachHangDTO TimKhachHangTheoMa(string maKH)
        {
            return _khachHangDAL.TimKhachHangTheoMa(maKH);
        }
    }
}