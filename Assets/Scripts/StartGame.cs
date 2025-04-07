using System.Collections;
//using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartGame : MonoBehaviour
{
    //public Animator fade1;
    //public Animator fade2;
    //public Animator cutsceneAnimator;
    //public DOTweenAnimation cutscene;
    //public Lines[] lines;
    //public HoverDetection exit;
    public GameObject fade;
    public GameObject intro;
    public VideoPlayer introPlayer;
    public bool firstTime;

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
        if (firstTime)
        {
            StartCoroutine(WaitThenStart());
        }
        else
        {
            StartCoroutine(NormalStart());
        }
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

        intro.SetActive(true);
        introPlayer.Play();

        yield return new WaitForSeconds(1f);

        while (introPlayer.isPlaying)
            yield return null;

        //foreach (Lines line in lines)
        //{
        //    line.fadeIn = true;
        //    yield return new WaitForSeconds(2f);
        //}
        //yield return new WaitForSeconds(1f);
        //fade2.Play("FadeOut");
        //yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(0);
    }

    IEnumerator NormalStart()
    {
        fade.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(0);
    }
}
