using System.Linq;
using System.Collections.Generic;
using TouhouMachineLearningSummary.Model;
using TouhouMachineLearningSummary.GameEnum;
namespace TouhouMachineLearningSummary.CardSpace
{
    /// <summary>
    /// 卡牌名称:作弊的天邪鬼
    /// 卡牌能力:部署：检索牌组中一张道具卡并打出
    /// </summary>
    public class Card2011004 : Card
    {
        public override void Init()
        {
            //初始化通用卡牌效果
            base.Init();
            AbalityRegister(TriggerTime.When, TriggerType.Play)
               .AbilityAdd(async (triggerInfo) =>
               {
                   await GameSystem.SelectSystem.SelectLocation(this, CardDeployTerritory, CardDeployRegion);
                   await GameSystem.TransferSystem.DeployCard(new TriggerInfoModel(this, this));
               })
               .AbilityAppend();
            AbalityRegister(TriggerTime.When, TriggerType.Deploy)
              .AbilityAdd(async (triggerInfo) =>
              {
                  //List<Card> cardList = GameSystem.InfoSystem.AgainstCardSet[Orientation.My][GameEnum.CardTag.Tool].CardList;
                  //await GameSystem.SelectSystem.SelectBoardCard(this, cardList);
                  //if (GameSystem.InfoSystem.SelectBoardCardRanks.Count > 0)
                  //{
                  //    await GameSystem.TransSystem.PlayCard(new TriggerInfoModel(this, cardList[GameSystem.InfoSystem.SelectBoardCardRanks[0]]));
                  //}
                  await GameSystem.SelectSystem.SelectBoardCard(this, GameSystem.InfoSystem.AgainstCardSet[Orientation.My][CardTag.Tool].CardList);
                  await GameSystem.TransferSystem.PlayCard(new TriggerInfoModel(this, GameSystem.InfoSystem.SelectBoardCards));

              }, Condition.Default)
              .AbilityAppend();
        }
    }
}