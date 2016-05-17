using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {

    private string _name;
    private int _value;
    private Rarity _rarity;
    private int _curDur;
    private int _maxDur;
    public Items()
    {
        _name = "Need Name";
        _value = 0;
        _rarity = Rarity.common;
        _maxDur = 50;
        _curDur = _maxDur;
    }
    public Items(string name,int value ,Rarity rare , int maxdur ,int curDur)
    {
        _name = name;
        _value = value;
        _rarity = rare;
        _maxDur = maxdur;
        _curDur = curDur;
    }
	

    public string Name
    {
        get { return _name; }
        set { _name = value; }

    }
    public int value
    {
        get { return _value; }
        set { _value = value; }

    }
    public Rarity Rarity
    {
        get { return _rarity; }
        set { _rarity = value; }

    }
    public int Maxdurability
    {
        get { return _maxDur; }
        set { _maxDur = value; }

    }
    public int Curdurability
    {
        get { return _curDur; }
        set { _curDur = value; }

    }


}

public enum Rarity
{
    common,
    Uncommon,
    Rare
}