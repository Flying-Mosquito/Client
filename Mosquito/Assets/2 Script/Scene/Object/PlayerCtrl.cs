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


    // 플레이어 상태와 변수상태가 들어가 있는 변수 
    public ulong state { get; private set; }
    public ulong variable { get; private set; } 


    //public ulong  
    private float fStamina; // 스테미나 총량
    public float fStaminaMinus { get; private set; }    // 스테미나 감소량 
    public float fSpeed;// { get; private set; }    // 플레이어 최종 속도값
    public float fSpeedVal = 2f;                    // 플레이어 속도 증가값 
    public float fBoostMinus = 20f;                  // 부스터 사용 후에 속도감소 변화값 
    public float fBoostSpeed { get; private set; }    // 기본속도에 더해지는 가속도값
    public float MAXBOOST { get; private set; }     // Boost 사용시의 최대 가속도값 
    public float OWNMAXBOOST = 6f;                       // 일반속도 최대값
    public float fOwnSpeed { get; private set; }    // 일반속도 값 
    public float fRotSpeed { get; private set; }
    public float fOwnRotSpeed { get; private set; }

    public float fXAngle;      // 좌우   회전값
    public float fYAngle;      // 위아래 회전값

    // ????? ======================
    //public  bool isBoost { get; private set; }


        //여기부터 주석처리함 
    //private bool bCheckBoost;       // 스테미나 부스트가 일정량이상 차있어야 사용 가능하게 하는 함수 
    //public  bool bCling;            // 붙을지 안붙을지의 상태 ( 빗방울 클릭했는지 아닌지의상태 ) 
    //public  bool isCling;           // 붙어있는지의 상태 
  //  public  bool isClickRaindrop;    // 빗방울에 붙을때 상태. 일반 사물에 붙는지, 빗방울을 향하는지 구별하기 위해 사용 
    //public  bool isConfused { get; private set; }   // 카메라가 흔들리는 상태( 추가로 키 안먹게 해야겠지 ) 
    //public  bool isStick;            // 혀에 달라붙은 상태
    public  bool isInRainzone;  // Rainzone 내부에 있는지 체크하는 변수 
   // public  bool isCanSlow { get; private set; } // TimeScale을 변경할 수 있는지에 대한 함수 

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
        variable = Constants.BV_bBoost | Constants.BV_Stick | Constants.BV_IsCanSlow | Constants.BV_bStaminaRecovery;

        //bCheckBoost = true;
        //bCling = false;
        //isCanSlow = true;
        //   bClickRaindrop = false;
        //isCling = false;
        //isStick = true;// false;
        //isInRainzone = false;
        //isClickRaindrop = false;
        //--------------- // 나중에 구조체로 묶을것
        fStamina = 100f;
        fStaminaMinus = 40f;

        fXAngle = 0f;
        fYAngle = 0f;

        fSpeed = 0f;     
       // fOwnSpeed = 6.0f;   // MaxSpeed// 원래 자신의 속도
        fRotSpeed = 55f;
        fOwnRotSpeed = 55f;

        fBoostSpeed = 0f;
        MAXBOOST = 10f;
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
                            // rigidBody.velocity = Vector3.zero;  // 이것도 해제해야 할 거야 
                            // print("STATE : " + state); // 플레이어 상태확인 
                            //print("Stamina : " + fStamina);
                             print("fSpeed : " + fSpeed);
       /* if ((variable & Constants.BV_IsBoost) > 0)
            print("부스터");
        else
            print("부스터아님");*/
    }
    private void KeyInput()     // StateCheck 로 이름을 바꾸자..
    {
        //# if UNITY_IOS
        //  fXAngle = Input.acceleration.x * 1.5f;      // fYRotation : 좌우 각도 변경  
        // fYAngle = -(Input.acceleration.y * 1.5f) - 0.5f;    // fXRotatino : 상하 각도 변경 , 0.4 는 각도 좀더 세울수 있게 마이너스 한것      
        //  #else
        fXAngle = Input.GetAxis("Horizontal");
        fYAngle = Input.GetAxis("Vertical");
        //#endif

        if ((-0.15f < fXAngle) && (fXAngle < 0.15f))
            fXAngle = 0f;
        if ((-0.1f < fYAngle) && (fYAngle < 0.15f))
            fYAngle = 0f;
        //  tr.Rotate((Vector3.up * fYRotation * Time.deltaTime * fRotSpeed) + (tr.right * -fXRotation * Time.deltaTime * fRotSpeed) , Space.World);

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
                        MakeParentNull();
                      /*  tr.transform.parent = null;
                        tr.transform.localScale = new Vector3(1, 1, 1);
                        ClingObj.transform.parent = null;
                        */
                        variable &= ~(Constants.BV_bCling); //  bCling = false;

                        Player_dest = null;       // 목표 물방울이 없어진다 
                        dest_RaindropScript = null;

                        //두개  | 로 합쳐도 되나?
                        variable &= ~(Constants.BV_ClickRaindrop);// isClickRaindrop = false;
                        variable &= ~(Constants.BV_IsCling);// isCling = false;// state = Constants.ST_FLYING; // 날아가는 상태로 바꿔주자
                        //state = Constants.//isCling = false;            // 붙지 않은 상태가 된다 - 다음번에 알아서 
                    }

                    // 후에 거리 추가해야함!!! - 일단 물방울과 마우스의 충돌체크 
                    // 물방울에 붙기위한 충돌 체크 
                    else if ((variable & Constants.BV_bCling) == 0)//false == bCling)//bClickRaindrop)   // 물방울이 클릭 되지 않은 상태면서 물방울에 붙은 상태가 아니라면 물방울을 클릭한 상태로 바꿔준다, // RAINDROP 레이어어를 가진 물체와 raycast // 상태변경 
                    {
                        if ((Player_dest = CollisionManager.Instance.Get_MouseCollisionObj(100f, "RAINDROP")) != null)
                        {
                            dest_RaindropScript = Player_dest.GetComponent<RainDrop>();
                            variable |= Constants.BV_bCling;//bCling = true;
                            variable |= Constants.BV_ClickRaindrop;//isClickRaindrop = true;
                        }
                    }
                }
            }
           // else // Rainzone 안에 없으면 부스터 사용 
            //    state = Constants.ST_BOOST;//|= Constants.ST_BOOST;
        }

        if( Input.GetKey(KeyCode.Space))
        {
            // state = Constants.ST_BOOST;
            //variable |= Constants.BV_IsBoost;
            variable |= Constants.BV_IsBoost;
      
            
        }


        ///////////////////////////////////////////////// 스페이스바 뗄 때
        if (Input.GetKeyUp(KeyCode.Space))//(Input.GetMouseButtonUp(0))
        {
            variable &= ~(Constants.BV_IsBoost);//isBoost = false;
            StartCoroutine("DelayStaminaRecovery", 1f);
            if (fStamina < 10)
            {
                variable &= ~(Constants.BV_bBoost); //bCheckBoost = false;
            }
        }
        ///////////////////////////////////////////////// 마우스 왼쪽 뗄 때 
      

        ///////////////////////////////////////////////// 마우스 오른쪽 클릭
        if (Input.GetMouseButtonDown(1))
        {
            if ( ( Player_dest = CollisionManager.Instance.Get_MouseCollisionObj(30f) ) != null)
               // if (CollisionManager.Instance.Check_RayHit(tr.position, tr.forward, "WALL", 3f))  // 벽에 붙을지 체크 
            {
                variable |= Constants.BV_bCling;//bCling = true;
           
                //state = Constants.ST_CLING;//|= Constants.ST_CLING;
            }
        }

        ///////////////////////////////////////////////// 마우스 오른쪽 뗄 때
        //  RainZone에서는 통하지 않음을 추가해야함 
        if (Input.GetMouseButtonUp(1))
        {
            if ( (variable & Constants.BV_bCling ) > 0 )//Constants.ST_CLING == state)
            {
                if(Constants.ST_CLING == state)
                    MakeParentNull();
                /*tr.transform.parent = null;
                //  tr.transform.localScale = new Vector3(1, 1, 1);  // ?? 이거 꼭 필요한가
                ClingObj.transform.parent = null;*/

                variable &= ~(Constants.BV_bCling);//bCling = false;
                variable &= ~(Constants.BV_IsCling);//isCling = false;
               // state = Constants.ST_FLYING;
            }
        }
    }
    private void Action()   // 플레이어 모델이 직접 움직이지는 않으나, 속도변경 같은 코드가 들어감. State의 상태는 여기서만 바뀌게 된다.
    {
        if (state == Constants.ST_IDLE) // Idle 이면 시작전 
            return;

        state = Constants.ST_FLYING;

        // if(((variable & Constants.BV_IsBoost) > 0))
        //   state = Constants.ST_BOOST;
        if (Boost()) // 붙어있는 경우도 추가해야할 듯 
            state = Constants.ST_BOOST;


        if ((variable & Constants.BV_IsCling) > 0)//isCling)
            state = Constants.ST_CLING;

        if ((variable & Constants.BV_bStun) > 0)
        {
            state = Constants.ST_STUN;
           // fXAngle = 0f;
           // fYAngle = 0f;
        }

        if (true == isInRainzone && ( (variable & Constants.BV_bCling) > 0 )   )// true == bCling )//&& false == isCling ) // rainzone 안에 있고, 빗방울에  붙으려고 할 때 
        {
            fSpeed = 40f;       // 수정필요 

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
    private bool Boost()
    {
        if ( state == Constants.ST_IDLE)
            return false;

        if (((variable & Constants.BV_bCollisionOthers) == 0) && ((variable & Constants.BV_IsBoost) > 0)  && ((variable & Constants.BV_bBoost) > 0)  && (fStamina > 0))//(bCheckBoost)) 
        { // 충돌하지 않았고, (_bool 이 true 일때 - 마우스가 클릭상태일때 이면서 , )현재 스테가 스테미나 감소량보다 크고, Boost가 가능할 때 
            fStamina -= (fStaminaMinus * Time.deltaTime);

            // 가속도 조절
            if (fOwnSpeed < OWNMAXBOOST)
                fOwnSpeed += (fBoostMinus * 0.5f * Time.deltaTime);
            else
            {
                fOwnSpeed = OWNMAXBOOST;

                fBoostSpeed += (fBoostMinus * 0.5f * Time.deltaTime);
                if (fBoostSpeed > MAXBOOST)
                    fBoostSpeed = MAXBOOST;
            }

            fSpeed = fOwnSpeed + fBoostSpeed;
            //fSpeed = fBoostVal + fOwnSpeed;
            if (fStamina < 0)               // 스테미나가 0 이하로 떨어지면 부스터를 사용할 수 없다 
            {
                fStamina = 0;
                variable &= ~(Constants.BV_bBoost); //bCheckBoost = false;
                                                    //   state = Constants.ST_FLYING;
                StartCoroutine("DelayStaminaRecovery", 2.5f); // 2.5초간 스테미나 회복을 할 수 없다 
            }
            return true;
        }
        else  // Boost가 아닌 일반이동
        {
            if((variable & Constants.BV_bStaminaRecovery) > 0)  // 스테미나가 회복되기 위한 조건 
                fStamina += 0.1f;

            // 최고속도까지 도달하지 못했으면 속도를 차츰 증가시켜준다 
            if (fOwnSpeed >= OWNMAXBOOST)
                fOwnSpeed = OWNMAXBOOST;      
            else
                fOwnSpeed += (fSpeedVal * Time.deltaTime);

            // 부스터를 사용한 후에 속도를 조금씩 줄여준다 
            if (fBoostSpeed > 0)
                fBoostSpeed -= (Time.deltaTime * fBoostMinus);
            else
                fBoostSpeed = 0f;

            fSpeed = fOwnSpeed + fBoostSpeed;
          //  fBoostVal -= 80f * Time.deltaTime;
           /* if (fBoostSpeed < 1)
                fBoostSpeed = 1f;
                */
            if (fStamina > 100)
                fStamina = 100;

            // fSpeed = fBoostVal + fOwnSpeed;
            //fSpeed += fBoostVal;

            if (fStamina > 10f)      // 스테미나가 10이상이면 사용가능
                variable |= Constants.BV_bBoost;//bCheckBoost = true;

            return false;
        }

    }
    private void Move()     // 실제 플레이어 객체가 움직이는 코드가 들어있다 
    {
        if ( (state == Constants.ST_IDLE) || (variable & Constants.BV_Stick) > 0 ) // 상태가 IDLE이거나 , 어딘가에 달라붙은 경우라면 움직이지 못함 
        {
        }
        else if( (state == Constants.ST_STUN)) // 플레이어가 스턴상태이면 중력을 받는 것 처럼 떨어뜨림
        {
            // rigidBody.MovePosition(tr.position + (-Vector3.up * Time.deltaTime));
            rigidBody.velocity = (-Vector3.up * Time.deltaTime);// tr.position + (-Vector3.up * Time.deltaTime);
            
        }
        else if (true == isInRainzone   && (Constants.ST_CLING != state) && ((variable & Constants.BV_ClickRaindrop)>0 ) && ((variable & Constants.BV_bCling) > 0))
        {// false == isCling && isClickRaindrop && true == bCling )//null != dest_Raindrop) // rainzone 안에 있고, 빗방울에  붙으려고 할 때 
            rigidBody.MovePosition(tr.position + (vDir * fSpeed * Time.deltaTime));
        }
        else if (!(Constants.ST_CLING == state)) // 어딘가에 붙어있지 않다면. 일반적인 움직임, Boost
        {
            // 회전 
            tr.Rotate(Vector3.up * fXAngle * Time.deltaTime * fRotSpeed, Space.World);
            tr.Rotate(Vector3.right * -fYAngle * Time.deltaTime * fRotSpeed, Space.Self);

            // 움직임
            movement.Set(tr.forward.x, tr.forward.y, tr.forward.z);
           // rigidBody.MovePosition(tr.position + (movement * fSpeed * Time.deltaTime));
            rigidBody.velocity = (movement * fSpeed);// * Time.deltaTime);
        }

        else // 붙어 있을 시 아무 동작도 하지 않도록 함 
        {
            // tr.Rotate(Vector3.up * fXAngle * Time.deltaTime * fRotSpeed, Space.Self);
            // tr.Rotate(Vector3.right * -fYAngle * Time.deltaTime * fRotSpeed, Space.Self);
        }
    }
    private void RotateAnimation()  // 플레이어모델이 회전할때 대각선으로 기울어진 느낌을 주도록 한다 
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
        tr_Mesh[1].localRotation = Quaternion.Euler(-fYAngle * 20.0f, fXAngle * 5f//0f
                                        , (-fXAngle * 20.0f));
    }


    // 플레이어가 어딘가에 붙어있다면 붙지 않은 상태로 만들어주는 함수
    public void SetState_NotCling()
    {
        if ( Constants.ST_CLING == state)//isCling)
        {
            variable &= ~(Constants.BV_ClickRaindrop);//isClickRaindrop = false;

            MakeParentNull();
            //tr.transform.parent = null;
            //ClingObj.transform.parent = null;

            variable &= ~(Constants.BV_bCling); //bCling = false;
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
            variable |= Constants.BV_Stick;//isStick = true;
        }
        else
        {
            state = Constants.ST_FLYING;
            variable &= ~(Constants.BV_Stick);//isStick = false;
        }
    }
    
    public void ChangeTimeScale()
    {                                                           // 이부분을 플레이어 상태와 비교해야 하나??
        if( ((variable & Constants.BV_IsCanSlow) > 0) && ((variable & Constants.BV_bCling) == 0)  )//true == isCanSlow && false == bCling)
        {
            Time.timeScale = 0.3f;
            variable &= ~(Constants.BV_IsCanSlow); //isCanSlow = false;
            StartCoroutine("ChangeSlowVal");
        }
    }

    private void MakeParentNull()
    {
        tr.transform.parent = null;
        //  tr.transform.localScale = new Vector3(1, 1, 1);  // ?? 이거 꼭 필요한가
        ClingObj.transform.parent = null;

    }
    private void MakeParent(Transform collTr)// Transform을 그대로 받아와도 되는지 모르겠네..
    {
        ClingObj.transform.parent = collTr;//coll.transform;
        tr.transform.parent = ClingObj.transform;
    }


    private IEnumerator StartConfused() // 캐릭터의 상태를 3초간 confused로 바꿔주는 함수
    {
        variable |= Constants.BV_bStun;//isConfused = true;
        yield return new WaitForSeconds(1f);

        variable &= ~(Constants.BV_bStun);//isConfused = false;
    }

    private IEnumerator ChangeSlowVal()
    {
        //  if (true == isCanSlow)  // 키입력으로 인해 이미 변수상태가 변경된 상태라면 코루틴 꺼줌
        //   yield return null;
        // 1초 후 시간을 원래대로 바꿔주고, 쿨타임은 2초 
      //  print("1초 후 시간을 원래대로");
        yield return new WaitForSeconds(0.3f); // 변수 지정해줘야함
        Time.timeScale = 1f;
      //  print("시간이 돌아왔다, 쿨타임 5초 ");
 
        yield return new WaitForSeconds(5f);

        variable |= Constants.BV_IsCanSlow;//isCanSlow = true;
    }

    private IEnumerator SetVelocityZero()
    {
        yield return new WaitForSeconds(0.5f);  // 0.5초동안 다른 물체의 영향을 받게 한다 
        rigidBody.velocity = Vector3.zero;
    }

    private IEnumerator DelayStaminaRecovery(float fTime)
    {
        variable &= ~(Constants.BV_bStaminaRecovery);   // 스테미나 회복 불가능 상태 

        yield return new WaitForSeconds(fTime);    // 부스터 사용 후 1초간 스테미너가 증가하지 않게 한다 
        variable |= Constants.BV_bStaminaRecovery;

    }

    public void Damaged(float fDamage)
    {

    }



    void OnCollisionEnter(Collision coll)
    {
        // 코드정리 필요 
        if (coll.gameObject != null)
        {
            variable &= ~(Constants.BV_ClickRaindrop);//isClickRaindrop = false;
            fOwnSpeed = 0f;
            fBoostSpeed = 0f;
            StartCoroutine("SetVelocityZero");
           //
        }
        if (coll.gameObject.tag == "WALL" || coll.gameObject.tag == "RAINDROP") // 벽이나 물방울이면 붙게한다 
        {
            if ((variable & Constants.BV_bCling) > 0)//bCling)  // 붙으려고 하는 상태면 
            {
                if (Player_dest.gameObject == coll.gameObject)    // 충돌한 물체가 목표물과 같다면 달라붙는다 -- 벽이 여기서 에러 
                {
                    variable |= Constants.BV_IsCling;//isCling = true;
                    //State = 
                    MakeParent(coll.transform);
                   // ClingObj.transform.parent = coll.transform;
                    //tr.transform.parent = ClingObj.transform;
                }
                else    // 충돌한 물체가 목표물과 다르다면 붙으려고 하는 상태 해제됨
                {
                    //isCling = false;
                    // state = Constants.ST_FLYING;
                    variable &= ~(Constants.BV_bCling);//bCling = false;
                }

            }
            else    // 벽이나 물방울에 붙지 않는 상태인데 일정속도 이상으로 부딪히면 충돌효과를 준다 
            {
                // 충돌효과 코드 좀더 수정필요, 속도도 수정 필요  
                if (fSpeed > OWNMAXBOOST + 3f) // _fPower > fPower 도 포함시켜야함 - confused가 되는 상황? 
                    StartCoroutine("StartConfused");
            }
        }
        else
            variable &= ~(Constants.BV_bCling);//bCling = false;

        if (coll.gameObject.tag == "FROG_TONGUE")
        {
            variable |= Constants.BV_Stick;//isStick = true;

            MakeParent(coll.transform);
           // ClingObj.transform.parent = coll.transform;
            //tr.transform.parent = ClingObj.transform;

            //tr.transform.position = coll.gameObject.transform.position; // 일단 개구리의 위치에 맞추긴 했는데 삐져나옴 

           // rigidBody.velocity = Vector3.zero;
            rigidBody.isKinematic = true;   // 물리적인 영향을 끔 
            
        }
    }
    
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject != null)
        {
            variable |= (Constants.BV_bCollisionOthers);   // 충돌했음을 알림 
        }
    }

    void OnTriggerExit(Collider coll)
    {
          variable &= ~(Constants.BV_bCollisionOthers);   // 다른물체와 충돌하지 않음으로 상태를 바꿈 
    }
    // 레이어 확인해서 붙을 수 있는앤지 없는지 확인하는 코드 필요 

       

    
}
