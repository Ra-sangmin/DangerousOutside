using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Chapter_2_AI : MonoBehaviour
{
    [SerializeField] Animator bossAnim;
    [SerializeField] Animator waveAnim;

    // Start is called before the first frame update
    public void Init()
    {
        StartCoroutine(StartOn());
    }

    IEnumerator StartOn()
    {
        yield return new WaitForEndOfFrame();

        Debug.LogWarning("2 스타트");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bossAnim.Play("Chapter2_boss");
            waveAnim.Play("Chapter2_stage_wave_boss");

            Debug.LogWarning("d");
        }
    }

}
