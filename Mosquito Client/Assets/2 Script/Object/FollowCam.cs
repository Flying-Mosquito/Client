using UnityEngine;
using System.Collections;
using DG.Tweening;


// 플레이어가 전역으로 바뀌었기 때문에 컴퍼넌트 필요 없으니 수정이 필요함 
public class FollowCam : MonoBehaviour
{
    public PlayerCtrl _Player;

    private Transform tr;
    //public  Transform   targetTr;   //Player Tr
    public Transform CamPivot; //private
    private float Target_fXAngle;    // 카메라를 좌우로 흔들기 위한 값
    public float Target_fYAngle;    // 카메라를 상하로 흔들기 위한 값 (?)이게 필요한가 
    public float Target_fSpeed;
    private Vector3 vPreLocaPosition;

    public float fDist;
    public float fHeight;
    public float fDampTrace;

    private Vector3 FirstLocalPosition;


    private float fPower;
    public bool bTemp = false;
    public float fX;

    // Use this for initialization
    void Awake()
    {



    }
    void Start()
    {

        tr = GetComponent<Transform>();
        fDist = 1f;
        fHeight = 0.4f;
        fDampTrace = 20.0f;

        // 카메라를 상하좌우로 흔들기 위한 캐릭터 기울기값
        _Player = PlayerCtrl.Instance;//GameObject.Find("Player").GetComponent<PlayerCtrl>();// PlayerCtrl.Instance;//GameManager.Instance.PlayerCtrl;//GameObject.Find("Player").GetComponent<Player>();


        Target_fXAngle = _Player.fXAngle;
        Target_fYAngle = _Player.fYAngle;
        Target_fSpeed = _Player.fSpeed;


        //Target_fSpeed  = Player.
        FirstLocalPosition = tr.localPosition;
        vPreLocaPosition = FirstLocalPosition;
        fPower = _Player.fSpeed;
        fX = 0f;
    }
    void Update()
    {
        //targetPlus.position = targetTr.position /*+ targetTr.up *2f*/ + targetTr.forward * 3f;

        //   Debug.DrawRay(targetPlus.transform.position, -targetPlus.forward * 100.0f, Color.green);
        //targetPlus.localPosition = targetTr.forward * 1.0f;
        //targetPlus.position = targetTr.transform.position + (tr.forward ) * 10.0f;
        //   print("ANgle xy : " + _Player.fXAngle + ", " + _Player.fYAngle);

    }

    // 타깃이 움직인 이후에 움직여야 하기 때문에 LateUpdate()
    void LateUpdate()
    {
        Target_fXAngle = _Player.fXAngle;
        Target_fYAngle = _Player.fYAngle;
        // Target_fSpeed = Player.fSpeed;

        //   fPreSpeed = Target_fSpeed;
        // Target_fSpeed = _Player.fSpeed;

        // 이 코드가 필요가 없다?
        //tr.position = Vector3.Lerp(tr.position, (_Player.transform.position + (-_Player.transform.forward * fDist)) + (_Player.transform.up * fHeight), fDampTrace * Time.deltaTime);
        //Target_fSpeed = _Player.fSpeed * 0.08f;
        //tr.position = Vector3.Lerp(tr.position, (_Player.transform.position + (-_Player.transform.forward * fDist)) + (_Player.transform.up * fHeight), fDampTrace * Time.deltaTime);

        //tr.position = Vector3.Lerp(tr.position, (targetTr.position + (-targetTr.forward * fDist )) + (targetTr.up * fHeight), fDampTrace * Time.deltaTime);
        Move_RightLeft();     // 카메라효과 - 좌우, 상하로 움직이기 
        Shake_Camera();


    }

