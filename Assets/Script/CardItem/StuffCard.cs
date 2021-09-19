using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StuffCard : MonoBehaviour
{
    public CardGrade cardGrade;
    [SerializeField] Image image;
    [SerializeField] List<Sprite> spriteList = new List<Sprite>();

    public void DataSet(CardGrade cardGrade)
    {
        this.cardGrade = cardGrade;
        ImageSet();
    }

    void ImageSet()
    {
        image.sprite = spriteList[(int)cardGrade];
    }
}
