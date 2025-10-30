using System;
using System.Collections.Generic;
using System.Linq;
using BLL_QLTBN;
using DTO_QLTBN;

namespace QuanLyTiemBanh.GUI
{
    public class KhachHangGUI
    {
        private KhachHangBLL khachHangBLL = new KhachHangBLL();
        public void HienThiMenuKhachHang(string filePath)
        {
            while (true)
            {
                Console.WriteLine("\n=== QUẢN LÝ KHÁCH HÀNG ===");
                Console.WriteLine("1. Xem danh sách khách hàng");
                Console.WriteLine("2. Tìm kiếm khách hàng");
                Console.WriteLine("3. Thêm khách hàng mới");
                Console.WriteLine("4. Xem khách hàng mua nhiều >3 SP");
                Console.WriteLine("5. Xem khách hàng mua nhiều tiền nhất");
                Console.WriteLine("6. Xem chi tiết đơn hàng của khách");
                Console.WriteLine("7. Xuất SP đã mua theo tên KH"); 
                Console.WriteLine("0. Quay lại");
                Console.Write("Chọn chức năng: ");
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        HienThiDanhSachKhachHang(filePath);
                        break;
                    case "2":
                        TimKiemKhachHang(filePath);
                        break;
                    case "3":
                        ThemKhachHangMoi(filePath);
                        break;
                    case "4":
                        HienThiKhachHangMuaNhieu(filePath);
                        break;
                    case "5":
                        HienThiKhachHangMuaNhieuTienNhat(filePath);
                        break;
                    case "6":
                        HienThiChiTietDonHang(filePath);
                        break;
                    case "7": 
                        XuatSanPhamTheoTenKhachHang(filePath);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        break;
                }
            }
        }

        private void XuatSanPhamTheoTenKhachHang(string filePath)
        {
            Console.WriteLine("\n=== XUẤT SẢN PHẨM ĐÃ MUA THEO TÊN KHÁCH HÀNG ===");

            Console.Write("Nhập tên khách hàng: ");
            string tenKH = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(tenKH))
            {
                Console.WriteLine("Tên khách hàng không được để trống!");
                return;
            }

            var khachHangs = khachHangBLL.LayTatCaKhachHang(filePath);
            var ketQua = khachHangs.Where(kh =>
                kh.TenKH.ToLower().Contains(tenKH.ToLower())).ToList();

            if (ketQua.Count == 0)
            {
                Console.WriteLine($"Không tìm thấy khách hàng nào có tên chứa '{tenKH}'!");
                return;
            }