    private void Move_RightLeft()   // 카메라효과 - 좌우, 상하로 움직이기
    {
        // tr.localPosition = Vector3.Lerp(tr.localPosition, new Vector3(Target_fXAngle *4f, 0.2f, -3f), 0.03f);
        //    tr.localPosition = Vector3.Lerp(FirstLocalPosition * Target_fSpeed , FirstLocalPosition + new Vector3(Target_fXAngle * 4f, 0.2f, -0.5f), 0.15f);
        /*  tr.localPosition = Vector3.Lerp(FirstLocalPosition
                                          , FirstLocalPosition + new Vector3(Target_fXAngle * 3f, 0.2f, -Target_fSpeed * 2f) 
                                           , 0.15f);*/

        Target_fSpeed = _Player.fSpeed;// * 0.3f;
        /*print(" fPreSpeed : " + fPreSpeed);
        print(" Target_fSpeed : " + Target_fSpeed);

        if ((Mathf.Abs(fPreSpeed - Target_fSpeed) > 5)) // 이전속도와 10이상 차이나게 되면
        {
            //print("차이나 우에에에");
            fPreSpeed = Mathf.Lerp(fPreSpeed, Target_fSpeed, 5f);
            print("차이나 fPreSpeed : " + fPreSpeed);
            tr.localPosition = Vector3.Lerp(FirstLocalPosition
                             , FirstLocalPosition + new Vector3(Target_fXAngle * 3f, 0.2f, -Target_fSpeed)
                               , 5f);
            
        }
        else
        {
            
  

        }
        */
        // 이전위치값을 넣고, localPosition을 다시 계산. 이전값과 현재의 값이 차이가 많이 나면 현재의 localPositon을 다시 계산한다.
        vPreLocaPosition = tr.localPosition;
        tr.localPosition = Vector3.Lerp(FirstLocalPosition
                                   , FirstLocalPosition + new Vector3(Target_fXAngle , 0.2f, -Target_fSpeed * 0.1f)
                                     , 0.5f);
        /*  tr.localPosition = Vector3.Lerp(FirstLocalPosition
                                     , FirstLocalPosition + new Vector3(Target_fXAngle * 3f, 0.2f, -Target_fSpeed * 0.3f)
                                       , 0.15f);

          //  print("PrelocalPosition : " + vPreLocaPosition);
          // print("tr.LocalPosition : " + tr.localPosition);
          
          if (Mathf.Abs(vPreLocaPosition.z - tr.localPosition.z) > 0.05f)
          {
               print("바뀜바뀜!!!!!!!!!!!!!!!!!!");
              tr.localPosition = Vector3.Lerp(FirstLocalPosition
                                     , FirstLocalPosition + new Vector3(Target_fXAngle * 3f, 0.2f, -Target_fSpeed * 0.3f)
                                       , 0.06f);
          }
          
        */

    }

    public void Shake_Camera() // 수정필요 
    {
        // 충격관련된 수학 - 아마 sin cos
        if ((PlayerCtrl.Instance.variable & Constants.BV_bStun) > 0)//_Player.isConfused) // 플레이어가 Confused 상태라면 흔들어주세요 ( 충돌 후 ) 
        {
            fX += 0.1f;
            // 여기서 흔들흔들
            tr.localPosition = Vector3.Lerp(tr.localPosition
                                            , new Vector3(tr.localPosition.x, Mathf.Sin(fX * 10.0f) * Mathf.Pow(0.5f, fX), tr.localPosition.z)
                                            , 0.1f);
            //tr.transform.localPosition.y;

            if (_Player.fSpeed > fPower)  // 더 큰 충격을 받았을 때? 
            {
                fPower = _Player.fSpeed;

            }
        }
        else
        {
            fPower = 0f;
            fX = 0f;
        }

    }

    /*
     public IEnumerator Shake_Camera(float _fPower)
     {

         Debug.Log("Shake_Camera");

         // 충격관련된 수학 - 아마 sin cos
         if (_fPower > fPower)   // 받은 충격보다 더 큰 충격이 들어오거나 , Shake 시간이 끝났을 때 => Shake 시작 
            yield return null;

         yield return new WaitForSeconds(3f);
     }
     */

}
