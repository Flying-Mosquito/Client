using UnityEngine;
using System.Collections;

public class stage : MonoBehaviour {

    public GUISkin scene;

    void OnGUI()
    {

        if (GUI.Button(new Rect(0, 0, 20, 20), "") == true)
        {
          //  Application.LoadLevel(2);
        }
    }
}
