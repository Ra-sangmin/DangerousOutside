using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Chapter_2_AI : Base_Boss_Chapter
{
    [SerializeField] TileController tileController;

    [SerializeField] Animator bossAnim;
    [SerializeField] Animator waveAnim;
    [SerializeField] Animator barAnim;

    bool waveDelayOn = false;
    float waveDelay = 0;
    int waveType = 0;

    public bool barigateOpen = false;

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

        waveDelay = 10;
        waveDelayOn = true;
    }

    private void Update()
    {
        WaveDelayCheck();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            barAnim.Play("Chapter2_boss_bar_down");
            barigateOpen = true;
        }

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

    void WaveDelayCheck()
    {
        if (waveDelayOn == false)  
            return;

        waveDelay -= Time.deltaTime;

        if (waveDelay < 0)
        {
            waveDelay = 10;
            StartCoroutine(WavePlayOn());
        }
    }

    IEnumerator WavePlayOn()
    {
        string animName = "Chapter2_stage_wave_boss";
        waveAnim.Play(animName);

        string bossAnimName = barigateOpen ? "Chapter2_boss_appear" : "Chapter2_boss_bump";
        bossAnim.Play(bossAnimName);

        yield return new WaitForSeconds(1);

        if (barigateOpen)
        {
            CitizenSaveData citizenData = new CitizenSaveData();

            citizenData.citizen_Type = Citizen_Type.Tourist;
            citizenData.pos.x = 4;
            citizenData.pos.y = 0;
            citizenData.citizenColor = CitizenColor.Red;

            tileController.CitizenCreate(citizenData);
        }

        

        Debug.LogWarning("clear");
    }

    
}
