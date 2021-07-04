using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mobcast.Coffee.UI;
using UnityEngine;
using UnityEngine.UI;

public class TaxController : MonoBehaviour
{
    public List<Image> imageList = new List<Image>();
    public RectTransform balloonImage;
    public Text taxValueText;
    public Slider slider;

    /// <summary>
    /// 새로 추가한 변수들
    /// </summary>
    float currentTax;
    public int currentTax_int;
    int beforeCurrentTax_int;
    [SerializeField] float getTaxPerSec;
    [SerializeField] float officeTax;

    public RectTransform rtHandle;

    bool isStart;

    // Start is called before the first frame update
    void Start()
    {
        //isStart = !GameManager.Ins.tutorialOn && GameManager.Ins.selectStageId > 0;
    }

    /// <summary>
    /// 초기화
    /// </summary>
    public void Init()
    {
        isStart = !GameManager.Ins.tutorialOn && GameManager.Ins.selectStageId > 0;
        //isStart = true;

        if (isStart)
        {
            taxValueText.text = "3";
            currentTax = 3;
            beforeCurrentTax_int = 3;
            SetTaxSlider();
            HandleShack();
        }
    }



    private void Update()
    {
        if (GameManager.Ins.resultOn)
            return;
        
        if (isStart)
        {
            GetTaxOnTime();
            SetTaxSlider();
        }
    }

    /// <summary>
    /// 시간에 따라 세금 획득
    /// </summary>
    void GetTaxOnTime()
    {
        currentTax += (Time.deltaTime / getTaxPerSec);
        if(currentTax >= 10)
        {
            currentTax = 10;
        }
        currentTax_int = (int)currentTax;
        if(currentTax_int != beforeCurrentTax_int)
        {
            SetTaxCount();
        }
        beforeCurrentTax_int = currentTax_int;
    }
    /// <summary>
    /// 세금 슬라이더 값 설정
    /// </summary>
    void SetTaxSlider()
    {
        slider.value = currentTax / 10;
    }

    /// <summary>
    /// 현재 세금 값을 텍스트로 표현
    /// </summary>
    void SetTaxCount()
    {
        if (currentTax_int > beforeCurrentTax_int)
        {
            SoundManager.Instance.PlaySe(SeEnum.GetTax);
        }

        taxValueText.text = currentTax_int.ToString();
        StartCoroutine(GetTaxAnim());

        if (currentTax_int > 7)
        {
            balloonImage.transform.DOLocalMoveX(-75, 1);
            balloonImage.transform.rotation = Quaternion.Euler(0, 180, 0);
            taxValueText.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            balloonImage.transform.DOLocalMoveX(66, 1);
            balloonImage.transform.rotation = Quaternion.Euler(0, 0, 0);
            taxValueText.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    IEnumerator GetTaxAnim()
    {
        rtHandle.transform.DOScale(new Vector3(1.15f, 1.15f, 1), 0.3f);
        yield return new WaitForSeconds(0.3f);
        rtHandle.transform.DOScale(new Vector3(1f, 1f, 1), 0.3f);
    }

    void HandleShack()
    {
        rtHandle.transform.DORotate(new Vector3(0, 0, -4), 0.2f).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// 회사 터치 시 현재 세금에서 추가
    /// </summary>
    public void TouchOffice()
    {
        AddCost(officeTax);
    }

    /// <summary>
    /// 세금 증가 및 감소
    /// </summary>
    /// <param name="addValue"></param>
    public void AddCost(float addValue)
    {
        currentTax += addValue;
        SetTaxSlider();
    }

    public bool CheckCost(float value, bool mgsOn = true)
    {
        bool haveTax = false;

        if(currentTax - value >= 0)
        {
            haveTax = true;
        }
        else
        {
            haveTax = false;

            if (mgsOn)
            {
                WarningManager.Instance.WarningSet("세금이 부족합니다!!");
            }
        }

        return haveTax;
    }
}
