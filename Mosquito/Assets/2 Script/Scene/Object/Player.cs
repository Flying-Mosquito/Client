using UnityEngine;
using System.Collections;
//using DG.Tweening;

// 해야할 일 : 기울기에 따라 플레이어가 너무 움직인다 - 조정필요 
// 2. 기본 가속도? 
public class Player : MonoBehaviour {

    //private CharacterController controller;
    private Transform tr;
    private Transform[] tr_Mesh;
    public Transform targetPlus;
    private Rigidbody rigidBody;
    Vector3 movement;
   // public FollowCam _Camera;

    // 나중에 상수로 다 처리해줘야할것들 
    const ulong ST_EDLE = 0x00000001;
    const ulong ST_BOOST = 0x00000002;
    const ulong ST_ACTIVE = 0x00000004;
    const ulong ST_WALLCOLLISION = 0x00000008;
    const ulong ST_CLING = 0x00000010;
    private ulong State = ST_EDLE;

   
    private float fStamina; // 스테미나 ( 미구현)  
    public float fSpeed;// { get; private set; }
    public float fOwnSpeed { get; private set; }
    public float fRotSpeed { get; private set; }
    public float fOwnRotSpeed { get; private set; }
    public float fBoostVal { get; private set; }
    public float fStaminaMinus { get; private set; }
    public float fBoostMax { get; private set; }
    public float fXAngle;      // 좌우 회전
    public float fYAngle;      // 위아래 회전

    private bool bCheckBoost;
    private bool bRotation;
    public  bool bWallCollision;
    public  bool isBoost { get; private set; }
    public  bool isCling;       // 벽에 붙어있는지의 상태 
    public  bool bCling;         // 벽에 붙을지 안붙을지의 상태
    public  bool isConfused { get; private set; }
    public bool isStick;


    public GameObject ClingObj;


    //private float fYRotation_Ani;

    //   private Vector3 vFirstAngle;
    private int iBlood; // 흡혈량 ( 미구현 )
    

    void Awake()
    {
        //임시
        // CollisionMgr = GetComponent<CollisionManager>();
        ClingObj = GameObject.Find("ClingObject");
         tr = GetComponent<Transform>();
        tr_Mesh = GetComponentsInChildren<Transform>();
        rigidBody = GetComponent<Rigidbody>();
       // _Camera = GetComponentInChildren<FollowCam>();

        // controller = GetComponent<CharacterController>();
        //---------------
        bCheckBoost = true;    // 스테미나 부스트가 일정량이상 차있어야 사용 가능하게 하는 함수 
        bWallCollision = false;
        bCling = false;
        isCling = false;
        isStick = false;
        //--------------- // 나중에 구조체로 묶을것
        fStamina = 100f;
        fXAngle = 0f;
        fYAngle = 0f;

        fSpeed = 3.0f;
        fOwnSpeed = 3.0f;
        fRotSpeed = 55f;
        fOwnRotSpeed = 55f;

        fBoostVal = 0f;
        fBoostMax = 10f;
        fStaminaMinus = 0.7f;
        
    }

    void FixedUpdate()
    {
        KeyInput();
        Move();
    }

    void Update() {
       
        Action();
        RotateAnimation();  // 플레이어 몸체 회전효과
    }


    private void KeyInput()
    {
        State = ST_EDLE;
  
        if (Input.GetMouseButton(0))    // 마우스왼쪽 클릭
        {
            isCling = false;
            State |= ST_BOOST;
        }

        if (Input.GetMouseButtonUp(0))  // 마우스 왼쪽 뗄 때 
        {
           
            if (fStamina < 10)
                bCheckBoost = false;
        }


        //Debug.DrawRay(tr.transform.position, tr.transform.forward * 3f, Color.yellow);
        if ( Input.GetMouseButtonDown(1))   // 마우스 오른쪽 클릭할때 
        {
           // Debug.Log("마우스 왼쪽 클릭중");
            if (CollisionManager.Instance.Check_RayHit(tr.position, tr.forward, "WALL", 3f))  // 벽에 붙을지 체크 
            {
              
                bCling = true;
                
                State |= ST_CLING;
            }
        }

        if (Input.GetMouseButtonUp(1))  // 마우스 오른쪽 뗄 때 
        {
            Debug.Log("뗐다 ");
            tr.transform.parent = null;
            tr.transform.localScale = new Vector3(1, 1, 1);
            ClingObj.transform.parent = null;

            bCling = false;
            isCling = false;
        }
    }

