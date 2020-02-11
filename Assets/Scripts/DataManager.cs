using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour { 

    
    public void SerializeData(string filePath, List<ItemsListChunk.ItemList> listOfItems, List<string> listOfSM, List<string> TagListToFill)
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
            writer.WriteLine(i.bought);
        }
        writer.WriteLine(TagListToFill.Count);
        foreach (string s in TagListToFill)
        {
            writer.WriteLine(s);
        }
        writer.WriteLine(listOfSM.Count);
       foreach(string s in listOfSM)
        {
            writer.WriteLine(s);
        }

        writer.Close();
    }

    public void DeserializeData(string filePath, List<ItemsListChunk.ItemList> itemListToFill, List<string> SMListToFill, List<string> TagListToFill)
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
                item.priority = (ItemsListChunk.ItemList.Priority)(int.Parse(reader.ReadLine()));
                item.supermarket = reader.ReadLine();
                item.tag = reader.ReadLine();
                string s = reader.ReadLine();
                item.bought = s=="True";
                itemListToFill.Add(item);
            }
            if (!reader.EndOfStream)
            {
                int num2 = int.Parse(reader.ReadLine());
                for (int i = 0; i < num2; i++)
                {
                    TagListToFill.Add(reader.ReadLine());
                }
            }
            if (!reader.EndOfStream)
            {
                int num2 = int.Parse(reader.ReadLine());
                for (int i = 0; i < num2; i++)
                {
                    SMListToFill.Add(reader.ReadLine());
                }
               
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
