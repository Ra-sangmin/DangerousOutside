using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter_3 : BaseChapter
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ClearEventOn()
    {
        base.ClearEventOn();

        int clearIndex = GameManager.Ins.selectStageId;

        if (clearIndex == 13)
        {
            bossAnim.Play("C3_start_w2r");
        }
        else if (clearIndex == 14)
        {
            bossAnim.Play("C3_r2b");
        }
        else
        {
            bossAnim.Play("C3_start_walk");
        }
    }

}
