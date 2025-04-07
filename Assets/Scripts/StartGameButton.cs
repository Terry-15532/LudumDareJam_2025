using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartGameButton : Button{
	//public Animator fade1;
	//public Animator fade2;
	//public Animator cutsceneAnimator;
	//public DOTweenAnimation cutscene;
	//public Lines[] lines;
	//public HoverDetection exit;
	public CustomUIElement fade;
	public GameObject intro;
	public VideoPlayer introPlayer;
	public bool firstTime;
	private Image img;
	private int emissionID = Shader.PropertyToID("_Emission");
	private int colorID = Shader.PropertyToID("_EdgeColor");


	public override void OnMouseEnter(){
		base.OnMouseEnter();
		SoundSys.PlaySound("Pop");
		img.material.SetColor(colorID, new Color(0.95f, 0.6f, 0.7f));
	}

	public override void OnMouseExit(){
		base.OnMouseExit();
		img.material.SetColor(colorID, Color.black);
	}

	public override void OnMouseDown(){
		base.OnMouseDown();
		SoundSys.PlaySound("Pop");
		img.material.SetColor(colorID, Color.black);
	}

	public override void OnMouseUpAsButton(){
		base.OnMouseUpAsButton();
		StartGame();
	}

	public void Start(){
		if (firstTime){
			img = GetComponent<Image>();
			img.material.SetColor(colorID, Color.black);
			img.material.SetFloat(emissionID, 2f);
		}
	}


	public void StartGame(){
		if (firstTime){
			StartCoroutine(WaitThenStart());
		}
		else{
			Tools.CallDelayed(() => { SceneSwitching.SwitchTo("1"); }, 0.6f);
		}
	}

	public void ExitGame(){
		Application.Quit();
	}

	IEnumerator WaitThenStart(){
		SoundSys.PlaySound("eat_short");
		fade.SetActive(true);
		fade.SetAttrAni(0, 1, 0.5f, ColorAttr.a);

		yield return new WaitForSeconds(0.6f);

		intro.SetActive(true);
		introPlayer.Play();

		yield return new WaitForSeconds(1f);

		while (introPlayer.isPlaying)
			yield return null;

		intro.SetActive(false);
		SceneSwitching.SwitchTo("1");
	}
}