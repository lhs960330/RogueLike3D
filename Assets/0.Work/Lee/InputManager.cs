using UnityEngine;

public class InputManager : Singleton<InputManager> 
{
    public void Test()
    {
        Debug.Log(GetInstanceID());
    }
}
