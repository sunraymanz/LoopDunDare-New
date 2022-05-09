using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyCursor : MonoBehaviour
{
    public SpriteRenderer sprToken;
    public SpriteRenderer haloToken;
    public Sprite cursor;
    public Sprite cursorPressed;
    public Sprite crossHair;
    public Sprite crossPressed;
    public Player playerToken;
    public List<Sprite> icon;
    public bool onMenu;
    public bool onBuying = false;
    int itemType;
    [SerializeField] float yPos;
    Vector2 mousePos;
    //PointerData
    PointerEventData pointerData;
    List<RaycastResult> results;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        sprToken = GetComponent<SpriteRenderer>();
        playerToken = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;
        //transform.right = mousePos - (Vector2)playerToken.transform.position;
        if (onBuying)
        {
            haloToken.transform.position = new Vector2(snap(mousePos.x), yPos);
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
            { FindObjectOfType<GameManager>().BuyItem(itemType, haloToken.transform.position); }
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            onBuying = false;
            haloToken.enabled = false;
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
        if (index > 3)
        {
            FindObjectOfType<GameManager>().BuyItem(itemType, haloToken.transform.position);
            return;
        }
        onBuying = true;
        haloToken.enabled = true;
        haloToken.sprite = icon[index];
        haloToken.transform.localScale = Vector3.one;
        if (index == 1)
        { yPos =  -3.1f; }
        else if (index == 2)
        { 
            yPos = -4.3f;
            haloToken.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
        else
        { yPos = -4.6f; }
    }

}
