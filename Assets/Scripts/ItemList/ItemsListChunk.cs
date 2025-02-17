﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsListChunk : MonoBehaviour
{
    public GameObject mainMenuChunk;
    public GameObject listContent;
    public GameObject itemsListPrefab;
    public GameObject itemsListSeparatorPrefab;
    public PriorityButtons priorityButtons;
    public DataManager dataManager;
    public Dropdown SMDropdown;
    public Dropdown TagDropdown;
    public float initialSeparationY = 100.0f;
    public float separation = 20.0f;

    List<ItemList> itemsList;
    public List<string> defSMList = new List<string>(){ "Tesco", "Aldi", "Asda", "Sainsbury" };
    public List<string> defTagList = new List<string>() { "Vegetables", "Fruit", "Fish", "Meat", "Lactic", "Various"};
    List<string> SMList;
    List<string> TagsList;

    public enum OrderBy
    {
        Default,
        Priority,
        Tag,
        SM
    }
    public OrderBy order = OrderBy.Default;

    public Text debug;

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
        public bool bought = false;

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
        SMList = new List<string>();
        TagsList = new List<string>();

        priorityButtons.InitButtons();

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            dataFilePath = "Assets/Saves/Data.txt";
        }
        else
        {
            dataFilePath = Application.persistentDataPath + "/Data.txt";
        }
        needToImport = true;

    }
    private void Update()
    {
        if (Time.time > 0.01f && needToImport) // in start the content is not well placed.
        {
            ImportData(null, null);
            UpdateItemsPosition();
            needToImport = false;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                gameObject.SetActive(false);
                mainMenuChunk.SetActive(true);
                return;
            }
        }
    }
    //------------------------------UI
    public void ButtonAddPressed()
    {
        InputField itemInput = GameObject.Find("ItemName_Input").GetComponent<InputField>();
        
        AddItem(priorityButtons.currPriority, itemInput.text, SMDropdown.options[SMDropdown.value].text, TagDropdown.options[TagDropdown.value].text);
        itemInput.text = "";
        ExportData();
        
    }

    public void ButtonClearPressed(bool clearAll)
    {

        dataManager.ResetDataFile(dataFilePath);

        ClearItemList();

        if (clearAll)
        {
            // TODO put dropdown index to 0 to avoid out of range
            ClearSMList();
            ClearTagList();
            ImportData(null, null);
        }
        else
        {
            ImportData(TagsList, SMList);
        }

        ExportData();
        UpdateItemsPosition();
    }

    public void ButtonAddSMPressed()
    {

        InputField i = SMDropdown.transform.Find("AddSM_Panel").Find("SM_Input").GetComponent<InputField>();
        AddSupermarket(i.text);
        i.text = "";
    }

    public void ButtonAddTagPressed()
    {
        InputField i = TagDropdown.transform.Find("AddTag_Panel").Find("Tag_Input").GetComponent<InputField>();
        AddTag(i.text);
        i.text = "";
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
    void AddItem(ItemList.Priority priority, string product, string SM, string tag, bool bought = false)
    {
        ItemList item = new ItemList() { productName = product };
        itemsList.Add(item);

        GameObject itemGO = Instantiate(itemsListPrefab, listContent.transform.position, listContent.transform.localRotation, listContent.transform);
        item.itemGO = itemGO;

        //Priority color
        Image priorityImg = itemGO.transform.Find("PriorityColor").GetComponent<Image>();
        item.priority = priority;
        switch (item.priority)
        {
            case ItemList.Priority.Low:
                priorityImg.color = priorityButtons.lowCol;
                break;
            case ItemList.Priority.Med:
                priorityImg.color = priorityButtons.medCol;
                break;
            case ItemList.Priority.High:
                priorityImg.color = priorityButtons.highCol;
                break;
        }

        //Supermarket
        Text SMText = itemGO.transform.Find("SM_Txt").GetComponent<Text>();
        SMText.text = SM;
        item.supermarket = SM; 
        AddSupermarket(SM);

        //Tag

        Text TagText = itemGO.transform.Find("Tag_Txt").GetComponent<Text>();
        TagText.text = tag;
        item.tag = tag;
        AddTag(tag);

        item.bought = bought;
        itemGO.GetComponent<itemListScript>().setItemBought(bought);

        //Product Name
        Text text = itemGO.transform.Find("Product_Txt").GetComponent<Text>();
        text.text = item.productName;


        OrderByBought();
    }
    void UpdateItemsPosition()
    {
        int c = 0;
        foreach (ItemList i in itemsList)
        {
            RectTransform itemBG_rect = i.itemGO.transform.GetComponent<RectTransform>();

            //RectTransform viewportRect = listContent.transform.parent.transform.GetComponent<RectTransform>();

            Vector3 newPos = new Vector3(listContent.transform.position.x + listContent.GetComponent<RectTransform>().rect.size.x * 0.05f,/*if changed, need to change OnPointerUp of itemListScript*/
           listContent.transform.position.y - initialSeparationY - (separation + itemBG_rect.rect.height*2.0f ) * c, // initial pos - margin - separation - height of the prefab * nº prefabs
           0);

            i.itemGO.transform.position = newPos;
            itemBG_rect.sizeDelta = new Vector2(listContent.GetComponent<RectTransform>().rect.size.x * 0.9f, itemBG_rect.sizeDelta.y);
            c++;
        }
    }

    
    //-------------CLEAR
    void ClearItemList()
    {
        foreach (ItemList i in itemsList)
        {
            i.CleanUp();
        }
        itemsList.Clear();
    }
    void ClearSMList()
    {
        SMList.Clear();
        SMDropdown.ClearOptions();
        AddSupermarket("");
        AddSupermarket(defSMList);
    }
    void ClearTagList()
    {
        TagsList.Clear();
        TagDropdown.ClearOptions();
        AddTag("");
        AddTag(defTagList);
    }
    

    //-------------ORDER BY

    public void OrderByPriority()
    {
        List<ItemList> aux = new List<ItemList>();
        bool auxBool = true;
        for (int num = 0; num < 2; num++)
        {
            auxBool = !auxBool;
            int enumCount = System.Enum.GetValues(typeof(ItemList.Priority)).Length;
            for (int j = enumCount - 1; j >= 0; j--)
            {
                foreach (ItemList i in itemsList)
                {
                    if (i.bought == auxBool && i.priority == (ItemList.Priority)j) 
                    {
                        aux.Add(i);
                    }
                }
            }
        }
        itemsList = aux;
        order = OrderBy.Priority;
        UpdateItemsPosition();
    }

    public void OrderByTag()
    {
        List<ItemList> aux = new List<ItemList>();
        bool auxBool = true;
        for (int num = 0; num < 2; num++)
        {
            auxBool = !auxBool;
            foreach (string s in TagsList)
            {
                foreach (ItemList i in itemsList)
                {
                    if (i.bought == auxBool && i.tag == s)
                    {
                        aux.Add(i);
                    }
                }
            }
        }
        itemsList = aux;
        order = OrderBy.Tag;
        UpdateItemsPosition();
    }

    public void OrderBySM()
    {
        List<ItemList> aux = new List<ItemList>();
        bool auxBool = true;
        for (int num = 0; num < 2; num++)
        {
            auxBool = !auxBool;
            foreach (string s in SMList)
            {
                foreach (ItemList i in itemsList)
                {
                    if (i.bought == auxBool && i.supermarket == s)
                    {
                        aux.Add(i);
                    }
                }
            }
        }
        itemsList = aux;
        order = OrderBy.SM;
        UpdateItemsPosition();
    }
    private void OrderByBought()
    {
        List<ItemList> aux = new List<ItemList>();
        bool auxBool = true;
        for (int num = 0; num < 2; num++)
        {
            auxBool = !auxBool;
            foreach (ItemList i in itemsList)
            {
                if (i.bought == auxBool )
                {
                    aux.Add(i);
                }
            }
        }
        itemsList = aux;
        UpdateItemsPosition();
    }
    //-------------------------BOUGHT

    public void SetItemBought(GameObject GO, bool bought)
    {
        foreach(ItemList i in itemsList)
        {
            if(i.itemGO == GO)
            {
                i.bought = bought;
                OrderByBought();
                ExportData();
                return;
            }
        }
    }


    //-------------------------SUPERMARKET DROPDOWN

    void AddSupermarket(string name)
    {
        foreach (string s in SMList)
        {
            if (name == s)//name already exist
            {
                debug.text = "SM already exists";
                return;
            }
        }
        SMList.Add(name);
        List<string> l = new List<string>();
        l.Add(name);
        SMDropdown.AddOptions(l);
    }

    void AddSupermarket(List<string> namesWOchecking)
    {
        foreach(string s in namesWOchecking)
        {
            SMList.Add(s);
        }
        SMDropdown.AddOptions(namesWOchecking);
    }
    //-----------------------------TAG

    void AddTag(string tag)
    {
        foreach (string s in TagsList)
        {
            if (tag == s)//name already exist
            {
                debug.text = "tag already exists"; ///
                return;
            }
        }
        TagsList.Add(tag); 
        List<string> l = new List<string>();
        l.Add(tag);
        TagDropdown.AddOptions(l);
    }
    void AddTag(List<string> tags)
    {
        foreach (string s in tags)
        {
            TagsList.Add(s);
        }
        TagDropdown.AddOptions(tags);
    }
    //-------------------------------DATA MANAGEMENT
    public void ImportData(List<string> preservedTags, List<string> preservedSM )
    {
        ClearItemList(); 
        
        if (preservedTags == null)
            ClearTagList();

        if (preservedSM==null)
            ClearSMList();
        
        debug.text = "tryToImport"; ///

        List<ItemList> nwItemList = new List<ItemList>();
        List<string> nwSMList = new List<string>();
        List<string> nwTagList = new List<string>();
        dataManager.DeserializeData(dataFilePath, nwItemList, nwSMList, nwTagList);

        debug.text = "deserialized"; ///
        
        foreach (string s in nwSMList)
        {
            AddSupermarket(s);
        }
        foreach (string s in nwTagList)
        {
            AddTag(s);
        }

        foreach (ItemList i in nwItemList)
        {
            AddItem(i.priority, i.productName, i.supermarket, i.tag, i.bought) ;
            
        }
       //if preserved SM & Tags
        if(preservedSM !=null)
        {
            foreach (string s in preservedSM)
            {
                AddSupermarket(s);
            }
        }
        if (preservedTags != null)
        {
            foreach (string s in preservedTags)
            {
                AddTag(s);
            }
        }

        debug.text = nwItemList.Count.ToString();///
    }

    public void ExportData()
    {
        debug.text = "try to export"; ///
        dataManager.SerializeData(dataFilePath, itemsList,SMList, TagsList);
        debug.text = "exported";      ///  
    }
}
