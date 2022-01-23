using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Citizen : MonoBehaviour
{
    [System.NonSerialized] public Building home = null;
    public CitizenColor citizen_color = CitizenColor.White;
    public Citizen_Type citizen_Type = Citizen_Type.Normal;
    [SerializeField] Animator animator;
    [SerializeField] SpeechBubble speechBubble;
    CitizenAnimState animState = CitizenAnimState.Stop;
    TileController tileController;

    [System.NonSerialized] public bool cleanerOn;
    bool initOn;
    bool moveOn;
    public bool waitOn;

    Queue<Tile> targetMoveList = new Queue<Tile>();
    [System.NonSerialized] public Tile currentTile;

    float currentMoveDelay;
    float moveDelay = 5;
    float moveSpeed = 1f;

    Astar_SM astar;

    bool goHome;
    Tweener moveTween;

    // Start is called before the first frame update
    void Start()
    {
        TileCheck();
    }

    public Citizen(Citizen_Type citizen_Type, Building home = null,CitizenColor citizenColor = CitizenColor.White)
    {
        this.citizen_Type = citizen_Type;
        this.home = home;
        this.citizen_color = citizenColor;
    }

    public void Init(TileController tileController, Tile currentTile)
    {
        this.tileController = tileController;
        this.currentTile = currentTile;

        astar = new Astar_SM(tileController);

        ResetPos(currentTile);
        AnimReset(CitizenAnimState.Stop);

        currentMoveDelay = 2;
            
        moveSpeed = 3f;

        initOn = true;
    }

    void AnimReset(CitizenAnimState animState)
    {
        this.animState = animState;

        bool stop = this.animState == CitizenAnimState.Stop;
        bool back = this.animState == CitizenAnimState.Back;
        bool side = this.animState == CitizenAnimState.Side;

        animator.SetBool("stop", stop);
        animator.SetBool("back", back);
        animator.SetBool("side", side);
    }

    public void BanAreaOn(Tile tile)
    {
        moveTween.Kill();
        moveOn = true;
        waitOn = true;

        Debug.LogWarning(currentTile.pos.x + " , " + currentTile.pos.y);
        StartCoroutine(BanAreaOnCoroutine(tile));
    }

    IEnumerator BanAreaOnCoroutine(Tile tile)
    {
        yield return new WaitForEndOfFrame();
        ResetPos(tile);
    }

    public void ResetPos(Tile tile)
    {
        RectTransform rect = tile.transform.GetComponent<RectTransform>();

        Transform originParant = this.transform.parent;

        this.transform.SetParent(tile.transform);
        this.transform.localPosition = Vector3.zero;
        this.transform.SetParent(originParant);

        float size = rect.sizeDelta.x;

        transform.localScale = Vector3.one * (size / 105);

        currentTile = tile;
    }

    public void ChangeColor(CitizenColor newColor)
    {
        citizen_color = newColor;
        ChangeColorAnimator(newColor);
    }

    void ChangeColorAnimator(CitizenColor color)
    {
        switch (color)
        {
            case CitizenColor.White:
                animator.SetBool("white", true);
                animator.SetBool("red", false);
                animator.SetBool("blue", false);
                break;
            case CitizenColor.Red:
                animator.SetBool("white", false);
                animator.SetBool("red", true);
                animator.SetBool("blue", false);
                break;
            case CitizenColor.Blue:
                animator.SetBool("white", false);
                animator.SetBool("red", false);
                animator.SetBool("blue", true);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (initOn == false || GameManager.Ins.resultOn || GameManager.Ins.tutorialOn)
            return;

        MoveTimeCheck();
    }

    void MoveTimeCheck()
    {
        if (moveOn || waitOn)
            return;

        if (targetMoveList != null && targetMoveList.Count > 0)
        {
            MoveDoing(targetMoveList.Dequeue(),()=> moveOn = false);
        }
        else
        {
            if (goHome)
            {
                home.Enter(new Citizen(citizen_Type, home, citizen_color));
                goHome = false;
                Destroy(gameObject);
            }
            else
            {
                AnimReset(CitizenAnimState.Stop);

                currentMoveDelay -= Time.deltaTime;

                if (currentMoveDelay <= 0)
                {
                    currentMoveDelay = moveDelay;
                    PathSet();
                }
            }
        }

    }

    void PathSet(bool ForseHome = false)
    {
        Tile tempTile = null;

        if (home != null && (ForseHome || home.CheckCanInCitizen()))
        {
            tempTile = home.tile;
            goHome = true;
        }
        else
        {
            tempTile = TargetTileGet(); 
        }

        if (tempTile == null)
        {
            moveOn = false;
            return;
        }

        targetMoveList = new Queue<Tile>();

        Node startNode = new Node(currentTile.pos);
        Node endNode = new Node(tempTile.pos);

        List<Node> pathList = astar.GetPathList(startNode, endNode);
        
        foreach (var path in pathList)
        {
            targetMoveList.Enqueue(tileController.tiles[(int)path.X, (int)path.Y]);
        }

        if (targetMoveList.Count == 0)
            return;
        
        MoveDoing(targetMoveList.Dequeue(),()=> moveOn = false);

        if (tempTile.tile_Type == Tile_Type.Red)
        {
            speechBubble.ShowSpeechBubble(EmoticonType.Question, citizen_color == CitizenColor.Red);
        }
        else if (goHome)
        {
            speechBubble.ShowSpeechBubble(EmoticonType.House, citizen_color == CitizenColor.Red);
        }
        else
        {
            speechBubble.ShowSpeechBubble(EmoticonType.Walk, citizen_color == CitizenColor.Red);
        }
    }

    public void MoveDoing(Tile targetTile,TweenCallback completeEvent)
    {
        moveTween.Kill();

        moveOn = true;

        currentTile = targetTile;

        Vector2 curentPos = transform.transform.position;
        Vector2 targetPos = currentTile.transform.position;

        bool moveUpOn = curentPos.y < targetPos.y;
        bool moverightOn = curentPos.x < targetPos.x;

        CitizenAnimState animState = moveUpOn ? CitizenAnimState.Back : CitizenAnimState.Side;
        AnimReset(animState);

        Quaternion quaternion = moverightOn == true ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(Vector3.zero);
        animator.transform.rotation = quaternion;

        moveTween = transform.DOMove(currentTile.transform.position, moveSpeed)
                                .SetEase(Ease.Linear)
                                .OnComplete(completeEvent);
        Observable
            .Timer(TimeSpan.FromSeconds(moveSpeed * 0.5f))
            .Subscribe(_ => TileCheck())
            .AddTo(gameObject);

    }

    public void GoHome()
    {
        PathSet(true);
    }

    Tile TargetTileGet()
    {
        Tile targetTile = null;

        if (citizen_color != CitizenColor.Red)
        {
            targetTile = GetRedTile();
        }
        
        if (targetTile == null)
        {
            targetTile = GetRandomTitle();
        }

        return targetTile; 

    }

    Tile GetRedTile()
    {
        Tile targetTile = null;

        Vector2 pos = currentTile.pos;

        List<Vector2> nodePosList = new List<Vector2>()
        {
            new Vector2(pos.x +1 , pos.y),
            new Vector2(pos.x -1, pos.y),
            new Vector2(pos.x , pos.y +1),
            new Vector2(pos.x , pos.y -1)
        };

        foreach (var nodePos in nodePosList)
        {
            Tile tempTile = tileController.GetTile(nodePos);

            if (tempTile != null && tempTile.tile_Type == Tile_Type.Red)
            {
                targetTile = tempTile;
                break;
            }
        }

        return targetTile;
    }

    Tile GetRandomTitle()
    {
        Tile targetTile = null;

        List<Vector2> nodePosList = new List<Vector2>();

        for (int x = -5; x <= 5; x++)
        {
            for (int y = -5; y <= 5; y++)
            {
                Vector2 checkPos = Vector2.zero;
                checkPos = new Vector2(currentTile.pos.x + x, currentTile.pos.y + y);

                if (checkPos != currentTile.pos &&
                    checkPos.x >= 0 && checkPos.y >= 0 && checkPos.x < tileController.x_max_value && checkPos.y < tileController.y_max_value)
                {
                    nodePosList.Add(checkPos);
                }
            }
        }

        List<Vector2> checkList = new List<Vector2>();

        foreach (var nodePos in nodePosList)
        {
            if (nodePos.x >= 0 && nodePos.y >= 0 && nodePos.x < tileController.x_max_value && nodePos.y < tileController.y_max_value)
            {
                checkList.Add(nodePos);
            }
        }

        if (checkList.Count == 0)
        {
            targetTile = tileController.GetTile(currentTile.pos);
        }
        else
        {
            int ranIndex = Random.Range(0, checkList.Count);
            Vector2 targetPos = checkList[ranIndex];
            targetTile = tileController.GetTile(targetPos);
        }

        return targetTile;
    }

    List<Vector2> GetTileList(Vector2 startPos , Vector2 endPos)
    {

        return null;
    }

    public void TileCheck()
    {
        if (currentTile.tile_Type == Tile_Type.Blue)
        {
            if (citizen_color == CitizenColor.Red)
            {
                ChangeColor(CitizenColor.White);
                currentTile.TileChange(Tile_Type.White);
            }
            else if (citizen_color == CitizenColor.White)
            {
                ChangeColor(CitizenColor.Blue);
            }
        }
        else if (currentTile.tile_Type == Tile_Type.Red)
        {
            if (citizen_color == CitizenColor.Blue)
            {
                ChangeColor(CitizenColor.White);
            }
            else if (citizen_color == CitizenColor.White)
            {
                ChangeColor(CitizenColor.Red);
            }
        }
        else if (currentTile.tile_Type == Tile_Type.White)
        {
            if (citizen_color == CitizenColor.Red)
            {
                currentTile.TileChange(Tile_Type.Red);
            }
        }
    }
}
