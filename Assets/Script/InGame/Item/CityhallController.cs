using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CityhallController : BaseItem
{
    bool initOn = false;

    public Text giftCnttext;
    int gift_currentValue = 0;
    int gift_maxValue = 10;

    float giftAddCurrentDelay;
    float giftAddValue = 1f;
    float giftAddMaxDelay = 10;

    List<Building> allBuildingList = new List<Building>();
    List<Building> homeBuildingList = new List<Building>();

    [SerializeField] Image fillImage;

    [SerializeField] RectTransform coopangParant;

    bool useDelayOn = false;
    float useDelay = 0;

    // Start is called before the first frame update
    void Start()
    {   
    }

    void SetEvent()
    {
        //TimeManager.Ins.secOn += SecTimeOn;
    }

    public override void Init()
    {
        base.Init();

        if (activeOn)
        {
            gift_currentValue = 0;
            gift_maxValue = 10;

            GiftCntTextReset();

            initOn = true;
        }
    }

    void SecTimeOn()
    {
        GiftAddDelayCheck();
    }

    public void SetBuilding(List<Building> _buildingList)
    {
        this.allBuildingList = _buildingList;

        foreach (var building in allBuildingList)
        {
            if (HomeCheck(building))
            {
                homeBuildingList.Add(building);
            }   
        }

        foreach (var homeBuilding in homeBuildingList)
        {
            homeBuilding.buildingClickOn += BuildingClick;
        }
    }

    bool HomeCheck(Building building)
    {
        return building.building_Type == Building_Type.Small ||
               building.building_Type == Building_Type.Middle ||
               building.building_Type == Building_Type.Big;
    }

    public void BuildingClick(Building building)
    {
        if (!HomeCheck(building))
            return;

        if (gift_currentValue == 0)
        {
            WarningManager.Instance.WarningSet("구호물자가 부족합니다.");
            return;
        }

        if (useDelayOn)
            return;

        useDelayOn = true;
        useDelay = 1.5f;

        GiftOn(building , -1);
    }

    public void AllHomeBuildingGiftOn()
    {
        foreach (var homeBuild in homeBuildingList)
        {
            GiftOn(homeBuild, 0);
        }
    }

    void GiftOn(Building building, int addCnt)
    {
        SoundManager.Instance.PlaySe(SeEnum.Car_horn);
        SoundManager.Instance.PlaySe(SeEnum.Car_move);

        GiftCntAdd(addCnt);

        GameObject giftCarObj = Instantiate(Resources.Load("InGame/Item/CoopangObj"), coopangParant) as GameObject;
        
        Vector3 targetPos = building.transform.position;
        targetPos.y -= 1;
        RectTransform giftCarObjRect = giftCarObj.GetComponent<RectTransform>();

        Vector3 currentPos = giftCarObjRect.transform.position;

        giftCarObj.transform.position = new Vector3(currentPos.x, targetPos.y , currentPos.z);

        giftCarObj.transform.DOMoveX(targetPos.x, 2).OnComplete(() =>
        {
            GameObject supplyGaugeOBJ = Instantiate(Resources.Load("InGame/Item/SupplyGaugeOBJ"), transform) as GameObject;
            supplyGaugeOBJ.transform.position = new Vector3(targetPos.x, targetPos.y + 1.1f , targetPos.z);

            building.ReviceFood();

            giftCarObj.transform.rotation = Quaternion.Euler(0, 180, 0);

            giftCarObj.transform.DOMoveX(coopangParant.transform.position.x, 2).SetDelay(1).OnComplete(() =>
            {
                giftCarObj.gameObject.SetActive(false);
            });
        });   
    }

    public void GiftCntTextReset()
    {
        giftCnttext.text = string.Format("{0}/{1}", gift_currentValue, gift_maxValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (initOn == false || GameManager.Ins.resultOn || gift_currentValue == gift_maxValue)
            return;
        GiftUseDelayCheck();
        GiftAddDelayCheck();
    }

    void GiftUseDelayCheck() 
    {
        if (useDelayOn == false)
            return;

        useDelay -= Time.deltaTime;

        if (useDelay < 0 )
        {
            useDelay = 0;
            useDelayOn = false;
        }
    }

    void GiftAddDelayCheck()
    {
        giftAddCurrentDelay += Time.deltaTime * giftAddValue;

        if (giftAddCurrentDelay >= giftAddMaxDelay)
        {
            giftAddCurrentDelay = 0;
            GiftCntAdd(1);
        }
        FillImageReset();
    }

    public void GiftCntAdd(int addCnt)
    {
        int originCnt = gift_currentValue;

        gift_currentValue = Mathf.Min(gift_currentValue + addCnt, gift_maxValue);
        GiftCntTextReset();

        if (gift_currentValue == gift_maxValue)
        {
            giftAddCurrentDelay = giftAddMaxDelay;
            FillImageReset();
        }

        if (gift_currentValue > originCnt )
        {
            SoundManager.Instance.PlaySe(SeEnum.Building_enter);
        }
    }

    void FillImageReset()
    {
        float value = (float)giftAddCurrentDelay / giftAddMaxDelay;
        //fillImage.DOFillAmount(value, 1).SetEase(Ease.Linear);
        fillImage.fillAmount = value;
    }
}
