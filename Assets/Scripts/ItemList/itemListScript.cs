using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class itemListScript : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    ItemsListChunk itemsListChunk;
    public GameObject boughtButtons;
    public Color BoughtColor = Color.gray;
    public Color notBoughtColor = Color.white;
    private Vector2 currPressCoords = Vector3.zero;
    private Vector2 initPressCoords = Vector3.zero;
    private RectTransform contentRect;
    private bool dragged = false;

    // Start is called before the first frame update
    void Start()
    {
        itemsListChunk = GameObject.FindObjectOfType<ItemsListChunk>();
        contentRect = transform.parent.GetComponent<RectTransform>();
    }
    
    
    void Update()
    {
        
    }
    public void ButtonBoughtPressed(bool bought)
    {
        setItemBought(bought);
        itemsListChunk.SetItemBought(gameObject, bought);
    }
    public void setItemBought(bool bought)
    {        

        Image bg = gameObject.GetComponent<Image>();
        Text t = gameObject.transform.Find("Product_Txt").GetComponent<Text>();
        if (bought)// IF BOUGHT ( DARKER)
        {
            bg.color = BoughtColor;
            t.color = Color.white;
        }
        else //IF NOT BOUGHT (WHITE)
        {
            bg.color = notBoughtColor;
            t.color = Color.black;
        }
        boughtButtons.transform.Find("Bought_Button").gameObject.SetActive(!bought);
        boughtButtons.transform.Find("ToBuy_Button").gameObject.SetActive(bought);
    }
    public void DeleteItem()
    {
        itemsListChunk.RemoveItem(gameObject);

    }
    //DRAGGING TO ERASE
    public void OnDrag(PointerEventData eventData)
    {
        if (dragged)
        {
            Vector2 diff = eventData.position - currPressCoords;//dragging pos.x each frame
            if (diff.x > 0 || currPressCoords.x > initPressCoords.x)
            {
                transform.position = new Vector3(transform.position.x + diff.x, transform.position.y, transform.position.z);
                currPressCoords = eventData.position;

                if (currPressCoords.x - initPressCoords.x > contentRect.position.x + contentRect.rect.width / 2)
                {
                    DeleteItem();
                }
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        currPressCoords =  initPressCoords = eventData.position;
        dragged = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        currPressCoords = initPressCoords = Vector2.zero;
        transform.position = new Vector3(contentRect.position.x + contentRect.rect.size.x * 0.1f, /*if changed, need to change UpdateItemsPosition*/
            transform.position.y , transform.position.z);
        dragged = false;
    }
}
