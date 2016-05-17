using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance =  FindObjectOfType(typeof(T)) as T;
                return _instance;
            }
            return _instance;
        }
    }

    public virtual void Init()
    { }

    // 종료시 싱글톤 삭제 
    void OnApplicationQuit()
    {
        _instance = null;
    }

}
