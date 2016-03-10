using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {
    /*
   
    */
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

       // Init_Singleton();
	}

 
    
    void Init_Singleton()
    {
        // 싱글톤 미리 할당
      //  CollisionManager.Instance.Init();
    }
	

}
