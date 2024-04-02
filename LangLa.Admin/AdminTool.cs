using System.Linq;
using LangLa.Data;
using LangLa.Hander;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.Admin
{
    public static class AdminTool
    {
        public static void ProcessChat(Character character, string chat)
        {
            var info = chat.Split(" ");

            if (chat.Contains("point"))
            {
                character.Point.DiemTiemNang += short.Parse(info[1]);
                character.Point.DiemKyNang += short.Parse(info[1]);
                character.UpdateDataChar();
            }
            else
            {
                InfoThu infoThu = null;

                if (chat.Contains("item"))
                {
                    var template = DataServer.ArrItemTemplate.ToList().Find(n => n.id.ToString().Equals(info[1]));
                    Item item = new Item(template.id, IsLock: false);
                    item.Quantity = int.Parse(info[2]);
                    item.IdClass = character.Info.IdClass;
                    infoThu = new InfoThu(item);
                    infoThu.Id = (short)character.Thus.Count;
                    infoThu.Bac = 0;
                    infoThu.BacKhoa = 0;
                    infoThu.Vang = 0;
                    infoThu.VangKhoa = 0;
                    infoThu.Exp = 0L;
                    infoThu.Title = "Hệ thống";
                    infoThu.NameSender = "Lục đạo chân nhân";
                    infoThu.Content = "";
                    infoThu.TimeEnd = 90000L;
                }

                if (chat.Contains("money"))
                {
                    infoThu = new InfoThu();
                    infoThu.Id = (short)character.Thus.Count;
                    infoThu.Bac = int.Parse(info[1]);
                    infoThu.BacKhoa = int.Parse(info[1]);
                    infoThu.Vang = int.Parse(info[1]);
                    infoThu.VangKhoa = int.Parse(info[1]);
                    infoThu.Exp = 0L;
                    infoThu.Title = "Hệ thống";
                    infoThu.NameSender = "Lục đạo chân nhân";
                    infoThu.Content = "";
                    infoThu.TimeEnd = 90000L;
                }

                SendItem(character, infoThu);
            }
        }
        
        public static void SendItem(Character character, InfoThu infoThu)
        {
            character.Thus.Add(infoThu);
            ThuHander.ReloadThu(character);
            character.SendMessage(UtilMessage.ClearSceen());
        }
    }
}