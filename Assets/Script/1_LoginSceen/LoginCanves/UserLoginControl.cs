﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TouhouMachineLearningSummary.GameEnum;
using TouhouMachineLearningSummary.Manager;
using TouhouMachineLearningSummary.Model;
using UnityEngine;
using UnityEngine.UI;

namespace TouhouMachineLearningSummary.Control
{
    public class UserLoginControl : MonoBehaviour
    {
        public Text Account;
        public Text Password;

        bool isAleardyLogin = false;
        async void Start()
        {
            Manager.TakeLoopManager.Init();
            if (!isAleardyLogin)
            {
                Command.Network.NetCommand.Init();
                await CardAssemblyManager.SetCurrentAssembly(""); //加载卡牌配置数据
                //UserLogin();//自动登录
                //TestBattleAsync();
            }
            else
            {
                await Command.BookCommand.InitAsync();
                //if (Command.Network.NetCommand.GetPlayerState(""))
                //{

                //}
            }

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Command.MenuStateCommand.ShowStare();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (Command.MenuStateCommand.GetCurrentStateRank())
                {
                    case (1)://如果当前状态为登录前，则关闭程序
                        {
                            _ = Command.GameUI.NoticeCommand.ShowAsync("退出游戏？",
                            okAction: async () =>
                            {
                                Application.Quit();
                            });
                            break;
                        }
                    case (2)://如果当前状态为第主级页面，则询问并退出登录
                        {
                            _ = Command.GameUI.NoticeCommand.ShowAsync("退出登录",
                            okAction: async () =>
                            {
                                CameraViewManager.MoveToSceneViewPositionAsync();
                                Command.MenuStateCommand.RebackStare();
                                Command.MenuStateCommand.ChangeToMainPage(MenuState.Login);
                                await Command.BookCommand.SetCoverStateAsync(false);
                                Info.GameUI.UiInfo.loginCanvas.SetActive(true);
                            }
                            );
                            break;
                        }
                    default://如果当前状态为多级页面，则返回上级（个别页面需要询问）
                        {
                            //如果是组牌模式，则询问是否返回上一页，否则直接返回上一页
                            if (Command.MenuStateCommand.GetCurrentState() == MenuState.CardListChange)
                            {
                                _ = Command.GameUI.NoticeCommand.ShowAsync("不保存卡组？",
                                okAction: async () =>
                                {
                                    Command.MenuStateCommand.RebackStare();
                                });
                            }
                            else
                            {
                                Command.MenuStateCommand.RebackStare();
                            }
                            break;
                        }
                }
            }
        }
        public void UserRegister()
        {
            try
            {
                _ = Command.Network.NetCommand.RegisterAsync(Account.text, Password.text);
            }
            catch (System.Exception e) { Debug.LogException(e); }
        }

        public async void UserLogin()
        {
            try
            {
                bool isSuccessLogin = await Command.Network.NetCommand.LoginAsync(Account.text, Password.text);
                if (isSuccessLogin)
                {
                    PlayerInfo.UserState onlineUserState = Info.AgainstInfo.onlineUserInfo.OnlineUserState;
                    if (onlineUserState.Step == 0 && onlineUserState.Step == 0)
                    {
                        Command.DialogueCommand.Play("0-0");


                    }
                    Manager.UserInfoManager.Refresh();
                    //_ = Command.Network.NetCommand.UpdateInfoAsync(UpdateType.Decks, new List<CardDeck>() { Info.AgainstInfo.onlineUserInfo.UseDeck, Info.AgainstInfo.onlineUserInfo.UseDeck, Info.AgainstInfo.onlineUserInfo.UseDeck });
                    _ = Command.Network.NetCommand.CheckRoomAsync(Account.text, Password.text);
                }
            }
            catch (System.Exception e) { Debug.LogException(e); }
        }

        public async Task TestBattleAsync()
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
            Debug.Log("对战start");
            await AgainstManager.Start();
        }

        public void UserServerSelect() => Info.AgainstInfo.isHostNetMode = !Info.AgainstInfo.isHostNetMode;

        private void OnApplicationQuit() => Command.Network.NetCommand.Dispose();
    }
}