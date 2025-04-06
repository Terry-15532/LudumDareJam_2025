// using System;
// using System.Collections;
// using UnityEngine;
// using UnityEngine.Rendering;
//
// public enum FoodCategory{
// 	Lemon = 0,
// 	Mango = 1,
// 	Eggs = 2,
// 	Banana = 3,
// 	Milk = 4,
// 	Cola = 5,
// 	Pepsi = 6,
// 	Soda = 7,
// 	Salad = 8,
// 	Cheese = 9,
// 	Sandwich = 10,
// 	RiceBall = 11,
// 	DonutBlue = 12,
// 	Watermelon = 13,
// 	DonutPink = 14
// }
//
// public enum MovementMode
// {
//     Keyboard,
//     MouseVelocity,
//     MousePosition
// }
//
// public class Food : MonoBehaviour{
// 	[Header("食物种类")] public FoodCategory category;
// 	[Space] public SpriteRenderer icon;
//
// 	public static readonly Color[] outlineColors = new[]{
// 		new Color(0.8f, 0.7f, 0.1f), // Lemon
// 		new Color(1f, 0.6f, 0.2f), // Mango
// 		new Color(1f, 1f, 0.9f), // Eggs
// 		new Color(1f, 0.9f, 0.3f), // Banana
// 		new Color(1f, 1f, 1f), // Milk
// 		new Color(0.7f, 0f, 0f), // Cola
// 		new Color(0f, 0.4f, 0.65f), // Pepsi
// 		new Color(0.6f, 0.8f, 0.2f), // Soda
// 		new Color(0.4f, 0.8f, 0.4f), // Salad
// 		new Color(1f, 0.85f, 0.4f), // Cheese
// 		new Color(0.8f, 0.6f, 0.4f), // Sandwich
// 		new Color(0.9f, 0.8f, 0.6f), // RiceBall
// 		new Color(0.6f, 0.95f, 1f), // DonutBlue
// 		new Color(0.2f, 0.6f, 0.2f), // Watermelon
// 		new Color(0.95f, 0.6f, 0.6f), // DonutPink
// 	};
//
//
// 	private bool isWandering = false;
// 	private bool controlling = false;
// 	private bool selected = false;
// 	public float moveUpDistance = 5f;
// 	public float moveUpDuration = 1.5f;
// 	public float moveSpeed = 5f;
// 	public float zMoveSpeed = 5f;
// 	public float destroyDistance = 5f;
//
// 	public float maxWanderSpeed = 2f;
// 	public float transitionDuration = 0.5f;
// 	public float directionChangeInterval = 2f;
//
// 	private Vector3 currentVelocity = Vector3.zero;
// 	private Vector3 targetDirection = Vector3.zero;
//
// 	public float knockbackDistance = 0.5f;
// 	public float knockbackDuration = 0.5f;
// 	public int flashCount = 3;
// 	public float flashInterval = 0.5f;
//
// 	public static int emissionID = Shader.PropertyToID("_Emission");
// 	public static int colorID = Shader.PropertyToID("_EdgeColor");
//
// 	public static event Action<Food> OnCollisionEvent;
// 	private QTEController qte;
// 	private SpriteRenderer sr;
//
// 	[SerializeField] private MovementMode mode;
//     public float maxMouseSpeed = 5f;
//     public float mouseAcceleration = 10f;
//
//     private Vector3 velocity;
// 	private Camera mainCamera;
//
//     private Vector3 virtualPoint;
//     private Vector3 virtualVelocity;
//     public float stopThreshold = 0.05f; // Distance to mouse before stopping
// 	public float maxWanderDistance = 1;
// 	private Vector3 wanderPosition;
//
//     void Start(){
//         mainCamera = Camera.main;
//         icon = GetComponentInChildren<SpriteRenderer>();
// 		icon.sprite = ResourceManager.Load<Sprite>("Sprites/FoodIcons/" + category.ToString());
// 		qte = LevelManager.instance.qteController;
// 		sr = GetComponent<SpriteRenderer>();
// 		sr.material.SetColor(colorID, outlineColors[(int)category]);
// 		sr.material.SetFloat(emissionID, 0);
//         virtualPoint = transform.position;
//     }
//
// 	void Update(){
// 		if (controlling && !qte.getQTEStarted()){
// 			if (mode.ToString().CompareTo("Keyboard") == 0)
// 			{
//                 Vector3 move = Vector3.zero;
//
//                 if (Input.GetKey(KeyCode.W))
//                     move += Vector3.up;
//                 if (Input.GetKey(KeyCode.S))
//                     move += Vector3.down;
//                 if (Input.GetKey(KeyCode.A))
//                     move += Vector3.left;
//                 if (Input.GetKey(KeyCode.D))
//                     move += Vector3.right;
//
//                 move = move.normalized * (moveSpeed * Time.deltaTime);
//
//                 if (Input.GetKey(KeyCode.Space))
//                 {
//                     Vector3 toCamera = (Camera.main.transform.position - transform.position).normalized;
//                     move += toCamera * (zMoveSpeed * Time.deltaTime);
//                 }
//
//                 transform.position += move;
//
//                 // Check distance to camera
//                 float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
//                 if (distanceToCamera < destroyDistance)
//                 {
//                     OnReachCamera();
//                 }
//
//                 if (isWandering)
//                 {
//                     currentVelocity = Vector3.Lerp(currentVelocity, targetDirection * maxWanderSpeed, Time.deltaTime / transitionDuration);
//                     transform.position += currentVelocity * Time.deltaTime;
//                 }
//             }
// 			else if (mode.ToString().CompareTo("MouseVelocity") == 0)
// 			{
//                 // Get raw mouse delta (not clamped to screen)
//                 float mouseX = Input.GetAxis("Mouse X");
//                 float mouseY = Input.GetAxis("Mouse Y");
//
//                 // Project to XY plane
//                 Vector3 direction = new Vector3(mouseX, mouseY, 0f).normalized;
//                 float mouseSpeed = new Vector2(mouseX, mouseY).magnitude / Time.deltaTime;
//
//                 // Cap speed
//                 float cappedSpeed = Mathf.Min(mouseSpeed * 0.01f, maxMouseSpeed); // tweak factor
//                 Vector3 targetVelocity = direction * cappedSpeed;
//
//                 // Accelerate toward target velocity
//                 velocity = Vector3.MoveTowards(velocity, targetVelocity, mouseAcceleration * Time.deltaTime);
//
//                 // Move the object
//                 transform.position += velocity * Time.deltaTime;
//
//                 Vector3 move = Vector3.zero;
//                 if (Input.GetKey(KeyCode.Space))
//                 {
//                     Vector3 toCamera = (Camera.main.transform.position - transform.position).normalized;
//                     move += toCamera * (zMoveSpeed * Time.deltaTime);
//                 }
//                 transform.position += move;
//
//                 // Check distance to camera
//                 float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
//                 if (distanceToCamera < destroyDistance)
//                 {
//                     OnReachCamera();
//                 }
//                 if (isWandering)
//                 {
//                     currentVelocity = Vector3.Lerp(currentVelocity, targetDirection * maxWanderSpeed, Time.deltaTime / transitionDuration);
//                     transform.position += currentVelocity * Time.deltaTime;
//                 }
//             }
// 			else
// 			{
// 				// ---- Create virtual point ----
//                 Plane movementPlane = new Plane(-mainCamera.transform.forward, virtualPoint);
//                 Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//
//                 if (movementPlane.Raycast(ray, out float enter))
//                 {
//                     Vector3 mouseWorldPos = ray.GetPoint(enter);
//
//                     Vector3 toMouse = mouseWorldPos - virtualPoint;
//                     Vector3 targetVirtualVelocity = toMouse.normalized * maxMouseSpeed;
//                     virtualVelocity = Vector3.MoveTowards(virtualVelocity, targetVirtualVelocity, mouseAcceleration * Time.deltaTime);
//                     virtualPoint += virtualVelocity * Time.deltaTime;
//                 }
// 				Debug.Log(virtualPoint);
//
//                 // ---- Wander movement ----
//                 if (isWandering)
//                 {
// 					if (wanderPosition.sqrMagnitude > 1) // If too far away from virtual point, ensure wandering towards virtual point
// 					{
//                         currentVelocity = Vector3.Lerp(currentVelocity, (virtualPoint - transform.position) * maxWanderSpeed, Time.deltaTime / transitionDuration);
//                     }
// 					else
// 					{
//                         currentVelocity = Vector3.Lerp(currentVelocity, targetDirection * maxWanderSpeed, Time.deltaTime / transitionDuration);
//                     }
// 					wanderPosition += currentVelocity * Time.deltaTime;
// 					transform.position = virtualPoint + wanderPosition;
//                 }
//                 
//                 // Move the object
//                 transform.position += velocity * Time.deltaTime;
//
//                 Vector3 move = Vector3.zero;
//                 if (Input.GetKey(KeyCode.Space))
//                 {
// 	                Vector3 toCamera = (Camera.main.transform.position - transform.position).normalized;
// 	                move += toCamera * (zMoveSpeed * Time.deltaTime);
//                 }
//                 transform.position += move;
//
//                 // Check distance to camera
//                 float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
//                 if (distanceToCamera < destroyDistance)
//                 {
// 	                OnReachCamera();
//                 }
//                 if (isWandering)
//                 {
// 	                currentVelocity = Vector3.Lerp(currentVelocity, targetDirection * maxWanderSpeed, Time.deltaTime / transitionDuration);
// 	                transform.position += currentVelocity * Time.deltaTime;
//                 }
//             }
// 		}
//
//
// 	}
//
// 	public void OnClick(){
// 		selected = true;
// 		StartCoroutine(MoveUpSmoothly());
// 		sr.material.SetFloat(emissionID, 3.5f);
// 	}
//
// 	public void OnMouseEnter(){
// 		if (!selected){
// 			sr.material.SetFloat(emissionID, 2.5f);
// 		}
// 	}
//
// 	public void OnMouseExit(){
// 		if (!selected){
// 			sr.material.SetFloat(emissionID, 0);
// 		}
// 	}
//
// 	void OnReachCamera(){
// 		Debug.Log("Object reached the camera and is being destroyed.");
// 		controlling = false;
// 		Camera.main.GetComponent<Click>().StopControlling();
// 		LevelManager.OnFoodReached(this);
//
// 		Destroy(gameObject);
// 	}
//
// 	void OnTriggerEnter(Collider other){
// 		if (other.CompareTag("ClickableSprite") || other.CompareTag("Obstacle")){
// 			Debug.Log("Collided with " + other.tag);
// 			OnReachObstacle();
// 			OnCollisionEvent?.Invoke(this);
// 		}
//
// 		if (other.CompareTag("Layer")){
// 			GetComponent<SpriteRenderer>().sortingOrder = other.GetComponent<Layer>().orderInLayer + 1;
// 		}
// 	}
//
// 	void OnReachObstacle(){
// 		// You can add more effects here if you want
// 		controlling = false;
// 		// selected = false;
// 		CameraController.instance.Shake(0.5f);
// 		StartCoroutine(SmoothKnockback());
// 	}
//
// 	IEnumerator MoveUpSmoothly(){
// 		Vector3 targetPos = transform.position + Vector3.up * moveUpDistance;
// 		Vector3 velocity = Vector3.zero;
//
// 		float distanceThreshold = 0.01f;
// 		while (Vector3.Distance(transform.position, targetPos) > distanceThreshold){
// 			transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, moveUpDuration);
// 			yield return null;
// 		}
//
// 		transform.position = targetPos;
//
//         controlling = true;
// 		isWandering = true;
//
// 		StartCoroutine(ChangeDirectionRoutine());
// 	}
//
// 	IEnumerator ChangeDirectionRoutine(){
// 		yield return new WaitForSeconds(transitionDuration);
// 		while (isWandering){
// 			// Pick a new random direction on XY plane
// 			float angle = UnityEngine.Random.Range(0f, 360f);
// 			targetDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f).normalized;
//
// 			yield return new WaitForSeconds(directionChangeInterval);
// 		}
// 	}
//
// 	IEnumerator SmoothKnockback(){
// 		StartCoroutine(FlashRoutine());
// 		Vector3 start = transform.position;
// 		Vector3 end = start + transform.forward * knockbackDistance;
//
// 		float elapsed = 0f;
// 		while (elapsed < knockbackDuration){
// 			transform.position = Vector3.Lerp(start, end, elapsed / knockbackDuration);
// 			elapsed += Time.deltaTime;
// 			yield return null;
// 		}
//
// 		transform.position = end;
// 	}
//
// 	IEnumerator FlashRoutine(){
// 		Renderer rend = GetComponent<Renderer>();
// 		if (rend == null) yield break;
//
// 		for (int i = 0; i < flashCount; i++){
// 			rend.enabled = false;
// 			yield return new WaitForSeconds(flashInterval / 2f);
//
// 			rend.enabled = true;
// 			yield return new WaitForSeconds(flashInterval / 2f);
// 		}
//
// 		controlling = true;
// 	}
// }

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public enum FoodCategory{
	Lemon = 0,
	Mango = 1,
	Eggs = 2,
	Banana = 3,
	Milk = 4,
	Cola = 5,
	Pepsi = 6,
	Soda = 7,
	Salad = 8,
	Cheese = 9,
	Sandwich = 10,
	RiceBall = 11,
	DonutBlue = 12,
	Watermelon = 13,
	DonutPink = 14
}

