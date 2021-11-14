using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanManCustomTest : MonoBehaviour
{
    [SerializeField] Animator anim;

    [SerializeField] Image mask_image;
    [SerializeField] Image body_image;
    [SerializeField] Image cap_image;
    [SerializeField] Image cap_image2;
    [SerializeField] Image cloth_image;

    [SerializeField] List<Sprite> maskSpriteList = new List<Sprite>();
    [SerializeField] List<Sprite> bodySpriteList = new List<Sprite>();
    [SerializeField] List<Sprite> capSpriteList = new List<Sprite>();

    //[SerializeField] Image mask_image;
    //[SerializeField] Image mask_image;
    //[SerializeField] Image mask_image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimChange(int status)
    {
        if (status == 0)
        {
            anim.SetBool("back", false);
            anim.SetBool("side", true);
        }
        else if (status == 1) 
        {
            anim.SetBool("back", true);
            anim.SetBool("side", false);
        }
    }

    public void PartChange(int status)
    {
        mask_image.sprite = maskSpriteList[status];
        body_image.sprite = bodySpriteList[status];
        cap_image.sprite = capSpriteList[status];
        float alphaValue = capSpriteList[status] == null ? 0 : 1;
        cap_image.color = new Color(1, 1, 1, alphaValue);

        cap_image2.sprite = capSpriteList[status];
        cap_image2.color = new Color(1, 1, 1, alphaValue);
    }
}
