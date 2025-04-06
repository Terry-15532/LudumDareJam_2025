using System.Collections;
//using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    //public Animator fade1;
    //public Animator fade2;
    //public Animator cutsceneAnimator;
    //public DOTweenAnimation cutscene;
    //public Lines[] lines;
    //public HoverDetection exit;
    public GameObject fade;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void StartButton()
    {
        //GetComponent<HoverDetection>().enabled = false;
        //exit.enabled = false;
        // var seq = DOTween.Sequence();
        // seq.AppendInterval(1f);
        // seq.Append(cutscene.tween);
        StartCoroutine(WaitThenStart());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator WaitThenStart()
    {
        //fade1.Play("FadeOut");
        //yield return new WaitForSeconds(1f);
        fade.SetActive(true);

        //cutsceneAnimator.GetComponent<AudioSource>().Play();
        // yield return new WaitForSeconds(1f);
        //cutsceneAnimator.Play("FadeIn");
        //cutscene.DOPlay();
        yield return new WaitForSeconds(0.6f);

        //foreach (Lines line in lines)
        //{
        //    line.fadeIn = true;
        //    yield return new WaitForSeconds(2f);
        //}
        //yield return new WaitForSeconds(1f);
        //fade2.Play("FadeOut");
        //yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(1);
    }
}
