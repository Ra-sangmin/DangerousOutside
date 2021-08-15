using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter_1 : BaseChapter
{
    [SerializeField] Animator lightAnim;
    [SerializeField] RectTransform lightObj;
    
    [SerializeField] List<Animator> activeTileAnimList = new List<Animator>();
    float activeTileDelay = 0;

    protected int beforeIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void ChapterLoadReadyOn()
    {
        base.ChapterLoadReadyOn();
        lightObj.transform.DOLocalRotate(new Vector3(0, 0, GetLightRotValue(GameManager.Ins.beforeSelectStageId+1)), 0).SetEase(Ease.Linear);
    }

    protected override void ClearEventOn()
    {
        StartCoroutine(SetLightRot());
    }

    float GetLightRotValue(int index)
    {
        float value = -230f;

        //빛이 회전해야될 각도 
        switch (index)
        {
            case 0:
                value = 50f;
                break;
            case 1:
                value = -20f;
                break;
            case 2:
                value = -90f;
                break;
            case 3:
                value = -160f;
                break;
            case 4:
                value = -230f;
                break;
        }
        return value;
    }

    IEnumerator SetLightRot()
    {
        if (clearIndex < 4)
        {
            float rotValue = GetLightRotValue(clearIndex+1);

            //빛 움직임 준비
            lightAnim.Play("Chapter1_Light_begin_anim");
            yield return new WaitForSeconds(1);

            //빛 움직임 시작
            lightAnim.Play("Chapter1_Light_moving_anim");
            lightObj.DOLocalRotate(new Vector3(0, 0, rotValue), 1).SetEase(Ease.Linear);

            yield return new WaitForSeconds(1f);

            //빛 움직임 끝
            lightAnim.Play("Chapter1_Light_end_aim");
        }

        yield return new WaitForSeconds(1f);

        if (clearIndex == 3)
        {
            bossAnim.Play("Stage1_Silhouette");
        }
        if (clearIndex == 4)
        {
            bossAnim.Play("Chapter1_Silhouette_clear");
        }
        else
        {
            bossAnim.Play("Chapter1_Silhouette_default");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ActiveTileDelayCheck();
    }

    void ActiveTileDelayCheck()
    {
        activeTileDelay -= Time.deltaTime;

        if (activeTileDelay < 0)
        {
            activeTileDelay = 3;

            int ran = Random.Range(0, activeTileAnimList.Count);
            activeTileAnimList[ran].Play("Stage1_bg");
        }
    }


}
