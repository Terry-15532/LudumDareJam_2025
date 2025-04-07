using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using static UnityEngine.InputSystem.UI.VirtualMouseInput;

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
	DonutPink = 14,
	Cake = 15,
	Pudding = 16
}

public enum MovementMode{
	Keyboard,
	MouseVelocity,
	MousePosition
}

#if UNITY_EDITOR
[CustomEditor(typeof(Food))]
public class FoodEditor : Editor{
	private Food food;
	private SerializedProperty categoryProp;
	private FoodCategory previousCategory;

	private void OnEnable(){
		food = (Food)target;
		categoryProp = serializedObject.FindProperty("category");
		previousCategory = food.category;
	}

	public override void OnInspectorGUI(){
		serializedObject.Update();

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(categoryProp);

		GUILayout.Space(10);

		if (EditorGUI.EndChangeCheck()){
			serializedObject.ApplyModifiedProperties();

			if (food.category != previousCategory){
				ReplaceWithPrefab(food.category);
				return;
			}
		}

		if (GUILayout.Button("转换为障碍")){
			food.gameObject.tag = "Obstacle";
			var colliders = food.GetComponentsInChildren<BoxCollider>();
			foreach (var c in colliders){
				c.isTrigger = true;
			}

			DestroyImmediate(food);
		}

		DrawDefaultInspector();
	}

	private void ReplaceWithPrefab(FoodCategory category){
		Food prefab = Resources.Load<Food>("Prefabs/Foods/" + category.ToString());

		if (prefab == null){
			Debug.LogWarning($"找不到预制体：{category}");
			return;
		}

		GameObject oldFood = food.gameObject;
		Vector3 pos = oldFood.transform.position;
		Quaternion rot = oldFood.transform.rotation;
		Transform parent = oldFood.transform.parent;

		Food newFood = Instantiate(prefab, parent, true);
		int siblingIndex = oldFood.transform.GetSiblingIndex();
		newFood.transform.SetSiblingIndex(siblingIndex);
		newFood.transform.position = pos;
		newFood.transform.rotation = rot;

		SerializedObject oldSO = new SerializedObject(oldFood);
		SerializedObject newSO = new SerializedObject(newFood);
		SerializedProperty prop = oldSO.GetIterator();

		while (prop.NextVisible(true)){
			if (prop.name == "m_Script" || prop.name == "category")
				continue;

			SerializedProperty newProp = newSO.FindProperty(prop.name);
			if (newProp != null)
				newProp.serializedObject.CopyFromSerializedProperty(prop);
		}

		newSO.ApplyModifiedProperties();

		newFood.name = category.ToString();
		newFood.icon = newFood.GetComponent<SpriteRenderer>();


		newFood.category = category;

		Selection.activeGameObject = newFood.gameObject;

		Undo.RegisterCreatedObjectUndo(newFood, category.ToString());
		Undo.DestroyObjectImmediate(oldFood);
	}
}
#endif

