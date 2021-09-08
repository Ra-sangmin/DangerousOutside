using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Mobcast.Coffee.UI;

public class CleanerController : BaseItem
{
    public TileController tileCont;
    [SerializeField] AtlasImage mainImage;
    public List<Cleaner> cleanerList = new List<Cleaner>();
    int cleanerMaxCnt = 5;
    public Text cntText;

    bool ambulanceOn = false;

    public GameObject ambulanceObj;

    public Animator congalAnim;
    public Animator curtainAnim;

    // Start is called before the first frame update
    void Start()
    {
        Init();        
    }

    public override void Init()
    {
        base.Init();

        if (activeOn)
        {
            cleanerList = new List<Cleaner>();

            for (int i = 0; i < cleanerMaxCnt; i++)
            {
                Cleaner cleaner = new Cleaner();
                cleaner.index = i;
                cleanerList.Add(cleaner);
            }
        }
    }

    public void CleanerClickOn()
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);

        if (ambulanceOn)
        {
            WarningManager.Instance.WarningSet("수송차가 사용중입니다.");
            return;
        }

        if (GetCleaningCnt() == cleanerMaxCnt)
        {
            WarningManager.Instance.WarningSet("세탁기가 가득 찼습니다.");
            return;
        }

        GetTarget();
    }

    void GetTarget()
    {
        Cleaner beFreeCleaner = null;

        foreach (var cleaner in cleanerList)
        {
            if (cleaner.citizen == null)
            {
                beFreeCleaner = cleaner;
                break;
            }
        }

        Debug.LogWarning(beFreeCleaner);

        if (beFreeCleaner != null)
        {
            foreach (var citizen in tileCont.citizenList)
            {
                if (citizen.cleanerOn == false && citizen.citizen_color == CitizenColor.Red)
                {
                    beFreeCleaner.CitizenOn(citizen);
                    GoAmbulance(beFreeCleaner);
                    break;
                }
            }
            
        }
    }


    void GoAmbulance(Cleaner cleaner)
    {
        SoundManager.Instance.PlaySe(SeEnum.Car_horn);
        SoundManager.Instance.PlaySe(SeEnum.Car_move);
        ambulanceOn = true;

        ambulanceObj.transform.rotation = Quaternion.Euler(0, 180, 0);

        Vector3 citizenPos = cleaner.citizen.transform.position;
        float xValue = -535;
        float yValue = citizenPos.y;
        ambulanceObj.transform.position = new Vector2(xValue, yValue);
        ambulanceObj.gameObject.SetActive(true);

        ambulanceObj.transform.DOMoveX(citizenPos.x, 2).OnComplete(()=>
        {
            cleaner.citizen.gameObject.SetActive(false);

            ambulanceObj.transform.rotation = Quaternion.Euler(0, 0, 0);

            ambulanceObj.transform.DOMoveX(xValue, 2).SetDelay(1).OnComplete(()=>
            {
                ambulanceOn = false;
                cleaner.SetState(CleanerState.Cleaning);
                ambulanceObj.gameObject.SetActive(false);
            });
        });
    }

    void Update()
    {
        CheckCleaning();
        InputCheck();
    }

    void InputCheck()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    congalAnim.gameObject.SetActive(true);
        //    congalAnim.Play("Congal_new");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
            
        //    congalAnim.Play("Congal_move");
        //    congalAnim.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(-74, 1.0f).SetDelay(0.8f);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    congalAnim.Play("Congal_move");
        //    congalAnim.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(-148, 1).SetDelay(0.8f);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    Vector3 pos = congalAnim.transform.parent.GetComponent<RectTransform>().anchoredPosition3D;
        //    pos.x = 0;
        //    congalAnim.transform.parent.GetComponent<RectTransform>().anchoredPosition3D = pos;

        //    congalAnim.Play("Congal_enter");
            
        //    curtainAnim.Play("Skill_hospital_curtain");
        //}
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            curtainAnim.Play("Skill_hospital_doctor_home2");
        }
        
    }

    void CheckCleaning()
    {
        if (activeOn == false || GameManager.Ins.tutorialOn)
            return;

        int cleaningCnt = GetCleaningCnt();
        cntText.text = string.Format("{0}    {1}", cleaningCnt, cleanerMaxCnt);
    }

    public int GetCleaningCnt()
    {
        int cleaningCnt = 0;

        foreach (var cleaner in cleanerList)
        {
            if (cleaner.cleanerState == CleanerState.Cleaning)
            {
                cleaner.takeTime += Time.deltaTime;

                if (cleaner.takeTime > 10)
                {
                    cleaner.SetState(CleanerState.CleaningEnd);
                    continue;
                }

                cleaningCnt++;
            }
        }

        return cleaningCnt;
    }

}
