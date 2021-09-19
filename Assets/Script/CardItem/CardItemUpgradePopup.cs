using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardItemUpgradePopup : MonoBehaviour
{
    Dictionary<CardGrade, int> cardCntDic = new Dictionary<CardGrade, int>();

    [SerializeField] Text normalCntText;
    [SerializeField] Text rareCntText;

    [SerializeField] StuffCard stuffCardPrefab;
    [SerializeField] RectTransform stuffCardParant;

    private List<StuffCard> stuffCardList = new List<StuffCard>();

    [SerializeField] Text lvText;
    [SerializeField] Text currentExpText;
    [SerializeField] Text maxExpText;
    [SerializeField] Image SliderFillImage;

    int currentExp = 205;

    LvDataManager lvDataManager = new LvDataManager();

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        cardCntDic = new Dictionary<CardGrade, int>()
        {
            { CardGrade.Normal, 8},
            { CardGrade.Rare, 3}
        };
        CntTextSet();

        stuffCardList = new List<StuffCard>();

        LvDataChange();

    }

    void CntTextSet()
    {
        foreach (var cardCnt in cardCntDic)
        {
            switch (cardCnt.Key) 
            {
                case CardGrade.Normal:
                    normalCntText.text = string.Format("{0}장", cardCnt.Value);
                    break;
                case CardGrade.Rare:
                    rareCntText.text = string.Format("{0}장", cardCnt.Value);
                    break;
                case CardGrade.Special:
                    //normalCntText.text = string.Format("{0}장", cardCnt.Value);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardAddBtnClick(int status)
    {
        CardGrade cardGrade = (CardGrade)status;

        if (cardCntDic[cardGrade] <= 0)
        {
            Debug.LogWarning("없음");
        }
        else
        {
            CreateCard(cardGrade);
        }
    }

    void CreateCard(CardGrade cardGrade)
    {
        cardCntDic[cardGrade] -= 1;
        CntTextSet();

        StuffCard stuffCard = Instantiate(stuffCardPrefab, stuffCardParant);
        stuffCard.DataSet(cardGrade);
        stuffCard.gameObject.SetActive(true);
        stuffCard.GetComponent<Button>().onClick.AddListener(() => CardRemoveBtnClick(stuffCard));

        stuffCardList.Add(stuffCard);

        LvDataChange();
    }

    public void CardRemoveBtnClick(StuffCard stuffCard)
    {
        cardCntDic[stuffCard.cardGrade] += 1;
        CntTextSet();

        stuffCardList.Remove(stuffCard);
        Destroy(stuffCard.gameObject);

        LvDataChange();
    }

    public void LvDataChange()
    {
        int resultExp = currentExp + GetStuffCardExp();

        LvData lvData =lvDataManager.GetLvData(resultExp);
        lvText.text = string.Format("Lv.{0}", lvData.lv);

        LvData beforeLvData = lvDataManager.GetLvDataFromLv(lvData.lv-1);

        int currentLvExp = resultExp - beforeLvData.maxExp;
        string currentExpStr = currentLvExp.ToString();
        currentExpText.text = currentExpStr;
        maxExpText.text = lvData.addExp.ToString();

        float fillAmount = (float)currentLvExp / lvData.addExp;
        SliderFillImage.fillAmount = fillAmount;
    }

    int GetStuffCardExp()
    {
        int resultExp = 0;
            
        for (int i = 0; i < stuffCardList.Count; i++)
        {
            switch (stuffCardList[i].cardGrade)
            {
                case CardGrade.Normal:
                    resultExp += 5;
                    break;
                case CardGrade.Rare:
                    resultExp += 10;
                    break;
                case CardGrade.Special:
                    break;
            }
        }

        return resultExp;
    }

    public void PopupCloseClickOn()
    {
        gameObject.SetActive(false);
    }
}

public enum CardGrade
{
    Normal = 0,
    Rare,
    Special,
}

public class LvData
{
    public int lv = 1;
    public int maxExp = 0;
    public int addExp = 0;

    public LvData(int lv , int maxExp , int addExp)
    {
        this.lv = lv;
        this.maxExp = maxExp;
        this.addExp = addExp;
    }
}

public class LvDataManager
{
    List<LvData> LvDataList = new List<LvData>();

    public LvDataManager()
    {
        Init();
    }

    void Init()
    {
        LvDataList = new List<LvData>()
        {
            new LvData(0,0,0),
            new LvData(1,50,0),
            new LvData(2,125,75),
            new LvData(3,225,100),
            new LvData(4,350,125),
            new LvData(5,500,150),
            new LvData(6,675,175)
        };
    }

    public LvData GetLvData(int exp)
    {
        foreach (LvData data in LvDataList)
        {
            if (data.maxExp > exp)
            {
                return data;
            }
        }

        return null;
    }

    public LvData GetLvDataFromLv(int lv)
    {
        foreach (LvData data in LvDataList)
        {
            if (data.lv == lv)
            {
                return data;
            }
        }

        return null;
    }
}