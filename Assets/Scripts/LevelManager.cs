using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{
	public static LevelManager instance;

	[Header("当前是第几关")] public int currLevel;

	[Header("是否是最后一关")] public bool isLastLevel = false;

	[Header("食物类别")] public FoodCategory[] foods;

	[Header("食物需求数量（依次对应）")] public int[] counts;

	[Header("玩家是否需要按照顺序拾取")] public bool inOrder;

	public float maxTime;
	public int maxQTECount;

	public NoteMenu noteMenu;

	public void Awake(){
		InitNoteMenu();
		instance = this;
	}

	public void InitNoteMenu(){
		noteMenu.inOrder = inOrder;
		int i = 0;
		foreach (FoodCategory c in foods){
			noteMenu.AddFood(c, counts[i]);
			i++;
		}
	}

	public void ToNextLevel(){
		if (!isLastLevel){
			SceneSwitching.SwitchTo((currLevel + 1).ToString());
		}
		else{
			SceneSwitching.SwitchTo("Ending");
		}
	}

	public void RestartScene(){
		SceneSwitching.SwitchTo(currLevel.ToString());
	}
}