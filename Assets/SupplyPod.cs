using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyPod : MonoBehaviour
{
    Rigidbody2D rigidToken;
    public GameObject fireM;
    public GameObject fireL;
    public GameObject fireR;
    public GameObject podLid;
    [SerializeField] float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigidToken = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (transform.position.y < 7 && rigidToken.velocity.y < -2f)
        {
            if (timer < 0)
            {
                rigidToken.AddForce(Vector2.up*3,ForceMode2D.Impulse);
                LandingFire();
                timer = 0.15f;
            }
        }
        else { fireL.SetActive(false); fireR.SetActive(false); fireM.SetActive(false); }

    }
    public void LandingFire() 
    {
        fireL.SetActive(false); fireR.SetActive(false); fireM.SetActive(false);    
        int temp = Random.Range(1, 9);
        if (temp == 1) fireM.SetActive(true);
        else if (temp == 2) fireL.SetActive(true);
        else if (temp == 3) fireR.SetActive(true);
        else if (temp == 4) { fireL.SetActive(true); fireR.SetActive(true); }
        else if (temp == 5) { fireL.SetActive(true); fireM.SetActive(true); }
        else if (temp == 6) { fireR.SetActive(true); fireM.SetActive(true); }
        else if (temp == 7) { fireL.SetActive(false); fireR.SetActive(false); fireM.SetActive(false); }
        else if (temp == 8) { fireL.SetActive(true); fireR.SetActive(true); fireM.SetActive(true); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            podLid.GetComponent<Rigidbody2D>().simulated = true;
            podLid.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.2f, 1) * 5, ForceMode2D.Impulse);
            rigidToken.simulated = false;
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject,2f);
        }
    }
}
