using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Chapter_3_AI : Base_Boss_Chapter
{
    [SerializeField] Animator bossAnim;
    [SerializeField] Animator bossEndingAnim;

    // Start is called before the first frame update
    public override void Init()
    {
        StartCoroutine(StartOn());
        bossEndingAnim.gameObject.SetActive(false);
    }

    IEnumerator StartOn()
    {
        yield return new WaitForEndOfFrame();

        bossAnim.Play("C3_Boss_appear");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bossAnim.Play("C3_Boss_skill");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartCoroutine(EndOn());
        }
    }

    IEnumerator EndOn()
    {
        bossAnim.Play("C3_clear");

        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(()=> EndCheck(bossAnim, "C3_clear") );

    }

    bool EndCheck(Animator anim , string name)
    {
        bool endOn = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(name) == false || anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            endOn = true;
        }

        return endOn;
    }

}

