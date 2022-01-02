using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Chapter_1_AI : Base_Boss_Chapter
{
    [SerializeField] BossAI bossAI;
    [SerializeField] BossAI miniBossAIPrefab;

    public override void Init()
    {
        StartCoroutine(StartOn());
    }

    IEnumerator StartOn()
    {
        yield return new WaitForEndOfFrame();

        bossAI.skillEndOn = CreateMiniBoss;
        bossAI.currentTile = GameManager.Ins.tileController.GetTile(new Vector2(3, 3));
        bossAI.BossMove(0, false);

        yield return new WaitForEndOfFrame();

        bossAI.bossAnim.gameObject.SetActive(true);
        bossAI.bossAnim.Play("Chapter1_boss_appear_anim");

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(bossAI.GetAnimTime());

        yield return bossAI.SkillPlayOn();

        bossAI.initOn = true;
    }

    void CreateMiniBoss()
    {
        BossAI miniBossAI = Instantiate(miniBossAIPrefab, transform);
        miniBossAI.currentTile = bossAI.currentTile;
        miniBossAI.BossMove(0, false);
        miniBossAI.bossAnim.gameObject.SetActive(true);
        miniBossAI.initOn = true;
        miniBossAI.MoveDoing();
        miniBossAI.ClickCntReset();
    }
}
