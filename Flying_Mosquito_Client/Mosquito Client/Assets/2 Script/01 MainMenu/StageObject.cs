using UnityEngine;
using System.Collections;

// 스테이지에 사용될 - 클릭될 물체에 사용
public class StageObject : MonoBehaviour {
    private  Component col;
    public int stageNum;
    void Awake()
    {
       col = GetComponent<Collider>();
    }

    void Update()
    {
        if ( Input.GetMouseButtonDown(0))
        {
            bool bCheck = CollisionManager.Instance.Check_MouseCollision(col);

            // 선택됐다면 씬 변경, ID가 있어서 씬 선택을 하게 될거야
            if(bCheck)
            {
               GameManager.Instance.StartCoroutine("StartLoad", "Stage" + stageNum.ToString()) ;
            }
        }
    }
}
