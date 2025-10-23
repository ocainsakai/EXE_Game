using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "CardNameMapper", menuName = "MyGame/Card Name Mapper")]
public class CardNameMapper : ScriptableObject
{
    [System.Serializable]
    public class CardMapping
    {
        [Tooltip("Tên trong SO (English)")]
        public string englishName;
        
        [Tooltip("Tên sprite tương ứng (Vietnamese hoặc custom)")]
        public string spriteName;
        
        [Tooltip("Các tên khác có thể dùng")]
        public List<string> aliases = new List<string>();
    }

    [Header("Rank Mappings")]
    [Tooltip("Map các rank (Ace, Two, Jack...) sang tên Việt")]
    public List<RankMapping> rankMappings = new List<RankMapping>()
    {
        new RankMapping { english = "Ace", vietnamese = "Xì", number = "A" },
        new RankMapping { english = "Two", vietnamese = "2", number = "2" },
        new RankMapping { english = "Three", vietnamese = "3", number = "3" },
        new RankMapping { english = "Four", vietnamese = "4", number = "4" },
        new RankMapping { english = "Five", vietnamese = "5", number = "5" },
        new RankMapping { english = "Six", vietnamese = "6", number = "6" },
        new RankMapping { english = "Seven", vietnamese = "7", number = "7" },
        new RankMapping { english = "Eight", vietnamese = "8", number = "8" },
        new RankMapping { english = "Nine", vietnamese = "9", number = "9" },
        new RankMapping { english = "Ten", vietnamese = "10", number = "10" },
        new RankMapping { english = "Jack", vietnamese = "J", number = "J" },
        new RankMapping { english = "Queen", vietnamese = "Q", number = "Q" },
        new RankMapping { english = "King", vietnamese = "K", number = "K" },
    };

    [Header("Suit Mappings")]
    [Tooltip("Map các suit (Clubs, Diamonds...) sang tên Việt")]
    public List<SuitMapping> suitMappings = new List<SuitMapping>()
    {
        new SuitMapping { english = "Clubs", vietnamese = "Chuồn", short_vietnamese = "c" },
        new SuitMapping { english = "Diamonds", vietnamese = "Rô", short_vietnamese = "ro" },
        new SuitMapping { english = "Hearts", vietnamese = "Cơ", short_vietnamese = "co" },
        new SuitMapping { english = "Spades", vietnamese = "Bích", short_vietnamese = "b" },
    };

    [Header("Custom Mappings")]
    [Tooltip("Các mapping đặc biệt (nếu có)")]
    public List<CardMapping> customMappings = new List<CardMapping>();

    [Header("Pattern Settings")]
    public string spriteNamePattern = "{rank}_{suit}"; // VD: "2_Bích", "Xì_Chuồn"
    public bool toLowerCase = true;
    public bool removeSpaces = true;

    [System.Serializable]
    public class RankMapping
    {
        public string english;
        public string vietnamese;
        public string number;
    }

    [System.Serializable]
    public class SuitMapping
    {
        public string english;
        public string vietnamese;
        public string short_vietnamese;
    }

    /// <summary>
    /// Chuyển đổi tên card từ English sang sprite name
    /// </summary>
    public string ConvertToSpriteName(string cardName)
    {
        // Check custom mapping trước
        var customMatch = customMappings.FirstOrDefault(m => 
            m.englishName.Equals(cardName, System.StringComparison.OrdinalIgnoreCase) ||
            m.aliases.Any(a => a.Equals(cardName, System.StringComparison.OrdinalIgnoreCase))
        );
        
        if (customMatch != null)
            return ProcessSpriteName(customMatch.spriteName);

        // Parse card name (VD: "Ace of Diamonds" -> rank="Ace", suit="Diamonds")
        string[] parts = cardName.Split(new[] { " of ", "_of_", "-" }, System.StringSplitOptions.RemoveEmptyEntries);
        
        if (parts.Length < 2)
        {
            Debug.LogWarning($"Cannot parse card name: {cardName}");
            return ProcessSpriteName(cardName);
        }

        string rankEng = parts[0].Trim();
        string suitEng = parts[1].Trim();

        // Find rank mapping
        var rankMap = rankMappings.FirstOrDefault(r => 
            r.english.Equals(rankEng, System.StringComparison.OrdinalIgnoreCase));
        
        // Find suit mapping
        var suitMap = suitMappings.FirstOrDefault(s => 
            s.english.Equals(suitEng, System.StringComparison.OrdinalIgnoreCase));

        if (rankMap == null || suitMap == null)
        {
            Debug.LogWarning($"No mapping found for: {cardName}");
            return ProcessSpriteName(cardName);
        }

        // Build sprite name theo pattern
        string spriteName = spriteNamePattern
            .Replace("{rank}", rankMap.vietnamese)
            .Replace("{suit}", suitMap.vietnamese)
            .Replace("{rank_num}", rankMap.number)
            .Replace("{suit_short}", suitMap.short_vietnamese);

        return ProcessSpriteName(spriteName);
    }

    /// <summary>
    /// Xử lý tên sprite theo settings (lowercase, remove spaces...)
    /// </summary>
    private string ProcessSpriteName(string name)
    {
        if (removeSpaces)
            name = name.Replace(" ", "");
        
        if (toLowerCase)
            name = name.ToLower();

        return name;
    }

    /// <summary>
    /// Tìm tất cả các tên sprite có thể match (để fuzzy search)
    /// </summary>
    public List<string> GetPossibleSpriteNames(string cardName)
    {
        List<string> possibilities = new List<string>();
        
        // Tên chính
        possibilities.Add(ConvertToSpriteName(cardName));
        
        // Parse card name
        string[] parts = cardName.Split(new[] { " of ", "_of_", "-" }, System.StringSplitOptions.RemoveEmptyEntries);
        
        if (parts.Length >= 2)
        {
            string rankEng = parts[0].Trim();
            string suitEng = parts[1].Trim();

            var rankMap = rankMappings.FirstOrDefault(r => 
                r.english.Equals(rankEng, System.StringComparison.OrdinalIgnoreCase));
            
            var suitMap = suitMappings.FirstOrDefault(s => 
                s.english.Equals(suitEng, System.StringComparison.OrdinalIgnoreCase));

            if (rankMap != null && suitMap != null)
            {
                // Các variations khác nhau
                possibilities.Add(ProcessSpriteName($"{rankMap.number}_{suitMap.vietnamese}"));
                possibilities.Add(ProcessSpriteName($"{rankMap.number}_{suitMap.short_vietnamese}"));
                possibilities.Add(ProcessSpriteName($"{rankMap.vietnamese}_{suitMap.short_vietnamese}"));
                possibilities.Add(ProcessSpriteName($"{rankMap.number}{suitMap.vietnamese}"));
                possibilities.Add(ProcessSpriteName($"{rankMap.number}{suitMap.short_vietnamese}"));
                
                // English variations
                possibilities.Add(ProcessSpriteName($"{rankMap.number}_of_{suitEng}"));
                possibilities.Add(ProcessSpriteName($"{rankEng}_of_{suitEng}"));
            }
        }

        // Remove duplicates
        return possibilities.Distinct().ToList();
    }

    /// <summary>
    /// Test conversion (gọi từ Editor để test)
    /// </summary>
    public void TestConversion(string testCardName)
    {
        Debug.Log($"Testing: {testCardName}");
        Debug.Log($"Main result: {ConvertToSpriteName(testCardName)}");
        
        var possibilities = GetPossibleSpriteNames(testCardName);
        Debug.Log($"All possibilities: {string.Join(", ", possibilities)}");
    }
}