using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMessageText : MonoBehaviour
{
    public Text tx;
    private string First_text = "< Help Message > \r\n\r\n← ↑ ↓ → : 이동 \r\nSpace : 점프 \r\nT : 상호작용 \r\nZ : 구체 생성 \r\nA : 구체 회전속도 증가 \r\nW : 구체 크기 증가 " +
        "\r\nQ : 광역 스킬\r\nA키 연타 : FeverMode\r\nF1 : 도움말  F2 : 스텟창";
    static int Help_var;

    private void Start()
    {
        if (Help_var == 0)
        {
            Help_var++;
            StartCoroutine(_typing());
        }
        else
        {
            tx.text = First_text;
            this.gameObject.SetActive(false);
        }
    }
    private void Update()
    {


    }

    IEnumerator _typing()
    {

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < First_text.Length; i++)
        {
            tx.text = First_text.Substring(0, i + 1);
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(0.2f);
     
    }
    public void ExitButton()
    {
        this.gameObject.SetActive(false);        
    }
}
