using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    GameManager token;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        token = FindObjectOfType<GameManager>();
        token.Pause();
        token.isOnMenu = true;
    }
    private void OnDisable()
    {
        token.Unpause();
        token.isOnMenu = false;
    }
}
