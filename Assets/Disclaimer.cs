using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Disclaimer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Screen.fullScreen);
        Debug.Log(Screen.currentResolution);
    }
    public void LoadTitle() 
    {
        SceneManager.LoadScene("Title", 0);
    }
}
