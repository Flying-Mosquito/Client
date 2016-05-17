using UnityEngine;
using System.Collections;
using DG.Tweening;


// 플레이어가 전역으로 바뀌었기 때문에 컴퍼넌트 필요 없으니 수정이 필요함 
public class FollowCam : MonoBehaviour
{
    public PlayerCtrl _Player;

    private Transform tr;
    //public  Transform   targetTr;   //Player Tr
    // public Transform CamPivot; //private
    private float Target_fXAngle;    // 카메라를 좌우로 흔들기 위한 값
    public float Target_fYAngle;    // 카메라를 상하로 흔들기 위한 값 (?)이게 필요한가 
    public float Target_fSpeed;
    //private Vector3 vPreLocaPosition;

    public float fDist;
    public float fHeight;
    public float fDampTrace;

    private Vector3 FirstLocalPosition;
    private Quaternion FirstLocalRotation;



    private float fPower;
    //public bool bTemp = false;
    public float fX;
    private bool isLookFar;
    private bool isCollisionOthers;

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
        FirstLocalRotation = tr.localRotation;
        //vPreLocaPosition = FirstLocalPosition;
        fPower = _Player.fSpeed;
        fX = 0f;
        isLookFar = false;
        isCollisionOthers = false;

    }
    public void SetIsCollisionOthers(bool _b)
    {
        isCollisionOthers = _b;
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

        Target_fSpeed = _Player.fSpeed;// * 0.3f;
        if (_Player.state != Constants.ST_CLING)        // 붙어있지 않을 때의 카메라 효과(위치)
        {
            isLookFar = false;
            tr.localPosition = Vector3.Lerp(tr.localPosition
                                        , FirstLocalPosition + new Vector3(Target_fXAngle * 0.8f, 0.1f, -Target_fSpeed * 0.01f)
                                        , 0.1f);
        }
        else //어딘가 붙을때의 카메라 효과 
        {

            if (isLookFar != true)
            {
                StartCoroutine("DelayLerpPosition", 1f);
                tr.localPosition = Vector3.Lerp(tr.localPosition
                                     , FirstLocalPosition + new Vector3(Target_fXAngle * 0.8f, 0.15f, -Target_fSpeed * 0.1f)
                                     , 0.1f);
            }
            else
            {
                if (isCollisionOthers != true)  // 카메라 충돌 체크, 플레이어가 가려지는 일이 발생하지 않게 함
                {
                    tr.localPosition = Vector3.Lerp(tr.localPosition
                                          , FirstLocalPosition + new Vector3(Target_fXAngle * 0.8f, 0.15f, -20f)
                                          , 0.05f);
                }
            }
        }

        if (_Player.state == Constants.ST_BOOST)        // 부스터 사용할 때의 카메라 회전값 설정
        {
            //print("부스터 쓰는중");
            tr.localRotation = Quaternion.Slerp(tr.localRotation, Quaternion.Euler(60f, 0f, 0f), 0.005f);
        }
        else                                            // 부스터 사용하지 않을 때의 회전값 설정 
            tr.localRotation = Quaternion.Slerp(tr.localRotation, FirstLocalRotation, 0.01f);


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

    private IEnumerator DelayLerpPosition(float fTime)
    {

        yield return new WaitForSeconds(fTime);

        isLookFar = true;

    }


}
