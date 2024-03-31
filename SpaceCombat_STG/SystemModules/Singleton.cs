using UnityEngine;

//单例模式泛型基类
public class Singleton<T> : MonoBehaviour where T : Component
{
    /*
     *  T,指泛型，
     *  where T:Component 指定了泛型的约束，该类型必须是继承Component的
     *  而Component是附加到任意游戏物体的基础类(Base class for everything attached to GameObjects.)，所以允许该单例装载任何挂载到游戏物体上的类型
     */
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as T;
    }//返回该类型的实例
    
}
