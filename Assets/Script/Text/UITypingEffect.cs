using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITypingEffect : MonoBehaviour
{
    public GameObject panel;
    public Text tx;
    private string m_text = "MOLMOL GAME\r\n\r\n PROTO ";

    private void Start()
    {
        StartCoroutine(_typing());
    }

    IEnumerator _typing()
    {
        yield return new WaitForSeconds(2f);
        for(int i = 0; i < m_text.Length; i++)
        {
            tx.text = m_text.Substring(0, i+1);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);        
        tx.text = "";
        yield return new WaitForSeconds(0.2f);
        tx.text = m_text;
        yield return new WaitForSeconds(0.2f);
        tx.text = "";
        yield return new WaitForSeconds(0.2f);
        tx.text = m_text;
        yield return new WaitForSeconds(0.2f);
        panel.SetActive(true);
    }

}
