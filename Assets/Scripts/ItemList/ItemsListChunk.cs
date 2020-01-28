using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsListChunk : MonoBehaviour
{
    public GameObject listContent;
    public GameObject itemsListPrefab;
    public DataManager dataManager;
    public float separation = 20.0f;

    List<ItemList> itemsList;

    public Text debug;
    private PriorityButtons priorityButt;

    string dataFilePath;

    private bool needToImport = true;

    public class ItemList
    {

        public enum Priority
        {
            Low,
            Med,
            High
        }

        public string productName = "";
        public Priority priority;
        public string supermarket = "";
        public string tag = "";

        public GameObject itemGO;

        public void CleanUp()
        {
            Destroy(itemGO);

        }
    }


    //-------------------------LOOP CYCLE
    private void Start()
    {
        itemsList = new List<ItemList>();
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            dataFilePath = "Assets/Saves/Data.txt";
        }
        else
        {
            dataFilePath = Application.persistentDataPath + "/Data.txt";
        }
        needToImport = true;

        priorityButt = FindObjectOfType<PriorityButtons>();
    }
    private void Update()
    {
        if (Time.time > 0.01f && needToImport) // in start the content is not well placed.
        {
            ImportData();
            UpdateItemsPosition();
            needToImport = false;
        }
    }

    //------------------------------UI
    public void ButtonAddPressed()
    {
        InputField inputText = GameObject.Find("ItemNameInput").GetComponent<InputField>();
        AddItem(priorityButt.currPriority, inputText.text);
        inputText.text = "";
        ExportData();
        
    }

    public void ButtonClearPressed()
    {
        ClearItemList();
        dataManager.ResetDataFile(dataFilePath);
        UpdateItemsPosition();
        ImportData();
    }

    //-----------------------------ITEMS LIST MANAGEMENT
    public void RemoveItem(GameObject goItem)
    {
        for (int i = 0; i < itemsList.Count; i++)
        {
            if (itemsList[i].itemGO == goItem)
            {
                itemsList[i].CleanUp();
                itemsList.Remove(itemsList[i]);
                UpdateItemsPosition();
                ExportData();
            }
        }
    }
    void AddItem(ItemList.Priority priority, string txt)
    {

        ItemList item = new ItemList() { productName = txt };
        itemsList.Add(item);

        GameObject itemGO = Instantiate(itemsListPrefab, listContent.transform.position, listContent.transform.localRotation, listContent.transform);
        item.itemGO = itemGO;

        //Priority color
        Image priorityImg = itemGO.transform.Find("PriorityColor").GetComponent<Image>();
        item.priority = priority;
        switch (item.priority)
        {
            case ItemList.Priority.Low:
                priorityImg.color = priorityButt.lowCol;
                break;
            case ItemList.Priority.Med:
                priorityImg.color = priorityButt.medCol;
                break;
            case ItemList.Priority.High:
                priorityImg.color = priorityButt.highCol;
                break;
        }

        //Product Name
        Text text = itemGO.transform.Find("Product_Txt").GetComponent<Text>();
        text.text = item.productName;

        UpdateItemsPosition();


    }
    void ClearItemList()
    {
        foreach (ItemList i in itemsList)
        {
            i.CleanUp();
        }
        itemsList.Clear();
    }

    public void OrderByPriority()
    {
        List<ItemList> aux = new List<ItemList>();

        foreach (ItemList i in itemsList)
        {
            if (i.priority == ItemList.Priority.High)
            {
                aux.Add(i);
            }
        }
        foreach (ItemList i in itemsList)
        {
            if (i.priority == ItemList.Priority.Med)
            {
                aux.Add(i);
            }
        }
        foreach (ItemList i in itemsList)
        {
            if (i.priority == ItemList.Priority.Low)
            {
                aux.Add(i);
            }
        }
        itemsList = aux;
        UpdateItemsPosition();
    }

    void OrderBySuperMarket() { }

    void UpdateItemsPosition()
    {
        int c = 0;
        foreach (ItemList i in itemsList)
        {
            RectTransform itemBG_rect = i.itemGO.transform.GetComponent<RectTransform>();

            RectTransform viewportRect = listContent.transform.parent.transform.GetComponent<RectTransform>();

            Vector3 newPos = new Vector3(listContent.transform.position.x+ viewportRect.rect.size.x * 0.1f,
           listContent.transform.position.y - viewportRect.rect.size.y * 0.12f - separation - itemBG_rect.rect.height * 2.0f * c, // initial pos - margin - separation - height of the prefab * nº prefabs
           0); 
                
            i.itemGO.transform.position = newPos;
            itemBG_rect.sizeDelta = new Vector2(viewportRect.rect.size.x * 0.9f, itemBG_rect.sizeDelta.y);
            c++;
        }

    }



    //-------------------------------DATA MANAGEMENT
    public void ImportData()
    {
        debug.text = "clearing";
        ClearItemList();
        debug.text = "tryToImport";

        List<ItemList> nwList = new List<ItemList>();
        dataManager.DeserializeItems(dataFilePath, nwList);
        debug.text = "deserialized";
        foreach (ItemList i in nwList)
        {
            AddItem(i.priority, i.productName);
        }
        debug.text = nwList.Count.ToString();
    }

    public void ExportData()
    {
        debug.text = "try to export";
        dataManager.SerializeItems(dataFilePath, itemsList);
        debug.text = "exported";
        
    }
}
