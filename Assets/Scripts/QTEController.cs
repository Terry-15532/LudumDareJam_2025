using UnityEngine;

public class QTEController : MonoBehaviour
{
    public int maxGrace;
    private int gracePeriod;
    public int maxPulses;
    public int successPulses;
    public int pulseLeft;
    private int currentSuccess;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pulseLeft = maxPulses;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gracePeriod > 0)
            { BeatSuccess(); }
            else
            { gracePeriod = maxGrace; }
        }
        if (gracePeriod > 0)
        {
            gracePeriod--;
        }
    }

    public void BeatHit()
    {
        if (gracePeriod > 0)
        { BeatSuccess(); }
        else
        { gracePeriod = maxGrace; }
    }

    void BeatSuccess()
    {
        currentSuccess++;
        Debug.Log(currentSuccess);
    }
}
