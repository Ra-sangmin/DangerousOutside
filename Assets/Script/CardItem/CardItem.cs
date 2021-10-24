using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardItem : BaseCardItem
{
    public UnityAction<CardItem> cardClickOn;

    // Start is called before the first frame update
    void Start()
    {
        BGSet();
    }

    public void CardClickOn()
    {
        if (cardClickOn != null)
        {
            cardClickOn(this);
        }
        
    }
}
