using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class BattleVFXManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform vfxContainer; // nơi chứa các hiệu ứng (Canvas hoặc GameObject trống)
    [SerializeField] private Transform playerPoint;
    [SerializeField] private Transform enemyPoint;

    [Header("Prefabs")]
    [SerializeField] private GameObject hitVFXPrefab;
    [SerializeField] private GameObject critVFXPrefab;
    [SerializeField] private GameObject healVFXPrefab;
    [SerializeField] private TextMeshProUGUI damageTextPrefab;

    [Header("UI References")]
    [SerializeField] private UISliderBarHelper playerHealthBar;
    [SerializeField] private UISliderBarHelper enemyHealthBar;

    private Queue<GameObject> vfxPool = new Queue<GameObject>();

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    // ============================================================
    // 🧨 TẤN CÔNG
    // ============================================================

    public IEnumerator PlayAttackVFX(bool isCritical = false)
    {
        //GameObject prefab = isCritical ? critVFXPrefab : hitVFXPrefab;
        //if (prefab == null) return null;

        // Hiệu ứng va chạm giữa hai bên
        //GameObject vfx = Instantiate(prefab, vfxContainer);
        //vfx.transform.position = Vector3.Lerp(playerPoint.position, enemyPoint.position, 0.5f);
        //Destroy(vfx, 1f);

        // Camera rung nhẹ
        yield return null;
    }

    public void PlayHitVFX(int damage, bool isPlayer = false, bool isCritical = false)
    {
        UISliderBarHelper targetBar = isPlayer ? playerHealthBar : enemyHealthBar;
        Transform targetPoint = isPlayer ? playerPoint : enemyPoint;

        /*// 1. Thanh máu rung nhẹ
        targetBar.transform.DOShakePosition(0.2f, 5f, 10, 90, false, true);

        // 2. Flash màu đỏ
        targetBar.FlashVFX(Color.red, 0.25f);*/

        // 3. Floating damage text
        var dmgText = Instantiate(damageTextPrefab, targetPoint.position, Quaternion.identity, vfxContainer);
        dmgText.text = (isCritical ? "CRIT " : "") + "-" + damage;
        dmgText.color = isCritical ? Color.yellow : Color.red;

        dmgText.transform.DOMoveY(dmgText.transform.position.y + 1f, 0.6f);
        dmgText.DOFade(0, 0.6f).OnComplete(() => Destroy(dmgText.gameObject));

        // 4. Camera rung mạnh hơn nếu chí mạng
        if (isCritical)
            mainCamera.transform.DOShakePosition(0.25f, 0.4f);
    }

    // ============================================================
    // 💚 HỒI MÁU
    // ============================================================

    public void PlayHealVFX(int amount, bool isPlayer = false)
    {
        UISliderBarHelper targetBar = isPlayer ? playerHealthBar : enemyHealthBar;
        Transform targetPoint = isPlayer ? playerPoint : enemyPoint;

        // Hiệu ứng particle heal (nếu có)
        if (healVFXPrefab)
        {
            var vfx = Instantiate(healVFXPrefab, vfxContainer);
            vfx.transform.position = targetPoint.position;
            Destroy(vfx, 1.2f);
        }

        // Flash xanh lá
        targetBar.FlashVFX(Color.green, 0.25f);

        // Floating heal text
        var healText = Instantiate(damageTextPrefab, targetPoint.position, Quaternion.identity, vfxContainer);
        healText.text = "+" + amount;
        healText.color = Color.green;

        healText.transform.DOMoveY(healText.transform.position.y + 1f, 0.6f);
        healText.DOFade(0, 0.6f).OnComplete(() => Destroy(healText.gameObject));
    }
}