    private void Action()
    {
        ulong mask = 0;

        // Boost인지 아닌지 체크 
        mask = (State & ST_BOOST);
        if ((mask) > 0)
            Boost(true);
        else
            Boost(false);

        mask = (State & ST_WALLCOLLISION);
       // if ((mask) > 0)
        //   StartCoroutine()

            

    }
    private void Boost(bool _bool)
    {
        //Debug.Log("_bool : " + _bool.ToString() + ", "+ "bCheckBoost : " + bCheckBoost.ToString()  );

        if ((_bool) && (fStamina > fStaminaMinus) && (bCheckBoost)) // _bool 이 true 일때 - 마우스가 클릭상태일때 이면서 , 현재 스테가 스테미나 감소량보다 크고, Boost가 가능할 때 
        {
            fStamina -= fStaminaMinus;
            isBoost = true;
            // 가속도 조절
            fBoostVal += 50f * Time.deltaTime;
            if (fBoostVal > fBoostMax)
                fBoostVal = fBoostMax;

            fSpeed = fBoostVal + fOwnSpeed;
            if (fStamina < 1)               // 스테미나가 1 이하로 떨어지면 부스터를 사용할 수 없다 
            {
                fStamina = 1;
                bCheckBoost = false;
            }
        }
        else
        {
            fStamina += 0.1f;
            isBoost = false;
            // 가속도 조절
            fBoostVal -= 80f * Time.deltaTime;
            if (fBoostVal < 1)
                fBoostVal = 1f;

            if (fStamina > 100)
                fStamina = 100;

            fSpeed = fBoostVal + fOwnSpeed;

            if (fStamina > 10f)      // 스테미나가 10이상이면 사용가능
                bCheckBoost = true;
        }

    
       //Debug.Log("fBoostVal = " + fBoostVal.ToString() + ", " + "fSpeed = " + fSpeed.ToString() + ", " + "fStamina = " + fStamina.ToString());

    }

    private void Move()     // 일단은 키보드 움직임에 따라서 각도가 변하고, 앞으로 가는것은 자동 
    {
        //# if UNITY_IOS
        //fXAngle = Input.acceleration.x;      // fYRotation : 좌우 각도 변경  
       // fYAngle = -Input.acceleration.y - 0.4f;    // fXRotatino : 상하 각도 변경 , 0.4 는 각도 좀더 세울수 있게 마이너스 한것      
//  #else
      fXAngle = Input.GetAxis("Horizontal");
      fYAngle = Input.GetAxis("Vertical");
        //#endif

        //  tr.Rotate((Vector3.up * fYRotation * Time.deltaTime * fRotSpeed) + (tr.right * -fXRotation * Time.deltaTime * fRotSpeed) , Space.World);
        if ( !isCling)
        {
            tr.Rotate(Vector3.up * fXAngle * Time.deltaTime * fRotSpeed, Space.World);
            tr.Rotate(Vector3.right * -fYAngle * Time.deltaTime * fRotSpeed, Space.Self);
        }
        else
        {
           // tr.Rotate(Vector3.up * fXAngle * Time.deltaTime * fRotSpeed, Space.Self);
           // tr.Rotate(Vector3.right * -fYAngle * Time.deltaTime * fRotSpeed, Space.Self);
        }


        // tr.Rotate(tr.right * -fXRotation * Time.deltaTime * fRotSpeed);

        // Debug.DrawRay(tr.transform.position, tr.transform.forward * 100.0f, Color.blue);

        // tr.Translate(Vector3.forward * Time.deltaTime * fSpeed, Space.Self); // Vector3 Dir1 = tr.forward * 0.5f;  tr.Translate( Dir1 * Time.deltaTime * fSpeed); // 이것과 같음..

        // 뒤에 Space.Self 가 빠져 있어서 내가 생각한대로 안됐었던 것이었다고 한다... ㅠㅠㅠ
        
        if(isStick )
        {
            print("움직이면 안돼");
        }
        else  if( !isCling)   // 앞으로 이동 
        {
            print("움직이고있음");
            movement.Set(tr.forward.x, tr.forward.y, tr.forward.z);
            rigidBody.MovePosition(tr.position + (movement * fSpeed * Time.deltaTime));
       }
 
        Debug.DrawRay(tr.transform.position, Vector3.up * 100.0f, Color.red);
      
    }


    private void RotateAnimation()
    {
       /*  for (int i = 1; i < tr_Mesh.Length; ++i)
             {
              tr_Mesh[i].localRotation = Quaternion.Euler((Vector3.up * fXAngle * 20.0f)
                                                   + (Vector3.right * -fYAngle * 20.0f));
            // tr_Mesh[i].localRotation = Quaternion.Euler(-fYAngle * 20.0f, 0f
             //                                      , (fXAngle * 20.0f));
             }
    */
   // tr_Mesh[1].localRotation = Quaternion.Euler((Vector3.up * fXAngle * 20.0f)
                  //                                 + (Vector3.right * -fYAngle * 20.0f));
                  tr_Mesh[1].localRotation = Quaternion.Euler(-fYAngle * 20.0f, 0f
                                                  , (-fXAngle * 20.0f));
    }

    void OnCollisionEnter(Collision coll)
    {
       // Debug.Log("충돌함");
        if (coll.gameObject.tag == "WALL")
        {
            if(bCling)
            {
                Debug.Log(" 붙었습니다 ");
                isCling = true;

                ClingObj.transform.parent = coll.transform;
                tr.transform.parent = ClingObj.transform;

            }
            else
            {
                if (fSpeed > fOwnSpeed + 3f) // _fPower > fPower 도 포함시켜야함 - confused가 되는 상황?
                    StartCoroutine("StartConfused");  
            }
        }

        if( coll.gameObject.tag == "FROG_TONGUE")
        {
            isStick = true;

            ClingObj.transform.parent = coll.transform;
            tr.transform.parent = ClingObj.transform;
        }



    }

    private IEnumerator StartConfused() // 캐릭터의 상태를 3초간 confused로 바꿔주는 함수
    {
        isConfused = true;
        yield return new WaitForSeconds(3f);        

        isConfused = false;
    }
}
