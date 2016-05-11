using UnityEngine;
using System.Collections;

// 이름 바꿔줘야함 Player를 지워..
public class PlayerTimescaleCtrl : MonoBehaviour
{

    //private Transform tr;
    // Use this for initialization
    void Awake()
    {
        //   tr = GetComponent<Transform>();
    }


    void Update()
    {
        // 플레이어에게 raycast, 거리 안에 들어오면 플레이어 내부에 있는 TimeScale조절 함수를 부르게 한다.

        if (Input.GetMouseButtonDown(0))
            Time.timeScale = 1f;

        if (CollisionManager.Instance.Check_Sight(PlayerCtrl.Instance.transform, transform.position, 50f, 30f))//Check_RayHit(PlayerCtrl.Instance.transform, "PLAYER", 300f)) 
        {
            //Debug.DrawRay(transform.position, (PlayerCtrl.Instance.transform.position - transform.position).normalized * 1000f, Color.blue);
            //Time.timeScale = 0.3f;
            PlayerCtrl.Instance.ChangeTimeScale();
        }
    }
}
