using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DTO_QLTBN;

namespace DAL_QLTBN
{
    public class DSKhachHangDAL
    {
        private List<KhachHangDTO> lstKH = new List<KhachHangDTO>();
        public List<KhachHangDTO> LstKH
        {
            get { return lstKH; }
            set { lstKH = value; }
        }

        public DSKhachHangDAL() { }

        private BanhDTO TaoBanhTuXmlNode(XmlNode node, string loai)
        {
            string maBanh = node["MaBanh"].InnerText;
            string tenBanh = node["TenBanh"].InnerText;
            DateTime ngaySX = DateTime.Parse(node["NgaySanXuat"].InnerText);
            int hanSD = int.Parse(node["HanSuDung"].InnerText);
            double giaBan = double.Parse(node["GiaBan"].InnerText);

            switch (loai)
            {
                case "BanhKem":
                    int kichThuoc = int.Parse(node["KichThuoc"].InnerText);
                    return new BanhKemDTO
                    {
                        MaBanh = maBanh,
                        TenBanh = tenBanh,
                        NgaySanXuat = ngaySX,
                        HanSuDung = hanSD,
                        GiaBan = giaBan,
                        KichThuoc = kichThuoc
                    };
                case "BanhNgot":
                    string cachBaoQuan = node["CachBaoQuan"].InnerText;
                    return new BanhNgotDTO
                    {
                        MaBanh = maBanh,
                        TenBanh = tenBanh,
                        NgaySanXuat = ngaySX,
                        HanSuDung = hanSD,
                        GiaBan = giaBan,
                        CachBaoQuan = cachBaoQuan
                    };
                case "BanhHandmade":
                    string dongBanh = node["DongBanh"].InnerText;
                    return new BanhHandmadeDTO
                    {
                        MaBanh = maBanh,
                        TenBanh = tenBanh,
                        NgaySanXuat = ngaySX,
                        HanSuDung = hanSD,
                        GiaBan = giaBan,
                        DongBanh = dongBanh
                    };
                default:
                    return null;
            }
        }

        public List<KhachHangDTO> DocDanhSachKH(string filename)
        {
            lstKH.Clear();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filename);
                XmlNodeList khachHangNodes = doc.SelectNodes("/Tiem/Khach");

                foreach (XmlNode khachNode in khachHangNodes)
                {
                    string maKH = khachNode["MaKH"].InnerText;
                    string tenKH = khachNode["TenKH"].InnerText;
                    string sdt = khachNode["SDT"].InnerText;

                    KhachHangDTO khachHang = new KhachHangDTO
                    {
                        MaKH = maKH,
                        TenKH = tenKH,
                        SoDienThoai = sdt
                    };

                    XmlNodeList banhNodes = khachNode.SelectNodes("DanhSachBanh/Banh");
                    foreach (XmlNode banhNode in banhNodes)
                    {
                        string loaiBanh = banhNode.Attributes["Loai"].Value;
                        BanhDTO banh = TaoBanhTuXmlNode(banhNode, loaiBanh);
                        if (banh != null)
                        {
                            khachHang.DanhSachBanh.Add(banh);
                        }
                    }
                    khachHang.TinhTongTien();
                    lstKH.Add(khachHang);
                }
                return lstKH;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đọc file: {ex.Message}");
                return lstKH ?? new List<KhachHangDTO>();
            }
        }

        public void ThemKhachHang(KhachHangDTO khachHang)
        {
            khachHang.TinhTongTien();
            lstKH.Add(khachHang);
        }

        public bool XoaKhachHang(string maKH)
        {
            var khachHang = lstKH.Find(kh => kh.MaKH == maKH);
            if (khachHang != null)
            {
                lstKH.Remove(khachHang);
                return true;
            }
            return false;
        }

        public KhachHangDTO TimKhachHangTheoMa(string maKH)
        {
            return lstKH.Find(kh => kh.MaKH == maKH);
        }

        public bool GhiDanhSachKH(string filename, List<KhachHangDTO> danhSachKH)
        {
            try
            {
                using (var writer = XmlWriter.Create(filename, new XmlWriterSettings { Indent = true }))
                {
                    writer.WriteStartElement("Tiem");
                    foreach (var kh in danhSachKH)
                    {
                        writer.WriteStartElement("Khach");
                        writer.WriteElementString("MaKH", kh.MaKH);
                        writer.WriteElementString("TenKH", kh.TenKH);
                        writer.WriteElementString("SDT", kh.SoDienThoai);
                        writer.WriteStartElement("DanhSachBanh");

                        foreach (var banh in kh.DanhSachBanh)
                        {
                            string loai = banh.GetType().Name.Replace("DTO", "");
                            writer.WriteStartElement("Banh");
                            writer.WriteAttributeString("Loai", loai);
                            writer.WriteElementString("MaBanh", banh.MaBanh);
                            writer.WriteElementString("TenBanh", banh.TenBanh);
                            writer.WriteElementString("NgaySanXuat", banh.NgaySanXuat.ToString("yyyy-MM-dd"));
                            writer.WriteElementString("HanSuDung", banh.HanSuDung.ToString());
                            writer.WriteElementString("GiaBan", banh.GiaBan.ToString());
                            if (banh is BanhKemDTO bk) writer.WriteElementString("KichThuoc", bk.KichThuoc.ToString());
                            if (banh is BanhNgotDTO bn) writer.WriteElementString("CachBaoQuan", bn.CachBaoQuan);
                            if (banh is BanhHandmadeDTO bh) writer.WriteElementString("DongBanh", bh.DongBanh);
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement(); 
                        writer.WriteEndElement(); 
                    }
                    writer.WriteEndElement();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}