using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriorityButtons : MonoBehaviour
{
    public ItemsListChunk.ItemList.Priority currPriority = ItemsListChunk.ItemList.Priority.Low;
    GameObject selBG;

    Button low;
    public Color lowCol;
    Button med;
    public Color medCol;
    Button high;
    public  Color highCol;
    public void InitButtons()
    {
        selBG = transform.GetChild(0).gameObject;
        low = transform.GetChild(1).gameObject.GetComponent<Button>();
        lowCol = low.gameObject.GetComponent<Image>().color;
        med = transform.GetChild(2).gameObject.GetComponent<Button>();
        medCol = med.gameObject.GetComponent<Image>().color;
        high = transform.GetChild(3).gameObject.GetComponent<Button>();
        highCol = high.gameObject.GetComponent<Image>().color;
        UpdateBGPos();
    }

    public void ButtonPressed(int index)
    {
        currPriority =(ItemsListChunk.ItemList.Priority)index;
        UpdateBGPos();
    }


    void UpdateBGPos()
    {
        switch (currPriority)   
        {
            case ItemsListChunk.ItemList.Priority.Low:
                selBG.transform.position = low.transform.position;
                break;
            case ItemsListChunk.ItemList.Priority.Med:
                selBG.transform.position = med.transform.position;
                break;
            case ItemsListChunk.ItemList.Priority.High:
                selBG.transform.position = high.transform.position;
                break;
        }
    }


}
