using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScale = 1.2f;   // 放大倍速
    public float smoothSpeed = 10f;   // 平滑速度

    private Vector3 originalScale;    // 原始缩放
    private Vector3 targetScale;      // 目标缩放

    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // 用 unscaledDeltaTime：暂停（timeScale=0）时 UI 动画不受影响
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * smoothSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale; // 目标设为放大
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale; // 目标设为还原
    }
}