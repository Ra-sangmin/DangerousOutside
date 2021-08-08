using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Chapter_3_AI : MonoBehaviour
{
    [SerializeField] Animator bossAnim;

    // Start is called before the first frame update
    public void Init()
    {
        StartCoroutine(StartOn());
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
            bossAnim.Play("C3_clear");
        }
    }

}
