using UnityEngine;
using System.Collections;

public class Randommove : MonoBehaviour
{
    public Transform move2;
    // Use this for initialization
    void Start()
    {
        move2 = transform;
    }

    // Update is called once per frame
    void Update()
    {
        float sizeX = Random.Range(-20f,20f);
        float sizeY = 0;
        float sizeZ = Random.Range(-18f, 22.5f);
       move2.transform.position = new Vector3(sizeX, sizeY, sizeZ);


    }
}
