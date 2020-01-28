using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour { 

    
    public void SerializeItems(string filePath, List<ItemsListChunk.ItemList> listOfItems)
    {
        File.Delete(filePath);
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine(listOfItems.Count);
        foreach(ItemsListChunk.ItemList i in listOfItems)
        {
            writer.WriteLine(i.productName);
            writer.WriteLine((int)i.priority);
            writer.WriteLine(i.supermarket);
            writer.WriteLine(i.tag);
        }

        writer.Close();
    }

    public void DeserializeItems(string filePath, List<ItemsListChunk.ItemList> listToFill)
    {
        
        if (!File.Exists(filePath))
        {
            Debug.Log("Cannot find file.");
        }
        else
        {
            StreamReader reader = new StreamReader(filePath);
            int num = int.Parse(reader.ReadLine());
            for (int j = 0; j < num; j++)
            {
                ItemsListChunk.ItemList item = new ItemsListChunk.ItemList();
                item.productName = reader.ReadLine();
                string r = reader.ReadLine();
                int i = int.Parse(r);
                item.priority = (ItemsListChunk.ItemList.Priority)( i);
                item.supermarket = reader.ReadLine();
                item.tag = reader.ReadLine();
                listToFill.Add(item);
            }  
            reader.Close();

        }
    }
    public void ResetDataFile(string fileName)
    {
        
        File.Delete(fileName);
        File.Delete(fileName + ".meta");

       // StreamWriter writer = new StreamWriter(fileName);
       // writer.Close();
        
    }
}
