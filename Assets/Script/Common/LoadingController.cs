using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingOn());
    }

    IEnumerator LoadingOn()
    {
        yield return new WaitForSeconds(2);

        anim.Play("Loading_end");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(GameManager.Ins.targetScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
