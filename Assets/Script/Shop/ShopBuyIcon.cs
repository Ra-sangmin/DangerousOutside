using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyIcon : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text priceText;
    [SerializeField] Image iconImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DataSet(ShopBuyIconData shopBuyIconData)
    {
        nameText.text = shopBuyIconData.name;
        iconImage.sprite = shopBuyIconData.iconImage;
        priceText.text = shopBuyIconData.price.ToString();
    }
}
