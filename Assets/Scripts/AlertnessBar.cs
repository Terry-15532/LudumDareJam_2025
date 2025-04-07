using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Sound QTEsound;
    public Image head;
    public Color alertColor;

    private float swingAngle = 20f;  // Max angle from center
    private float swingSpeed = 2f;   // How fast it swings

    private void Start()
    {
        startingColor = leftBar.color;
        badEnding.GetComponent<Image>().color = Color.black;
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

        // Swing head
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        head.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, angle);
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
        leftBar.color = alertColor;
        rightBar.color = alertColor;

        OnBarFrozen();
    }

    void OnBarFrozen()
    {
        // Do whatever you need when it freezes
        Debug.Log("Bar frozen. Calling function.");

        // Call your function here
        LevelManager.instance.PauseBGM();
        if (alertLevel < 3)
        {
            QTEsound = SoundSys.PlaySound("QTE", volume: 0.7f);
            FindAnyObjectByType<QTEController>().startQTE();
        }
        else
        {
            GameOver();
        }
    }

    public void UnfreezeBar()
    {
        alertLevel++;
        head.sprite = ResourceManager.Load<Sprite>("Sprites/bar" + alertLevel.ToString());
        frozen = false;
        leftBar.color = startingColor;
        rightBar.color = startingColor;
        Destroy(QTEsound.gameObject);
        LevelManager.instance.ResumeBGM();
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        LevelManager.instance.StopBGM();
        Destroy(QTEsound.gameObject);
        SoundSys.PlaySound("bad_ending_voice", false);
        SoundSys.PlaySound("bad_ending", false, 2);
        badEnding.SetActive(true);
        StartCoroutine(DelayedLoadGameOver());
        
    }

    IEnumerator DelayedLoadGameOver()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(4);
    }
}
