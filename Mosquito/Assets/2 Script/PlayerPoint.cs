using UnityEngine;
using System.Collections;

public class PlayerPoint : MonoBehaviour {

	// Use this for initialization
    void Awake()
    {
       PlayerCtrl.Instance.SetTransform(transform.position, transform.rotation);
        //SetPlayerSetTransform();
        //if (null == GameManager.Instance.Player.name)
        //    print("플레이어는 널이래");
        // else
        //  GameManager.Instance.PlayerCtrl.SetTransform(transform.position, transform.rotation);
    }
	//void Start () {
      
	//}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerCtrl.Instance.SetStateIdle(false);
            gameObject.SetActive(false);    //비활성화 
        }

       

    }
    
}
