using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{

    private bool isCollisionOthers = false;
    private FollowCam ParentComp;
    void Start()
    {
        //ParentTr = transform.parent;
        //   ParentComp = GameObject.Find("CamPivot");//PlayerCtrl.Instance.GetComponent<FollowCam>();
        ParentComp = PlayerCtrl.Instance.GetComponentInChildren<FollowCam>();
    }

    void FixedUpdate()
    {
        isCollisionOthers = false;
    }

    void OnTriggerStay(Collider coll)
    {

        if (isCollisionOthers == false) // 한번만 보내준다 
        {
            ParentComp.SetIsCollisionOthers(true);

            isCollisionOthers = true;
        }

    }

    void OnTriggerExit(Collider coll)
    {
        ParentComp.SetIsCollisionOthers(false);
    }

}
