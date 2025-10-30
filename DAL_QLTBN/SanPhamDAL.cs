using System.Collections.Generic;
using DTO_QLTBN;
using System.Linq;
using System;

namespace DAL_QLTBN
{
    public class SanPhamDAL
    {
        private DSKhachHangDAL _khachHangDAL;

        public SanPhamDAL()
        {
            _khachHangDAL = new DSKhachHangDAL();
        }

        public List<BanhDTO> LayTatCaBanh(string filename)
        {
            try
            {
                var khachHangs = _khachHangDAL.DocDanhSachKH(filename);
                var tatCaBanh = new List<BanhDTO>();

                if (khachHangs != null && khachHangs.Count > 0)
                {
                    foreach (var khachHang in khachHangs)
                    {
                        if (khachHang.DanhSachBanh != null)
                        {
                            tatCaBanh.AddRange(khachHang.DanhSachBanh);
                        }
                    }
                }

                return tatCaBanh;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách bánh: {ex.Message}");
                return new List<BanhDTO>();
            }
        }

        public List<BanhDTO> TimBanhTheoTen(string filename, string tenBanh)
        {
            try
            {
                var tatCaBanh = LayTatCaBanh(filename);

                if (tatCaBanh == null || tatCaBanh.Count == 0)
                {
                    Console.WriteLine("Không có sản phẩm nào trong hệ thống!");
                    return new List<BanhDTO>();
                }

                string tuKhoa = tenBanh?.Trim().ToLower() ?? "";

                if (string.IsNullOrWhiteSpace(tuKhoa))
                {
                    return new List<BanhDTO>();
                }

                var ketQua = tatCaBanh.Where(b =>
                    !string.IsNullOrWhiteSpace(b.TenBanh) &&
                    b.TenBanh.Trim().ToLower().Contains(tuKhoa)
                ).ToList();

                return ketQua;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tìm kiếm: {ex.Message}");
                return new List<BanhDTO>();
            }
        }
        public List<BanhDTO> LayBanhTheoLoai(string filename, string loaiBanh)
        {
            try
            {
                var tatCaBanh = LayTatCaBanh(filename);
                return tatCaBanh.Where(b => b.GetType().Name == loaiBanh + "DTO").ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy bánh theo loại: {ex.Message}");
                return new List<BanhDTO>();
            }
        }
    }
}