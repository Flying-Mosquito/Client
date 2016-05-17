using UnityEngine;
using System.Collections;

public class TimescaleChange : MonoBehaviour {
    public float fLength = 50f;
    public float fAngle = 30f;

    // 객체에서 플레이어로 향하는 각도를 계산하여 TimeScale을 바꿈 
    void Update()
    {
        // 플레이어에게 raycast, 거리 안에 들어오면 플레이어 내부에 있는 TimeScale조절 함수를 부르게 한다.
        if (Input.GetMouseButtonDown(0))        // 이거 플레이어쪽으로 빼줘야함 
            Time.timeScale = 1f;


        if (CollisionManager.Instance.Check_Sight(transform, PlayerCtrl.Instance.transform.position, fLength, fAngle))//PlayerCtrl.Instance.transform, transform.position, fLength, fAngle))//Check_RayHit(PlayerCtrl.Instance.transform, "PLAYER", 300f)) 
            PlayerCtrl.Instance.ChangeTimeScale();

    }
}
