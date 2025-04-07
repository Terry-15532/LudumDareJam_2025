using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class QTEController : MonoBehaviour{
	public int maxGrace;
	private int gracePeriodSpacebar;
	private int gracePeriodBeat;
	private bool currentBeatSuccess = false;
	public int maxSpacebarCD = 30;
	private int spacebarCD;
	public int maxPulses;
	public int successPulses;
	public int pulseLeft;
	private int currentSuccess;
	private bool started = false;
	private bool ringStarted = false;
	public UIRing ring1;
	public UIRing ring2;
	public Animator textboxAnimator;

	public void startQTE(){
		StartCoroutine(startedQTE());
	}

	IEnumerator startedQTE(){
		pulseLeft = maxPulses;
		started = true;
		textboxAnimator.Play("Pop-up");
		playRandomVoiceline();
		transform.Find("Fade").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

		ringStarted = true;
		transform.Find("QTE Parent").gameObject.SetActive(true);
		ring1.GetComponent<RingPulse>().Init();
        ring2.GetComponent<RingPulse>().Init();
        ring1.setRadius(500);
		ring2.setRadius(1000);
		currentSuccess = 0;
		ring1.GetComponent<RingPulse>().Activate();
		ring2.GetComponent<RingPulse>().Activate();
		yield return new WaitForSeconds(1.5f);
		textboxAnimator.Play("Pop-down");
	}

	private void Update(){
		if (started && ringStarted){
			if (Input.GetKeyDown(KeyCode.Space) && spacebarCD <= 0){
				Debug.Log("Spacebar");
				spacebarCD = maxSpacebarCD;
				if (gracePeriodBeat > 0){
					currentBeatSuccess = true;
					BeatSuccess(gracePeriodBeat);
				}
				else{
					gracePeriodSpacebar = maxGrace;
				}
			}

			//Debug.Log("pulse left: " + pulseLeft + "; gracePeriod: " + gracePeriod);
			if (pulseLeft == 0 && gracePeriodSpacebar <= 0 && gracePeriodBeat <= 0){
                ring1.GetComponent<RingPulse>().StopAll();
                ring2.GetComponent<RingPulse>().StopAll();
                started = false;
                ringStarted = false;
                FindFirstObjectByType<AlertnessBar>().GameOver();
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate(){
		if (started && ringStarted){
			if (gracePeriodSpacebar > 0)
			{
				gracePeriodSpacebar--;
			}
			if (gracePeriodBeat > 0)
			{
				gracePeriodBeat--;
				if (gracePeriodBeat == 0 && !currentBeatSuccess)
				{
					BeatFailed();
				}
			}
			spacebarCD--;
		}
	}

	public void BeatHit(){
		pulseLeft--;
		if (started){
			if (gracePeriodSpacebar > 0){
                currentBeatSuccess = true;
                BeatSuccess(gracePeriodSpacebar);
			}
			else{
				gracePeriodBeat = maxGrace;
				currentBeatSuccess = false;
			}
		}
	}

	void BeatSuccess(int gracePeriod){
		Debug.Log("success");
        SoundSys.PlaySound("heartbeat_final_clipped");
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
            ring1.GetComponent<RingPulse>().StopAll();
            ring2.GetComponent<RingPulse>().StopAll();
            transform.Find("QTE Parent").gameObject.SetActive(false);
            transform.Find("Fade").gameObject.SetActive(false);
            started = false;
			ringStarted = false;
			FindFirstObjectByType<AlertnessBar>().UnfreezeBar();
        }
	}

	void BeatFailed()
	{
        QTEVFX.Create(QTERanking.Failed);
    }

	public bool getQTEStarted(){
		return started;
	}

	private void playRandomVoiceline()
	{
		int rVal = UnityEngine.Random.Range(1, 2);
		if (rVal == 1)
		{
			SoundSys.PlaySound("what_the_hell_denoised");
			textboxAnimator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "What the hell?";
		}else if (rVal == 2)
		{
			SoundSys.PlaySound("whats_that");
            textboxAnimator.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "What's that?";
        }
	}
}