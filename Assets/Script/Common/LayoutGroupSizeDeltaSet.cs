using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LayoutGroupSizeDeltaSet : MonoBehaviour
{
    /// <summary>
    /// GridLayoutGroup 높이 설정
    /// </summary>
    public void GridSizeDeltaSet()
    {
        GridLayoutGroup glg = transform.GetComponent<GridLayoutGroup>();
        SetSizeDelta(glg, glg.spacing.y);
    }

    /// <summary>
    /// VerticalLayoutGroup 높이 설정
    /// </summary>
    public void VerticalSizeDeltaSet()
    {
        VerticalLayoutGroup vlg = transform.GetComponent<VerticalLayoutGroup>();
        SetSizeDelta(vlg, vlg.spacing);
    }

    /// <summary>
    /// sizeDelta 세팅하기
    /// </summary>
    /// <param name="lg"></param>
    /// <param name="spacing"></param>
    void SetSizeDelta(LayoutGroup lg , float spacing)
    {
        //자식들의 총 높이 취득
        float heightValue = GetChildHeight(lg, spacing);

        //padding의 값 추가
        heightValue += lg.padding.top + lg.padding.bottom;

        RectTransform lgRect = lg.GetComponent<RectTransform>();
        lgRect.sizeDelta = new Vector2(lgRect.sizeDelta.x, heightValue);
    }

    /// <summary>
    /// 자식들의 총 높이 취득
    /// </summary>
    /// <param name="lg"></param>
    /// <param name="spacing"></param>
    /// <returns></returns>
    float GetChildHeight(LayoutGroup lg , float spacing)
    {
        List<float> rowHeightList = new List<float>();

        float beforeValue = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform childRect = transform.GetChild(i).GetComponent<RectTransform>();

            if (lg.GetType() == typeof(VerticalLayoutGroup))
            {
                rowHeightList.Add(childRect.sizeDelta.y);
            }
            else if (lg.GetType() == typeof(GridLayoutGroup))
            {
                //이전 Y위치와 다르다면 높이값 추가
                if (beforeValue != childRect.anchoredPosition3D.y)
                {
                    beforeValue = childRect.anchoredPosition3D.y;
                    rowHeightList.Add(childRect.sizeDelta.y);
                }
            }
        }

        //자식들의 총 높이 계산
        float childHeight = rowHeightList.Sum();

        //자식들의 총 높이에서 spacing 값 더하기
        if (rowHeightList.Count > 0)
            childHeight += (rowHeightList.Count - 1) * spacing;
        
        return childHeight;
    }
}
