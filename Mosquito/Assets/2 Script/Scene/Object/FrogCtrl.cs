using UnityEngine;
using System.Collections;

public class FrogCtrl : MonoBehaviour {
    public Player _Player;

    private GameObject _Cube;
    private GameObject _Tongue;
    private Transform  _TongueTr;
    private float x;

    private Transform tr;
    private RaycastHit hit;
    private float fLength;

    private bool bCheck;
    private bool isInSight;
    private bool bSwallow;
    private Vector3 vTongueDir;

    // Use this for initialization
    void Awake () {
        _Player = GameObject.Find("Player").GetComponent<Player>();
        tr = GetComponent<Transform>();

        //temp
        _Cube = GameObject.Find("Cube");
        _TongueTr = tr.transform.FindChild("Tongue");
        _Tongue = _TongueTr.gameObject;
        //_Tongue = GameObject.Find("Tongue");
        x = 0f;
        fLength = 5f;
        bCheck = false;
        isInSight = false;
        bSwallow = false;
        vTongueDir = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
        isInSight = Check_Sight();
       // Debug.DrawRay(tr.position, tr.forward * 200f, Color.blue);
        Vector3 vTemp = (_Player.transform.position - tr.position);
        vTemp.Normalize();
        Debug.DrawRay(tr.position, vTemp * fLength, Color.yellow);

        if (isInSight)
        {
          
            _Tongue.SendMessage("SetMoveState", true, SendMessageOptions.DontRequireReceiver);
            _Tongue.SendMessage("SetDir",  vTongueDir, SendMessageOptions.DontRequireReceiver);
        }
/*
        Debug.Log("(Update) - x : " + vTongueDir.x.ToString() +
         "y : " + vTongueDir.y.ToString() +
         "z : " + vTongueDir.z.ToString());*/
    }

  

    private bool Check_Sight()
    {
        Vector3 vDir = _Player.transform.position - tr.position ;   // 플레이어 
      //  print("vDir Length = " + Vector3.Distance(_Player.transform.position, tr.position).ToString());
        vDir.Normalize();
       
        /*
         if (Physics.Raycast(_Obj.transform.position, _Obj.transform.forward, out hit, _fDist))
            if( hit.collider.tag == _Tag)
        */
        //Debug.Log("Angle : " + Vector3.Angle(tr.forward , vDir).ToString());

        if (Physics.Raycast(tr.position, vDir, out hit, fLength) && (Vector3.Angle(tr.forward, vDir) < 40))   // 범위안에 들어와 있으면서, 각도가 40보다 작다
        {
             //  Debug.Log("들어옴");
            //  if (false == bCheck)
            //  {
            //   bCheck = true;
           
                vTongueDir = _Player.transform.position - _Tongue.transform.position;    // 방향벡터 구하기

                vTongueDir.Normalize();               // 정규화
                                                //  _Tongue.transform.rotation = Quaternion.Euler(vDir);

            //}
            return true;
            
        }
       
          //  bCheck = false;
          // Debug.Log("안들어옴");
            return false;
        
    }
}
