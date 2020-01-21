using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsList : MonoBehaviour
{
    public GameObject listContent;
    public GameObject itemsListPrefab;
    public float separation = 20.0f;
    int itemCount;
    List<ItemList> itemsList ;

    string fileName = "Saves/Data.csv";
    
    struct ItemList
    {
        public int index;
        public Color color;  
        public string text;
    }
    private void Start()
    {
        itemsList = new List<ItemList>();
    }

    public void ButtonAddPressed()
    {
        InputField inputText = GameObject.Find("ItemNameInput").GetComponent<InputField>();
        ItemList item = new ItemList() { text = inputText.text, index = itemCount };
        
        itemsList.Add(item);
        itemCount++;

        RectTransform prefabRect = itemsListPrefab.transform.Find("Background").GetComponent<RectTransform>();

        //- Screen.height * 0.05f-  separation 

        Vector3 position = new Vector3(listContent.transform.position.x + Screen.width * 0.3f,
            listContent.transform.position.y - Screen.height * 0.05f - separation - prefabRect.rect.height*2*item.index, // initial pos - margin - separation - height of the prefab * nº prefabs
            0);
        GameObject nwItem = Instantiate(itemsListPrefab,position, listContent.transform.localRotation, listContent.transform);
        RectTransform rect = nwItem.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(Screen.width * 0.9f, rect.sizeDelta.y);
    }


    
    

    void ImportData() { 
    
    
    }

    void ExportData() { 

    }

}
