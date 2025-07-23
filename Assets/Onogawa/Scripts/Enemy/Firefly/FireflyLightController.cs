using UnityEngine;
using System.Collections;

public class FireflyLightController : MonoBehaviour
{
    private Material _material;
    [Header("発光色")][ColorUsage(true, true)][SerializeField] private Color _emissionColor = new Color32(168, 191, 0, 255);
    [Header("点滅の総間隔")][SerializeField] private float _interval = 1.5f;
    [Header("完全に点灯している時間")][SerializeField] private float _peakDuration = 0.3f;

    void Start()
    {
        if (_material == null)
            _material = GetComponent<Renderer>()?.material;
        if (_material == null)
        {
            Debug.Log("Materialが取得できません");
            return;
        }
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        float fadeTime = _interval / 8.0f;
        float waitTime = _interval - (fadeTime * 2 + _peakDuration);
        if (waitTime < 0)
            waitTime = 0;
        while (true)
        {
            // 消灯
            // SetEmission(Color.black);
            yield return new WaitForSeconds(waitTime);
            // フェードイン
            yield return StartCoroutine(SmoothFade(Color.black, _emissionColor, fadeTime));
            // 点灯
            // SetEmission(emissionColor);
            yield return new WaitForSeconds(_peakDuration);
            // フェードアウト
            yield return StartCoroutine(SmoothFade(_emissionColor, Color.black, fadeTime));
        }
    }

    IEnumerator SmoothFade(Color from, Color to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            SetEmission(Color.Lerp(from, to, t / duration));
            t += Time.deltaTime;
            yield return null;
        }
        SetEmission(to);
    }

    void SetEmission(Color color)
    {
        _material.EnableKeyword("_EMISSION");
        _material.SetColor("_EmissionColor", color);
    }
}