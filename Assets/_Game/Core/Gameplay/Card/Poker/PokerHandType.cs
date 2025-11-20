using System.Collections.Generic;

namespace CardSystem.PokerSystem
{
    // Enum sử dụng tên tiếng Việt không dấu (an toàn cho C#)
    public enum PokerHandType
    {
        KhongCo,
        BaiCao,
        MotDoi,
        HaiDoi,
        SamCo,
        Sanh,
        Thung,
        CuLu,
        TuQuy,
        ThungPhaSanh,
        ThungPhaSanhHoangGia
    }

    // Class cung cấp tên hiển thị (Display Name) bằng tiếng Việt có dấu
    public static class PokerHandNames
    {
        public static readonly IReadOnlyDictionary<PokerHandType, string> DisplayNames =
            new Dictionary<PokerHandType, string>
        {
            { PokerHandType.KhongCo, "Không Có" },
            { PokerHandType.BaiCao, "Bài Cao" },
            { PokerHandType.MotDoi, "Một Đôi" },
            { PokerHandType.HaiDoi, "Hai Đôi" },
            { PokerHandType.SamCo, "Tam Cô" },
            { PokerHandType.Sanh, "Sảnh" },
            { PokerHandType.Thung, "Thùng" },
            { PokerHandType.CuLu, "Cù Lũ" },
            { PokerHandType.TuQuy, "Tứ Quý" },
            { PokerHandType.ThungPhaSanh, "Thùng Phá Sảnh" },
            { PokerHandType.ThungPhaSanhHoangGia, "Sảnh Rồng" }
        };

        public static string GetDisplayName(PokerHandType type)
        {
            if (DisplayNames.TryGetValue(type, out string name))
            {
                return name;
            }
            // Trả về tên enum không dấu nếu không tìm thấy ánh xạ (phòng trường hợp lỗi)
            return type.ToString();
        }
    }
}