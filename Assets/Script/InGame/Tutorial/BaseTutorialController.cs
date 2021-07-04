using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTutorialController : MonoBehaviour
{
    protected TileController tileController;
    protected ChatController chatController;
    [SerializeField] protected RectTransform tutorialObjPanel;
    [SerializeField] protected List<RectTransform> tutorialObjList = new List<RectTransform>();

    protected bool nextOn;

    // Start is called before the first frame update
    void Start()
    {
        //Init();
    }

    protected virtual void Init()
    {
        GameManager.Ins.tutorialOn = true;
        SoundManager.Instance.PlayBGM(BGMEnum.Tutorial);
        ClassSet();
        TileSet();
    }

    void ClassSet()
    {
        tileController = FindObjectOfType<TileController>();
        chatController = FindObjectOfType<ChatController>();
    }

    void TileSet()
    {
        List<TileSaveData> tileSaveDataList = new List<TileSaveData>();

        /*
        for (int x = 0; x < TileController.x_max_value; x++)
        {
            for (int y = 0; y < TileController.y_max_value; y++)
            {
                TileSaveData tileSaveData = new TileSaveData();
                tileSaveData.pos = new Vector2(x, y);

                if (y < 4 && (x <= 2 || x >= 12))
                {
                    tileSaveData.tile_Type = Tile_Type.None;
                }
                else
                {
                    tileSaveData.tile_Type = Tile_Type.White;
                }

                tileSaveDataList.Add(tileSaveData);
            }
        }
        */

        foreach (var tile in tileSaveDataList)
        {
            tileController.TileCreate(tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}