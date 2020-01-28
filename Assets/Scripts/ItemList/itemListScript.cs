using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemListScript : MonoBehaviour
{
    ItemsListChunk itemsListChunk;
    // Start is called before the first frame update
    void Start()
    {
        itemsListChunk = GameObject.FindObjectOfType<ItemsListChunk>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteItem()
    {
        itemsListChunk.RemoveItem(gameObject);

    }
}
