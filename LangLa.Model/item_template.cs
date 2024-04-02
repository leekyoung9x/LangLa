using System.ComponentModel.DataAnnotations;

namespace LangLa.Model
{
    public class item_template
    {
        [Key]
        public short id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public bool is_cong_don { get; set; }
        public sbyte gioi_tinh { get; set; }
        public sbyte type { get; set; }
        public sbyte id_class { get; set; }
        public short id_icon { get; set; }
        public short level_need { get; set; }
        public int tai_phu_need { get; set; }
        public short id_mob { get; set; }
        public short id_char { get; set; }
    }
}