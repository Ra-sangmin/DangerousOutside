using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseChapter : MonoBehaviour
{
    public int chapterIndex;
    public Animator bossAnim;
    [SerializeField] StageIcon stageIconPrefab;
    [SerializeField] RectTransform stageIconPanel;
    protected List<StageIcon> stageIconList = new List<StageIcon>();

    public UnityAction<StageIconData> clickEvent;

    protected int clearIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init()
    {
        StageIconCreate();
    }

    void StageIconCreate()
    {
        if (stageIconPanel == null || stageIconPrefab == null)
            return;

        for (int z = 0; z < 5; z++)
        {
            if (z >= stageIconPanel.childCount)
                continue;

            Transform parantObj = stageIconPanel.GetChild(z);

            StageIcon stageIcon = Instantiate(stageIconPrefab, parantObj);

            int stageId = (chapterIndex * 5) + z;

            StageIconData stageData = StageIconDataManager.Ins.GetStageData(stageId);
            stageIcon.DataSet(stageData, clickEvent);

            stageIconList.Add(stageIcon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ChapterLoadReadyOn()
    {
        if (GameManager.Ins.clearOn)
        {
            clearIndex = GameManager.Ins.selectStageId;

            foreach (var stageIcon in stageIconList)
            {
                if (stageIcon.stageData.id == clearIndex)
                {
                    stageIcon.StarAllInactive();
                }   
            }

        }
            
    }

    public virtual void ChapterDataSet()
    {
        if (GameManager.Ins.clearOn == false)
            return;

        GameManager.Ins.clearOn = false;

        ClearEventOn();

        foreach (var stageIcon in stageIconList)
        {
            if (stageIcon.stageData.id == clearIndex)
            {
                stageIcon.ClearEventOn();
            }
        }
    }

    protected virtual void ClearEventOn()
    {
        
    }
}
