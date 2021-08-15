using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Chapter_1_AI : Base_Boss_Chapter
{
    [SerializeField] Animator bossAnim;
    [SerializeField] TileController tileController;
    private Tile currentTile;
    bool initOn = false;
    public bool stopOn = false;
    private bool moveOn = false;
    private float delayTime = 2;
    private float skillDelay = 5;

    public override void Init()
    {
        StartCoroutine(StartOn());
    }


    IEnumerator StartOn()
    {
        yield return new WaitForEndOfFrame();

        currentTile = GameManager.Ins.tileController.GetTile(new Vector2(3, 3));
        BossMove(0, false);

        bossAnim.gameObject.SetActive(true);
        bossAnim.Play("Chapter1_boss_appear_anim");

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(GetAnimTime());

        yield return SkillPlayOn();

        initOn = true;
    }

    IEnumerator SkillPlayOn()
    {
        moveOn = true;

        bossAnim.Play("Chapter1_boss_side_skill_using_anim");

        Tile tempCurrentTile = currentTile;

        yield return BossSkillMoveOn(new Vector2(tempCurrentTile.pos.x - 1, tempCurrentTile.pos.y));
        yield return BossSkillMoveOn(new Vector2(tempCurrentTile.pos.x + 1, tempCurrentTile.pos.y));
        yield return BossSkillMoveOn(new Vector2(tempCurrentTile.pos.x, tempCurrentTile.pos.y - 1));
        yield return BossSkillMoveOn(new Vector2(tempCurrentTile.pos.x, tempCurrentTile.pos.y + 1));

        yield return BossSkillMoveOn(tempCurrentTile.pos, false, 0);

        bossAnim.Play("Chapter1_boss_side_skill_end_anim");

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(GetAnimTime());

        bossAnim.Play("Chapter1_boss_side_stop_anim");

        delayTime = 2;
        moveOn = false;
        skillDelay = 5;
    }

    IEnumerator BossSkillMoveOn(Vector2 pos, bool tileCheckOn = true, float waitDelay = 0.85f)
    {
        stopOn = true;

        Tile tempTile = GameManager.Ins.tileController.GetTile(pos);

        if (tempTile != null)
        {
            currentTile = tempTile;
            BossMove(0, tileCheckOn, true);
            yield return new WaitForSeconds(waitDelay);
        }

        stopOn = false;
    }


    float GetAnimTime()
    {
        return bossAnim.GetCurrentAnimatorStateInfo(0).length;
    }

    void MoveDoing()
    {
        Tile tempTile = TargetTileGet();
        if (tempTile == null)
        {
            moveOn = false;
            return;
        }

        currentTile = tempTile;
        BossMove(tileReset: false);
    }

    void BossMove(float moveTime = 1, bool tileReset = true, bool animSkip = false)
    {
        float curentY = bossAnim.transform.position.y;

        Vector2 targetPos = currentTile.transform.position;
        targetPos.y -= 80;

        bool moveUp = targetPos.y > curentY;

        if (animSkip == false)
        {
            if (targetPos.y > curentY)
            {
                bossAnim.Play("Chapter1_boss_back_walk_anim");
            }
            else
            {
                bossAnim.Play("Chapter1_boss_side_anim");
            }
        }



        bossAnim.transform.DOMove(targetPos, moveTime).OnComplete(() =>
        {
            if (animSkip == false)
            {
                bossAnim.Play("Chapter1_boss_side_stop_anim");
            }

            if (tileReset)
            {
                TileReset();
            }

            moveOn = false;
        });
    }

    Tile TargetTileGet()
    {
        Vector2 pos = currentTile.pos;

        List<Vector2> nodePosList = new List<Vector2>()
        {
            new Vector2(pos.x +1 , pos.y),
            new Vector2(pos.x -1, pos.y),
            new Vector2(pos.x , pos.y +1),
            new Vector2(pos.x , pos.y -1)
        };

        List<Vector2> checkList = new List<Vector2>();

        foreach (var nodePos in nodePosList)
        {
            if (nodePos.x >= 0 && nodePos.y >= 0 && nodePos.x < tileController.x_max_value && nodePos.y < tileController.y_max_value &&
                GameManager.Ins.tileController.IsWall(nodePos) == false)
            {
                checkList.Add(nodePos);
            }
        }

        if (checkList.Count == 0)
        {
            return GameManager.Ins.tileController.GetTile(pos);
        }
        else
        {
            int ranIndex = Random.Range(0, checkList.Count);
            Vector2 targetPos = checkList[ranIndex];
            return GameManager.Ins.tileController.GetTile(targetPos);
        }
    }

    public void TileReset()
    {
        if (currentTile.tile_Type == Tile_Type.Blue)
        {
            currentTile.TileChange(Tile_Type.White);
        }
        else if (currentTile.tile_Type == Tile_Type.White)
        {
            currentTile.TileChange(Tile_Type.Red);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (initOn == false || stopOn)
            return;

        MoveTimeCheck();
        SkillDelayCheck();
    }

    void MoveTimeCheck()
    {
        if (moveOn)
            return;

        delayTime -= Time.deltaTime;

        if (delayTime < 0)
        {
            moveOn = true;
            delayTime = 2;
            MoveDoing();
        }
    }

    void SkillDelayCheck()
    {
        if (moveOn)
            return;

        skillDelay -= Time.deltaTime;

        if (skillDelay < 0)
        {
            skillDelay = 5;
            moveOn = true;
            StartCoroutine(SkillPlayOn());
        }
    }
}
