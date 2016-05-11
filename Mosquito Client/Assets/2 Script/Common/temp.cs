using UnityEngine;
using System.Collections;

public class temp : MonoBehaviour {

    public float MaxNum;
    public float fTime;
    public Transform tr;
    public Vector3 dir;
    public float fSpeed;

	// Use this for initialization
	void Start () {
        MaxNum = 1.1f;
        tr = GetComponent<Transform>();
        dir = new Vector3(1, 0, 0);
        fSpeed = 0.2f;
        fTime = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        fTime += Time.deltaTime;
        if (fTime > MaxNum || fTime < -MaxNum)
        {
            fTime = 0f;
            dir *= -1;
        }

        tr.Translate(dir *fSpeed);
	}
}
