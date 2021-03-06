//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Game()
        {
            this.HoaDons = new HashSet<HoaDon>();
        }
    
        public string MaGame { get; set; }
        public string TenGame { get; set; }
        public Nullable<System.DateTime> NgayRaMat { get; set; }
        public string MoTa { get; set; }
        public Nullable<int> DanhGia { get; set; }
        public Nullable<int> SoLuong_DG { get; set; }
        public Nullable<decimal> Gia { get; set; }
        public string AnhMinhHoa { get; set; }
        public Nullable<int> LuotTai { get; set; }
        public string Hinh1 { get; set; }
        public string Hinh2 { get; set; }
        public string Hinh3 { get; set; }
        public string Hinh4 { get; set; }
        public string Dungluong { get; set; }
        public string TenFile { get; set; }
        public string OSName { get; set; }
        public string MemoryName { get; set; }
        public string ProcessName { get; set; }
        public string GraphicsName { get; set; }
        public string MaTheLoai { get; set; }
        public string MaNPT { get; set; }
        public string MaDoTuoi { get; set; }
        public string MaNN { get; set; }
    
        public virtual DoTuoi DoTuoi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual NgonNgu NgonNgu { get; set; }
        public virtual NhaPhatTrien NhaPhatTrien { get; set; }
        public virtual TheLoai TheLoai { get; set; }
    }
}
