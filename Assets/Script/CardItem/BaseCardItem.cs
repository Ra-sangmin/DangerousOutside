using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCardItem : MonoBehaviour
{
    public CardColorEnum cardColorEnum;

    [SerializeField] Image bgImage;

    // Start is called before the first frame update
    void Start()
    {
        BGSet();
    }

    public void BGSet()
    {
        string bgName = string.Empty;

        switch (cardColorEnum)
        {
            case CardColorEnum.Blue:
                bgName = "BO_bgB";
                break;
            case CardColorEnum.Green:
                bgName = "BO_bgG";
                break;
            case CardColorEnum.Navy:
                bgName = "BO_bgN";
                break;
            case CardColorEnum.Pink:
                bgName = "BO_bgP";
                break;
            case CardColorEnum.Yellow:
                bgName = "BO_bgY";
                break;

        }

        Sprite sprite = Resources.Load<Sprite>("CardItemBG/" + bgName);
        bgImage.sprite = sprite;
    }

    void GetSprite()
    {

    }
}


public enum CardColorEnum
{
    Blue,
    Green,
    Navy,
    Pink,
    Yellow
}