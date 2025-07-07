using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [SerializeField] private float m_roughness = 25f;
    [SerializeField] private float m_magnitude = 0.5f;

    private Vector3 originalPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        originalPosition = transform.localPosition;
    }

    public void Shake(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration));
    }

    private IEnumerator ShakeCoroutine(float duration)
    {
        float halfDuration = duration / 2f;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime / halfDuration;

            tick += Time.deltaTime * m_roughness;
            Vector3 shakeOffset = new Vector3(
                Mathf.PerlinNoise(tick, 0f) - 0.5f,
                Mathf.PerlinNoise(0f, tick) - 0.5f,
                0f
            ) * m_magnitude * Mathf.PingPong(elapsed, halfDuration);

            transform.localPosition = originalPosition + shakeOffset;
            yield return null;
        }

        transform.localPosition = originalPosition; 
    }
}
