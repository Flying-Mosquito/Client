using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

  //    public Player Player;
 //  public static GameObject PlayerPrefab;

  //  public static GameObject Player;
    //public static PlayerCtrl PlayerCtrl;


    //GameObject rainDrop = (GameObject)Instantiate(RainDropPrefab);
    void Awake()
    {
        DontDestroyOnLoad(this);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

       
   
            //Player = (GameObject)Instantiate(PlayerPrefab); // Player를 프리팹을 통해 만들고
          //  PlayerCtrl = Player.GetComponent<PlayerCtrl>(); // PlayerCtrl 컴포넌트도 할당한다 
          //  print("플레이어 만들기");

    }
	void Start () {
       
       // Player = GameObject.Find("Player").GetComponent<Player>();

       // Init_Singleton();
	}
    
    void Init_Singleton()
    {
        // 싱글톤 미리 할당
      //  CollisionManager.Instance.Init();
    }

}
