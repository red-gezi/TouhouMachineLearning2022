using System.Linq;
using System.Collections.Generic;
using TouhouMachineLearningSummary.Model;
using TouhouMachineLearningSummary.GameEnum;
namespace TouhouMachineLearningSummary.CardSpace
{
    /// <summary>
    /// 卡牌名称:诹坊子
    /// 卡牌能力:我方回合结束时，若自身右侧存在单位，则向右移动一格位置并给与对方一点伤害（多个同类型效果可能会有bug）
    /// </summary>
    public class Card2201002 : Card
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
            AbalityRegister(TriggerTime.When, TriggerType.TurnEnd)
               .AbilityAdd(async (triggerInfo) =>
               {
                   if (RightCard != null)
                   {
                       await GameSystem.TransferSystem.MoveCard(new TriggerInfoModel(this, this).SetLocation(RightCard.CurrentOrientation, RightCard.CurrentRegion, RightCard.CurrentIndex));
                       await GameSystem.SelectSystem.SelectUnite(this, GameSystem.InfoSystem.AgainstCardSet[Orientation.Op][GameRegion.Battle][CardRank.NoGold].CardList, 1, true);
                       await GameSystem.PointSystem.Hurt(new TriggerInfoModel(this, GameSystem.InfoSystem.SelectUnits).SetPoint(1).SetBullet(new BulletModel()));
                   }
               }, Condition.Default,Condition.OnMyTurn)
               .AbilityAppend();
        }
    }
}