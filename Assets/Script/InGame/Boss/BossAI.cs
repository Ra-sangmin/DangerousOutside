using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    public Animator bossAnim;
    public Tile currentTile;
    public bool initOn = false;
    public bool stopOn = false;
    private bool moveOn = false;
    private float delayTime = 2;
    private float skillDelay = 5;

    public UnityAction skillEndOn;

    public bool isMini = false;

    [SerializeField] Slider clickCntSlider;
    [SerializeField] Text clickCntText;
    private int clickCnt = 10;
    private int clickMaxCnt = 10;

    private void Awake()
    {
        bossAnim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public IEnumerator SkillPlayOn()
    {
        moveOn = true;

        bossAnim.Play("Chapter1_boss_side_skill_using_anim");

        Tile tempCurrentTile = currentTile;

        if (isMini)
        {
            yield return BossSkillMoveOn(tempCurrentTile.pos);
            yield return BossSkillMoveOn(tempCurrentTile.pos , false);
            moveOn = true;
        }
        else
        {
            yield return BossSkillMoveOn(new Vector2(tempCurrentTile.pos.x - 1, tempCurrentTile.pos.y));
            yield return BossSkillMoveOn(new Vector2(tempCurrentTile.pos.x + 1, tempCurrentTile.pos.y));
            yield return BossSkillMoveOn(new Vector2(tempCurrentTile.pos.x, tempCurrentTile.pos.y - 1));
            yield return BossSkillMoveOn(new Vector2(tempCurrentTile.pos.x, tempCurrentTile.pos.y + 1));

            yield return BossSkillMoveOn(tempCurrentTile.pos, false, 0);
        }

        bossAnim.Play("Chapter1_boss_side_skill_end_anim");

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(GetAnimTime());

        bossAnim.Play("Chapter1_boss_side_stop_anim");

        delayTime = 2;
        moveOn = false;
        skillDelay = 5;

        if (skillEndOn != null)
        {
            skillEndOn();
        }
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

    public void BossMove(float moveTime = 1, bool tileReset = true, bool animSkip = false)
    {
        float curentY = bossAnim.transform.position.y;

        Vector3 targetPos = currentTile.transform.position;

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

    void MiniBossDestroy()
    {
        Destroy(gameObject);
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

    public float GetAnimTime()
    {
        return bossAnim.GetCurrentAnimatorStateInfo(0).length;
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

    public void MoveDoing()
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
            if (nodePos.x >= 0 && nodePos.y >= 0 && nodePos.x < GameManager.Ins.tileController.x_max_value && nodePos.y < GameManager.Ins.tileController.y_max_value &&
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

    public void ClickOn()
    {
        clickCnt = Mathf.Max(--clickCnt , 0);
        ClickCntReset();

        if (clickCnt == 0)
        {
            MiniBossDestroy();
        }
    }

    public void ClickCntReset()
    {
        clickCntText.text = clickCnt.ToString();

        float sliderValue = clickCnt / (float)clickMaxCnt;
        clickCntSlider.value = sliderValue;
    }

}
