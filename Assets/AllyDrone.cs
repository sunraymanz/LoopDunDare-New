using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyDrone : MonoBehaviour
{
    [SerializeField] PlayerGun droneGunToken;
    [SerializeField] Animator animGunToken;
    [SerializeField] Animator animBodyToken;
    Vector3 targetPos;


    // Update is called once per frame
    void LateUpdate()
    {
        if (FindObjectOfType<GameManager>().isEnd)
        {
            animBodyToken.enabled = true;
            animGunToken.SetBool("Dead",true);
            Destroy(this.gameObject, 1.5f);
            this.enabled = false;
            return;
        }
        CheckFollowTarget();
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (Mathf.Abs(transform.position.x - targetPos.x) > 2 || Mathf.Abs(transform.position.y - targetPos.y) > 1.5f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2f);
            if (transform.position.y < -3f)
            {
                transform.position = new Vector3(transform.position.x, -3f, transform.position.z);
            }
        }
    }

    private void CheckFollowTarget()
    {
        if (FindObjectOfType<Player>())
        {
            targetPos = FindObjectOfType<Player>().transform.position;
            droneGunToken.enabled = true;
        }
        else
        {
            targetPos = FindObjectOfType<HQBase>().transform.position;
            droneGunToken.enabled = false;
        }
    }
}
