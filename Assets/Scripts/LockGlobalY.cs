using JetBrains.Annotations;
using UnityEngine;

public class LockGlobalY : MonoBehaviour
{
    public float lockedY = 0f;
    private float objectDistance;
    public float maxDistance;
    public float minScale;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        objectDistance = Vector3.Distance(transform.position, transform.parent.position);
        float newScale = objectDistance > maxDistance ? minScale : (1 - minScale) - ((objectDistance / maxDistance) * (1 - minScale)) + minScale;
        Debug.Log(newScale);
        transform.localScale = originalScale * newScale;
    }
    void LateUpdate()
    {
        Vector3 worldPos = transform.position;
        worldPos.y = lockedY;
        transform.position = worldPos;
    }
}
