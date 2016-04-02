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
    // private Vector3 vMoveToRaindrop;
    private GameObject dest_Raindrop;
   // public FollowCam _Camera;
    
    private ulong State = Constants.ST_EDLE;

   
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

   // private bool bClickRaindrop;    // 빗방울 클릭했는지 아닌지의상태
    private bool bCheckBoost;       // 스테미나 부스트가 일정량이상 차있어야 사용 가능하게 하는 함수 
    private bool bRotation;
    public  bool bWallCollision;        // 현재 사용하고 있지 않음 
    public  bool isBoost { get; private set; }
    public  bool isCling;           // 붙어있는지의 상태 
    public  bool bCling;            // 붙을지 안붙을지의 상태 ( 빗방울 클릭했는지 아닌지의상태 ) 
    public  bool isConfused { get; private set; }   // 카메라가 흔들리는 상태( 추가로 키 안먹게 해야겠지 ) 
    public  bool isStick;            // 혀에 달라붙은 상태
    public  bool isInRainzone;  // Rainzone 내부에 있는지 체크하는 변수 


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
        //vMoveToRaindrop = Vector3.zero;

        // _Camera = GetComponentInChildren<FollowCam>();

        // controller = GetComponent<CharacterController>();
        //---------------
        bCheckBoost = true;    
        bWallCollision = false;
        bCling = false;
     //   bClickRaindrop = false;
        isCling = false;
        isStick = false;
        isInRainzone = false;
        //--------------- // 나중에 구조체로 묶을것
        fStamina = 100f;
        fXAngle = 0f;
        fYAngle = 0f;

        fSpeed = 3.0f;      // 변화하는 속도 
        fOwnSpeed = 3.0f;   // 원래 자신의 속도
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

    void Update()
    {
      //  print("isINRainzone = " + isInRainzone);
        //    print("sizeOf ULONG : " + sizeof(ulong));
        //   print("sizeOf INT : " + sizeof(int));
        //   print("sizeOf LONG : " + sizeof(long));
        Action();
        RotateAnimation();  // 플레이어 몸체 회전효과
    }


    private void KeyInput()
    {
        State = Constants.ST_EDLE;
        
        ///////////////////////////////////////////////// 마우스왼쪽 클릭
        if (Input.GetMouseButton(0))    
        {
            //isCling = false;
          
            if (true == isInRainzone) // Rainzone 안에 있으면서 물방울을 향하는 행동이나, 물방울에서 떨어지는 상태 변경 
            {
                if (Input.GetMouseButtonDown(0)) // 입력을 한번만 받는다
                {
                    print("입력은 한번만 받을 거에요 ");
                    if (true == isCling)    // 붙어있는 상태라면 떨어질 수 있게 한다 . 붙은 상태여야 떨어질 수 있음 
                    {
                        Debug.Log("뗐다 ");

                        tr.transform.parent = null;
                        tr.transform.localScale = new Vector3(1, 1, 1);
                        ClingObj.transform.parent = null;

                        bCling = false;
                        dest_Raindrop = null;       // 목표 물방울이 없어진다 
                        isCling = false;            // 붙지 않은 상태가 된다

                    }

                    // 후에 거리 추가해야함!!! - 일단 물방울과 마우스의 충돌체크 
                    // 물방울에 붙기위한 충돌 체크 
                    else if (false == bCling)//bClickRaindrop)   // 물방울이 클릭 되지 않은 상태면서 물방울에 붙은 상태가 아니라면 , // RAINDROP 레이어어를 가진 물체와 raycast // 상태변경 
                    {
                        if ((dest_Raindrop = CollisionManager.Instance.Get_MouseCollisionObj("RAINDROP")) != null)
                        {
                            print("물방울과의 충돌체크 ");
                            //bClickRaindrop = true;
                            bCling = true;
                        }
                    }
                }
            }
            else // Rainzone 안에 없으면 부스터 사용 
                State |= Constants.ST_BOOST;
        }


        ///////////////////////////////////////////////// 마우스 왼쪽 뗄 때 
        if (Input.GetMouseButtonUp(0))  
        {
            if (fStamina < 10)
                bCheckBoost = false;
        }


        ///////////////////////////////////////////////// 마우스 오른쪽 클릭
        if ( Input.GetMouseButtonDown(1))   
        {
            if (CollisionManager.Instance.Check_RayHit(tr.position, tr.forward, "WALL", 3f))  // 벽에 붙을지 체크 
            {
                bCling = true;
                
                State |= Constants.ST_CLING;
            }
        }


        ///////////////////////////////////////////////// 마우스 오른쪽 뗄 때
        //  RainZone에서는 통하지 않음을 추가해야함 
        if (Input.GetMouseButtonUp(1))   
        {
            Debug.Log("뗐다 ");
            tr.transform.parent = null;
          //  tr.transform.localScale = new Vector3(1, 1, 1);  // ?? 이거 꼭 필요한가
            ClingObj.transform.parent = null;

            bCling = false;
            isCling = false;
        }
    }

    private void Action()
    {
        ulong mask = 0;

        // Boost인지 아닌지 체크 
        mask = (State & Constants.ST_BOOST);
        if ((mask) > 0)
            Boost(true);
        else
            Boost(false);

        mask = (State & Constants.ST_WALLCOLLISION);
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
      //  fXAngle = Input.acceleration.x;      // fYRotation : 좌우 각도 변경  
      //  fYAngle = -Input.acceleration.y - 0.4f;    // fXRotatino : 상하 각도 변경 , 0.4 는 각도 좀더 세울수 있게 마이너스 한것      
//  #else
      fXAngle = Input.GetAxis("Horizontal");
      fYAngle = Input.GetAxis("Vertical");
        //#endif

        //  tr.Rotate((Vector3.up * fYRotation * Time.deltaTime * fRotSpeed) + (tr.right * -fXRotation * Time.deltaTime * fRotSpeed) , Space.World);
        if ( !isCling) // 붙어 있다면
        {
            tr.Rotate(Vector3.up * fXAngle * Time.deltaTime * fRotSpeed, Space.World);
            tr.Rotate(Vector3.right * -fYAngle * Time.deltaTime * fRotSpeed, Space.Self);
        }
        else // 붙어 있을 시 아무 동작도 하지 않도록 함 
        {
           // tr.Rotate(Vector3.up * fXAngle * Time.deltaTime * fRotSpeed, Space.Self);
           // tr.Rotate(Vector3.right * -fYAngle * Time.deltaTime * fRotSpeed, Space.Self);
        }


        // tr.Rotate(tr.right * -fXRotation * Time.deltaTime * fRotSpeed);
        // tr.Translate(Vector3.forward * Time.deltaTime * fSpeed, Space.Self); // Vector3 Dir1 = tr.forward * 0.5f;  tr.Translate( Dir1 * Time.deltaTime * fSpeed); // 이것과 같음..
        //print("isInRainzone : " + isInRainzone.ToString() + ", " + "bClickRaindrop : " + bCling.ToString());
        if (isStick )
        {
            print("움직이면 안돼");
        }
        else if (true == isInRainzone && true == bCling) //bClickRaindrop)
        {
            // 속도증가 추가 
            // 이동벡터를 구하고, 전진은 그쪽으로 ,
            // dest_Raindrop.transform.position - tr.position; 
           // print("나는 지금 물방울을 향해 움직이고 있다");
            Vector3 vec = dest_Raindrop.transform.position - tr.position;
            vec.Normalize();
            rigidBody.MovePosition(tr.position + (vec * fSpeed * Time.deltaTime));
        }
        else  if( !isCling)   // 붙어있지 않다면 앞으로 이동 
        {
            movement.Set(tr.forward.x, tr.forward.y, tr.forward.z);
            rigidBody.MovePosition(tr.position + (movement * fSpeed * Time.deltaTime));
        }
        // 지금 구현해야 하는것 : rainzone에 들어와 있으면서 , 빗방울 클릭시 -> 이동방향이 정해지고 , 방향변경은 가능하게 ( 방향변경을 해도 이동방향은 변함이 없다) 
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
       // print("isInRainzone : " + isInRainzone);
        if (coll.gameObject.tag == "RAINZONE")
        {
            print("RAINZONE");
            isInRainzone = true;
        }
        
        if (coll.gameObject.tag == "WALL" || coll.gameObject.tag == "RAINDROP") // 벽이나 물방울이면 붙게한다 
        {
            if(bCling)
            {
                Debug.Log(" 붙었습니다 ");
                isCling = true;

                ClingObj.transform.parent = coll.transform;
                tr.transform.parent = ClingObj.transform;

            }
            else    // 벽이나 물방울에 붙지 않는 상태인데 일정속도 이상으로 부딪히면 충돌효과를 준다 
            {
                // 충돌효과 코드 좀더 수정해야 할 듯 
                if (fSpeed > fOwnSpeed + 3f) // _fPower > fPower 도 포함시켜야함 - confused가 되는 상황?
                    StartCoroutine("StartConfused");  
            }
        }

        if( coll.gameObject.tag == "FROG_TONGUE")
        {
            isStick = true;

            ClingObj.transform.parent = coll.transform;
            tr.transform.parent = ClingObj.transform;
            //tr.transform.position = coll.gameObject.transform.position; // 일단 개구리의 위치에 맞추긴 했는데 삐져나옴 
            
            rigidBody.velocity = Vector3.zero;
            print("RigidBody : " + rigidBody.velocity.x.ToString() + ", " + rigidBody.velocity.y.ToString() + " ," + rigidBody.velocity.z.ToString());
            rigidBody.isKinematic = true;   // 물리적인 영향을 끔 

        }
    }

    private IEnumerator StartConfused() // 캐릭터의 상태를 3초간 confused로 바꿔주는 함수
    {
        isConfused = true;
        yield return new WaitForSeconds(3f);        

        isConfused = false;
    }
}