public enum MovementMode{
	Keyboard,
	MouseVelocity,
	MousePosition
}

public class Food : MonoBehaviour{
	[Header("食物种类")] public FoodCategory category;
	[Space] public SpriteRenderer icon;

	public static readonly Color[] outlineColors = new[]{
		new Color(0.8f, 0.7f, 0.1f), // Lemon
		new Color(1f, 0.6f, 0.2f), // Mango
		new Color(1f, 1f, 0.9f), // Eggs
		new Color(1f, 0.9f, 0.3f), // Banana
		new Color(1f, 1f, 1f), // Milk
		new Color(0.7f, 0f, 0f), // Cola
		new Color(0f, 0.4f, 0.65f), // Pepsi
		new Color(0.6f, 0.8f, 0.2f), // Soda
		new Color(0.4f, 0.8f, 0.4f), // Salad
		new Color(1f, 0.85f, 0.4f), // Cheese
		new Color(0.8f, 0.6f, 0.4f), // Sandwich
		new Color(0.9f, 0.8f, 0.6f), // RiceBall
		new Color(0.6f, 0.95f, 1f), // DonutBlue
		new Color(0.2f, 0.6f, 0.2f), // Watermelon
		new Color(0.95f, 0.6f, 0.6f), // DonutPink
	};


