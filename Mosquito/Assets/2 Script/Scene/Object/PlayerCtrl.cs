using UnityEngine;
using System.Collections;
//using DG.Tweening;

// 해야할 일 : 기울기에 따라 플레이어가 너무 움직인다 - 조정필요 
// 2. 기본 가속도? 
public class PlayerCtrl : Singleton<PlayerCtrl>//MonoBehaviour
{
    //private CharacterController controller;
    private Transform tr;
    private Transform[] tr_Mesh;
    public Transform targetPlus;
    private Rigidbody rigidBody;
    Vector3 movement;
    private Vector3 vDir;
    private GameObject Player_dest;
    private RainDrop dest_RaindropScript;
    // public FollowCam _Camera;

    private ulong state = Constants.ST_FLYING;
    //public ulong  
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

    // ????? ======================
    private bool bCheckBoost;       // 스테미나 부스트가 일정량이상 차있어야 사용 가능하게 하는 함수 
    //public  bool isBoost { get; private set; }
    public  bool bCling;            // 붙을지 안붙을지의 상태 ( 빗방울 클릭했는지 아닌지의상태 ) 
    public  bool isCling;           // 붙어있는지의 상태 
    public  bool isClickRaindrop;    // 빗방울에 붙을때 상태. 일반 사물에 붙는지, 빗방울을 향하는지 구별하기 위해 사용 
    public  bool isConfused { get; private set; }   // 카메라가 흔들리는 상태( 추가로 키 안먹게 해야겠지 ) 
    public  bool isStick;            // 혀에 달라붙은 상태
    public  bool isInRainzone;  // Rainzone 내부에 있는지 체크하는 변수 
    public  bool isCanSlow { get; private set; } // TimeScale을 변경할 수 있는지에 대한 함수 

    public GameObject ClingObj;

    private int iBlood; // 흡혈량 ( 미구현 )

    
    void Awake()
    {
        DontDestroyOnLoad(this);

        ClingObj = GameObject.Find("ClingObject");
        tr = GetComponent<Transform>();
        tr_Mesh = GetComponentsInChildren<Transform>();
        rigidBody = GetComponent<Rigidbody>();
        vDir = Vector3.zero;
        state = Constants.ST_IDLE;//ST_CLING;
        //---------------
        bCheckBoost = true;
        bCling = false;
        isCanSlow = true;
        //   bClickRaindrop = false;
        isCling = false;
        isStick = true;// false;
        isInRainzone = false;
        isClickRaindrop = false;
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

        Move();
    }

    void Update()
    {
        KeyInput();
        Action();
        RotateAnimation();  // 플레이어 몸체 회전효과
        rigidBody.velocity = Vector3.zero;

       //print("STATE : " + state); // 플레이어 상태확인 
    }


    private void KeyInput()     // StateCheck 로 이름을 바꾸자..
    {
        state = Constants.ST_FLYING;

        if (isCling)
            state = Constants.ST_CLING;

        ///////////////////////////////////////////////// 마우스왼쪽 클릭
        if (Input.GetMouseButton(0))
        {
           // state = Constants.ST_FLYING;
            if (true == isInRainzone) // Rainzone 안에 있으면서 물방울을 향하는 행동이나, 물방울에서 떨어지는 상태 변경 
            {
                if (Input.GetMouseButtonDown(0)) // 입력을 한번만 받는다
                {
                    
                    if (Constants.ST_CLING == state)//true == isCling)    // 붙어있는 상태라면 떨어질 수 있게 한다 . 붙은 상태여야 떨어질 수 있음 
                    {

                        tr.transform.parent = null;
                        tr.transform.localScale = new Vector3(1, 1, 1);
                        ClingObj.transform.parent = null;

                        bCling = false;
                        Player_dest = null;       // 목표 물방울이 없어진다 
                        dest_RaindropScript = null;
                        isClickRaindrop = false;
                        isCling = false;// state = Constants.ST_FLYING; // 날아가는 상태로 바꿔주자
                        //state = Constants.//isCling = false;            // 붙지 않은 상태가 된다 - 다음번에 알아서 
                    }

                    // 후에 거리 추가해야함!!! - 일단 물방울과 마우스의 충돌체크 
                    // 물방울에 붙기위한 충돌 체크 
                    else if (false == bCling)//bClickRaindrop)   // 물방울이 클릭 되지 않은 상태면서 물방울에 붙은 상태가 아니라면 물방울을 클릭한 상태로 바꿔준다, // RAINDROP 레이어어를 가진 물체와 raycast // 상태변경 
                    {
                        if ((Player_dest = CollisionManager.Instance.Get_MouseCollisionObj("RAINDROP")) != null)
                        {
                            dest_RaindropScript = Player_dest.GetComponent<RainDrop>();
                            bCling = true;
                            isClickRaindrop = true;
                        }
                    }
                }
            }
           // else // Rainzone 안에 없으면 부스터 사용 
            //    state = Constants.ST_BOOST;//|= Constants.ST_BOOST;
        }

        if( Input.GetKey(KeyCode.Space))
        {
            state = Constants.ST_BOOST;
        }


        ///////////////////////////////////////////////// 마우스 왼쪽 뗄 때 
        if (Input.GetMouseButtonUp(0))
        {
            if (fStamina < 10)
                bCheckBoost = false;
        }


        ///////////////////////////////////////////////// 마우스 오른쪽 클릭
        if (Input.GetMouseButtonDown(1))
        {

            if ( ( Player_dest = CollisionManager.Instance.Get_MouseCollisionObj("",30f) ) != null)
               // if (CollisionManager.Instance.Check_RayHit(tr.position, tr.forward, "WALL", 3f))  // 벽에 붙을지 체크 
            {
          
                bCling = true;
                //state = Constants.ST_CLING;//|= Constants.ST_CLING;
            }
        }


        ///////////////////////////////////////////////// 마우스 오른쪽 뗄 때
        //  RainZone에서는 통하지 않음을 추가해야함 
        if (Input.GetMouseButtonUp(1))
        {
            if (Constants.ST_CLING == state)
            {
                tr.transform.parent = null;
                //  tr.transform.localScale = new Vector3(1, 1, 1);  // ?? 이거 꼭 필요한가
                ClingObj.transform.parent = null;

                bCling = false;
                isCling = false;
               // state = Constants.ST_FLYING;
            }
        }
    }

