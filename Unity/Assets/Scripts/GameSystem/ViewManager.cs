using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance;

    [Header(" Full HD 고정 코드")]
    public int viewWidth = 1920;
    public int viewHeight = 1080; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if(Screen.width  >= viewWidth || Screen.height >= viewHeight)
        {
            Screen.SetResolution(viewWidth, viewHeight, FullScreenMode.FullScreenWindow);
            Camera.main.backgroundColor = Color.black;
        }
    }

}
