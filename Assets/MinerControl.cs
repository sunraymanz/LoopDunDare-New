using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerControl : MonoBehaviour
{
    public bool isWorking = true;
    // Start is called before the first frame update
    public void ToggleStatus()
    {
        isWorking = !isWorking;
    }
}
