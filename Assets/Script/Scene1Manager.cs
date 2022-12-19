using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene1Manager : MonoBehaviour
{
    public Text tx;
    private string m_text = "Game Start!";
    static int scene1_var;

    private void Start()
    {
        if (scene1_var == 0)
        {
            tx.transform.parent.gameObject.SetActive(true);
            scene1_var++;
            StartCoroutine(_typing());

        }
        
    }
    private void Update()
    {
       
    }
    IEnumerator _typing()
    {
       
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < m_text.Length; i++)
        {
            tx.text = m_text.Substring(0, i + 1);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.2f);
        tx.text = "";
        yield return new WaitForSeconds(0.2f);
        tx.text = m_text;
        yield return new WaitForSeconds(0.2f);
        tx.text = "";
        yield return new WaitForSeconds(0.2f);
        tx.text = m_text;
        yield return new WaitForSeconds(2f);
        tx.transform.parent.gameObject.SetActive(false);
        yield break;

    }
   
}
