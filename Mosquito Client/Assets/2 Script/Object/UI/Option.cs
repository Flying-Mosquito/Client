using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour {
    public Button exit;
    public Button back;
    // Use this for initialization


    public bool backBool;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Click()
    {
        SceneManager.LoadScene(4);
    }
}
