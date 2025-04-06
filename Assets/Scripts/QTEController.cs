using UnityEngine;

public class QTEController : MonoBehaviour{
	public int maxGrace;
	private int gracePeriod;
	public int maxPulses;
	public int successPulses;
	public int pulseLeft;
	private int currentSuccess;
	private bool started = false;
	public UIRing ring1;
	public UIRing ring2;

	public void startQTE(){
		transform.GetChild(0).gameObject.SetActive(true);
		ring1.setRadius(500);
		ring2.setRadius(1000);
		pulseLeft = maxPulses;
		currentSuccess = 0;
		started = true;
	}

	// Update is called once per frame
	void FixedUpdate(){
		if (started){
			if (Input.GetKeyDown(KeyCode.Space)){
				if (gracePeriod > 0){
					BeatSuccess();
				}
				else{
					gracePeriod = maxGrace;
				}
			}

			if (gracePeriod > 0){
				gracePeriod--;
			}

			if (pulseLeft == 0 && gracePeriod == 0){
				started = false;
				FindFirstObjectByType<AlertnessBar>().GameOver();
			}
		}
	}

	public void BeatHit(){
		if (started){
			if (gracePeriod > 0){
				BeatSuccess();
			}
			else{
				gracePeriod = maxGrace;
			}
		}
	}

	void BeatSuccess(){
		Debug.Log("success");
		currentSuccess++;
		if (currentSuccess >= 3){
			transform.GetChild(0).gameObject.SetActive(false);
			started = false;
			FindFirstObjectByType<AlertnessBar>().UnfreezeBar();
		}
	}

	public bool getQTEStarted(){
		return started;
	}
}