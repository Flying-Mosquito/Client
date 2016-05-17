using UnityEngine;
using System.Collections;
using System.Collections.Generic;   // List 사용을 위해 추가 

// Trigger, RainDrop들을 생성하고 List를 가지고 있다.
public class RainZone : MonoBehaviour
{

    // private PlayerCtrl _Player;
    public GameObject RainDropPrefab;
    private int iMaxRainDrop = 15;          // 풀에 넣을 빗방울 수    
    public List<GameObject> raindropList = new List<GameObject>();
    private Transform[] rainPoints;
    private float fTime;

    private int MAXINDEX = 3;    // 중복된 곳에서 빗방울이 내려오지 않게 도와줄 변수 
    private int[] Arr = { };// = new int[3];             // iMaxIndex와 같은 숫자로 넣어주세요. 이전에 빗방울이 떨어진 위치를 저장하는 변수 
    private int index = 0;

    void Awake()
    {
        // RainDropPrefab = Resources.Load("RainDrop");//.Find("RainDrop"); // 로드에 문제있으면 이거 찾아봐야돼ㅠㅠ

        Arr = new int[MAXINDEX];


        //_Player = PlayerCtrl.Instance;//.GetComponent<PlayerCtrl>();//GameObject.Find("Player").GetComponent<PlayerCtrl>(); //PlayerCtrl.Instance;//
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

    void Start()
    {
        Random.seed = System.Environment.TickCount;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider coll)
    {
        if ("PLAYER" == coll.gameObject.tag)
        {
            //_Player.isInRainzone = true;
            //  PlayerCtrl.Instance.isInRainzone = true;
            StartCoroutine("CreateRaindrop");
        }

    }

    void OnTriggerExit(Collider coll)
    {
        if ("PLAYER" == coll.gameObject.tag)
        {
            //_Player.isInRainzone = false;
            //   PlayerCtrl.Instance.isInRainzone = false;
            StopCoroutine("CreateRaindrop");
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
                    int randomIdx = Random.Range(1, rainPoints.Length);


                    if (randomIdx == Arr[0] || randomIdx == Arr[1] || randomIdx == Arr[2])   // 이전에 빗방울이 생성된곳과 위치가 같다면 빠져나감 .. for문으로 만들면 좋은데 
                        break;


                    raindropList[i].SetActive(true);
                    raindropList[i].transform.position = rainPoints[randomIdx].transform.position;

                    Arr[index] = randomIdx;

                    ++index;
                    if (index > MAXINDEX - 1)
                        index = 0;

                    break;  // FOR문을 빠져나가게 되면 waitforsecond를 하게 됨 
                }
            }
        }
    }
}
