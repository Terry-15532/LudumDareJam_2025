using UnityEngine;

public class CameraController_TwoCharacters : MonoBehaviour{
	public static CameraController_TwoCharacters instance;
	
	public float distance, dampSensitivity;

	public Quaternion angels;

	public Transform characterA, characterB;
	public Vector3 lookTarget;
	public Vector2 mouseSensitivity = Vector2.one;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start(){
		Cursor.lockState = CursorLockMode.Locked;
		lookTarget = (characterA.position + characterB.position) / 2;
		instance = this;
	}

	// Update is called once per frame
	void Update(){
		UpdateTransform();
		CheckInput();
	}

	public void UpdateTransform(){
		var mid = (characterA.position + characterB.position) / 2;
		var targetPos = mid + angels * (Vector3.forward * distance);
		lookTarget = Vector3.Lerp(lookTarget, mid, dampSensitivity * Time.deltaTime);
		transform.position = Vector3.Lerp(transform.position, targetPos, dampSensitivity * Time.deltaTime);
		transform.LookAt(lookTarget);
	}

	public void CheckInput(){
		angels = Quaternion.Euler(angels.eulerAngles + new Vector3(Input.GetAxis("Mouse Y") * mouseSensitivity.x, Input.GetAxis("Mouse X") * mouseSensitivity.y, 0) * Time.deltaTime);
		distance -= Input.mouseScrollDelta.y * 0.5f;
	}
}