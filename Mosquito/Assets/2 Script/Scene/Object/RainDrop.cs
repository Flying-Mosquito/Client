using UnityEngine;
using System.Collections;



// 이펙트같은것들을 추가로 넣어야 한다 . 
public class RainDrop : MonoBehaviour {


    private Rigidbody rigidBody;
    private Collider col;

    public bool bCheck;//{ get; private set; } // 플레이어가 타겟으로 삼았는지 아닌지에 대한 변수 
    public bool isPlop;  // 플레이어가 매달릴 수 있는지 없는지의 유무 .  플레이어가 부모-자식관계를 해제하게 한다
   // public bool isCollision;// { get; private set; }   // 플레이어 아닌 사물들에 충돌했는지에 대한 변수
    
    public Vector3 vGravity;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        bCheck = false;
        rigidBody = GetComponent<Rigidbody>();
        vGravity = new Vector3(0f, -9.8f, 0f);

       // isCollision = false;
        isPlop = false;

        //vGravity = Vector3.zero;
        
    }
	void Start () {
       // rigidBody.AddForce(new Vector3(0f, -1f, 0f) * fSpeed);
	}

    void FixedUpdate()
    {
    

        transform.Translate(vGravity * Time.fixedDeltaTime);
        
        //  rigidBody.MovePosition(transform.position + ( vGravity * Time.fixedDeltaTime));
        //rigidBody.velocity = vGravity;
    }
    // Update is called once per frame
    void Update ()
    {
     //  transform.Translate(vGravity * Time.deltaTime);
     //   print("RainDrop Velocity : " + rigidBody.velocity.x + ", " + rigidBody.velocity.y + ", " + rigidBody.velocity.z);
   
    }

    /*
     void OnTriggerEnter(Collider coll)
    {
        //if ( "PLAYER" == coll.gameObject.tag )
       // {
              // bCheck가 안들어간 이유는 OnTriggerEnter에 들어왔다는 것 자체가 bCheck == true라는 소리 
            if ( isCollision)            //  (bCheck && isCollision)플레이어의 타겟이면서 이미 충돌한 상태라면 ( 중력이 적용되지 않는 상태 - 정지상태 )
            {
                
                col.isTrigger = false;
                StartCoroutine("Late_SetActiveFalse");
            }
       // }
    }
    */
    void OnCollisionEnter(Collision coll)
    {
        if ("RAINDROP" == coll.gameObject.tag)
            return;

        if (coll.gameObject != null)    // 순서상 플레이어가 타겟을 먼저 해지해야 하기 때문에 , 코루틴을 써줌 
        {
           // rigidBody.velocity = Vector3.zero;
            if ("PLAYER" == coll.gameObject.tag)        // 플레이어와 부딪히면 
            {
                rigidBody.velocity = Vector3.zero;
           
            }
            else                                        // 플레이어 아닌 다른 물체와 부딪히면       
            {
               // isCollision = true;                     // 다른 물체에 충돌했음을 알리는 변수 - 필요한가?

                /* if (bCheck)                              // 플레이어가 타겟으로 삼았다면 빗방울이 장애물에 쳐박혔을 시 그 자리에 있게 한다 
                 {
                     // rigidBody.useGravity = false;
                     //rigidBody.velocity = Vector3.zero;
                     col.isTrigger = true;
                     rigidBody.isKinematic = true;
                     print("트리거트루, 키네매틱트루 ");
                     vGravity = new Vector3(0f, 0f, 0f);


                     //여기에 투명처리 
                 }
                 else
                 {
                     print("셋엑티브폴스");
                     Change_CheckState(false);       // 곧 Active가 바뀔거니가 플레이어까 클릭한 것을 해제 , 원상태로 돌아갑니다
                     isCollision = false;
                     gameObject.SetActive(false);

                     vGravity = new Vector3(0f, -9.8f, 0f);
                 }*/
                isPlop = true;
                StartCoroutine("Plop");
                
            }

           
        }


    }

    public void Change_CheckState(bool _State)
    {  
        bCheck = _State;
    }

    IEnumerator Plop() // 바닥에 떨어짐 
    {
        //1. 깨지는 이펙트
        //2. 상태변경
        //if()
        //2. 깨지는 이펙트 
       
        yield return null;
        isPlop = false;
        gameObject.SetActive(false);
       


    }
    /*
    private IEnumerator Late_SetActiveFalse()
    {
        print("트리거 폴스 , 키네매틱폴스 ");
        yield return new WaitForSeconds(1f);
       // rigidBody.useGravity = true;        // 정지한 중력을 다시 동작하게 만든다
        Change_CheckState(false);           // 곧 Active가 바뀔거니가 플레이어가 클릭한 것을 해제
        
        isCollision = false;
        col.isTrigger = false;
   
        //  rigidBody.velocity = vGravity;
        gameObject.SetActive(false);   // 이건 플레이어 쪽에서 해줄 것 
        rigidBody.isKinematic = false;
        rigidBody.velocity = Vector3.zero;
        vGravity = new Vector3(0f, -9.8f, 0f);
        //vGravity = new Vector3(0f, -9.8f * 0.01f, 0f);
    }
    */
}
