using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Camera cam;
    public GameObject cover;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            cover.SetActive(true);
            Invoke(nameof(LoadNextScene),1f); 
        }        
    }

    private void FixedUpdate()
    {
        if (cam.transform.position.z > -15f)
        { cam.transform.position -= new Vector3(0f, 0f, 0.25f); }
    }

    void LoadNextScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
