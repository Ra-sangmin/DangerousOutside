using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] Boss_Chapter_1_AI boss_Chapter_1_AI;
    [SerializeField] Boss_Chapter_2_AI boss_Chapter_2_AI;
    [SerializeField] Boss_Chapter_3_AI boss_Chapter_3_AI;

    // Start is called before the first frame update
    void Start()
    {
        if ((GameManager.Ins.selectStageId + 1) % 5 == 0)
        {
            int bossIndex = (GameManager.Ins.selectStageId + 1) / 5;
            if (bossIndex == 1)
            {
                boss_Chapter_1_AI.gameObject.SetActive(true);
                boss_Chapter_1_AI.Init();
            }
            else if (bossIndex == 2)
            {
                boss_Chapter_2_AI.gameObject.SetActive(true);
                boss_Chapter_2_AI.Init();
            }
            else if (bossIndex == 3)
            {
                boss_Chapter_3_AI.gameObject.SetActive(true);
                boss_Chapter_3_AI.Init();
            }


        }
    }

}
