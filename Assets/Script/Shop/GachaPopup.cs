using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaPopup : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DataSet(int status)
    {
        string animName = string.Empty;

        switch (status)
        {
            case 0: 
                animName = "Gacha_normal";
                break;
            case 1:
                animName = "Gacha_Rare";
                break;
            case 2:
                animName = "Gacha_Special";
                break;
        }

        anim.Play(animName);
    }

    public void CloseBtnClick()
    {
        Destroy(gameObject);
    }

}
