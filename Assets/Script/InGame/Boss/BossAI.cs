using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    Vector2 pos = new Vector2(2, 2);

    [SerializeField] Image bgImage;
    [SerializeField] Sprite sprite_0;
    [SerializeField] Sprite sprite_1;
    [SerializeField] Sprite sprite_2;

    // Start is called before the first frame update
    void Start()
    {
        MoveOn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            pos.x -= 1;
            MoveOn();
            bgImage.sprite = sprite_0;
            bgImage.SetNativeSize();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            pos.x += 1;
            MoveOn();
            bgImage.sprite = sprite_0;
            bgImage.SetNativeSize();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            pos.y += 1;
            MoveOn();
            bgImage.sprite = sprite_1;
            bgImage.SetNativeSize();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            pos.y -= 1;
            MoveOn();
            bgImage.sprite = sprite_0;
            bgImage.SetNativeSize();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            bgImage.sprite = sprite_2;
            bgImage.SetNativeSize();


            List<Vector2> newPos = new List<Vector2>()
            {
                new Vector2(pos.x+1 , pos.y),
                new Vector2(pos.x-1 , pos.y),
                new Vector2(pos.x , pos.y+1),
                new Vector2(pos.x , pos.y-1),
            };

            for (int i = 0; i < newPos.Count; i++)
            {
                Tile tile = GameManager.Ins.tileController.GetTile(newPos[i]);

                if (tile != null)
                {
                    tile.TileChange(Tile_Type.Red);
                }
            }
        }
    }

    void MoveOn()
    {
        Tile tile = GameManager.Ins.tileController.GetTile(pos);
        transform.DOMove(tile.transform.position, 1);
    }

}
