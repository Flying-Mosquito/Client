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
        Debug.Log("Check_RayHIt");
        // 특정 거리가 입력되지 않았다면 거리를 길게 잡음 
        if (0 == _fDist)
            _fDist = 2000f;

        if (Physics.Raycast(_position, _Forward, out hit, _fDist))
            if (hit.collider.tag == _Tag)
                return true;

        return false;
    }
}
