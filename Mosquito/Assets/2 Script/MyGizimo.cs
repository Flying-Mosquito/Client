using UnityEngine;
using System.Collections;

public class MyGizimo : MonoBehaviour {

   public float fRad = 0.3f;
    public Color color;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, fRad);
    }


}
