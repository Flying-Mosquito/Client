using UnityEngine;
using System.Collections;

public class ClingObject : Singleton<ClingObject> {

 
	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(this);
       
    }
	
	// Update is called once per frame
	void Update () {
      
    }
}
