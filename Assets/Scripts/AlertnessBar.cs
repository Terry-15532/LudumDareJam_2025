using UnityEngine;
using UnityEngine.UI;

public class AlertnessBar : MonoBehaviour
{
    public Image leftBar;
    public Image rightBar;

    public float currentValue = 0f;
    public float[] QTEthresholds;
    private int alertLevel = 1;

    public float oscillationSpeed = 1f;
    public float collisionAddValue = 8f;
    private bool increasing = true;
    private bool frozen = false;

    private Color startingColor;
    public GameObject badEnding;

    private void Start()
    {
        startingColor = leftBar.color;
    }

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

        currentValue += oscillationSpeed * Time.deltaTime;
        
        float fillAmount = Mathf.Clamp01(currentValue / QTEthresholds[2]);
        leftBar.fillAmount = fillAmount;
        rightBar.fillAmount = fillAmount;

        if (alertLevel == 1 && currentValue >= QTEthresholds[0])
            FreezeBar();
        else if (alertLevel == 2 && currentValue >= QTEthresholds[1])
            FreezeBar();
        else if (alertLevel == 3 && currentValue >= QTEthresholds[2])
            FreezeBar();
    }

    public void TriggerEvent(Food food)
    {
        if (frozen) return;

        currentValue += collisionAddValue;

        switch (alertLevel)
        {
            case 1:
                currentValue = Mathf.Clamp(currentValue, 0, QTEthresholds[0]);
                break;
            case 2:
                currentValue = Mathf.Clamp(currentValue, QTEthresholds[0], QTEthresholds[1]);
                break;
            case 3:
                currentValue = Mathf.Clamp(currentValue, QTEthresholds[1], QTEthresholds[2]);
                break;
        }
        
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
        if (alertLevel < 3)
        {
            FindAnyObjectByType<QTEController>().startQTE();
        } else
        {
            GameOver();
        }
    }

    public void UnfreezeBar()
    {
        alertLevel++;
        frozen = false;
        leftBar.color = startingColor;
        rightBar.color = startingColor;
    }

    public void GameOver()
    {
        badEnding.SetActive(true);
        Debug.Log("Game Over");
    }
}
