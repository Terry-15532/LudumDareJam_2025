using UnityEngine;
using UnityEngine.UI;

public class AlertnessBar : MonoBehaviour
{
    public Image leftBar;
    public Image rightBar;

    public float currentValue = 0f;
    private float maxValue = 100f;

    public float oscillationSpeed = 1f;
    private bool increasing = true;
    private bool frozen = false;

    void OnEnable()
    {
        Food.OnCollisionEvent += TriggerEvent;
    }

    void OnDisable()
    {
        Food.OnCollisionEvent -= TriggerEvent;
    }

    void Update()
    {
        if (frozen) return;

        Oscillate();

        float fillAmount = Mathf.Clamp01(currentValue / maxValue);
        leftBar.fillAmount = fillAmount;
        rightBar.fillAmount = fillAmount;

        if (currentValue >= 75f)
        {
            FreezeBar();
        }
    }

    void Oscillate()
    {
        float delta = oscillationSpeed * Time.deltaTime * (increasing ? 1 : -1);
        currentValue += delta;

        if (currentValue >= 60f)
            increasing = false;
        else if (currentValue <= 0f)
            increasing = true;
    }

    public void TriggerEvent(Food food)
    {
        if (frozen) return;

        currentValue += 40f;
    }

    void FreezeBar()
    {
        frozen = true;
        leftBar.color = Color.red;
        rightBar.color = Color.red;

        OnBarFrozen();
    }

    void OnBarFrozen()
    {
        // Do whatever you need when it freezes
        Debug.Log("Bar frozen. Calling function.");
        // Call your function here
    }
}
