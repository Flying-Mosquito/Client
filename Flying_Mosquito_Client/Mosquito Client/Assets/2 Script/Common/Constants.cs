//using UnityEngine;
//using System.Collections;

static class Constants
{


    // State
    public const ulong ST_IDLE = 0x01;
    public const ulong ST_FLYING = 0x02;
    public const ulong ST_BOOST = 0x04;
    public const ulong ST_ACTIVE = 0x08;
    public const ulong ST_CLING = 0x10;
    public const ulong ST_STUN = 0x20;
    public const ulong ST_DEATH = 0x40;
    public const ulong ST_BLOOD = 0x80;
    //public const ulong ST_WALLCOLLISION = 0x10;

    //40 80 


    // Bool Variable
    public const ulong BV_bBoost = 0x01;  // 플레이어가 Boost를 사용할 수 있는지의 여부    , 스테미나
    public const ulong BV_IsBoost = 0x02;  // 플레이어가 부스터를 사용하고 있는지에 대한 변수, 스페이스바
    public const ulong BV_bCling = 0x04;  // 플레이어가 물체에 붙으려고 하는지의 여부
    public const ulong BV_IsCling = 0x08;  // 플레이어가 물체에 붙어있는지에 대한 변수
    public const ulong BV_ClickRaindrop = 0x10;  // 플레이어가 빗방울을 클릭했는지에 대한 변수 
    public const ulong BV_IsCanSlow = 0x20;  // 플레이어의 슬로우변수가 쿨타임인지 아닌지에 대한 변수 
    public const ulong BV_Stick = 0x40;  // 플레이어가 어딘가에 붙은지에 대한 변수 ( 빠져나오지 못하는 상황. 개구리 혀, 거미줄 같은 )
    public const ulong BV_bStun = 0x80;  // 플레이어가 스턴상태 인지의 여부 
    public const ulong BV_bCollisionOthers = 0x100;    // 다른 오브젝트에 충돌하고 있는지의 여부 
    public const ulong BV_bStaminaRecovery = 0x200;
    public const ulong BV_bBlood = 0x400; // 스테미나 회복이 가능한지의 여부 
    //public const ulong BV_InRainzone = 0x20;  // 바깥에서 변경해 줘야 하는 함수여서 여기에 안적음 
}
