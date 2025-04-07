using System.Collections;
using UnityEngine;

public class RingPulse : MonoBehaviour
{
    public UIRing ring;
    public QTEController controller;
    public float maxRadius = 100f;
    public float minRadius = 50f;
    public float shrinkSpeed = 50f;
    private bool deactivated = false;
    private Transform center;
    private Vector3 originalScale;

    public float pulseDuration = 0.4f;
    public float scaleMultiplier = 1.2f;
    private bool isPulsing = false;

    public void Init()
    {
        center = transform.parent.Find("Center");
        originalScale = center.localScale;
    }

    void Update()
    {
        if (ring.Radius > minRadius && !deactivated)
        {
            ring.Radius -= shrinkSpeed * Time.deltaTime;
        }
        else if (!deactivated)
        {
            controller.BeatHit();
            if (gameObject.activeSelf)
            {
                StartCoroutine(ScalePulse());
            }
            if (controller.pulseLeft > 1)
            {
                //Debug.Log(controller.pulseLeft);
                ring.Radius = maxRadius;
            }
            else
            {
                deactivated = true;
            }
        }
    }

    public void Activate()
    {
        deactivated = false;
    }

    IEnumerator ScalePulse()
    {
        Debug.Log("start scaling");
        isPulsing = true;

        Vector3 targetScale = originalScale * scaleMultiplier;
        float halfDuration = pulseDuration / 2f;
        float t = 0f;

        // Scale up
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            center.localScale = Vector3.Lerp(originalScale, targetScale, t / halfDuration);
            yield return null;
        }

        t = 0f;
        // Scale back down
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            center.localScale = Vector3.Lerp(targetScale, originalScale, t / halfDuration);
            yield return null;
        }

        center.localScale = originalScale;
        isPulsing = false;
    }

    public void StopAll()
    {
        StopAllCoroutines();
    }
}
