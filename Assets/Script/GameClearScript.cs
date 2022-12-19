using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameClearScript : MonoBehaviour
{
    public Text tx;
    private string m_text = "G A M E   C L E A R !\r\n\r\nGame Designer\r\n\r\n강우진 조성익\r\n\r\n조정익 임태균\r\n\r\nTHANKS FOR PLAYING!";

    private void Start()
    {
        StartCoroutine(_typing());
    }

    IEnumerator _typing()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 22; i++)
        {
            tx.text = m_text.Substring(0, i + 1);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);
        for (int i = 23; i < m_text.Length; i++)
        {
            tx.text = m_text.Substring(23, i + 1);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.2f);
        tx.text = "";
        yield return new WaitForSeconds(0.2f);
        tx.text = m_text;
        yield return new WaitForSeconds(0.2f);
        tx.text = "";
        yield return new WaitForSeconds(0.2f);
        tx.text = m_text;



        yield return new WaitForSeconds(0.2f);
    }

}
