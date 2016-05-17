using UnityEngine;
using System.Collections;

// 개구리의 원점(Tongue)에 들어갈 코드
// Tongue는 TongeObject를 가지고 있다.
public class FrogTongue : MonoBehaviour
{

    //private Transform tr;
    private Transform child_tr;

    public bool bSwallow;   // 뻗는 상태인지, 삼키는 상태인지
    public bool bMove;      // 모기가 사거리 안에 들어온 상태 
    public bool isMove;     // 혀를 움직이고 있는 상태
    public bool bIdle;

    private float x;
    private float fLength;
    private float fSpeed;



    private Vector3 vDir;

    public void SetMoveState(bool _bMove)
    {
        bMove = _bMove;
    }
    public void SetDir(Vector3 _vDir)
    {
        if (!isMove)       // 혀를 집어넣은 상태에서만 각도변경 가능 
        {
            isMove = true;
            vDir = _vDir;

        }
    }

    void Awake()
    {
        //tr = GetComponent<Transform>();
        child_tr = GetComponentInChildren<Transform>();
        fLength = 30f;
        bSwallow = false;
        bMove = false;
        isMove = false;
        vDir = Vector3.zero;
        bIdle = true;
        fSpeed = 2.5f;
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        MoveTongue();
    }
    // Update is called once per frame
    void Update()
    {

        //MoveTongue();
    }

    private void MoveTongue()
    {
        // print("bMove : " + bMove + ", isMove : " + isMove);
        if (vDir != Vector3.zero)
            child_tr.rotation = Quaternion.LookRotation(vDir);  // 혀 메쉬를 방향벡터의 방향으로 회전 

        if (bMove && isMove)
        {
            //print("혓바닥이 움직일거야 ");
            if (bSwallow)
                x -= fSpeed;
            if (!bSwallow)
                x += fSpeed;

            if (x >= fLength)   // 길이가 최대길이보다 커지면 삼켜야함
                bSwallow = true;

            if (x <= 0f)
            {
                bMove = false;
                bSwallow = false;
                bIdle = true;
                StartCoroutine("ChangeMoveState");
            }


        }
        child_tr.localScale = new Vector3(0.2f, 0.2f, x);

    }

    IEnumerator ChangeMoveState()
    {
        isMove = false;
        yield return new WaitForSeconds(0.5f);
        //print("ChangeMoveState");

    }

    /*
    public void Eat(Vector3 _vDir)
    {
        //  _Cube.transform.parent = _Tongue.transform;

        //_Tongue.transform.Rotate(vTongueDir);
        tr.transform.rotation = Quaternion.LookRotation(_vDir);
        x += 0.1f;
        tr.transform.localScale = new Vector3(0.2f, 0.2f, x);
    }
    */
    void EnterCollision()
    {
        //bSwallow = true;
    }
}