	private bool applyNoise = false;
	private bool controlling = false;
	private bool selected = false;
	public float moveUpDistance = 5f;
	public float moveUpDuration = 1.5f;
	[FormerlySerializedAs("moveSpeed")] public float maxSpeed = 5f;
	public float zMoveSpeed = 1f;
	public float sensitivity = 10f;
	public float destroyDistance = 5f;

	public float knockbackDistance = 0.5f;
	public float knockbackDuration = 0.5f;
	public int flashCount = 3;
	public float flashInterval = 0.5f;

	public static int emissionID = Shader.PropertyToID("_Emission");
	public static int colorID = Shader.PropertyToID("_EdgeColor");

	public static event Action<Food> OnCollisionEvent;
	private QTEController qte;
	private SpriteRenderer sr;

	[SerializeField] private MovementMode mode;

	private Vector3 targetPos;
	public float targetZ;

	public float noiseAmplitude, noiseFrequency;

	void Start(){
		icon = GetComponentInChildren<SpriteRenderer>();
		icon.sprite = ResourceManager.Load<Sprite>("Sprites/FoodIcons/" + category.ToString());
		qte = LevelManager.instance.qteController;
		sr = GetComponent<SpriteRenderer>();
		sr.material.SetColor(colorID, outlineColors[(int)category]);
		sr.material.SetFloat(emissionID, 0);
		targetPos = transform.position;
		targetZ = transform.position.z;
	}


