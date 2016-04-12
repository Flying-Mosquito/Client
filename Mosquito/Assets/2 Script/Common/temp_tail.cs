using UnityEngine;
using System.Collections;

public class temp_tail : MonoBehaviour {
    public Rigidbody rBody;
	// Use this for initialization
	void Start ()
    {
        rBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        

        if (Input.GetMouseButton(0))
        {
            bool bCheck = CollisionManager.Instance.Check_MouseCollision(rBody);
          //  if (bCheck)
          //  {
                //rBody.AddForce(new Vector3(0, 0, 500));
            //rBody.get
           // }
        }
	}
}
