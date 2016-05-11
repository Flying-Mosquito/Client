using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    [SerializeField]
    private float fillAmount;
    [SerializeField]
    private Image content;
    // Use this for initialization
    private void HandleBar()
    {
        content.fillAmount = fillAmount;

    }
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        HandleBar();
    }
}
