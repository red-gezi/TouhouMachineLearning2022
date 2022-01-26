﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouhouMachineLearningSummary.Control;
using TouhouMachineLearningSummary.GameEnum;
using TouhouMachineLearningSummary.Info;
//using TouhouMachineLearningSummary.Manager;
using TouhouMachineLearningSummary.Model;
using UnityEngine;

namespace TouhouMachineLearningSummary.Command
{
    //具体实现，还需进一步简化
    public static class CardCommand
    {
        internal static async Task OnMouseDown(Card thisCard)
        {
            if (thisCard.isPrepareToPlay)
            {
                AgainstInfo.playerPrePlayCard = thisCard;
            }
        }
        public static async Task OnMouseUp(Card thisCard)
        {
            if (AgainstInfo.playerPrePlayCard != null)
            {
                if (AgainstInfo.PlayerFocusRegion != null && AgainstInfo.PlayerFocusRegion.name == "下方_墓地")
                {
                    Info.AgainstInfo.playerDisCard = Info.AgainstInfo.playerPrePlayCard;
                }
                //将卡牌放回
                else if (Info.AgainstInfo.PlayerFocusRegion != null && (AgainstInfo.PlayerFocusRegion.name == "下方_领袖" || AgainstInfo.PlayerFocusRegion.name == "下方_手牌"))
                {
                }
                else
                {
                    Info.AgainstInfo.playerPlayCard = Info.AgainstInfo.playerPrePlayCard;
                }
                Info.AgainstInfo.playerPrePlayCard = null;

            }
        }



        public static void OrderHandCard()
        {
            AgainstInfo.cardSet[GameRegion.Hand].SingleRowInfos.ForEach(singleRowInfo =>
            {
                AgainstInfo.cardSet[singleRowInfo.RowRank] = AgainstInfo.cardSet[singleRowInfo.RowRank].OrderByDescending(card => card.cardRank).ThenBy(card => card.BasePoint).ThenBy(card => card.CardID).ToList();
            });
        }

