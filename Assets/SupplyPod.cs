using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SupplyPod : MonoBehaviour
{
    Rigidbody2D rigidToken;
    GameManager token;
    public GameObject fireM;
    public GameObject fireL;
    public GameObject fireR;
    public GameObject podLid;
    public Transform dropPos;
    public bool isOpen = false;
    public TMPro.TextMeshPro textToken;
    [SerializeField] float timer = 0;
    [SerializeField] string[] grade = { "s", "a", "b" };
    // Start is called before the first frame update
    void Start()
    {
        rigidToken = GetComponent<Rigidbody2D>();
        token = FindObjectOfType<GameManager>();
        int temp = Random.Range(1,101)+(token.waveNum / 5);
        if (temp > 95 ) textToken.text = grade[0];
        else if (temp > 65 ) textToken.text = grade[1];
        else textToken.text = grade[2];      
        Debug.Log("Random Roll : "+temp);
        Debug.Log("Grade : "+ textToken.text);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (transform.position.y < 7 && rigidToken.velocity.y < -4f)
        {
            if (timer < 0)
            {
                rigidToken.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
                LandingFire();
                timer = 0.1f;
            }
        }
        else { fireL.SetActive(false); fireR.SetActive(false); fireM.SetActive(false); }

    }
    public void LandingFire()
    {
        fireL.SetActive(false); fireR.SetActive(false); fireM.SetActive(false);
        int temp = Random.Range(1, 8);
        if (temp == 1) fireM.SetActive(true);
        else if (temp == 2) fireL.SetActive(true);
        else if (temp == 3) fireR.SetActive(true);
        else if (temp == 4) { fireL.SetActive(true); fireR.SetActive(true); }
        else if (temp == 5) { fireL.SetActive(true); fireM.SetActive(true); }
        else if (temp == 6) { fireR.SetActive(true); fireM.SetActive(true); }
        else if (temp == 7) { fireL.SetActive(true); fireR.SetActive(true); fireM.SetActive(true); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            podLid.GetComponent<Rigidbody2D>().simulated = true;
            podLid.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.2f, 1) * 5, ForceMode2D.Impulse);
            GetComponent<BoxCollider2D>().enabled = false;
            rigidToken.simulated = false;
            token.StartCoroutine(token.DelaySpawnCore(1, dropPos.position, 0));
            //Additional Drop
            if (textToken.text == "s")
            token.StartCoroutine(token.DelaySpawnCore(1, dropPos.position, 1));
            else if (textToken.text == "a")
            token.StartCoroutine(token.DelaySpawnCore(1, dropPos.position, 0));
            isOpen = true;
            Destroy(gameObject, 2f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stage"))
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
