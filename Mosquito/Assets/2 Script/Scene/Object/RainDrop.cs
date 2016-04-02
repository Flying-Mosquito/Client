using UnityEngine;
using System.Collections;



// 이펙트같은것들을 추가로 넣어야 한다 . 
public class RainDrop : MonoBehaviour {

    public float fSpeed = 500f;

    private Component col;
    
    //Rigidbody rigidBody;
    void Awake()
    {
        //rigidBody = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
       
    }
	void Start () {
       // rigidBody.AddForce(new Vector3(0f, -1f, 0f) * fSpeed);
	}

	// Update is called once per frame
	void Update ()
    {
     
	
	}

    void Plop() // 바닥에 떨어짐 
    {
        //1. 깨지는 이펙트
        //2. 상태변경
        gameObject.SetActive(false);  //2. 깨지는 이펙트 
    }

    void OnCollisionEnter(Collision coll)
    {
        //if(coll.gameObjec)
        //{
       
        if(coll.gameObject != null)
             Plop();
        //}
    }
}
