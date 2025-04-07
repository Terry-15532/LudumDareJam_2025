using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDetection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public Vector3 minScale = Vector3.one;
    //public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);
    //public float scaleSpeed = 5f;

    //private Vector3 targetScale;
    //private RectTransform rectTransform;

    void Awake()
    {
        //rectTransform = GetComponent<RectTransform>();
        //targetScale = minScale;
        //rectTransform.localScale = minScale;
    }

    void Update()
    {
        //rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //targetScale = maxScale;
        SoundSys.PlaySound("Pop", volume: 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //targetScale = minScale;
        SoundSys.PlaySound("Pop", volume: 0.3f);
    }
}
