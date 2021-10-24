using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemController : MonoBehaviour
{
    public CardItemUpgradePopup cardItemUpgradePopup;

    [SerializeField] List<CardItem> cardItemList = new List<CardItem>();

    // Start is called before the first frame update
    void Start()
    {
        EventSet();   
    }

    void EventSet()
    {
        foreach (var item in cardItemList)
        {
            item.cardClickOn = UpgradeClick;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeClick(CardItem cardItem)
    {
        cardItemUpgradePopup.gameObject.SetActive(true);
        cardItemUpgradePopup.DataSet(cardItem);
    }
}
