using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic; // using List

public class Q_Test : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    private List<Q_Item> scoreList = new List<Q_Item>();


    protected virtual void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var interactBtn = PlayerInputs.Instance.InteractAndPickUpItemBool;
        if (collision.gameObject.tag == "Item" && interactBtn == true)
        {
            var auxItem = collision.GetComponent<Q_Item>();
            AddToInvetory(auxItem);
        }
    }

    private void AddToInvetory( Q_Item _item)
    {
        // if inventory is full 

        // if doesnt  has item
        if (true == hasItem(0))
        {

        }
        // if already has item
            // if stack esta lleno 
                //if 
    }

    private void RemoveFromInventory( int _itemID)
    {
        
        
    }

    private bool hasItem(uint _itemID)
    {
        foreach (var item in scoreList)
        {
           if (item.m_ID == _itemID)
           {
                return true;
           }
        }
        return false;
    }

}