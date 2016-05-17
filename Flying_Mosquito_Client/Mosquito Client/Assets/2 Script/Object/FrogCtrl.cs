using UnityEngine;
using System.Collections;


public class FrogCtrl : MonoBehaviour
{
    Animator anim;

    public PlayerCtrl _Player;

    //private GameObject _Cube;
    private GameObject _Tongue;
    private Transform _TongueTr;
    //private float x;

    private Transform tr;
    private RaycastHit hit;
    private float fLength;

    //private bool bCheck;
    private bool isInSight;
    //private bool bSwallow;
    private Vector3 vTongueDir;




    // Use this for initialization
    void Awake()
    {
        _Player = GameObject.Find("Player").GetComponent<PlayerCtrl>(); //PlayerCtrl.Instance;// 
        tr = GetComponent<Transform>();

        //temp
        //_Cube = GameObject.Find("Cube");
        _TongueTr = tr.transform.FindChild("Tongue");   // 자식으로 가진 Tongue의 Transform을 가져오기 위해 사용 
        _Tongue = _TongueTr.gameObject;                 //
                                                        //_Tongue = GameObject.Find("Tongue");
                                                        // x = 0f;
        fLength = 6f;
        //bCheck = false;
        isInSight = false;
        // bSwallow = false;
        vTongueDir = Vector3.zero;
        // fPlayerOwnSpeed = _Player.fOwnSpeed; // 플레이어 초기화 이후에 실행되어야 한다 - 혹시나 해서 Start로 뺌

        anim = GetComponent<Animator>();
        anim.Play("idle");
        ////

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {    //xecution Order를 변경했기 때문에 Player 이후에 호출됨
         // isInSight = Check_Sight();

        isInSight = CollisionManager.Instance.Check_Sight(tr, _Player.transform.position, fLength, 40f);
        if (isInSight)
        {

            vTongueDir = (_Player.transform.position + _Player.transform.forward) - _Tongue.transform.position;    // 방향벡터 구하기

            vTongueDir.Normalize();               // 정규화

        }

        // Debug.DrawRay(tr.position, tr.forward * 200f, Color.blue);
        Vector3 vTemp = (_Player.transform.position - tr.position);
        vTemp.Normalize();
        //Debug.DrawRay(tr.position, vTemp * fLength, Color.yellow);

        if (isInSight)
        {

            _Tongue.SendMessage("SetMoveState", true, SendMessageOptions.DontRequireReceiver);
            _Tongue.SendMessage("SetDir", vTongueDir, SendMessageOptions.DontRequireReceiver);
        }
        /*
                Debug.Log("(Update) - x : " + vTongueDir.x.ToString() +
                 "y : " + vTongueDir.y.ToString() +
                 "z : " + vTongueDir.z.ToString());*/
    }

    void LateUpdate()
    {

    }



    private bool Check_Sight()
    {
        // Debug.Log("_Player.position : " + _Player.transform.position.x.ToString() + ", " + _Player.transform.position.y.ToString() + ", " + _Player.transform.position.z.ToString());
        // Debug.Log("tr.position : " + tr.position.x.ToString() + ", " + tr.position.y.ToString() + ", " + tr.position.z.ToString());

        Vector3 vDir = _Player.transform.position - tr.position;   // 플레이어 
                                                                   //  print("vDir Length = " + Vector3.Distance(_Player.transform.position, tr.position).ToString());
        vDir.Normalize();


        //     if (Physics.Raycast(_Obj.transform.position, _Obj.transform.forward, out hit, _fDist))
        //     if( hit.collider.tag == _Tag)

        //Debug.Log("Angle : " + Vector3.Angle(tr.forward , vDir).ToString());

        Debug.DrawRay(tr.position, vDir * fLength, Color.blue);
        if ((Vector3.Angle(tr.forward, vDir) < 40) && Physics.Raycast(tr.position, vDir, out hit, fLength))   // 범위안에 들어와 있으면서, 각도가 40보다 작다
        {

            //Debug.Log("들어옴");
            //  if (false == bCheck)
            //  {
            //   bCheck = true;
            if (hit.collider.tag == "PLAYER")
            {

                vTongueDir = (_Player.transform.position + _Player.transform.forward) //  * _Player.fOwnSpeed * Time.deltaTime
                            - _Tongue.transform.position;    // 방향벡터 구하기 - 플레이어의 다음 위치의 방향에 
                //
                vTongueDir.Normalize();               // 정규화
                                                      //  _Tongue.transform.rotation = Quaternion.Euler(vDir);

                //}
                return true;
            }

        }

        //  bCheck = false;
        // Debug.Log("안들어옴");
        return false;

    }

}
