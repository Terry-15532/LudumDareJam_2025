using UnityEngine;

public class EndGameButton : Button{
	public void EndGame(){
		SceneSwitching.SwitchTo("MainMenu");
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
