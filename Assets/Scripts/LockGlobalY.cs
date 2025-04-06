using JetBrains.Annotations;
using UnityEngine;

public class LockGlobalY : MonoBehaviour
{
    private float objectDistance;
    public float maxDistance;
    public float minScale;
    private Vector3 originalScale;
    private float startingY;

    private void Start()
    {
        originalScale = transform.localScale;
        startingY = LevelManager.instance.posMin.y - 0.11f;
    }

    private void Update()
    {
        objectDistance = Vector3.Distance(transform.position, transform.parent.position);
        float newScale = objectDistance > maxDistance ? minScale : (1 - minScale) - ((objectDistance / maxDistance) * (1 - minScale)) + minScale;
        // Debug.Log(newScale);
        transform.localScale = originalScale * newScale;
    }
    void LateUpdate()
    {
        Vector3 worldPos = transform.position;
        worldPos.y = startingY;
        transform.position = worldPos;
    }
}
