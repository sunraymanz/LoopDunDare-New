using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoMenu : MonoBehaviour
{
    public GameManager token;
    public Player playerToken;
    public HQBase HQToken;
    public Animator animToken;
    bool isShow = false;
    public bool isBusy = false;
    
    // Start is called before the first frame update
    void Start()
    {      
        playerToken = FindObjectOfType<Player>();
        HQToken = FindObjectOfType<HQBase>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (!token.isOnMenu)
        {
            token.isOnMenu = true;
        }       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();            
        }
    }   
    public void Close() 
    {
        gameObject.SetActive(false);
    }
    public void ToggleShow()
    {
        if (!isBusy)
        {
            isShow = !isShow;
            animToken.SetBool("IsShow", isShow);
        }
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
