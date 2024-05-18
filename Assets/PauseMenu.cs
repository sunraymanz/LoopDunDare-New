using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    GameManager token;
    [SerializeField]GameObject optionPage;
    [SerializeField]GameObject pausePage;
    bool isOptionOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOptionOpen)
            {
                ToggleOption();
            }
            else Close();
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void ToggleOption()
    {
        isOptionOpen = !isOptionOpen;
        optionPage.SetActive(isOptionOpen);
        pausePage.SetActive(!isOptionOpen);
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
