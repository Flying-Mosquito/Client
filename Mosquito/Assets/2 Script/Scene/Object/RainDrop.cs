using UnityEngine;
using System.Collections;



// 이펙트같은것들을 추가로 넣어야 한다 . 
public class RainDrop : MonoBehaviour {

    private Rigidbody rigidBody;
    //private Collider col;
    public bool bCheck;//{ get; private set; } // 플레이어가 타겟으로 삼았는지 아닌지에 대한 변수 
    public bool isPlop;  // 플레이어가 매달릴 수 있는지 없는지의 유무 .  플레이어가 부모-자식관계를 해제하게 한다
   // public bool isCollision;// { get; private set; }   // 플레이어 아닌 사물들에 충돌했는지에 대한 변수
    
    public Vector3 vGravity;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        //col = GetComponent<Collider>();
        bCheck = false;
        rigidBody = GetComponent<Rigidbody>();
        vGravity = new Vector3(0f, -15f, 0f);

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
    }
    
    void OnCollisionEnter(Collision coll)
    {
        if ("RAINDROP" == coll.gameObject.tag)
            return;

        if (coll.gameObject != null)    // 순서상 플레이어가 타겟을 먼저 해지해야 하기 때문에 , 코루틴을 써줌 
        {
           // rigidBody.velocity = Vector3.zero;
            if ("PLAYER" == coll.gameObject.tag)        // 플레이어와 부딪히면 힘을 받지 않음 
            {
                rigidBody.velocity = Vector3.zero;
            }
            else                                        // 플레이어 아닌 다른 물체와 부딪히면       
            {
                if (true == bCheck) // 어딘가에 부딪혀서 빗방울이 사라지는 상태. 플레이어의 체력을 깎는다 
                {
                    if((PlayerCtrl.Instance.state & Constants.ST_CLING) > 0)    // 플레이어가 붙어있을 때만 체력을 깎음 . 바닥에 충돌할 때에는 플레이어가 알아서 할거야 
                        PlayerCtrl.Instance.Damaged(5);
                    //else
                      //  PlayerCtrl.Instance // bCling상태면 , click을 해제해줘도 됨...? 클릭상태 해제 후, 플레이어가 다른 오브젝트에 충돌하면 bCling상태를 지워줌 
                }

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
        bCheck = false;
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