            if (ketQua.Count > 1)
            {
                Console.WriteLine($"\nTìm thấy {ketQua.Count} khách hàng:");
                for (int i = 0; i < ketQua.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {ketQua[i].MaKH} - {ketQua[i].TenKH} - {ketQua[i].SoDienThoai}");
                }

                Console.Write("\nChọn khách hàng (số thứ tự): ");
                if (int.TryParse(Console.ReadLine(), out int stt) && stt > 0 && stt <= ketQua.Count)
                {
                    HienThiChiTietSanPham(ketQua[stt - 1]);
                }
                else
                {
                    Console.WriteLine("Lựa chọn không hợp lệ!");
                }
            }
            else
            {
                HienThiChiTietSanPham(ketQua[0]);
            }
        }


        private void HienThiChiTietSanPham(KhachHangDTO khachHang)
        {
            Console.WriteLine($"\n{'=',-80}");
            Console.WriteLine($"DANH SÁCH SẢN PHẨM ĐÃ MUA CỦA KHÁCH HÀNG: {khachHang.TenKH.ToUpper()}");
            Console.WriteLine($"Mã KH: {khachHang.MaKH}");
            Console.WriteLine($"SĐT: {khachHang.SoDienThoai}");
            Console.WriteLine($"{'=',-80}");

            if (khachHang.DanhSachBanh.Count == 0)
            {
                Console.WriteLine("Khách hàng chưa mua sản phẩm nào!");
                return;
            }

            Console.WriteLine($"\n{"STT",-5} {"Mã bánh",-10} {"Tên bánh",-25} {"Loại",-15} {"Giá gốc",-12} {"Giá sau KM",-12} {"Giá TT",-12}");
            Console.WriteLine(new string('-', 100));

            int stt = 1;
            double tongTien = 0;

            foreach (var banh in khachHang.DanhSachBanh)
            {
                string loai = banh.GetType().Name.Replace("DTO", "");
                double giaSauGiam = banh.TinhGiamGia();
                double giaThanhToan = giaSauGiam;

                if (banh is ITroGia banhTroGia)
                {
                    giaThanhToan = banhTroGia.TinhTroGia();
                }

                Console.WriteLine($"{stt,-5} {banh.MaBanh,-10} {banh.TenBanh,-25} {loai,-15} {banh.GiaBan,-12:N0} {giaSauGiam,-12:N0} {giaThanhToan,-12:N0}");

                tongTien += giaThanhToan;
                stt++;
            }

            Console.WriteLine(new string('-', 100));
            Console.WriteLine($"{"TỔNG CỘNG:",-72} {tongTien,-12:N0} VNĐ");
            Console.WriteLine($"Số lượng sản phẩm: {khachHang.DanhSachBanh.Count}");
        }


        public List<KhachHangDTO> LayDanhSachKhachHang(string filePath)
        {
            return khachHangBLL.LayTatCaKhachHang(filePath);
        }

        public List<KhachHangDTO> LayKhachHangMuaNhieuHon3SP(string filePath)
        {
            return khachHangBLL.LayKhachHangMuaNhieuHon3SanPham(filePath);
        }

        public KhachHangDTO LayKhachHangMuaNhieuTienNhat(string filePath)
        {
            return khachHangBLL.LayKhachHangMuaNhieuTienNhat(filePath);
        }

        private void HienThiDanhSachKhachHang(string filePath)
        {
            Console.WriteLine("\n=== DANH SÁCH KHÁCH HÀNG ===");
            var khachHangs = khachHangBLL.LayTatCaKhachHang(filePath);
            if (khachHangs.Count == 0)
            {
                Console.WriteLine("Không có khách hàng nào!");
                return;
            }
            Console.WriteLine($"{"Mã KH",-10} {"Tên KH",-20} {"SĐT",-15} {"Số SP",-8} {"Tổng tiền",-12}");
            Console.WriteLine(new string('=', 70));
            foreach (var kh in khachHangs)
            {
                Console.WriteLine($"{kh.MaKH,-10} {kh.TenKH,-20} {kh.SoDienThoai,-15} {kh.DanhSachBanh.Count,-8} {kh.TongTien,-12:N0}");
            }
        }

        private void TimKiemKhachHang(string filePath)
        {
            Console.WriteLine("\n=== TÌM KIẾM KHÁCH HÀNG ===");
            Console.Write("Nhập mã khách hàng: ");
            string tuKhoa = Console.ReadLine();
            var ketQua = khachHangBLL.TimKiemKhachHang(filePath, tuKhoa);
            if (ketQua.Count == 0)
            {
                Console.WriteLine("Không tìm thấy khách hàng nào!");
                return;
            }

            Console.WriteLine($"\nTìm thấy {ketQua.Count} khách hàng:");
            Console.WriteLine($"{"Mã KH",-10} {"Tên KH",-20} {"SĐT",-15} {"Số SP",-8} {"Tổng tiền",-12}");
            Console.WriteLine(new string('=', 70));

            foreach (var kh in ketQua)
            {
                Console.WriteLine($"{kh.MaKH,-10} {kh.TenKH,-20} {kh.SoDienThoai,-15} {kh.DanhSachBanh.Count,-8} {kh.TongTien,-12:N0}");
            }
        }

        private void ThemKhachHangMoi(string filePath)
        {
            Console.WriteLine("\n=== THÊM KHÁCH HÀNG MỚI ===");

            Console.Write("Nhập mã KH: ");
            string maKH = Console.ReadLine();

            Console.Write("Nhập tên KH: ");
            string tenKH = Console.ReadLine();

            Console.Write("Nhập SĐT: ");
            string sdt = Console.ReadLine();

            var khachHangTonTai = khachHangBLL.TimKhachHangTheoMa(maKH);
            if (khachHangTonTai != null)
            {
                Console.WriteLine("Mã KH đã tồn tại!");
                return;
            }

            KhachHangDTO khachHangMoi = new KhachHangDTO
            {
                MaKH = maKH,
                TenKH = tenKH,
                SoDienThoai = sdt
            };

            Console.WriteLine("\nThêm sản phẩm vào đơn hàng (nhập 'done' để kết thúc):");
            while (true)
            {
                Console.Write("Nhập mã bánh (hoặc 'done'): ");
                string maBanh = Console.ReadLine();

                if (maBanh.ToLower() == "done")
                    break;

                Console.Write("Nhập tên bánh: ");
                string tenBanh = Console.ReadLine();

                Console.Write("Nhập giá bán (số thực): ");
                if (!double.TryParse(Console.ReadLine(), out double giaBan))
                {
                    Console.WriteLine("Giá bán không hợp lệ!");
                    continue;
                }

                Console.Write("Nhập hạn sử dụng (số ngày): ");
                int hanSuDung = int.Parse(Console.ReadLine());

                Console.Write("Chọn loại bánh (1-Bánh Kem, 2-Bánh Ngọt, 3-Bánh Handmade): ");
                string loai = Console.ReadLine();

                BanhDTO banhMoi = null;

                switch (loai)
                {
                    case "1":
                        int kichThuoc;
                        while (true)
                        {
                            Console.Write("Nhập kích thước (18, 20, 24, 28, 32): ");
                            if (!int.TryParse(Console.ReadLine(), out kichThuoc))
                            {
                                Console.WriteLine("Giá trị nhập không hợp lệ! Vui lòng nhập số nguyên.");
                                continue;
                            }

                            if (kichThuoc == 18 || kichThuoc == 20 || kichThuoc == 24 || kichThuoc == 28 || kichThuoc == 32)
                                break;
                            else
                                Console.WriteLine("Kích thước không hợp lệ! Vui lòng nhập lại!");
                        }

                        banhMoi = new BanhKemDTO
                        {
                            MaBanh = maBanh,
                            TenBanh = tenBanh,
                            NgaySanXuat = DateTime.Now,
                            HanSuDung = hanSuDung,
                            GiaBan = giaBan,
                            KichThuoc = kichThuoc
                        };
                        break;


                    case "2":
                                Console.Write("Nhập cách bảo quản (lạnh/ không lạnh): ");
                                string cachBaoQuan = Console.ReadLine();
                                banhMoi = new BanhNgotDTO
                                {
                                    MaBanh = maBanh,
                                    TenBanh = tenBanh,
                                    NgaySanXuat = DateTime.Now,
                                    HanSuDung = hanSuDung,
                                    GiaBan = giaBan,
                                    CachBaoQuan = cachBaoQuan
                                };
                                break;

                            case "3":
                                Console.Write("Nhập dòng bánh  (singapore, đài loan,….): ");
                                string dongBanh = Console.ReadLine();
                                banhMoi = new BanhHandmadeDTO
                                {
                                    MaBanh = maBanh,
                                    TenBanh = tenBanh,
                                    NgaySanXuat = DateTime.Now,
                                    HanSuDung = hanSuDung,
                                    GiaBan = giaBan,
                                    DongBanh = dongBanh
                                };
                                break;

                            default:
                                Console.WriteLine("Loại bánh không hợp lệ!");
                                continue;
                            }

                if (banhMoi != null)
                {
                    khachHangMoi.DanhSachBanh.Add(banhMoi);
                    Console.WriteLine($"Đã thêm {banhMoi.TenBanh}");
                }
            }

            khachHangMoi.TinhTongTien();

            try
            {
                bool thanhCong = khachHangBLL.ThemDonHangMoi(filePath, khachHangMoi);

                if (thanhCong)
                {
                    Console.WriteLine("Thêm khách hàng thành công!");
                    Console.WriteLine($"Tổng tiền: {khachHangMoi.TongTien:N0} VNĐ");
                }
                else
                {
                    Console.WriteLine("Lỗi khi thêm khách hàng!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        private void HienThiKhachHangMuaNhieu(string filePath)
        {
            Console.WriteLine("\n=== KHÁCH HÀNG MUA NHIỀU HƠN 3 SẢN PHẨM ===");

            var khachHangs = khachHangBLL.LayKhachHangMuaNhieuHon3SanPham(filePath);

            if (khachHangs.Count == 0)
            {
                Console.WriteLine("Không có khách hàng nào mua nhiều hơn 3 sản phẩm!");
                return;
            }

            foreach (var kh in khachHangs)
            {
                Console.WriteLine($"{kh.MaKH} - {kh.TenKH} - Số SP: {kh.DanhSachBanh.Count} - Tổng tiền: {kh.TongTien:N0} VNĐ");
            }
        }

        private void HienThiKhachHangMuaNhieuTienNhat(string filePath)
        {
            Console.WriteLine("\n=== KHÁCH HÀNG MUA NHIỀU TIỀN NHẤT ===");

            var khachHang = khachHangBLL.LayKhachHangMuaNhieuTienNhat(filePath);

            if (khachHang == null)
            {
                Console.WriteLine("Không có dữ liệu!");
                return;
            }

            Console.WriteLine($"Mã KH: {khachHang.MaKH}");
            Console.WriteLine($"Tên KH: {khachHang.TenKH}");
            Console.WriteLine($"SĐT: {khachHang.SoDienThoai}");
            Console.WriteLine($"Số sản phẩm: {khachHang.DanhSachBanh.Count}");
            Console.WriteLine($"Tổng tiền: {khachHang.TongTien:N0} VNĐ");

            Console.WriteLine("\nDanh sách sản phẩm đã mua:");
            foreach (var banh in khachHang.DanhSachBanh)
            {
                Console.WriteLine($"  - {banh.TenBanh} ({banh.GetType().Name.Replace("DTO", "")}): {banh.GiaBan:N0} VNĐ");
            }
        }

        private void HienThiChiTietDonHang(string filePath)
        {
            Console.WriteLine("\n=== CHI TIẾT ĐƠN HÀNG ===");

            var khachHangs = khachHangBLL.LayTatCaKhachHang(filePath);

            if (khachHangs.Count == 0)
            {
                Console.WriteLine("Không có khách hàng nào!");
                return;
            }

            Console.WriteLine("Danh sách khách hàng:");
            for (int i = 0; i < khachHangs.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {khachHangs[i].MaKH} - {khachHangs[i].TenKH}");
            }

            Console.Write("Chọn khách hàng (số thứ tự): ");
            if (int.TryParse(Console.ReadLine(), out int stt) && stt > 0 && stt <= khachHangs.Count)
            {
                var khachHang = khachHangs[stt - 1];

                Console.WriteLine($"\nĐƠN HÀNG CỦA {khachHang.TenKH.ToUpper()}");
                Console.WriteLine($"Mã KH: {khachHang.MaKH}");
                Console.WriteLine($"SĐT: {khachHang.SoDienThoai}");
                Console.WriteLine($"Tổng tiền: {khachHang.TongTien:N0} VNĐ");

                Console.WriteLine("\nCHI TIẾT SẢN PHẨM:");
                Console.WriteLine($"{"Tên bánh",-25} {"Loại",-15} {"Giá gốc",-12} {"Giá sau KM",-12}");
                Console.WriteLine(new string('=', 70));

                foreach (var banh in khachHang.DanhSachBanh)
                {
                    double giaSauGiam = banh.TinhGiamGia();
                    if (banh is ITroGia banhTroGia)
                    {
                        giaSauGiam = banhTroGia.TinhTroGia();
                    }

                    Console.WriteLine($"{banh.TenBanh,-25} {banh.GetType().Name.Replace("DTO", ""),-15} {banh.GiaBan,-12:N0} {giaSauGiam,-12:N0}");
                }
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ!");
            }
        }
    }
}