using System;
using DTO_QLTBN;

namespace QuanLyTiemBanh.GUI
{
    class Program
    {
        private static KhachHangGUI khachHangGUI = new KhachHangGUI();
        private static SanPhamGUI sanPhamGUI = new SanPhamGUI();
        private static string filePath = "../../../Data/TTHoaDon.xml";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                HienThiMenuChinh();
                Console.Write("Chọn chức năng: ");
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        khachHangGUI.HienThiMenuKhachHang(filePath);
                        break;
                    case "2":
                        sanPhamGUI.HienThiMenuSanPham(filePath);
                        break;
                    case "3":
                        ThongKeTongQuan();
                        break;
                    case "0":
                        Console.WriteLine("Thoát chương trình!");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        break;
                }
            }
        }

        static void HienThiMenuChinh()
        {
            Console.WriteLine("\n=== QUẢN LÝ TIỆM BÁNH NGỌT ===");
            Console.WriteLine("1. Quản lý Khách hàng");
            Console.WriteLine("2. Quản lý Sản phẩm");
            Console.WriteLine("3. Thống kê tổng quan");
            Console.WriteLine("0. Thoát");
        }

        static void ThongKeTongQuan()
        {
            Console.WriteLine("\n=== THỐNG KÊ TỔNG QUAN ===");

            var khachHangs = khachHangGUI.LayDanhSachKhachHang(filePath);
            var banhs = sanPhamGUI.LayTatCaBanh(filePath);

            Console.WriteLine($"Tổng số khách hàng: {khachHangs.Count}");
            Console.WriteLine($"Tổng số sản phẩm: {banhs.Count}");

            var khachMuaNhieu = khachHangGUI.LayKhachHangMuaNhieuHon3SP(filePath);
            Console.WriteLine($"Khách mua >3 sản phẩm: {khachMuaNhieu.Count}");

            var khachMuaNhieuTien = khachHangGUI.LayKhachHangMuaNhieuTienNhat(filePath);
            Console.WriteLine($"Khách mua nhiều tiền nhất: {khachMuaNhieuTien?.TenKH}");

            var banhCu = sanPhamGUI.LayBanhNgaySXHon3Thang(filePath);
            Console.WriteLine($"Sản phẩm sản xuất >3 tháng: {banhCu.Count}");

            double doanhThu = 0;
            foreach (var kh in khachHangs)
            {
                doanhThu += kh.TongTien;
            }
            Console.WriteLine($"Tổng doanh thu: {doanhThu:N0} VNĐ");
        }
    }
}