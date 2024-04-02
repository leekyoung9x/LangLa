using System.ComponentModel.DataAnnotations;

namespace LangLa.Model
{
    public class item_template
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public bool? is_cong_don { get; set; }
        public int? gioi_tinh { get; set; }
        public int? type { get; set; }
        public int? id_class { get; set; }
        public int? id_icon { get; set; }
        public int? level_need { get; set; }
        public int? tai_phu_need { get; set; }
        public int? id_mob { get; set; }
        public int? id_char { get; set; }
    }
}