using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QLTBN
{
    public class BanhKemDTO : BanhDTO
    {
        #region Attributes
        private int kichThuoc;
        public int KichThuoc
        {
            get => kichThuoc;
            set
            {
                if (value > 0)
                {
                    if (value == 18 || value == 20 || value == 24 || value == 28 || value == 32)
                    {
                        kichThuoc = value;
                    }
                    else
                    {
                        Console.WriteLine($"Cảnh báo: Kích thước {value} không hợp lệ, tự động gán mặc định = 18cm.");
                        kichThuoc = 18;
                    }
                }
                else
                {
                    kichThuoc = 18;
                }
            }
        }
        #endregion
        #region Methods
        public BanhKemDTO() : base()
        {
            this.KichThuoc = 18;
        }
        public BanhKemDTO(string maBanh, string tenBanh, DateTime ngaySanXuat, int hanSuDung, double giaBan, int kichThuoc)
            : base(maBanh, tenBanh, ngaySanXuat, hanSuDung, giaBan)
        {
            KichThuoc = kichThuoc;
        }
        public override double TinhGiamGia()
        {
            return KichThuoc >= 28 ? GiaBan * 0.93 : GiaBan;
        }
        public override void XuatThongTin()
        {
            base.XuatThongTin();
            Console.WriteLine(" {0,12} cm │ {1,-12} │", KichThuoc, "Bánh Kem");
        }
        #endregion
    }
}
