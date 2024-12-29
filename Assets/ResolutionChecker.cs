using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionChecker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Screen.fullScreen+" : "+Screen.width+" x "+ Screen.height+" @ "+ Screen.currentResolution.refreshRateRatio+"Hz";
    }
}
