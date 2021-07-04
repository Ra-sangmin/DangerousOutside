using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStageController : MonoBehaviour
{
    public ChallengeCntController challengeCntController;
    public StagePageController stagePageController;
    public SettingPopup settingPopup;
    [SerializeField] Animator mapAnim;
    [SerializeField] RectTransform l_Btn;
    [SerializeField] RectTransform r_Btn;
    [SerializeField] RectTransform enter_Btn;
    [SerializeField] RectTransform bgPanel;
    [SerializeField] RectTransform backBtn;

    // Start is called before the first frame update
    void Start()
    {
        EventSet();
        SoundManager.Instance.PlayBGM(BGMEnum.StageSelect);
        BtnActiveSet();
        backBtn.gameObject.SetActive(false);
    }

    void EventSet()
    {
        stagePageController.startEvent += GameStartOn;
    }

    public void SettingBtnClickOn()
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        settingPopup.gameObject.SetActive(true);
    }

    void GameStartOn()
    {
        //Debug.LogWarning("start ON1 " + GameManager.Ins.challengeCurrentCnt);

        //if (GameManager.Ins.challengeCurrentCnt <= 0)
          //  return;

        //challengeCntController.CountAddOn(-1);

        GameManager.Ins.StageStartOn(stagePageController.stageData.id);        
    }

    public void GameExitOn()
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mapAnim.Play("C1_to_C2_R");
            
            Debug.LogWarning("1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mapAnim.Play("C2_to_C1_R");
        }
    }

    int index = 0;

    public void NextBtnClick(bool nextOn)
    {
        index = nextOn ? index + 1 : index - 1;

        AllDeActive();

        if (index == 1)
        {
            mapAnim.Play("C1_to_C2_RR");
            bgPanel.anchoredPosition3D = new Vector2(-1080, 0);
        }
        else if (index == 0)
        {
            mapAnim.Play("C2_to_C1_RR");
            bgPanel.anchoredPosition3D = new Vector2(0, 0);
        }

        StartCoroutine(NextBtnClickOn());
        
    }

    IEnumerator NextBtnClickOn()
    {
        yield return new WaitForSeconds(2f);
        BtnActiveSet();
        enter_Btn.gameObject.SetActive(true);
    }

    void BtnActiveSet()
    {
        if (index == 0)
        {
            l_Btn.gameObject.SetActive(false);
            r_Btn.gameObject.SetActive(true);
        }
        else if (index == 1)
        {
            l_Btn.gameObject.SetActive(true);
            r_Btn.gameObject.SetActive(false);
        }
        else
        {
            l_Btn.gameObject.SetActive(true);
            r_Btn.gameObject.SetActive(true);
        }
    }

    void AllDeActive()
    {
        l_Btn.gameObject.SetActive(false);
        r_Btn.gameObject.SetActive(false);
        enter_Btn.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);
    }

    public void EnterBtnClick()
    {
        AllDeActive();

        if (index == 0)
        {
            mapAnim.Play("C1_toIngame_r");
        }
        else if (index == 1)
        {
            mapAnim.Play("C2_toIngame_r");
        }

        StartCoroutine(EntroBtnClickOn());
    }

    IEnumerator EntroBtnClickOn()
    {
        yield return new WaitForSeconds(1.5f);
        backBtn.gameObject.SetActive(true);

        stagePageController.selectStageOn = true;
    }


    public void BackBtnClick()
    {
        stagePageController.selectStageOn = false;

        AllDeActive();

        if (index == 0)
        {
            mapAnim.Play("C1_toIngame_r_back");
        }
        else if (index == 1)
        {
            mapAnim.Play("C2_toIngame_r_back");
        }

        StartCoroutine(BackBtnClickOn());
    }
    IEnumerator BackBtnClickOn()
    {
        yield return new WaitForSeconds(1.5f);

        BtnActiveSet();
        enter_Btn.gameObject.SetActive(true);
    }

}
