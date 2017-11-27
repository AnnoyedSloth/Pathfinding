using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q6 : MonoBehaviour
{
    char?[,] kakao;
    bool[,] flag;

    void Start()
    {
        kakao = new char?[6, 6] {{'T', 'T', 'T', 'A', 'N', 'T'},
                                 {'R', 'R', 'F', 'A', 'C', 'C'},
                                 {'R', 'R', 'R', 'F', 'C', 'C'},
                                 {'T', 'R', 'R', 'R', 'A', 'A'},
                                 {'T', 'T', 'M', 'M', 'M', 'F'},
                                 {'T', 'M', 'M', 'T', 'T', 'J'}};

        flag = new bool[6, 6];

        for (int a = 0; a < 6; a++)
        {
            for (int b = 0; b < 6; b++)
            {
                flag[a, b] = true;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Assignment();
            Print();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Cascade();
        }
    }

    void Assignment()
    {
        for (int a = 0; a < 6 - 1; a++)
        {
            for (int b = 0; b < 6 - 1; b++)
            {
                if (kakao[a, b] != kakao[a, b + 1]) break;
                if (kakao[a, b] != kakao[a + 1, b]) break;
                if (kakao[a, b] != kakao[a + 1, b + 1]) break;
                if (kakao[a, b] == kakao[a, b + 1] && kakao[a, b] == kakao[a + 1, b] && kakao[a, b] == kakao[a + 1, b + 1])
                {
                    print("flag" + a + ", " + b + "Checked");
                    flag[a, b] = false;
                    flag[a, b + 1] = false;
                    flag[a + 1, b] = false;
                    flag[a + 1, b + 1] = false;
                }
            }
        }
        for (int a = 0; a < 6; a++)
        {
            for (int b = 0; b < 6; b++)
            {
                if (flag[a, b] == false) kakao[a, b] = null;
            }
        }
    }

    void Cascade()
    {
        for (int a = 5; a > 0; a--)
        {
            for (int b = 5; b >= 0; b--)
            {
                if (flag[a, b] == false)
                {
                    if (flag[a - 1, b] == true)
                    {
                        flag[a, b] = true;
                        kakao[a, b] = kakao[a - 1, b];
                        flag[a - 1, b] = false;
                        kakao[a - 1, b] = null;
                    }
                    else continue;
                    
                }
            }
        }
        Print();
    }

    void Print()
    {
        string[] str;
        str = new string[6];
        string[] str2;
        str2 = new string[6];

        print("------------");
        for (int a = 0; a < 6; a++)
        {
            for (int b = 0; b < 6; b++)
            {
                if (kakao[a, b] == null)
                {
                    str[a] = str[a] + "  ";
                }
                else
                    str[a] = str[a] + kakao[a, b].ToString();
            }
            Debug.Log(str[a]);
        }

        //for (int a = 0; a < 6; a++)
        //{
        //    for (int b = 0; b < 6; b++)
        //    {
        //        str2[a] = str2[a] + flag[a, b].ToString();
        //    }
        //    Debug.Log(str2[a]);
        //}
    }
}