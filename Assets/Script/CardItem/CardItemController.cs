using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItemController : MonoBehaviour
{
    public CardItemUpgradePopup cardItemUpgradePopup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeClick()
    {
        cardItemUpgradePopup.gameObject.SetActive(true);
    }
}
