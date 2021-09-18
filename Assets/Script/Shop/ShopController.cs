using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopController : MonoBehaviour
{
    [SerializeField] ShopBuyIcon shopBuyIconPrefab;

    [SerializeField] GridLayoutGroup goldBuyIconPanel;
    [SerializeField] GridLayoutGroup cashBuyIconPanel;

    [SerializeField] VerticalLayoutGroup content;

    bool changeOn = false;

    [SerializeField] GachaPopup gachaPopupPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        ShopAllData allData = Resources.Load<ShopAllData>("ShopData/ShopAllData");

        BuyIconCreate(allData.GetList(ShopBuyIconType.Gold) , goldBuyIconPanel);
        BuyIconCreate(allData.GetList(ShopBuyIconType.Cash), cashBuyIconPanel);
    }

    void BuyIconCreate(List<ShopBuyIconData> dataList , GridLayoutGroup glg)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            ShopBuyIcon iconObj = Instantiate(shopBuyIconPrefab , glg.transform);
            iconObj.DataSet(dataList[i]);
            iconObj.gameObject.SetActive(true);
        }

        //GridLayoutGroup 높이 설정
        glg.GetComponent<LayoutGroupSizeDeltaSet>().GridSizeDeltaSet();

        //VerticalLayoutGroup 높이 설정
        glg.transform.parent.GetComponent<LayoutGroupSizeDeltaSet>().VerticalSizeDeltaSet();
    }

    // Update is called once per frame
    void Update()
    {
        if (changeOn)
        {
            content.SetLayoutVertical();
            changeOn = false;
        }
    }

    public void GachaOnBtnClick(int status)
    {
        GachaPopup gachaPopup = Instantiate(gachaPopupPrefab, transform);
        gachaPopup.DataSet(status);
    }

}
