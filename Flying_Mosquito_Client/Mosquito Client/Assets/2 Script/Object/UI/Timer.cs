using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : Singleton<Timer>
{

    public Text timerText;
    private float startTime;
    public Transform gameover;
	// Use this for initialization
	void Start () {
        
        startTime = Time.time;
       
       
    }

    // Update is called once per frame
    void Update() {
        if (PlayerCtrl.Instance.iHP < 10)
        {
            gameover.gameObject.SetActive(true);
        }
        else
            gameover.gameObject.SetActive(false);
           
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        timerText.text = minutes + ":" + seconds;

	}
}
