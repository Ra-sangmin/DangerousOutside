using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] List<Base_Boss_Chapter> bossChpaterAIList = new List<Base_Boss_Chapter>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var bossChpaterAI in bossChpaterAIList)
        {
            bossChpaterAI.gameObject.SetActive(false);
        }

        if ((GameManager.Ins.selectStageId + 1) % 5 == 0)
        {
            int bossIndex = ((GameManager.Ins.selectStageId + 1) / 5) -1;

            if (bossIndex > -1)
            {
                bossChpaterAIList[bossIndex].gameObject.SetActive(true);
                bossChpaterAIList[bossIndex].Init();
            }
        }
    }

}
