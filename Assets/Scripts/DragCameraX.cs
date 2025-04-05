using UnityEngine;

public class DragCameraX : MonoBehaviour
{
    public float dragSpeed = 5f;
    public float minX = -10f;
    public float maxX = 10f;

    private Vector3 lastMousePos;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float moveX = -delta.x * dragSpeed * Time.deltaTime;

            Vector3 newPos = transform.position + new Vector3(moveX, 0f, 0f);
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

            transform.position = newPos;

            lastMousePos = Input.mousePosition;
        }
    }
}