	void Update(){
		if (controlling && !qte.getQTEStarted()){
			if (controlling){
				Vector3 mousePos = Tools.GetMousePosGivenZ(targetPos.z);

				if (Input.GetKey(KeyCode.Space)){
					targetZ -= zMoveSpeed * Time.deltaTime;
					mousePos.z = targetZ;
				}

				var delta = Vector3.Lerp(targetPos, mousePos, sensitivity * Time.deltaTime) - targetPos;

				delta = delta.normalized * Mathf.Clamp(delta.magnitude, 0, maxSpeed * Time.deltaTime);

				targetPos += delta;

				Vector3 noise = GetNoise(noiseAmplitude);

				transform.position = Vector3.Lerp(transform.position, targetPos + noise, Time.deltaTime);
			}

			float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
			if (distanceToCamera < destroyDistance){
				OnReachCamera();
			}
		}
	}

	public void OnClick(){
		selected = true;
		StartCoroutine(MoveUpSmoothly());
		sr.material.SetFloat(emissionID, 3.5f);
	}

	public void OnMouseEnter(){
		if (!selected){
			sr.material.SetFloat(emissionID, 2.5f);
		}
	}

	public void OnMouseExit(){
		if (!selected){
			sr.material.SetFloat(emissionID, 0);
		}
	}