        public static void RemoveCard(Card card) => card.belongCardList.Remove(card);
        public static Card CreateCard(int id)
        {
            GameObject newCard = GameObject.Instantiate(Info.CardInfo.cardModel, new Vector3(0, 100, 0), Info.CardInfo.cardModel.transform.rotation);
            newCard.transform.SetParent(GameObject.FindGameObjectWithTag("Card").transform);
            newCard.name = "Card" + Info.CardInfo.CreatCardRank++;
            //Debug.Log("创建卡牌"+id);
            newCard.AddComponent(Manager.CardAssemblyManager.GetCardScript(id));
            Card card = newCard.GetComponent<Card>();
            var CardStandardInfo = Manager.CardAssemblyManager.GetCurrentCardInfos(id);
            card.CardID = CardStandardInfo.cardID;
            card.BasePoint = CardStandardInfo.point;
            card.Icon = CardStandardInfo.icon;
            card.CardDeployRegion = CardStandardInfo.cardDeployRegion;
            card.CardDeployTerritory = CardStandardInfo.cardDeployTerritory;
            card.cardTag = CardStandardInfo.cardTag;
            card.cardRank = CardStandardInfo.cardRank;
            card.cardType = CardStandardInfo.cardType;
            card.GetComponent<Renderer>().material.SetTexture("_Front", card.Icon);
            switch (card.cardRank)
            {
                case CardRank.Leader: card.GetComponent<Renderer>().material.SetColor("_side", new Color(0.43f, 0.6f, 1f)); break;
                case CardRank.Gold: card.GetComponent<Renderer>().material.SetColor("_side", new Color(0.8f, 0.8f, 0f)); break;
                case CardRank.Silver: card.GetComponent<Renderer>().material.SetColor("_side", new Color(0.75f, 0.75f, 0.75f)); break;
                case CardRank.Copper: card.GetComponent<Renderer>().material.SetColor("_side", new Color(1, 0.42f, 0.37f)); break;
                default: break;
            }
            card.Init();
            return card;
        }
        /// <summary>
        /// 从对战记录的简易卡牌模型复现原卡牌
        /// </summary>
        /// <param name="sampleCard"></param>
        /// <returns></returns>
        public static Card CreateCard(SampleCardModel sampleCard)
        {

            //Card card = NewCard.GetComponent<Card>();
            GameObject newCard = GameObject.Instantiate(Info.CardInfo.cardModel, new Vector3(0, 100, 0), Info.CardInfo.cardModel.transform.rotation);
            newCard.transform.SetParent(GameObject.FindGameObjectWithTag("Card").transform);
            newCard.name = "Card" + Info.CardInfo.CreatCardRank++;
            //Debug.Log("创建卡牌"+id);
            newCard.AddComponent(Manager.CardAssemblyManager.GetCardScript(sampleCard.CardID));
            newCard.AddComponent(Manager.CardAssemblyManager.GetCardScript(sampleCard.CardID));
            Card card = newCard.GetComponent<Card>();
            var CardStandardInfo = Manager.CardAssemblyManager.GetCurrentCardInfos(sampleCard.CardID);
            ///然后根据sampleCard设置具体参数，先暂时设为默认
            card.CardID = CardStandardInfo.cardID;
            card.BasePoint = CardStandardInfo.point;
            card.Icon = CardStandardInfo.icon;
            card.CardDeployRegion = CardStandardInfo.cardDeployRegion;
            card.CardDeployTerritory = CardStandardInfo.cardDeployTerritory;
            card.cardTag = CardStandardInfo.cardTag;
            card.cardRank = CardStandardInfo.cardRank;
            card.cardType = CardStandardInfo.cardType;
            card.GetComponent<Renderer>().material.SetTexture("_Front", card.Icon);
            switch (card.cardRank)
            {
                case CardRank.Leader: newCard.GetComponent<Renderer>().material.SetColor("_side", new Color(0.43f, 0.6f, 1f)); break;
                case CardRank.Gold: newCard.GetComponent<Renderer>().material.SetColor("_side", new Color(0.8f, 0.8f, 0f)); break;
                case CardRank.Silver: newCard.GetComponent<Renderer>().material.SetColor("_side", new Color(0.75f, 0.75f, 0.75f)); break;
                case CardRank.Copper: newCard.GetComponent<Renderer>().material.SetColor("_side", new Color(1, 0.42f, 0.37f)); break;
                default: break;
            }
            card.Init();
            return card;
        }
        public static async Task BanishCard(Card card)
        {
            card.GetComponent<CardControl>().CreatGap();
            await Task.Delay(800);
            card.GetComponent<CardControl>().FoldGap();
            await Task.Delay(800);
            card.GetComponent<CardControl>().DestoryGap();
            RemoveCard(card);
        }
        public static async Task SummonCard(Card targetCard)
        {
            List<Card> TargetRow = AgainstInfo
                .cardSet[(GameRegion)targetCard.CardDeployRegion][targetCard.orientation]
                .SingleRowInfos.First().CardList;
            Debug.LogWarning("召唤卡牌于" + targetCard.orientation);
            RemoveCard(targetCard);
            TargetRow.Add(targetCard);
            targetCard.IsCanSee = true;
            //targetCard.moveSpeed = 0.1f;
            targetCard.isMoveStepOver = false;
            await Task.Delay(1000);
            targetCard.isMoveStepOver = true;
            //targetCard.moveSpeed = 0.1f;
            await AudioCommand.PlayAsync(GameAudioType.DrawCard);
        }
        public static async Task MoveCard(Card targetCard, Location location)
        {

            List<Card> TargetRow = CardSet.GlobalCardList[location.X];
            RemoveCard(targetCard);
            int rank = location.Y >= 0 ? Math.Min(location.Y, TargetRow.Count) : Math.Max(0, TargetRow.Count + location.Y + 1);
            UnityEngine.Debug.Log("设置移动目标为" + location.X + "," + location.Y);
            TargetRow.Insert(rank, targetCard);
            targetCard.isMoveStepOver = false;
            await Task.Delay(500);
            targetCard.isMoveStepOver = true;
            _ = AudioCommand.PlayAsync(GameAudioType.DrawCard);
        }
        public static async Task DeployCard(Card targetCard)
        {
            RemoveCard(targetCard);
            AgainstInfo.SelectRowCardList.Insert(AgainstInfo.SelectRank, targetCard);
            //targetCard.moveSpeed = 0.1f;
            targetCard.isMoveStepOver = false;
            await Task.Delay(1000);
            targetCard.isMoveStepOver = true;
            _ = AudioCommand.PlayAsync(GameAudioType.Deploy);
        }
        /// <summary>
        /// 洗牌
        /// </summary>
        /// <param name="targetCard">要洗的卡牌</param>
        /// <param name="IsPlayerExchange">是否操控当前玩家洗牌</param>
        /// <param name="isRoundStartExchange">是否回合开始洗牌</param>
        /// <param name="WashInsertRank">洗入位置</param>
        /// <returns></returns>
        public static async Task ExchangeCard(Card targetCard, bool IsPlayerExchange = true, bool isRoundStartExchange = false, int WashInsertRank = 0)
        {
            //Debug.Log("交换卡牌");
            await WashCard(targetCard, IsPlayerExchange, WashInsertRank);
            await DrawCard(IsPlayerExchange, true);
            if (IsPlayerExchange)
            {
                CardBoardCommand.LoadBoardCardList(AgainstInfo.cardSet[isRoundStartExchange ? Orientation.Down : Orientation.My][GameRegion.Hand].CardList);
            }
        }
        internal static Task RebackCard()
        {
            throw new NotImplementedException();
        }
        public static async Task DrawCard(bool isPlayerDraw = true, bool ActiveBlackList = false, bool isOrder = true)
        {
            //Debug.Log("抽卡");
            _ = AudioCommand.PlayAsync(GameAudioType.DrawCard);
            Card TargetCard = AgainstInfo.cardSet[isPlayerDraw ? Orientation.Down : Orientation.Up][GameRegion.Deck].CardList.FirstOrDefault();
            if (TargetCard == null)
            {
                Debug.LogError("无法进行抽卡");
            }
            else
            {
                TargetCard.SetCardSeeAble(isPlayerDraw);
                CardSet TargetCardtemp = AgainstInfo.cardSet[isPlayerDraw ? Orientation.Down : Orientation.Up][GameRegion.Deck];

                AgainstInfo.cardSet[isPlayerDraw ? Orientation.Down : Orientation.Up][GameRegion.Deck].Remove(TargetCard);
                AgainstInfo.cardSet[isPlayerDraw ? Orientation.Down : Orientation.Up][GameRegion.Hand].Add(TargetCard);
            }
            if (isOrder)
            {
                OrderHandCard();
            }
            await Task.Delay(100);
        }
        public static async Task WashCard(Card TargetCard, bool IsPlayerWash = true, int InsertRank = 0)
        {
            Debug.Log("洗回卡牌");
            if (IsPlayerWash)
            {
                AgainstInfo.TargetCard = TargetCard;
                int MaxCardRank = AgainstInfo.cardSet[Orientation.Down][GameRegion.Deck].CardList.Count;
                AgainstInfo.washInsertRank = AiCommand.GetRandom(0, MaxCardRank);
                NetCommand.AsyncInfo(NetAcyncType.ExchangeCard);
                AgainstInfo.cardSet[Orientation.Down][GameRegion.Hand].Remove(TargetCard);
                AgainstInfo.cardSet[Orientation.Down][GameRegion.Deck].Add(TargetCard, AgainstInfo.washInsertRank);
                TargetCard.SetCardSeeAble(false);
            }
            else
            {
                AgainstInfo.cardSet[Orientation.Up][GameRegion.Hand].Remove(TargetCard);
                AgainstInfo.cardSet[Orientation.Up][GameRegion.Deck].Add(TargetCard, InsertRank);
            }
            await Task.Delay(500);
        }
        public static async Task PlayCard(Card targetCard, bool IsAnsy = true)
        {
            await Task.Delay(0);//之后实装卡牌特效需要时间延迟配合
            _ = AudioCommand.PlayAsync(GameEnum.GameAudioType.DrawCard);
            RowCommand.SetPlayCardMoveFree(false);
            targetCard.isPrepareToPlay = false;
            if (IsAnsy)
            {
                NetCommand.AsyncInfo(NetAcyncType.PlayCard);
            }
            targetCard.SetCardSeeAble(true);
            RemoveCard(targetCard);
            AgainstInfo.cardSet[Orientation.My][GameRegion.Uesd].Add(targetCard);
            AgainstInfo.playerPlayCard = null;
        }
        public static async Task DisCard(Card card)
        {
            await Task.Delay(0);//之后实装卡牌特效需要时间延迟配合
            card.isPrepareToPlay = false;
            card.SetCardSeeAble(false);
            RemoveCard(card);
            AgainstInfo.cardSet[Orientation.My][GameRegion.Grave].Add(card);
            AgainstInfo.playerDisCard = null;
        }

