using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mobcast.Coffee.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoController : MonoBehaviour
{
    [SerializeField] RectTransform moveBg;
    [SerializeField] Text touchText;

    bool startFlagOn = false;

    bool down;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMEnum.Logo);
        //moveBg.DOLocalMoveY(-600, 30f).SetLoops(-1, LoopType.Yoyo).SetDelay(3);
        StartCoroutine(TextEffectDoing());
    }

    // Update is called once per frame
    void Update()
    {
        MoveImage();
    }

    void MoveImage()
    {
        Vector2 pos =  moveBg.anchoredPosition3D;

        if (down)
        {
            if (pos.y > -200)
            {
                pos.y -= 0.2f;
            }
            else
            {
                down = false;
            }
        }
        else
        {
            if (pos.y < 200)
            {
                pos.y += 0.2f;
            }
            else
            {
                down = true;
            }
        }

        moveBg.anchoredPosition3D = pos;
    }

    public void StartOn()
    {
        if (startFlagOn)
        {
            SoundManager.Instance.PlaySe(SeEnum.Touch);
            SceneManager.LoadScene("SelectStage");
        }
    }

    IEnumerator TextEffectDoing()
    {
        startFlagOn = false;

        yield return new WaitForSeconds(1);

        startFlagOn = true;

        /*
        while (true)
        {
            float animTime = 1.7f;
            touchText.DOFade(0.3f, animTime).SetEase(Ease.Linear);
            touchText.GetComponent<Outline>().DOFade(0.3f, animTime).SetEase(Ease.Linear);
            yield return new WaitForSeconds(animTime);

            animTime = 0.5f;
            touchText.DOFade(1f, animTime).SetEase(Ease.Linear);
            touchText.GetComponent<Outline>().DOFade(1f, animTime).SetEase(Ease.Linear);
            yield return new WaitForSeconds(animTime + 3f);

        }
        */
        
    }


}
