using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenSettings()
    {
        Debug.Log("Settings");
    }

    // Update is called once per frame
    public void OpenLevelSelect()
    {
        Debug.Log("Level Select");
    }

    public void Exit()
    {
        Debug.Log("Quit Game");
    }
}
