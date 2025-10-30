using System;
using System.Collections.Generic;
using System.Linq;
using BLL_QLTBN;
using DTO_QLTBN;

namespace QuanLyTiemBanh.GUI
{
    public class SanPhamGUI
    {
        private BanhBLL banhBLL = new BanhBLL();

        public void HienThiMenuSanPham(string filePath)
        {
            while (true)
            {
                Console.WriteLine("\n=== QUẢN LÝ SẢN PHẨM ===");
                Console.WriteLine("1. Xem danh sách sản phẩm");
                Console.WriteLine("2. Tìm kiếm sản phẩm theo tên bánh");
                Console.WriteLine("3. Xem sản phẩm theo loại");
                Console.WriteLine("4. Xem 5 sản phẩm giá cao nhất");
                Console.WriteLine("5. Xem sản phẩm sản xuất >3 tháng");
                Console.WriteLine("6. Xem sản phẩm có giảm giá");
                Console.WriteLine("7. Cập nhật giá bánh Handmade +3%");
                Console.WriteLine("8. Xem sản phẩm giá >150k");
                Console.WriteLine("0. Quay lại");
                Console.Write("Chọn chức năng: ");

                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        HienThiTatCaSanPham(filePath);
                        break;
                    case "2":
                        TimKiemSanPham(filePath);
                        break;
                    case "3":
                        HienThiSanPhamTheoLoai(filePath);
                        break;
                    case "4":
                        HienThiSanPhamGiaCao(filePath);
                        break;
                    case "5":
                        HienThiSanPhamCu(filePath);
                        break;
                    case "6":
                        HienThiSanPhamGiamGia(filePath);
                        break;
                    case "7":
                        CapNhatGiaBanhHandmade(filePath);
                        break;
                    case "8":
                        HienThiSanPhamGiaTren150(filePath);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        break;
                }
            }
        }

        private void TimKiemSanPham(string filePath)
        {
            Console.WriteLine("\n=== TÌM KIẾM SẢN PHẨM THEO TÊN ===");
            Console.Write("Nhập tên sản phẩm cần tìm: ");
            string tenBanh = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(tenBanh))
            {
                Console.WriteLine("Tên sản phẩm không được để trống!");
                return;
            }

            var ketQua = banhBLL.TimKiemTheoTen(filePath, tenBanh);

            if (ketQua == null || ketQua.Count == 0)
            {
                Console.WriteLine($"\nKhông tìm thấy sản phẩm nào có tên chứa '{tenBanh}'!");
                Console.WriteLine("\nGợi ý: Hãy thử tìm với từ khóa ngắn hơn (VD: 'kem', 'matcha', 'donut')");
                return;
            }

            Console.WriteLine($"\nTìm thấy {ketQua.Count} sản phẩm:");
            Console.WriteLine($"\n{"STT",-5} {"Mã bánh",-10} {"Tên bánh",-30} {"Loại",-15} {"Giá gốc",-12} {"Giá KM",-12} {"Giá TT",-12}");
            Console.WriteLine(new string('=', 110));

            for (int i = 0; i < ketQua.Count; i++)
            {
                var banh = ketQua[i];
                string loai = banh.GetType().Name.Replace("DTO", "");

                double giaSauGiam = banh.TinhGiamGia();

                double giaThanhToan = giaSauGiam;
                if (banh is ITroGia banhTroGia)
                {
                    double troGia = (banh is BanhHandmadeDTO) ? 2000 : 1000;
                    giaThanhToan = giaSauGiam - troGia;
                }

                Console.WriteLine($"{i + 1,-5} {banh.MaBanh,-10} {banh.TenBanh,-30} {loai,-15} {banh.GiaBan,-12:N0} {giaSauGiam,-12:N0} {giaThanhToan,-12:N0}");
            }

