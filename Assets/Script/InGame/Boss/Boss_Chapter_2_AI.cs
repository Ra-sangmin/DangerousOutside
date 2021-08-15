using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Chapter_2_AI : Base_Boss_Chapter
{
    [SerializeField] Animator bossAnim;
    [SerializeField] Animator waveAnim;
    [SerializeField] Animator barAnim;

    // Start is called before the first frame update
    public override void Init()
    {
        StartCoroutine(StartOn());
    }

    IEnumerator StartOn()
    {
        barAnim.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();

        bossAnim.Play("Chapter2_boss_appear");
        waveAnim.Play("Chapter2_stage_wave_boss");

        yield return new WaitForSeconds(3);

        barAnim.gameObject.SetActive(true);
        barAnim.Play("Chapter2_boss_bar_appear");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    bossAnim.Play("Chapter2_boss_bump");
        //    waveAnim.Play("Chapter2_stage_wave_boss");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    int ranIndex = Random.Range(0, 3);

        //    string text = string.Empty;

        //    switch (ranIndex)
        //    {
        //        case 0: text = "C";
        //            break;
        //        case 1: text = "O";
        //            break;
        //        case 2: text = "N";
        //            break;
        //    }

        //    string animName = string.Format("Chapter2_boss_human{0}", text);

        //    bossAnim.Play(animName);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    barAnim.Play("Chapter2_boss_bar_down");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    bossAnim.Play("Chapter2_boss_skill");
        //    waveAnim.Play("Chapter2_stage_wave_boss");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    bossAnim.Play("Chapter2_boss_r2w");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    bossAnim.Play("Chapter2_boss_disappear");
        //    waveAnim.Play("Chapter2_stage_wave_boss");
        //}
    }

}
