using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WarningManager : MonoBehaviour
{
	private static WarningManager instance;
	public static WarningManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<WarningManager>();

				if (instance == null)
				{
					instance = new GameObject("WarningManager").AddComponent<WarningManager>();
					GameObject.DontDestroyOnLoad(instance.gameObject);

					Canvas canvas = instance.gameObject.AddComponent<Canvas>();
					canvas.sortingOrder = 300;
				}
			}
			return instance;
		}
	}

	public void DemoWarningOn()
	{
		WarningSet("데모버젼에는 지원하지 않습니다.");
	}


    public void WarningSet(string msg)
    {
		GameObject obj = Instantiate(Resources.Load("Common/pnlWarning"),transform) as GameObject;
		Text textObj = obj.transform.Find("Text").GetComponent<Text>();
		textObj.text = msg;

		Vector2 pos = obj.transform.localPosition;
		pos.y = -200;
		obj.transform.localPosition = pos;

		obj.transform.DOLocalMoveY(100, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
			obj.transform.DOLocalMoveY(100, 1f).OnComplete(() =>
			{
				Destroy(obj);
			});
		});
	}
}
