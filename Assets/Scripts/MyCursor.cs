using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyCursor : MonoBehaviour
{
    public SpriteRenderer sprToken;
    public GameObject haloToken;
    public Sprite cursor;
    public Sprite cursorPressed;
    public Sprite crossHair;
    public Sprite crossPressed;
    public Player playerToken;
    public GameObject tooltips;
    public List<Sprite> icon;
    public bool onMenu;
    public bool onBuying = false;
    int itemType;
    [SerializeField] float yPos;
    Vector2 mousePos;
    //PointerData
    PointerEventData pointerData;
    List<RaycastResult> results;

    //[SerializeField]BoxCollider2D baseDetector;
    public bool isOverlap;
    public List<GameObject> objList;
    public int objDetect = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        sprToken = GetComponent<SpriteRenderer>();
        playerToken = FindObjectOfType<Player>();
        //baseDetector = GetComponentInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;
        //transform.right = mousePos - (Vector2)playerToken.transform.position;
        if (onBuying)
        {
            //haloToken.transform.position = new Vector2(snap(mousePos.x), yPos);
            haloToken.transform.position = new Vector2( mousePos.x, yPos);
            UiDetect();
            ConfirmBuying();
        }
        else
        {
            UiDetect();
            MouseCheck();
        }
        
    }

    public static float snap(float pos)
    {
        return 0.5f+Mathf.FloorToInt(pos);
    }



    void ConfirmBuying()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!onMenu)
            {
                if (!isOverlap)
                {
                    FindObjectOfType<GameManager>().BuyItem(itemType, haloToken.transform.position);
                }
                else
                {
                    FindObjectOfType<GameManager>().ShowWarning("Can't Deploy Here",true);
                }
            }
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            onBuying = false;
            //haloToken.enabled = false;
            //baseDetector.enabled = false;
            haloToken.SetActive(false);
        }
    }

    void MouseCheck()
    {
        if (onMenu)
        {
            sprToken.sprite = cursor;
            if (Input.GetMouseButton(0))
            {
                sprToken.sprite = cursorPressed;
            }
        }
        else 
        {
            sprToken.sprite = crossHair;
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                sprToken.sprite = crossPressed;
            }
        }    
    }
    
    private void OnTriggerEnter2D(Collider2D collision)     
    {
        Debug.Log("Hit : " + collision.name);
        
        if (collision.gameObject.layer == 13 || collision.gameObject.layer == 15 || collision.gameObject.layer == 12 || collision.gameObject.tag == "MinerBase")
        {
            if (!objList.Contains(collision.gameObject))
            {
                objList.Add(collision.gameObject);
                objDetect = objList.Count;
            }
            isOverlap = true;
        }      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit : " + collision.name);
        if (collision.gameObject.layer == 13 || collision.gameObject.layer == 15 || collision.gameObject.layer == 12 || collision.gameObject.tag == "MinerBase")
        {
            if (objList.Contains(collision.gameObject))
            {
                objList.Remove(collision.gameObject);
                objDetect = objList.Count;
            }
            if (objDetect==0)
            {
                isOverlap = false;
            }
        }    
    }
    void UiDetect()
    {
        pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count > 0)
        {
            if (results[0].gameObject.layer == 5)
            {
                onMenu = true;
                //string dbg = "Root Element: {0} \n GrandChild Element: {1}";
                //Debug.Log(string.Format(dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name));
                results.Clear();
            }
        }
        else
        { onMenu = false; }
    }
    public void SelectIcon(int index)
    {
        itemType = index;
        if (index > 3) //miner
        {
            FindObjectOfType<GameManager>().BuyItem(itemType, haloToken.transform.position);
            return;
        }
        onBuying = true;
        //baseDetector.enabled = true;
        //haloToken.enabled = true;
        //haloToken.sprite = icon[index];
        haloToken.SetActive(true);
        haloToken.GetComponent<SpriteRenderer>().sprite = icon[index];
        haloToken.transform.localScale = Vector3.one;
        if (index == 1) //pole
        { 
            yPos =  -3.1f;
            haloToken.GetComponent<BoxCollider2D>().size = new Vector2(1.5f, 2);
            haloToken.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.8f);
            //baseDetector.size = new Vector2(1.3f, 2);
           // baseDetector.offset = new Vector2(0, -0.8f);
        }
        else if (index == 2) //miner base
        { 
            yPos = -4.3f;
            haloToken.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            haloToken.GetComponent<BoxCollider2D>().size = new Vector2(4, 2);
            haloToken.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0.4f);
            // baseDetector.size = new Vector2(4, 2);
            //baseDetector.offset = new Vector2(0, 0.4f);
        }
        else //turret
        {
            yPos = -4.6f;
            haloToken.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
            haloToken.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0f);
            //baseDetector.size = new Vector2(1, 1);
            //baseDetector.offset = new Vector2(0, 0f);
        }    
    }

}
