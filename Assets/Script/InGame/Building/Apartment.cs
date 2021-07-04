using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Apartment : Building
{
    [SerializeField] Slider foodSlider;

    public float currentFoodCnt;
    public float maxFoodCnt;

    float sliderValue
    {
        get
        {
            return currentFoodCnt / (float)maxFoodCnt;
        }
    } 

    private void Awake()
    {
        TimeManager.Ins.secOn += SecTimeOn;
    }
    private void OnDestroy()
    {
        TimeManager.Ins.secOn -= SecTimeOn;
    }

    public override void Init(BuildingSaveData buildingSaveData)
    {
        base.Init(buildingSaveData);

        DataSet();

        FoodSliderReSet();

        initOn = true;
    }

    void DataSet()
    {
        switch (building_Type)
        {
            case Building_Type.Small:
                maxFoodCnt = 10;
                currentFoodCnt = 10;
                maxCitizenCnt = 10;
                break;
            case Building_Type.Middle:
                maxFoodCnt = 30;
                currentFoodCnt = 30;
                maxCitizenCnt = 20;
                break;
            case Building_Type.Big:
                maxFoodCnt = 100;
                currentFoodCnt = 40;
                maxCitizenCnt = 40;
                break;
        }
    }

    int GetCitizenCnt()
    {
        return citizenList.Count;
    }

    void SecTimeOn()
    {
        if (initOn == false)
            return;

        float addValue = GetCitizenCnt() * 0.05f;

        AddFood(-addValue);
    }

    public void AddFood(float addValue)
    {
        currentFoodCnt = Mathf.Max(currentFoodCnt + addValue, 0);
        FoodSliderReSet();

        //현재 구호물품 기준 수용가능 인원 체크
        if (CheckMaxCitizenCnt() == false)
        {
            OutCitizen();
        }
    }

    public void FoodSliderReSet()
    {
        foodSlider.DOValue(sliderValue, 1).SetEase(Ease.Linear);
    }

    /// <summary>
    /// 현재 구호물품 기준 수용가능 인원 체크
    /// </summary>
    /// <returns> 구호물품 대비 인원 수용가능 Flag , true -> 가능 , false -> 불가ㄴ</returns>
    public bool CheckMaxCitizenCnt()
    {
        //현재 구호물품 기준 수용가능 인원
        int canCitizenCnt = (int)(maxCitizenCnt * sliderValue);
        return canCitizenCnt >= citizenList.Count;
    }

    public override bool CheckCanInCitizen()
    {
        //현재 구호물품 기준 수용가능 인원
        int canCitizenCnt = (int)(maxCitizenCnt * sliderValue);
        return canCitizenCnt-1 >= citizenList.Count;
    }

    public void OutCitizen()
    {
        if (citizenList.Count == 0)
            return;
        
        int ranIndex = Random.Range(0, citizenList.Count-1);
        Citizen citizen = citizenList[ranIndex];

        Exit(citizen);
    }

    public override void ReviceFood()
    {
        int addFood = (int)(maxFoodCnt *0.5f);
        currentFoodCnt = Mathf.Min(currentFoodCnt + addFood, maxFoodCnt);
        FoodSliderReSet();
    }
}
