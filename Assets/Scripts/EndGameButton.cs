using UnityEngine;

public class EndGameButton : Button{
	public static int levelIdx = 1;
	
	public void EndGame(){
		if (levelIdx > 0){
			SceneSwitching.SwitchTo(levelIdx.ToString());
		}
		else{
			SceneSwitching.SwitchTo("MainMenu");
		}
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
