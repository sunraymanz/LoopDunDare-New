using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        { SceneManager.LoadScene("Gameplay"); }
        if (cam.transform.position.z > -15f)
        { cam.transform.position -= new Vector3(0f, 0f, 0.05f); }
    }
}
