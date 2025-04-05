using UnityEngine;

public class RingPulse : MonoBehaviour
{
    public UIRing ring;
    public QTEController controller;
    public float maxRadius = 100f;
    public float minRadius = 50f;
    public float shrinkSpeed = 50f;

    void Update()
    {
        if (ring.Radius > minRadius)
        {
            ring.Radius -= shrinkSpeed * Time.deltaTime;
        }
        else
        {
            controller.BeatHit();
            if (controller.pulseLeft > 0)
            {
                controller.pulseLeft--;
                ring.Radius = maxRadius;
            }
            
        }
    }
}
