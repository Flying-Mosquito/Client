using UnityEngine;
using System.Collections;

public class CollisionManager : Singleton<CollisionManager>
{

    private RaycastHit hit; // Ray가 맞은 위치 정보를 받아올 구조체
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public bool Check_MouseCollision(Component _Obj) // 마우스 클릭시 충돌체가 있다면 true리턴 
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
     //   Debug.DrawRay(ray.origin, ray.direction, Color.green); // 여기서 해도 안보여요 

        Physics.Raycast(ray, out hit, 2000.0f);
        
        if(hit.collider == _Obj.GetComponent<Collider>())
            return true;

        return false;
    }
    public bool Check_MouseCollision(string _tag , float _fDist = 2000f) // 마우스 클릭시 충돌체가 있다면 true리턴 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //   Debug.DrawRay(ray.origin, ray.direction, Color.green); // 여기서 해도 안보여요 

        Physics.Raycast(ray, out hit, _fDist);
        if (null == _tag || null == hit.collider)
            return false;
        

        if (hit.collider.tag == _tag)
                return true;

        return false;
    }

    /*
    public GameObject Get_MouseCollisionObj(string _tag = "", float _fDist = 2000f)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //   Debug.DrawRay(ray.origin, ray.direction, Color.green); // 여기서 해도 안보여요 
        bool bRaycast;

        // _tag가 있으면 레이어 비교, _tag가 없으면 그냥 Raycast
        if ("" != _tag)
            bRaycast = Physics.Raycast(ray, out hit, _fDist, 1 << LayerMask.NameToLayer(_tag));
        else
            bRaycast = Physics.Raycast(ray, out hit, _fDist);

        if (!bRaycast)  // bRaycast가 false면 null 리턴 
            return null;
        
        if (hit.collider.tag == _tag)
            return hit.collider.gameObject;
        
        return null;
    }*/


    public GameObject Get_MouseCollisionObj(string _tag = "", float _fDist = 2000f)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //   Debug.DrawRay(ray.origin, ray.direction, Color.green); // 여기서 해도 안보여요 
        bool bRaycast;

        // _tag가 있으면 레이어 비교, _tag가 없으면 그냥 Raycast
        if ("" != _tag)
            bRaycast = Physics.Raycast(ray, out hit, _fDist, 1 << LayerMask.NameToLayer(_tag));
        else
            bRaycast = Physics.Raycast(ray, out hit, _fDist);

        if (!bRaycast)  // bRaycast가 false면 null 리턴 
            return null;

        if (hit.collider.tag == _tag)
            return hit.collider.gameObject;

        return null;
    }

    public bool Check_RayHit(Component _Obj, string _Tag, float _fDist = 0) // _Obj의 forward방향, _fDist길이 만큼 만든 Ray가 충돌한 충돌체가 _Tag이름과 같다면 true리턴
    {
        // 특정 거리가 입력되지 않았다면 거리를 길게 잡음 
        if (0 == _fDist)
            _fDist = 2000f;

        if (Physics.Raycast(_Obj.transform.position, _Obj.transform.forward, out hit, _fDist))
            if( hit.collider.tag == _Tag)
                return true;

        return false;
    }


    public bool Check_RayHit(Vector3 _position, Vector3 _Forward, string _Tag, float _fDist = 0) // _Obj의 forward방향, _fDist길이 만큼 만든 Ray가 충돌한 충돌체가 _Tag이름과 같다면 true리턴
    {

        // 특정 거리가 입력되지 않았다면 거리를 길게 잡음 
        if (0 == _fDist)
            _fDist = 2000f;

        if (Physics.Raycast(_position, _Forward, out hit, _fDist))
            if (hit.collider.tag == _Tag)
                return true;

        return false;
    }


    public bool Check_Sight(Transform _trPoint, Vector3 _targetPos, float _fLength = 10, float _fAngle = 40)    // Check_Sight(탐색을 시작할 점, 
    {
        Vector3 vDir = _targetPos - _trPoint.transform.position;   // 플레이어 
                                                                   //  print("vDir Length = " + Vector3.Distance(_Player.transform.position, tr.position).ToString());
        vDir.Normalize();

        if (Physics.Raycast(_trPoint.transform.position, vDir, out hit, _fLength) && (Vector3.Angle(_trPoint.forward, vDir) < _fAngle))   // 범위안에 들어와 있으면서, 각도가 40보다 작다
        {
            //  Debug.Log("들어옴");
            //  if (false == bCheck)
            //  {
            //   bCheck = true;

           // _outDir = _Player.transform.position - _Tongue.transform.position;    // 방향벡터 구하기

          //  vTongueDir.Normalize();               // 정규화
                                                  //  _Tongue.transform.rotation = Quaternion.Euler(vDir);

            //}
            return true;

        }

        //  bCheck = false;
        // Debug.Log("안들어옴");
        return false;

    }
}
