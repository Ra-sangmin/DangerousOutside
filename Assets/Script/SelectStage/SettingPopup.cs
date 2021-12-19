using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    [SerializeField] Toggle vibrationOnToggle;
    [SerializeField] Toggle vibrationOffToggle;

    [SerializeField] Text stageNumText;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        bgmSlider.value = SoundManager.Instance.bgmValue;
        seSlider.value = SoundManager.Instance.seValue;

        ToggleSet();
    }

    public void DataSet()
    {
        int stageNum = GameManager.Ins.selectStageId + 1;
        stageNumText.text = string.Format("스테이지 {0}", stageNum);
    }

    void ToggleSet()
    {
        if (GameManager.Ins.vibrationOn == true)
        {
            vibrationOnToggle.isOn = true;
        }
        else
        {
            vibrationOffToggle.isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public void BGMReset()
    {
        SoundManager.Instance.BgmValueReset(bgmSlider.value);
    }
    public void SeReset()
    {
        SoundManager.Instance.SeValueReset(seSlider.value);
    }

    public void VibrationOnSet(Toggle value)
    {
        if (value.isOn == false)
            return;

        GameManager.Ins.VibrationSet(true);
    }
    public void VibrationOffSet(Toggle value)
    {
        if (value.isOn == false)
            return;

        GameManager.Ins.VibrationSet(false);
    }

    public void VibrationChange()
    {
        GameManager.Ins.VibrationSet(!GameManager.Ins.vibrationOn);
        ToggleSet();
    }
}
