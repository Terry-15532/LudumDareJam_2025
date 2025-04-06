using System;
using System.Collections;
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
	public Animator textboxAnimator;

	public void startQTE(){
		StartCoroutine(startedQTE());
	}

	IEnumerator startedQTE(){
		started = true;
		textboxAnimator.Play("Pop-up");
        transform.Find("Fade").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
		transform.Find("QTE Parent").gameObject.SetActive(true);
		ring1.setRadius(500);
		ring2.setRadius(1000);
		pulseLeft = maxPulses;
		currentSuccess = 0;
		ring1.GetComponent<RingPulse>().Activate();
		ring2.GetComponent<RingPulse>().Activate();
		yield return new WaitForSeconds(1.5f);
		textboxAnimator.Play("Pop-down");
	}

	private void Update(){
		if (started){
			if (Input.GetKeyDown(KeyCode.Space)){
				Debug.Log("Spacebar");
				if (gracePeriod > 0){
					BeatSuccess();
				}
				else{
					gracePeriod = maxGrace;
				}
			}

			//Debug.Log("pulse left: " + pulseLeft + "; gracePeriod: " + gracePeriod);
			if (pulseLeft == 0 && gracePeriod <= 0){
				started = false;
				FindFirstObjectByType<AlertnessBar>().GameOver();
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate(){
		if (started){
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
		pulseLeft--;
		if (started){
			if (gracePeriod > 0){
				BeatSuccess();
			}
			else{
				gracePeriod = maxGrace;
				QTEVFX.Create(QTERanking.Failed);
			}
		}
	}

	void BeatSuccess(){
		Debug.Log("success");
		currentSuccess++;
        int error = Math.Abs(maxGrace - gracePeriod);
        if (error < maxGrace / 2)
        {
            QTEVFX.Create(QTERanking.Perfect);
        }
        else
        {
            QTEVFX.Create(QTERanking.Good);
        }
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