public class Food : MonoBehaviour{
	[HideInInspector] public FoodCategory category;
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
		new Color(0.95f, 0.6f, 0.6f), // Cake
		new Color(1f, 0.85f, 0.4f) // Pudding
	};


	private bool applyNoise = false;
	private bool controlling = false;
	private bool selected = false;
	private float moveUpDistance => LevelManager.instance.moveUpDistance;
	private float moveUpDuration => LevelManager.instance.moveUpDuration;
	private float maxSpeed => LevelManager.instance.maxSpeed;
	private float zMoveSpeed => LevelManager.instance.zMoveSpeed;
	private float sensitivity => LevelManager.instance.sensitivity;
	private float destroyDistance => LevelManager.instance.destroyDistance;

	private float knockbackDistance => LevelManager.instance.knockbackDistance;
	private float knockbackDuration => LevelManager.instance.knockbackDuration;
	private int flashCount => LevelManager.instance.flashCount;
	private float flashInterval => LevelManager.instance.flashInterval;
	private float baseAmplitude => LevelManager.instance.noiseAmplitude;
	private float noiseAmplitude;
	private float baseFrequency => LevelManager.instance.noiseFrequency;
	private float noiseFrequency;
	private float noiseFrequencyMultiplier => LevelManager.instance.noiseFrequencyMultiplier;
	private float noiseAmplitudeMultiplier => LevelManager.instance.noiseAmplitudeMultiplier;


	public static int emissionID = Shader.PropertyToID("_Emission");
	public static int colorID = Shader.PropertyToID("_EdgeColor");

	public static event Action<Food> OnCollisionEvent;
	private QTEController qte;
	private SpriteRenderer sr;

	[SerializeField] private MovementMode mode;

	private Vector3 targetPos;
	public float targetZ;


	void Start(){
		icon = GetComponentInChildren<SpriteRenderer>();
		icon.sprite = ResourceManager.Load<Sprite>("Sprites/FoodIcons/" + category.ToString());
		qte = LevelManager.instance.qteController;
		sr = GetComponent<SpriteRenderer>();
		sr.material.SetColor(colorID, Color.black);
		sr.material.SetFloat(emissionID, 1);
		targetPos = transform.position;
		targetZ = transform.position.z;
		noiseAmplitude = baseAmplitude;
		noiseFrequency = baseFrequency;
	}


	void Update(){
		if (controlling && !qte.getQTEStarted()){
			Vector3 mousePos = Tools.GetMousePosGivenZ(targetPos.z);

			if (Input.GetKey(KeyCode.Space)){
				targetZ -= zMoveSpeed * Time.deltaTime;
			}

			var delta = Vector3.Lerp(targetPos, mousePos, sensitivity * Time.deltaTime) - targetPos;
			delta.z = targetPos.z.Lerp(targetZ, 5 * Time.deltaTime) - targetPos.z;

			delta = delta.normalized * Mathf.Clamp(delta.magnitude, 0, maxSpeed * Time.deltaTime);

			targetPos += delta;
			var posMin = LevelManager.instance.posMin;
			var posMax = LevelManager.instance.posMax;

			targetPos = new Vector3(Mathf.Clamp(targetPos.x, posMin.x, posMax.x),
				Mathf.Clamp(targetPos.y, posMin.y, posMax.y), targetPos.z);

			var heightPercent = posMin.y.Lerp(posMax.y, 0.5f).invLerp(posMax.y, targetPos.y);

			noiseAmplitude = baseAmplitude * Mathf.Max(heightPercent * noiseAmplitudeMultiplier, 1);
			noiseFrequency = baseFrequency * Mathf.Max(heightPercent * noiseFrequencyMultiplier, 1);

			Vector3 noise = GetNoise(noiseAmplitude);


			transform.position = Vector3.Lerp(transform.position, targetPos + noise, 3 * Time.deltaTime);

			transform.position = new Vector3(Mathf.Clamp(transform.position.x, posMin.x - 0.2f, posMax.x + 0.2f),
				Mathf.Clamp(transform.position.y, posMin.y - 0.1f, posMax.y + 0.1f), transform.position.z);


			float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
			if (distanceToCamera < destroyDistance){
				OnReachCamera();
			}
		}
	}

	public void OnClick(){
		selected = true;
		LevelManager.instance.setCursorGrab();
		StartCoroutine(MoveUpSmoothly());
		sr.material.SetFloat(emissionID, 3.5f);
	}

	public void OnMouseEnter(){
		if (!selected){
			sr.material.SetFloat(emissionID, 2.5f);
			sr.material.SetColor(colorID, outlineColors[(int)category]);
		}
	}

	public void OnMouseExit(){
		if (!selected){
			sr.material.SetFloat(emissionID, 1);
			sr.material.SetColor(colorID, Color.black);
		}
	}

	void OnReachCamera(){
		Debug.Log("Object reached the camera and is being destroyed.");
		controlling = false;
		Camera.main.GetComponent<Click>().StopControlling();
		LevelManager.OnFoodReached(this);
		SoundSys.PlaySound("eat_short");
		LevelManager.instance.setCursorNormal();
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
		SoundSys.PlaySound("hit");
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
		targetZ = end.z;
		targetPos.z = end.z;

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