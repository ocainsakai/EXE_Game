using UnityEngine;
public enum AbilityType
{
    Passive_Revive,  // Bị động: Hồi sinh khi chết
    Active_Attack,   // Chủ động: Tấn công (ví dụ: bắn cầu lửa)
    Active_Buff,     // Chủ động: Tăng chỉ số (ví dụ: tăng giáp)
    Active_Debuff    // Chủ động: Gây hiệu ứng (ví dụ: làm chậm)
    // Bạn có thể thêm nhiều loại khác...
}

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
public class AbilityData : ScriptableObject
{
    [Header("Info & Type")]
    public AbilityType type; 
    public string displayName;
    [TextArea(3, 5)]
    public string description;

    [Header("Visuals & Sounds")]
    public GameObject effectPrefab; // Hiệu ứng chung (dùng cho buff, hồi sinh, v.v.)
    public AudioClip soundEffect;

    [Header("Logic - Dùng cho Active Skills")]
    public float cooldown = 5f; // Thời gian hồi chiêu
    public int damage = 10; // Sát thương (nếu là kỹ năng tấn công)
    public GameObject projectilePrefab; // Prefab đạn (nếu là kỹ năng bắn)

    [Header("Logic - Dùng cho Revive Skill")]
    [Tooltip("Chỉ dùng khi Type là Passive_Revive. (0.5 = 50%)")]
    [Range(0f, 1f)]
    public float reviveHpPercent = 0f;
}