            Console.WriteLine(new string('=', 110));
            Console.WriteLine("\nChú thích: Giá TT = Giá thanh toán (đã giảm giá + trợ giá)");
        }

        private void HienThiSanPhamTheoLoai(string filePath)
        {
            while (true)
            {
                Console.WriteLine("\n=== SẢN PHẨM THEO LOẠI ===");
                Console.WriteLine("1. Bánh Kem");
                Console.WriteLine("2. Bánh Ngọt");
                Console.WriteLine("3. Bánh Handmade");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn loại: ");

                string luaChon = Console.ReadLine();
                string loai = "";

                if (luaChon == "0")
                    break;

                switch (luaChon)
                {
                    case "1": loai = "BanhKem"; break;
                    case "2": loai = "BanhNgot"; break;
                    case "3": loai = "BanhHandmade"; break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        continue;
                }

                var banhs = banhBLL.LayBanhTheoLoai(filePath, loai);

                if (banhs == null || banhs.Count == 0)
                {
                    Console.WriteLine($"Không có sản phẩm loại {loai}!");
                    continue; 
                }

                Console.WriteLine($"\nDANH SÁCH {loai.ToUpper()}:");
                Console.WriteLine($"{"Mã",-10} {"Tên bánh",-30} {"Giá gốc",-12} {"Giá KM",-12} {"Chi tiết",-20}");
                Console.WriteLine(new string('=', 90));

                foreach (var banh in banhs)
                {
                    double giaSauGiam = banh.TinhGiamGia();
                    string chiTiet = "";

                    if (banh is BanhKemDTO banhKem)
                        chiTiet = $"Size: {banhKem.KichThuoc}cm";
                    else if (banh is BanhNgotDTO banhNgot)
                        chiTiet = $"BQ: {banhNgot.CachBaoQuan}";
                    else if (banh is BanhHandmadeDTO banhHandmade)
                        chiTiet = $"Dòng: {banhHandmade.DongBanh}";

                    Console.WriteLine($"{banh.MaBanh,-10} {banh.TenBanh,-30} {banh.GiaBan,-12:N0} {giaSauGiam,-12:N0} {chiTiet,-20}");
                }
            }
        }

        public List<BanhDTO> LayTatCaBanh(string filePath)
        {
            return banhBLL.LayTatCaBanh(filePath);
        }

        public List<BanhDTO> LayBanhNgaySXHon3Thang(string filePath)
        {
            return banhBLL.LayBanhNgaySXHon3Thang(filePath);
        }

        private void HienThiTatCaSanPham(string filePath)
        {
            Console.WriteLine("\n=== DANH SÁCH TẤT CẢ SẢN PHẨM ===");

            var banhs = banhBLL.LayTatCaBanh(filePath);

            if (banhs.Count == 0)
            {
                Console.WriteLine("Không có sản phẩm nào!");
                return;
            }

            Console.WriteLine($"{"Mã",-10} {"Tên bánh",-25} {"Loại",-15} {"Giá gốc",-12} {"HSD",-8} {"Ngày HH",-12} {"Giá KM",-12} {"Trợ giá",-12}");
            Console.WriteLine(new string('=', 120));

            foreach (var banh in banhs)
            {
                string loai = banh.GetType().Name.Replace("DTO", "");
                double giaSauGiam = banh.TinhGiamGia();
                double giaThanhToan = giaSauGiam;

                if (banh is ITroGia banhTroGia)
                {
                    double troGia = (banh is BanhHandmadeDTO) ? 2000 : 1000;
                    giaThanhToan = giaSauGiam - troGia;
                }

                Console.WriteLine($"{banh.MaBanh,-10} {banh.TenBanh,-25} {loai,-15} {banh.GiaBan,-12:N0} {banh.HanSuDung,-8} {banh.NgayHetHan():dd/MM/yyyy} {giaSauGiam,-12:N0} {giaThanhToan,-12:N0}");
            }
        }

        private void HienThiSanPhamGiaCao(string filePath)
        {
            Console.WriteLine("\n=== TOP 5 SẢN PHẨM GIÁ CAO NHẤT ===");

            var banhs = banhBLL.LayBanhGiaCaoNhat(filePath, 5);

            if (banhs.Count == 0)
            {
                Console.WriteLine("Không có sản phẩm nào!");
                return;
            }

            for (int i = 0; i < banhs.Count; i++)
            {
                var banh = banhs[i];
                double giaSauKM = banhBLL.TinhGiaBanhSauGiam(banh);
                Console.WriteLine($"{i + 1}. {banh.TenBanh} - {banh.GetType().Name.Replace("DTO", "")} - Giá: {banh.GiaBan:N0} - Sau KM: {giaSauKM:N0}");
            }
        }

        private void HienThiSanPhamCu(string filePath)
        {
            Console.WriteLine("\n=== SẢN PHẨM SẢN XUẤT TRÊN 3 THÁNG ===");

            var banhs = banhBLL.LayBanhNgaySXHon3Thang(filePath);

            if (banhs.Count == 0)
            {
                Console.WriteLine("Không có sản phẩm nào sản xuất trên 3 tháng!");
                return;
            }

            foreach (var banh in banhs)
            {
                int soThang = (int)(DateTime.Now - banh.NgaySanXuat).TotalDays / 30;
                Console.WriteLine($"{banh.MaBanh} - {banh.TenBanh} - Sản xuất: {banh.NgaySanXuat:dd/MM/yyyy} ({soThang} tháng)");
            }
        }

        private void HienThiSanPhamGiamGia(string filePath)
        {
            Console.WriteLine("\n=== SẢN PHẨM ĐƯỢC GIẢM GIÁ ===");

            var banhs = banhBLL.LayTatCaBanh(filePath);
            var banhGiamGia = banhs.Where(b => b.TinhGiamGia() < b.GiaBan).ToList();

            if (banhGiamGia.Count == 0)
            {
                Console.WriteLine("Không có sản phẩm nào được giảm giá!");
                return;
            }

            foreach (var banh in banhGiamGia)
            {
                double giaSauGiam = banh.TinhGiamGia();
                double phanTramGiam = (banh.GiaBan - giaSauGiam) / banh.GiaBan * 100;
                Console.WriteLine($"{banh.MaBanh} - {banh.TenBanh} - Giá gốc: {banh.GiaBan:N0} - Giảm: {phanTramGiam:F1}% - Còn: {giaSauGiam:N0}");
            }
        }

        private void CapNhatGiaBanhHandmade(string filePath)
        {
            Console.WriteLine("\n=== CẬP NHẬT GIÁ BÁNH HANDMADE +3% ===");

            var banhsCu = banhBLL.LayBanhTheoLoai(filePath, "BanhHandmade");
            var giaCu = new Dictionary<string, double>();

            foreach (var banh in banhsCu)
            {
                giaCu[banh.MaBanh] = banh.GiaBan;
            }

            bool thanhCong = banhBLL.CapNhatGiaBanhHandmade(filePath);

            if (thanhCong)
            {
                Console.WriteLine("Đã cập nhật giá thành công!\n");

                var banhsMoi = banhBLL.LayBanhTheoLoai(filePath, "BanhHandmade");

                if (banhsMoi.Count > 0)
                {
                    Console.WriteLine("DANH SÁCH BÁNH HANDMADE SAU KHI CẬP NHẬT:");
                    Console.WriteLine($"{"Mã",-10} {"Tên bánh",-30} {"Giá cũ",-12} {"Giá mới (+3%)",-15} {"Chi tiết",-30}");
                    Console.WriteLine(new string('=', 100));

                    foreach (var banh in banhsMoi)
                    {
                        if (banh is BanhHandmadeDTO banhHandmade)
                        {
                            double giaCuValue = giaCu.ContainsKey(banh.MaBanh) ? giaCu[banh.MaBanh] : 0;
                            Console.WriteLine($"{banh.MaBanh,-10} {banh.TenBanh,-30} {giaCuValue,-12:N0} {banh.GiaBan,-15:N0} Dòng: {banhHandmade.DongBanh}");
                        }
                    }

                    Console.WriteLine($"\nTổng số bánh Handmade: {banhsMoi.Count}");
                }
                else
                {
                    Console.WriteLine("Không có bánh Handmade nào trong hệ thống!");
                }
            }
            else
            {
                Console.WriteLine("Cập nhật thất bại!");
            }
        }

        private void HienThiSanPhamGiaTren150(string filePath)
        {
            Console.WriteLine("\n=== SẢN PHẨM CÓ GIÁ TRÊN 150K ===");

            var banhs = banhBLL.LayBanhTheoGia(filePath, 150000);

            if (banhs.Count == 0)
            {
                Console.WriteLine("Không có sản phẩm nào có giá trên 150k!");
                return;
            }

            Console.WriteLine($"{"Mã",-10} {"Tên bánh",-25} {"Loại",-15} {"Giá bán",-12}");
            Console.WriteLine(new string('=', 70));

            foreach (var banh in banhs)
            {
                Console.WriteLine($"{banh.MaBanh,-10} {banh.TenBanh,-25} {banh.GetType().Name.Replace("DTO", ""),-15} {banh.GiaBan,-12:N0}");
            }
        }
    }
}