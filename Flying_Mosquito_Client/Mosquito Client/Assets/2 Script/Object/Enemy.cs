using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public Transform target;
    public int moveSpeed;
    public int rotationSpeed;

    private Transform myTransform;

    void Awake()
    {
        myTransform = transform;
    }
	// Use this for initialization
	void Start () {
        GameObject go = GameObject.FindGameObjectWithTag("Move");

        target = go.transform;
	}
	
	// Update is called once per frame
	void Update () {
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,Quaternion.LookRotation(target.position-myTransform.position), rotationSpeed * Time.deltaTime);


        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        
    
    }
}
