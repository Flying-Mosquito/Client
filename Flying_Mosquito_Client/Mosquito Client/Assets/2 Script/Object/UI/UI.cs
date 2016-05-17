using UnityEngine;
using System.Collections;

public class UI : Singleton<UI>
{

    public Transform timer;
    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(this);
        print("setactiveTruee");
        timer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (PlayerCtrl.Instance.state != Constants.ST_IDLE)
            timer.gameObject.SetActive(true);
        if (PlayerCtrl.Instance.state == Constants.ST_IDLE)
        {
            timer.gameObject.SetActive(false);

        }
    }
}