	void OnReachCamera(){
		Debug.Log("Object reached the camera and is being destroyed.");
		controlling = false;
		Camera.main.GetComponent<Click>().StopControlling();
		LevelManager.OnFoodReached(this);

		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag("ClickableSprite") || other.CompareTag("Obstacle")){
			Debug.Log("Collided with " + other.tag);
			OnReachObstacle();
			OnCollisionEvent?.Invoke(this);
		}

		if (other.CompareTag("Layer")){
			GetComponent<SpriteRenderer>().sortingOrder = other.GetComponent<Layer>().orderInLayer + 1;
		}
	}

	void OnReachObstacle(){
		// You can add more effects here if you want
		controlling = false;
		// selected = false;
		CameraController.instance.Shake(0.5f);
		StartCoroutine(SmoothKnockback());
	}

	IEnumerator MoveUpSmoothly(){
		Vector3 targetPos = transform.position + Vector3.up * moveUpDistance;
		Vector3 velocity = Vector3.zero;
		float currAmplitude = 0, delta = noiseAmplitude / (moveUpDuration * 2);
		Vector3 virtualPos = transform.position;

		float distanceThreshold = 0.01f;
		while (Vector3.Distance(virtualPos, targetPos) > distanceThreshold){
			currAmplitude += delta * Time.deltaTime;

			virtualPos = Vector3.SmoothDamp(virtualPos, targetPos, ref velocity, moveUpDuration);
			transform.position = virtualPos + GetNoise(currAmplitude);
			yield return null;
		}

		Debug.Log(currAmplitude);

		controlling = true;
	}

	public Vector3 GetNoise(float amplitude){
		float t = Time.time * noiseFrequency;

		float xNoise = Mathf.Sin(t) * 0.5f + Mathf.Cos(t * 1.3f) * 0.5f;
		float yNoise = Mathf.Sin(t * 1.7f) * 0.5f + Mathf.Cos(t * 1.1f) * 0.5f;

		return new Vector3(xNoise, yNoise, 0f) * amplitude;
	}


	IEnumerator SmoothKnockback(){
		StartCoroutine(FlashRoutine());
		Vector3 start = transform.position;
		Vector3 end = start + transform.forward * knockbackDistance;

		float elapsed = 0f;
		while (elapsed < knockbackDuration){
			transform.position = Vector3.Lerp(start, end, elapsed / knockbackDuration);
			elapsed += Time.deltaTime;
			yield return null;
		}

		transform.position = end;
	}

	IEnumerator FlashRoutine(){
		Renderer rend = GetComponent<Renderer>();
		if (rend == null) yield break;

		for (int i = 0; i < flashCount; i++){
			rend.enabled = false;
			yield return new WaitForSeconds(flashInterval / 2f);

			rend.enabled = true;
			yield return new WaitForSeconds(flashInterval / 2f);
		}

		controlling = true;
	}
}