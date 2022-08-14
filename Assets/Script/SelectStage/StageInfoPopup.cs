using System.Collections;
using System.Collections.Generic;
using Mobcast.Coffee.UI;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoPopup : MonoBehaviour
{
    private StageIconData stageData;
    [SerializeField] Text stageIdText;
    [SerializeField] Text mission_0_Text;
    [SerializeField] Text mission_1_Text;

    [SerializeField] private List<Image> starIconList = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(StageIconData stageData, StageSaveData data)
    {
        this.stageData = stageData;

        int stageId = this.stageData.id +1;
        stageIdText.text = string.Format("스테이지 {0}", stageId);

        int min = (int)(data.minClearTime / 60);
        int sec = (int)(data.minClearTime % 60);

        mission_0_Text.text = string.Format("{0:d2}:{1:d2} 이내로 방역을 완료하세요!", min, sec);
        mission_1_Text.text = string.Format("누적 빨강 주민 인원 {0}명 이하로 클리어", data.minRedCitizen);

        for (int i = 0; i < starIconList.Count; i++)
        {
            starIconList[i].gameObject.SetActive(i < stageData.stageIconDataUserData.starCount);
        }
    }

}
