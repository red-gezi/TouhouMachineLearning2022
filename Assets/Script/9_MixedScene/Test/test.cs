﻿using Microsoft.AspNetCore.SignalR.Client;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TouhouMachineLearningSummary.Extension;
using TouhouMachineLearningSummary.GameEnum;
using TouhouMachineLearningSummary.Info;
using TouhouMachineLearningSummary.Manager;
using TouhouMachineLearningSummary.Model;
using UnityEngine;
using static UnityEngine.Mathf;

namespace TouhouMachineLearningSummary.Test
{
    public class test : MonoBehaviour
    {
        [ShowInInspector]
        public CardSet cardSet => AgainstInfo.cardSet;
        [ShowInInspector]
        public CardSet FiltercardSet;
        [ShowInInspector]
        public Texture2D tex;
        public string text;
        [Button("截图")]
        public void CaptureScreen(string name) => ScreenCapture.CaptureScreenshot(@"Assets/Art/Scene/" + name + ".png");

        static string ip => !Info.AgainstInfo.isHostNetMode ? "localhost:495" : "server.natappfree.cc:37048";
        class CardAbility
        {
            public int cardID;
            public object[] voice=new object[] {1};
            public List<(int, object[])> ability=new List<(int, object[])>();
        }
        [Button("测试网络启动")]
        public async void Click()
        {
            List < CardAbility > abilities = new List < CardAbility >();

            for (int i = 0; i < 10; i++)
            {
                CardAbility cardAbility = new CardAbility();
                cardAbility.cardID = i;
                cardAbility.ability.Add(new(33, new object[] { GameEnum.Territory.My, GameEnum.BattleRegion.Fire }));
                cardAbility.ability.Add(new(45, new object[] { new TriggerInfoModel(null, targetCard: null) }));
                cardAbility.ability.Add(new(00, new object[] { }));
                cardAbility.ability.Add(new(22, new object[] { new TriggerInfoModel(null, targetCard: null) , Orientation.My , CardRank.Copper, CardTag.Fairy , CardFeature.LowestUnites ,1,true}));
                cardAbility.ability.Add(new(18, new object[] { new TriggerInfoModel(null, targetCard: null) }));
                abilities.Add(cardAbility);
            }
            Debug.Log(abilities.ToJson());
            //Debug.Log("手动点击触发");
            //var Hub = new HubConnectionBuilder()
            //    .WithUrl($"https://gezi.ovyno.com/TouHouHub")
            //    .Build();
            //await Hub.StartAsync();
            //Debug.Log(await Hub.InvokeAsync<string>("GetCardConfigsVersion"));
        }
        public async void Start()
        {

            //Debug.Log("由unity触发");
            //var Hub = new HubConnectionBuilder()
            //    .WithUrl($"http://106.15.38.165:495/TouHouHub")
            //    .Build();
            //await Hub.StartAsync();
            //Debug.Log(await Hub.InvokeAsync<string>("GetCardConfigsVersion"));
        }
        [Button("下载拥有记录")]
        public void test1()
        {
            var result = Command.NetCommand.DownloadOwnerAgentSummaryAsync("0", 0, 100);
            Debug.Log(result.ToJson());
        }

        [Button("下载所有记录")]
        public void test2()
        {
            var result = Command.NetCommand.DownloadAllAgentSummaryAsync(0, 100);
            Debug.Log(result.ToJson());
        }
        [Button("跳转到指定回合")]
        public void Jump(int totalTurnRank, bool isOnTheOffensive, bool isPlayer1)
        {
            //先加载
            Info.AgainstInfo.summary = AgainstSummaryManager.Load("");
            Debug.LogWarning(Info.AgainstInfo.summary.ToJson());
            AgainstInfo.IsPlayer1 = isPlayer1;
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
        private void OnGUI()
        {
            //if (GUI.Button(new Rect(0, 0, 100, 50), "翻页模拟效果"))
            //{
            //    Command.BookCommand.SimulateFilpPage(true);
            //}
            //if (GUI.Button(new Rect(0, 100, 100, 50), "新版本效果"))
            //{
            //    AgainstManager.Init();
            //    //AgainstManager.SetPvPMode(false);
            //    //AgainstManager.SetTurnFirst(FirstTurn.PlayerFirst);
            //    AgainstManager.AutoSetPlayerInfo(new PlayerInfo(
            //             "NPC", "gezi", "yaya", "",
            //            new List<CardDeck>
            //            {
            //            new CardDeck("gezi", 10001, new List<int>
            //            {
            //                20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,
            //            })
            //            })
            //        );
            //    AgainstManager.AutoSetOpponentInfo(
            //       new PlayerInfo(
            //             "NPC", "gezi", "yaya", "",
            //            new List<CardDeck>
            //            {
            //            new CardDeck("gezi", 10001, new List<int>
            //            {
            //                20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,20002,
            //            })
            //            })
            //       );
            //    //AgainstManager.SetCardVersion("");
            //    Debug.Log("对战start");
            //    AgainstManager.AutoStart();
            //}
            //if (GUI.Button(new Rect(0, 150, 100, 50), "启动回放模式"))
            //{
            //    AgainstManager.Init();
            //    AgainstManager.AutoStart();
            //}
        }
    }
}

