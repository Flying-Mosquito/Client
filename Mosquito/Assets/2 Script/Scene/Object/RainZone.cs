using UnityEngine;
using System.Collections;
using System.Collections.Generic;   // List 사용을 위해 추가 

// Trigger, RainDrop들을 생성하고 List를 가지고 있다.
public class RainZone : MonoBehaviour {

    private Player _Player;
    public GameObject RainDropPrefab;
    private int iMaxRainDrop = 30; // 풀에 넣을 빗방울 수    
    public List<GameObject> raindropList = new List<GameObject>();
    private Transform[] rainPoints;
    private float fTime;
    

    void Awake()
    {
        // RainDropPrefab = Resources.Load("RainDrop");//.Find("RainDrop"); // 로드에 문제있으면 이거 찾아봐야돼ㅠㅠ
        
        _Player = GameObject.Find("Player").GetComponent<Player>();
        rainPoints = gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < iMaxRainDrop; ++i)  // 풀에넣을 빗방울들을 만들고 리스트에 넣어줌 
        {
            GameObject rainDrop = (GameObject)Instantiate(RainDropPrefab);
            rainDrop.name = "RainDrop_" + ((i).ToString());
            rainDrop.SetActive(false);

            raindropList.Add(rainDrop);
        }

        fTime = 0.1f;
       // print("Awake 넘어갑니다");


    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    
    void OnTriggerEnter(Collider coll)
    {
        // 트리거에 들어오면 코루틴 시작, 나가면 정지 
        if (coll.gameObject.tag == "PLAYER")
        {
            StartCoroutine("CreateRaindrop");

            _Player.isInRainzone = true;
        }


       // print("OnTriggerEnter 끝");
    }

    // 나가는 함수 추가 
    void OnTriggerExit(Collider coll)
    {
        if(coll.gameObject.tag == "PLAYER")
        {
            StopCoroutine("CreateRaindrop");
            _Player.isInRainzone = false;
        }
    }

IEnumerator CreateRaindrop()
{
        //print("iMaxRainDrop : " + iMaxRainDrop.ToString());
        // print("rainPoints.Length : " + rainPoints.Length.ToString());
        while (true)
        {
            yield return new WaitForSeconds(fTime);

            for (int i = 0; i < iMaxRainDrop; ++i)
            {
                if (raindropList[i].activeSelf == false) // 활성화 되지 않은 물방울이면 활성화
                {
                    raindropList[i].SetActive(true);

                    int idx = Random.Range(1, rainPoints.Length);


                    raindropList[i].transform.position = rainPoints[idx].transform.position;
                    break;
                   // print("호출");
                }
            }

        }
    }

}
