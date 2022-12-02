using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
   
	public float delay = 0.05f;
	public string fullText;
	private string currentText = "";
	private bool first = true;

	// Use this for initialization
	void Start()
	{
		StartCoroutine(ShowText());
			first = false;
	}


	IEnumerator ShowText()
	{

		for (int i = 0; i < fullText.Length; i++)
		{
			currentText = fullText.Substring(0, i);
			this.GetComponent<Text>().text = currentText.Replace("\\n", "\n");
			yield return new WaitForSeconds(delay);		
		}
	}

}