        public static async Task ReviveCard(TriggerInfoModel triggerInfo)
        {
            Card card = triggerInfo.targetCard;
            await AudioCommand.PlayAsync(GameAudioType.DrawCard);

            card.SetCardSeeAble(true);
            RemoveCard(card);
            AgainstInfo.cardSet[Orientation.My][GameRegion.Uesd].Add(card);
            await card.cardAbility[TriggerTime.When][TriggerType.Play][0](triggerInfo);
        }

        public static async Task SealCard(Card card)
        {
            card.transform.GetChild(2).gameObject.SetActive(true);
        }
        public static async Task UnSealCard(Card card)
        {
            card.transform.GetChild(2).gameObject.SetActive(false);
        }

        public static async Task Gain(TriggerInfoModel triggerInfo)
        {
            await BulletCommand.InitBulletAsync(triggerInfo);
            await Task.Delay(1000);
            triggerInfo.targetCard.ChangePoint += triggerInfo.point;
            await Task.Delay(1000);
        }
        public static async Task Hurt(TriggerInfoModel triggerInfo)
        {
            await BulletCommand.InitBulletAsync(triggerInfo);
            //悬浮伤害数字
            //await Manager.CardPointManager.CaretPointAsync(triggerInfo.targetCard, Mathf.Abs(triggerInfo.point), triggerInfo.point > 0 ? CardPointType.red : CardPointType.green);
            triggerInfo.targetCard.ChangePoint = Math.Max(triggerInfo.targetCard.ChangePoint - triggerInfo.point, 0);
            await Task.Delay(1000);
        }
        public static async Task MoveToGrave(Card card, int Index = 0)
        {
            Orientation orientation = card.belong == Territory.My ? Orientation.Down : Orientation.Up;
            RemoveCard(card);
            AgainstInfo.cardSet[orientation][GameRegion.Grave].SingleRowInfos[0].CardList.Insert(Index, card);
            card.SetCardSeeAble(false);
            card.ChangePoint = 0;
            card.isMoveStepOver = false;
            await Task.Delay(100);
            card.isMoveStepOver = true;
            await AudioCommand.PlayAsync(GameAudioType.DrawCard);
        }
    }
}