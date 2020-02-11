using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class itemListScript : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    ItemsListChunk itemsListChunk;
    public GameObject boughtButtons;
    public Color BoughtColor = Color.gray;
    public Color notBoughtColor = Color.white;
    private Vector2 currPressedCoords = Vector3.zero;
    private Vector2 initPressedCoords = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        itemsListChunk = GameObject.FindObjectOfType<ItemsListChunk>();
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
        if (bought)
        {
            bg.color = BoughtColor;
        }
        else
        {
            bg.color = notBoughtColor;
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
        Vector2 diff =  eventData.position- currPressedCoords;
        if (diff.x>0|| currPressedCoords.x>initPressedCoords.x)
        {
            transform.position = new Vector3(transform.position.x +diff.x, transform.position.y, transform.position.z);
            currPressedCoords = eventData.position;
            

            if (initPressedCoords.x - currPressedCoords.x > itemBG_rect.rect.width/2)
            {
                DeleteItem();
            }
        }
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        currPressedCoords =  initPressedCoords = eventData.position;
    }
}
