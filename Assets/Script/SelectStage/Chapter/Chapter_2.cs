using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter_2 : BaseChapter
{
    [SerializeField] Animator bgAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void ClearEventOn()
    {
        base.ClearEventOn();

        int clearIndex = GameManager.Ins.selectStageId;

        if (clearIndex == 8)
        {
            bgAnim.Play("Fog_all");
            bossAnim.Play("duck_g2r");
        }
        else if (clearIndex == 9)
        {
            bgAnim.Play("Fog_X");
            bossAnim.Play("duck_r2y");
        }
        else
        {
            bgAnim.Play("Fog_all");
            bossAnim.Play("duck_gray");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
