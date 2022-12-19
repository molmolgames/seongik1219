using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;
    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(14, new string[] { "뭘봐?:0", "말하는 고양이 처음 보나?:0", "귀찮게 굴지 말지?:1" }); // 예시

        talkData.Add(20, new string[] { "..... :0", "세상을... :0", "구해야 한다냥... :0", "zzZ... :1" });
        talkData.Add(21, new string[] { " -> -> -> " });
        talkData.Add(22, new string[] { " 경고 : 적 매우 강함, 권장 레벨 6이상 " });
        talkData.Add(23, new string[] { " 경고 : 낙하 주의, 권장 레벨 3이상 " });
        talkData.Add(602, new string[] { "Scene2 열쇠 획득!" });
        talkData.Add(603, new string[] { "Scene3 열쇠 획득!" });
        talkData.Add(604, new string[] { "Scene4 열쇠 획득!" });
        talkData.Add(702, new string[] { "첫번째 문이 열렸다!" });
        talkData.Add(703, new string[] { "두번째 문이 열렸다!" });
        talkData.Add(704, new string[] { "마지막 문이 열렸다!" });
        talkData.Add(705, new string[] { "맞는 열쇠가 없습니다." });
        talkData.Add(401, new string[] { "탈출구를 찾아보자"});
        talkData.Add(402, new string[] { "앞을 조심해!"});
        talkData.Add(403, new string[] { "우측으로 좀 더 가보자!"});
        talkData.Add(404, new string[] { "좌측으로 좀 더 가보자!"});
        talkData.Add(405, new string[] { "빨강,파랑,초록, 하나로는 부족할걸?"});
        talkData.Add(406, new string[] { "돌아가"});
        talkData.Add(407, new string[] { "문을 열려면 주문서가 필요해"});
        talkData.Add(408, new string[] { "돌무더기를 잘 살펴보자"});
        talkData.Add(201, new string[] { "열쇠가 부족한 걸?"}); // 1211 from jeongik

        portraitData.Add(14 + 0, portraitArr[1]); //예시
        portraitData.Add(14 + 1, portraitArr[2]); //예시

        portraitData.Add(20 + 0, portraitArr[0]);
        portraitData.Add(20 + 1, portraitArr[1]);

    }
    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
        {
            return null;
        }
        else
        {
            return talkData[id][talkIndex];
        }
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
