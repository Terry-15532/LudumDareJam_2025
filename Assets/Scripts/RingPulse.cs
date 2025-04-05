using UnityEngine;

public class RingPulse : MonoBehaviour
{
    public UIRing ring;
    public QTEController controller;
    public float maxRadius = 100f;
    public float minRadius = 50f;
    public float shrinkSpeed = 50f;
    private bool deactivated = false;

    void Update()
    {
        if (ring.Radius > minRadius && !deactivated)
        {
            ring.Radius -= shrinkSpeed * Time.deltaTime;
        }
        else if (!deactivated)
        {
            controller.BeatHit();
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
}
