using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterState : Singleton<CharacterState>

{

    public Scrollbar HealthBar;
    public Scrollbar StaminaBar;
    public Scrollbar bloodBar;


    //private float cachedY;
    //private float minXvalue;
    //private float maxXvalue;
    //private int currentHealth;
    //public int maxHealth;


    //public RectTransform healthTransform;   
    //public Image visualHealth;
    //public RectTransform staminaTransform;
    //public Image visuastamina;
    //public RectTransform bloodTransform;
    //public Image visuablood;


    
    //private float cachedY1;
    //private float minXvalue1;
    //private float maxXvalue1;
    //private int currentstamina;
    //public int maxstamina;
    


    //private float cachedY2;
    //private float minXvalue2;
    //private float maxXvalue2;
    //private int currentblood;
    //public int maxblood;
   
    //// Use this for initialization



    //public float coolDown;
    //private bool onCD;
    //public GUIText restartText;
    //public float coolDown1;
    //private bool onCD1;

    //public float coolDown2;
    //private bool onCD2;


    void Start () {


       
        //cachedY = healthTransform.position.y;
        //maxXvalue = healthTransform.position.x;
        //minXvalue = healthTransform.position.x - healthTransform.rect.width;
        //currentHealth = maxHealth;
        //onCD = false;
        //cachedY1 =staminaTransform.position.y;
        //maxXvalue1 = staminaTransform.position.x;
        //minXvalue1 =staminaTransform.position.x -staminaTransform.rect.width;
        //currentstamina = maxstamina;
        //onCD1 = false;
        //cachedY2 =bloodTransform.position.y;
        //maxXvalue2 =bloodTransform.position.x;
        //minXvalue2 =bloodTransform.position.x - bloodTransform.rect.width;
        //currentblood = maxblood;
        //onCD2 = false;



    }
	
	// Update is called once per frame
	void Update () {

        HealthBar.value = PlayerCtrl.Instance.iHP / 100f;

        StaminaBar.size = PlayerCtrl.Instance.fStamina/ 100f;
        bloodBar.value = PlayerCtrl.Instance.iBlood*0.01f;

    }

    //IEnumerator CoolDownDmg()
    //{
    //    onCD = true;
    //    yield return new WaitForSeconds(coolDown);
    //    onCD = false;

    //}

    //IEnumerator CoolDownDmg1()
    //{
    //    onCD1 = true;
    //    yield return new WaitForSeconds(coolDown1);
    //    onCD1 = false;

    //}

    //IEnumerator CoolDownDmg2()
    //{
    //    onCD2 = true;
    //    yield return new WaitForSeconds(coolDown2);
    //    onCD2 = false;

    //}

    //private void HandleHealth()
    //{

    //    float currentXvalue = MapValues(currentHealth, 0, maxHealth, minXvalue, maxXvalue);

    //    healthTransform.position = new Vector3(currentXvalue, cachedY);


    //}
    //private void Handlestamina()
    //{

    //    float currentXvalue1 = MapValues1(currentstamina, 0, maxstamina, minXvalue1, maxXvalue1);

    //   staminaTransform.position = new Vector3(currentXvalue1, cachedY1);


    //}

    //private void Handleblood()
    //{

    //    float currentXvalue2 = MapValues2(currentblood, 0, maxblood, minXvalue2, maxXvalue2);

    //    bloodTransform.position = new Vector3(currentXvalue2, cachedY2);


    //}

    //private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    //{
    //    return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    //}

    //private float MapValues1(float x, float inMin, float inMax, float outMin, float outMax)
    //{
    //    return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    //}
    //private float MapValues2(float x, float inMin, float inMax, float outMin, float outMax)
    //{
    //    return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    //}
    //private int CurrentHealth
    //{
    //    get { return currentHealth; }
    //    set
    //    {
    //        currentHealth = value;
    //        HandleHealth();
    //    }
    //}
    //private int Currentstamina
    //{
    //    get { return currentstamina; }
    //    set
    //    {
    //        currentstamina = value;
    //        Handlestamina();
    //    }
    //}
    //private int Currentblood
    //{
    //    get { return currentblood; }
    //    set
    //    {
    //        currentblood = value;
    //        Handleblood();
    //    }
    //}
    void OnTriggerStay(Collider other)
    {
        if (other.name == "Health")
        {
           

        }
        if (other.gameObject.tag == "damage")
        {
            print("hell");
            //  PlayerCtrl.Instance.iHP -= 3;

            PlayerCtrl.Instance.Damaged(5);
        }
    }



}
