using System.Collections;
using System.Collections.Generic;
using Mobcast.Coffee.UI;
using UnityEngine;
using UnityEngine.UI;

public class BaseItem : MonoBehaviour 
{
    /// <summary> 아이템 사용시 필요 코스트 </summary>
    protected int needCost;
    [SerializeField] Text needCostText;
    [SerializeField] Text titleText;
    [SerializeField] AtlasImage lockImage;

    public ItemState itemState = ItemState.None;
    
    /// <summary> 세금 컨트롤러 </summary>
    [System.NonSerialized] protected TaxController taxController;
    [System.NonSerialized] protected TileController tileController;
    [System.NonSerialized] protected CityhallController cityhallController;
    [System.NonSerialized] protected bool activeOn;

    // Start is called before the first frame update
    void Start()
    {
        //Init();   
    }

    public virtual void Init()
    {
        taxController = FindObjectOfType<TaxController>();
        tileController = FindObjectOfType<TileController>();
        cityhallController = FindObjectOfType<CityhallController>();
        SetNeedCost();
        SetLock();
        activeOn = ItemManager.Ins.GetItemOnData(itemState);
    }

    protected bool CheckCost(bool mgsOn = true)
    {
        if (taxController == null)
            return false;
        
        return taxController.CheckCost(needCost, mgsOn);

    }

    protected void AddCost()
    {
        if (taxController == null)
            return;

        taxController.AddCost(-needCost);
    }

    public void SetNeedCost()
    {
        string titleTextStr = "";
        switch (itemState)
        {
            case ItemState.GiftCntAdd:
                titleTextStr = "물자공급";
                needCost = 0; break;
            case ItemState.BanArea:
                titleTextStr = "금지구역";
                needCost = 0; break;
            case ItemState.CleanMan:
                titleTextStr = "방역이소환";
                needCost = 4; break;
            case ItemState.ForceGoHome:
                titleTextStr = "강제귀가";
                needCost = 0; break;
            case ItemState.Delivery:
                titleTextStr = "로켓배송";
                needCost = 0; break;
        }

        if (titleText != null)
            titleText.text = titleTextStr;
        
        if (needCostText != null) 
            needCostText.text = string.Format("cost: {0}", needCost);

    }

    protected void SetLock()
    {
        bool itemOn = ItemManager.Ins.GetItemOnData(itemState);
        lockImage.gameObject.SetActive(!itemOn);
    }

    public void LockImageClick()
    {
        WarningManager.Instance.WarningSet("아직 사용할수 없습니다.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
