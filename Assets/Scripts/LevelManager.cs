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

	[Header("最大关卡时间")] public float maxTime;

	[Header("最大QTE次数")] public int maxQTECount;

	[Header("场景物体")] public NoteMenu noteMenu;
	public QTEController qteController;

	public void Awake(){
		instance = this;
	}

	public void Start(){
		InitNoteMenu();
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
}