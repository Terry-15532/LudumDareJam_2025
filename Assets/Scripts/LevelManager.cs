using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{
	public static LevelManager instance;

	[Header("当前是第几关")] public int currLevel;

	[Header("是否是最后一关")] public bool isLastLevel = false;

	[Header("食物类别")] public FoodCategory[] foods;

	[Header("食物需求数量（依次对应）")] public int[] counts;

	[Header("玩家是否需要按照顺序拾取")] public bool inOrder;

	// [Header("最大关卡时间")] public float maxTime;

	[Header("场景物体")] public NoteMenu noteMenu;

	[Header("最大、最小坐标")] public Vector2 posMax;
	public Vector2 posMin;

	[Header("---食物相关设置---")]
	
	[Header("抬手距离&时间")]
	public float moveUpDistance = 0.2f;
	public float moveUpDuration = 0.2f;

	[Header("最大速度&Z轴速度")] public float maxSpeed = 2f;
	public float zMoveSpeed = 1f;

	[Header("灵敏度")] public float sensitivity = 10f;

	[Header("距离相机多近算吃到")] public float destroyDistance = 1f;

	[Header("击退距离&时间")] public float knockbackDistance = 0.5f;
	public float knockbackDuration = 0.5f;

	[Header("闪烁次数&间隔时间")] public int flashCount = 3;
	public float flashInterval = 0.3f;

	[Header("抖动幅度&频率")] public float noiseAmplitude = 0.3f;
	public float noiseFrequency = 1.5f;
	
	[Header("高处抖动幅度&频率倍率")]
	public float noiseAmplitudeMultiplier = 1.5f;
	public float noiseFrequencyMultiplier = 2f;

	[Space]
	[Header("其他设置")]
	public QTEController qteController;
	private static Sound BGM;

	public Texture2D defaultCursorTexture;
	public Texture2D grabCursorTexture;
	public Vector2 hotSpot = Vector2.zero;
	public UnityEngine.CursorMode cursorMode = UnityEngine.CursorMode.Auto;

	public void Awake(){
		instance = this;
	}

	public void Start(){
		InitNoteMenu();

		if (BGM == null){
			BGM = SoundSys.PlaySound("BGM", true);
		}
	}

	public void InitNoteMenu(){
		noteMenu.inOrder = inOrder;
		int i = 0;
		foreach (FoodCategory c in foods){
			noteMenu.AddFood(c, counts[i]);
			i++;
		}

		noteMenu.Show();
		Tools.CallDelayed(() => { noteMenu.Fold(); }, 2f);
	}

	public static void ToNextLevel(){
		Debug.Log("Level Complete!");
		Tools.CallDelayed(() => {
			if (!instance.isLastLevel){
				SceneSwitching.SwitchTo((instance.currLevel + 1).ToString());
			}
			else{
				SceneSwitching.SwitchTo("Ending");
			}
		}, 1);
	}

	public static void OnFoodReached(Food f){
		instance.noteMenu.OnFoodReached(f);
	}

	public void RestartScene(){
		SceneSwitching.SwitchTo(currLevel.ToString());
	}

	public void PauseBGM(){
		BGM.audioSource.Pause();
	}

	public void ResumeBGM(){
		BGM.audioSource.Play();
	}

	public void StopBGM(){
		Destroy(BGM.gameObject);
	}

	public void setCursorGrab(){
		Cursor.SetCursor(grabCursorTexture, hotSpot, cursorMode);
	}

	public void setCursorNormal(){
		Cursor.SetCursor(defaultCursorTexture, hotSpot, cursorMode);
	}
}