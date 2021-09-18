using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBar : MonoBehaviour
{
    public int mainTabStatus = 1;

    [SerializeField] List<RectTransform> mainMenuPanelList = new List<RectTransform>();
    [SerializeField] List<RectTransform> mainMenuBtnList = new List<RectTransform>();

    [SerializeField] RectTransform selectOnImage;

    public SettingPopup settingPopup;

    // Start is called before the first frame update
    void Start()
    {
        MainTabBtnClick(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void MainTabBtnClick(int status)
    {
        mainTabStatus = status;
        MainTapChange();
    }

    void MainTapChange()
    {
        for (int i = 0; i < mainMenuPanelList.Count; i++)
        {
            mainMenuPanelList[i].gameObject.SetActive(mainTabStatus == i);
        }

        selectOnImage.transform.position = mainMenuBtnList[mainTabStatus].transform.position;

    }


    public void SettingBtnClickOn()
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        settingPopup.gameObject.SetActive(true);
    }

}
