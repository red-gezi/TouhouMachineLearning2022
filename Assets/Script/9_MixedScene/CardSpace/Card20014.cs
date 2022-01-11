using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TouhouMachineLearningSummary.GameEnum;
using TouhouMachineLearningSummary.Info;
using TouhouMachineLearningSummary.Model;

namespace TouhouMachineLearningSummary.CardSpace
{
    public class Card20014 : Card
    {
        public override void Init()
        {
            this[CardField.Vitality] = 1;

            //初始化通用卡牌效果
            base.Init();
            AbalityRegister(TriggerTime.When, TriggerType.Play)
               .AbilityAdd(async (triggerInfo) =>
               {
                   await GameSystem.SelectSystem.SelectLocation(this,region,territory);
                   await GameSystem.TransSystem.DeployCard(new TriggerInfoModel(this).SetTargetCard(this));
               })
               .AbilityAppend();

            AbalityRegister(TriggerTime.When, TriggerType.Deploy)
             .AbilityAdd(async (triggerInfo) =>
             {
                 await GameSystem.SelectSystem.SelectUnite(this, AgainstInfo.cardSet[Orientation.My][GameRegion.Deck][CardRank.Copper][CardFeature.Lowest][CardType.Unite][CardTag.Fairy].CardList, 1, true);
                 await GameSystem.TransSystem.PlayCard(new TriggerInfoModel(this).SetTargetCard(AgainstInfo.SelectUnits));
             }, Condition.Default)
             .AbilityAppend();
        }
    }
}