    private void Action()
    {
        //ulong mask = 0;

        // Boost인지 아닌지 체크 
       // mask = (state & Constants.ST_BOOST);
        if (state == Constants.ST_BOOST)//(mask) > 0)
        {
            Boost(true);
        }
        else
            Boost(false);

       // mask = (state & Constants.ST_WALLCOLLISION);
        // if ((mask) > 0)
        //   StartCoroutine()

        if (true == bCling && true == isInRainzone) // RainZone안에서 물방울을 클릭한 상태라면
            fSpeed = 40f;

         if (true == isInRainzone && true == bCling )//&& false == isCling ) // rainzone 안에 있고, 빗방울에  붙으려고 할 때 
        {
            if ((null != Player_dest) && ( null != dest_RaindropScript) )                          // 목표 빗방울이 있다면 
            {
                if ( !dest_RaindropScript.isPlop)           // 빗방울이 활성화 상태라면 빗방울의 위치를 가져와서 방향벡터를 구함
                {
                    vDir = (Player_dest.transform.position) - tr.position;
                    vDir.Normalize();
                }
                else
                {
                    Player_dest = null;           // 목표 빗방울이 비활성화 상태라면 빗방울이 이미 다른 오브젝트에 충돌해서 사라졌다는 소리.빗방울이 없어지게 되면 vDir은 이전에 받은 값을 유지한다.(빗방울이 사라져서 없어진 방향 )
                    dest_RaindropScript = null;   
                   
                    SetState_NotCling();
                }
            }
        }
    }
    private void Boost(bool _bool)
    {
        if ((_bool) && (fStamina > fStaminaMinus) && (bCheckBoost)) // _bool 이 true 일때 - 마우스가 클릭상태일때 이면서 , 현재 스테가 스테미나 감소량보다 크고, Boost가 가능할 때 
        {
            fStamina -= fStaminaMinus;
            state = Constants.ST_BOOST;
            //isBoost = true;
            // 가속도 조절
            fBoostVal += 50f * Time.deltaTime;
            if (fBoostVal > fBoostMax)
                fBoostVal = fBoostMax;

            fSpeed = fBoostVal + fOwnSpeed;
            if (fStamina < 1)               // 스테미나가 1 이하로 떨어지면 부스터를 사용할 수 없다 
            {
                fStamina = 1;
                bCheckBoost = false;
             //   state = Constants.ST_FLYING;
            }
        }
        else
        {
            fStamina += 0.1f;
           // state = Constants.ST_FLYING;
            //state = Constants.ST_BOOST;
            //isBoost = false;
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

        if (isStick || (state == Constants.ST_IDLE) )
        {
            
            
        }
        else if (true == isInRainzone && true == bCling && isClickRaindrop && (Constants.ST_CLING != state) )// false == isCling)//null != dest_Raindrop) // rainzone 안에 있고, 빗방울에  붙으려고 할 때 
        {
          //  print("레")
            rigidBody.MovePosition(tr.position + (vDir * fSpeed * Time.deltaTime));
         
        }
        else if (!(Constants.ST_CLING == state))//!isCling)  // 붙어있지 않다면 움직이게 함
        {
            // 회전 
            tr.Rotate(Vector3.up * fXAngle * Time.deltaTime * fRotSpeed, Space.World);
            tr.Rotate(Vector3.right * -fYAngle * Time.deltaTime * fRotSpeed, Space.Self);

            // 움직임
            movement.Set(tr.forward.x, tr.forward.y, tr.forward.z);
            rigidBody.MovePosition(tr.position + (movement * fSpeed * Time.deltaTime));
            //Time.fixedDeltaTime;
    
        }

        else // 붙어 있을 시 아무 동작도 하지 않도록 함 
        {
            // tr.Rotate(Vector3.up * fXAngle * Time.deltaTime * fRotSpeed, Space.Self);
            // tr.Rotate(Vector3.right * -fYAngle * Time.deltaTime * fRotSpeed, Space.Self);
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
        isClickRaindrop = false;
        if (coll.gameObject.tag == "WALL" || coll.gameObject.tag == "RAINDROP") // 벽이나 물방울이면 붙게한다 
        {
            if (bCling)  // 붙으려고 하는 상태면 
            {
                if (Player_dest.gameObject == coll.gameObject)    // 충돌한 물체가 목표물과 같다면 달라붙는다 -- 벽이 여기서 에러 
                {
                    isCling = true;
                    //State = 
                    ClingObj.transform.parent = coll.transform;
                    tr.transform.parent = ClingObj.transform;
                    state = Constants.ST_CLING;
                }
                else    // 충돌한 물체가 목표물과 다르다면 붙으려고 하는 상태 해제됨
                {
                    //isCling = false;
                 //   print("충돌한 물체가 목표물과 다르다");
                   // state = Constants.ST_FLYING;
                    bCling = false;
                }

            }
            else    // 벽이나 물방울에 붙지 않는 상태인데 일정속도 이상으로 부딪히면 충돌효과를 준다 
            {
                // 충돌효과 코드 좀더 수정필요, 속도도 수정 필요  
                if (fSpeed > fOwnSpeed + 3f) // _fPower > fPower 도 포함시켜야함 - confused가 되는 상황? 
                    StartCoroutine("StartConfused");
            }
        }
        else
            bCling = false;

        if (coll.gameObject.tag == "FROG_TONGUE")
        {
            isStick = true;

            ClingObj.transform.parent = coll.transform;
            tr.transform.parent = ClingObj.transform;
            //tr.transform.position = coll.gameObject.transform.position; // 일단 개구리의 위치에 맞추긴 했는데 삐져나옴 

            rigidBody.velocity = Vector3.zero;
            rigidBody.isKinematic = true;   // 물리적인 영향을 끔 

        }
    }

    private IEnumerator StartConfused() // 캐릭터의 상태를 3초간 confused로 바꿔주는 함수
    {
        isConfused = true;
        yield return new WaitForSeconds(3f);

        isConfused = false;
    }

    // 플레이어가 어딘가에 붙어있다면 붙지 않은 상태로 만들어주는 함수
    public void SetState_NotCling()
    {
        if ( Constants.ST_CLING == state)//isCling)
        {
            isClickRaindrop = false;
 
            tr.transform.parent = null;
            ClingObj.transform.parent = null;

            bCling = false;
            //isCling = false;
          //  state = Constants.ST_FLYING;
        }
    }

    public void SetTransform(Vector3 _pos, Quaternion _rot)
    {
        transform.position = _pos;
        transform.rotation = _rot;
    }

    public void SetStateIdle(bool _bool)
    {
        if (_bool)
        {
            state = Constants.ST_IDLE;
            isStick = true;
        }
        else
        {
          //  state = Constants.ST_FLYING;
            isStick = false;
        }
    }

    public void ChangeTimeScale()
    {
        if(true == isCanSlow && false == bCling)
        {
            print("시간을 늦출거야");
            Time.timeScale = 0.3f;
            isCanSlow = false;
            StartCoroutine("ChangeSlowVal");
        }
    }

    private IEnumerator ChangeSlowVal()
    {
        //  if (true == isCanSlow)  // 키입력으로 인해 이미 변수상태가 변경된 상태라면 코루틴 꺼줌
        //   yield return null;
        // 1초 후 시간을 원래대로 바꿔주고, 쿨타임은 2초 
        print("1초 후 시간을 원래대로");
        yield return new WaitForSeconds(0.3f); // 변수 지정해줘야함
        Time.timeScale = 1f;
        print("시간이 돌아왔다, 쿨타임 5초 ");



        yield return new WaitForSeconds(5f);
       
        isCanSlow = true;
    }


    // 부모자식관계를 만들어주거나 해제시켜주는 함수 필요 
    // 레이어 확인해서 붙을 수 있는앤지 없는지 확인하는 코드 필요 
}
