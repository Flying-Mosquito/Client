using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {
    public Animator anim;
	// Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    /*
	void Start () {
        StartCoroutine("PlayerAnim"); // ?? 어떤게 낫지..
	}*/
    // Update is called once per frame
    void Update () {
        if (PlayerCtrl.Instance.state == Constants.ST_FLYING)
            anim.Play("fly");

        switch(PlayerCtrl.Instance.state)
        {
            case Constants.ST_FLYING:
                {
                    anim.SetInteger("state", 0);
                    //  anim.speed = PlayerCtrl.Instance.fSpeed * 0.2f;
                    // print("Player speed : " + PlayerCtrl.Instance.fSpeed);
                    // print("anim speed : " + anim.speed);
                    break;
                }

            case Constants.ST_IDLE:
                {
                    anim.SetInteger("state", 1);
                    break;
                }
            case Constants.ST_CLING:
                {
                    anim.SetInteger("state", 2);
                    break;
                }
                
            case Constants.ST_STUN:
                {
                    anim.SetInteger("state", 3);
                    break;
                }
        }
	}
}
