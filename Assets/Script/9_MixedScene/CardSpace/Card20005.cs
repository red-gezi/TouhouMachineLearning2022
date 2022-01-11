using TouhouMachineLearningSummary.GameEnum;
using TouhouMachineLearningSummary.Model;

namespace TouhouMachineLearningSummary.CardSpace
{
    public class Card20005 : Card
    {
        public override void Init()
        {
            base.Init();

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
                 for (int i = 0; i < 1 + GameSystem.FieldSystem.GetTwoSideField(this, CardField.Vitality); i++)
                 {
                     await GameSystem.SelectSystem.SelectUnite(this,GameSystem.InfoSystem.AgainstCardSet[Orientation.Op][GameRegion.Battle][CardRank.Copper, CardRank.Silver][CardFeature.Largest].CardList, 1, true);
                     await GameSystem.PointSystem.Hurt(new TriggerInfoModel(this).SetTargetCard(GameSystem.InfoSystem.SelectUnits).SetPoint(1));
                 }
             }, Condition.Default)
             .AbilityAppend();
        }
    }
}