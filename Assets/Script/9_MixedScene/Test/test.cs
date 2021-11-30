﻿using Sirenix.OdinInspector;
using System.Collections.Generic;
using TouhouMachineLearningSummary.Extension;
using TouhouMachineLearningSummary.GameEnum;
using TouhouMachineLearningSummary.Info;
using TouhouMachineLearningSummary.Manager;
using TouhouMachineLearningSummary.Model;
using UnityEngine;

namespace TouhouMachineLearningSummary.Test
{
    public class test : MonoBehaviour
    {
        [ShowInInspector]
        public CardSet cardSet => AgainstInfo.cardSet;
        [ShowInInspector]
        public CardSet FiltercardSet;


        public string text;
        [Button("上传记录")]
        public void test0()
        {
            AgainstInfo.summary.Upload();
            AgainstInfo.summary.Explort();
            AgainstInfo.summary.Show();
        }
        [Button("下载记录")]
        public void test1()
        {
            var result = Command.Network.NetCommand.DownloadAgentSummaryAsync("0", 0, 100);
        }
        [Button("跳转到指定回合")]
        public void Jump(int totalTurnRank, bool isOnTheOffensive, bool isPlayer1)
        {
            //先加载
            Info.AgainstInfo.summary = AgainstSummaryManager.Load(1);
            Debug.LogWarning(Info.AgainstInfo.summary.ToJson());
            AgainstInfo.isPlayer1 = isPlayer1;
            //然后跳转
            _ = AgainstInfo.summary.JumpToTurnAsync(totalTurnRank, isOnTheOffensive);
        }
        [Button]
        public void useLanguage(GameEnum.Language language)
        {
            TranslateManager.currentLanguage = language.ToString();
        }
        [Button("翻译标签")]
        public void ShowText(GameEnum.CardTag tag)
        {
            text = tag.ToString().Translation();
        }
        [Button("查找集合")]

        public void filterCardSet(List<GameEnum.CardTag> tags)
        {
            FiltercardSet = cardSet[tags.ToArray()];
        }
        static Vector3 a;
        static Vector3 b;
        static Vector3 c;
        private void Start()
        {
            a = new Vector3(5, 3, 4);
            b = Random.onUnitSphere;
            c = Vector3.Cross(a, b);
        }
        private void Update()
        {
            Debug.DrawLine(Vector3.zero, a, Color.red);
            Debug.DrawLine(Vector3.zero, b, Color.green);
            Debug.DrawLine(Vector3.zero, c, Color.blue);

            if (Input.GetMouseButtonDown(1))
            {
                AgainstManager.Init();
                AgainstManager.SetReplayMode(11);
                AgainstManager.Start();
            }
        }
        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 100, 50), "旧版本效果"))
            {
                AgainstManager.Init();
                AgainstManager.SetPvPMode(false);
                AgainstManager.SetTurnFirst(FirstTurn.PlayerFirst);
                AgainstManager.SetPlayerInfo(new PlayerInfo(
                        "gezi", "yaya", "",
                        new List<CardDeck>
                        {
                        new CardDeck("gezi", 10001, new List<int>
                        {
                            20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,
                        })
                        })
                    );
                AgainstManager.SetOpponentInfo(
                   new PlayerInfo(
                        "gezi", "yaya", "",
                        new List<CardDeck>
                        {
                        new CardDeck("gezi", 10001, new List<int>
                        {
                            20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,
                        })
                        })
                   );
                AgainstManager.SetCardVersion("2021_10_14");
                Debug.Log("对战start");
                AgainstManager.Start();
            }
            if (GUI.Button(new Rect(0, 100, 100, 50), "新版本效果"))
            {
                AgainstManager.Init();
                AgainstManager.SetPvPMode(false);
                AgainstManager.SetTurnFirst(FirstTurn.PlayerFirst);
                AgainstManager.SetPlayerInfo(new PlayerInfo(
                        "gezi", "yaya", "",
                        new List<CardDeck>
                        {
                        new CardDeck("gezi", 10001, new List<int>
                        {
                            20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,
                        })
                        })
                    );
                AgainstManager.SetOpponentInfo(
                   new PlayerInfo(
                        "gezi", "yaya", "",
                        new List<CardDeck>
                        {
                        new CardDeck("gezi", 10001, new List<int>
                        {
                            20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,
                        })
                        })
                   );
                AgainstManager.SetCardVersion("");
                Debug.Log("对战start");
                AgainstManager.Start();
            }
            if (GUI.Button(new Rect(0, 150, 100, 50), "启动回放模式"))
            {
                AgainstManager.Init();
                AgainstManager.SetReplayMode(11);
                AgainstManager.Start();
            }
        }
    }
}

