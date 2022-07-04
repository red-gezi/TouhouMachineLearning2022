using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TouhouMachineLearningSummary.GameEnum;
using TouhouMachineLearningSummary.Info;
using TouhouMachineLearningSummary.Model;

namespace TouhouMachineLearningSummary.CardSpace
{
    /// <summary>
    /// 卡牌名称:琪露诺
    /// 卡牌能力:部署，活力（x）:封印1+x个非金单位并给与2点伤害，x为两侧单位的活力值总和
    /// </summary>
    public class Card2002004 : Card
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
                 for (int i = 0; i < 1 + GameSystem.InfoSystem.GetTwoSideField(this, CardField.Inspire) + 1; i++)
                 {
                     await GameSystem.SelectSystem.SelectUnite(this, AgainstInfo.cardSet[GameRegion.Battle].CardList, 1, false);
                     await GameSystem.StateSystem.ChangeState(new TriggerInfoModel(this, GameSystem.InfoSystem.SelectUnits).SetTargetState(CardState.Seal));
                 }
             }, Condition.Default)
             .AbilityAppend();
        }
    }
}