using UnityEngine;

public class EndGameButton : Button{
	public static int levelIdx = 1;
	
	public void EndGame(){
		SceneSwitching.SwitchTo(levelIdx.ToString());
	}
	
	public override void OnMouseEnter(){
		base.OnMouseEnter();
		SoundSys.PlaySound("Pop");
	}
	public override void OnMouseDown(){
		base.OnMouseDown();
		SoundSys.PlaySound("Pop");
	}

}
