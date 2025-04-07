using UnityEngine;

public class DragCameraX : MonoBehaviour
{
	public float dragSpeed = 5f;
	public float minX = -10f;
	public float maxX = 10f;
	public float smoothing = 5f;

	private Vector3 initialPosition;
	private float targetX;
	private bool isDragging = false;
	private Vector3 lastMousePosition;

	void Start()
	{
		initialPosition = transform.position;
		targetX = transform.position.x;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			isDragging = true;
			lastMousePosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(1))
		{
			isDragging = false;
		}

		if (isDragging)
		{
			Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
			float deltaX = mouseDelta.x * dragSpeed * -Time.deltaTime;

			targetX = Mathf.Clamp(targetX + deltaX, initialPosition.x + minX, initialPosition.x + maxX);
			lastMousePosition = Input.mousePosition;
		}
		
		Vector3 currentPosition = transform.position;
		currentPosition.x = Mathf.Lerp(currentPosition.x, targetX, Time.deltaTime * smoothing);
		transform.position = currentPosition;
	}
}