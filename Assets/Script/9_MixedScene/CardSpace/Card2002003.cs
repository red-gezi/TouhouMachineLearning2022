using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouhouMachineLearningSummary.Command;
using TouhouMachineLearningSummary.GameEnum;
using TouhouMachineLearningSummary.Info;
using TouhouMachineLearningSummary.Model;

namespace TouhouMachineLearningSummary.CardSpace
{
    public class Card2002003 : Card
    {
        public override void Init()
        {

            //��ʼ��ͨ�ÿ���Ч��
            base.Init();
            AbalityRegister(TriggerTime.When, TriggerType.Play)
                .AbilityAdd(async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectLocation(this, CardDeployTerritory, CardDeployRegion);
                    await GameSystem.TransSystem.DeployCard(new TriggerInfoModel(this, this));
                })
                .AbilityAppend();
            //����Ч��
            AbalityRegister(TriggerTime.When, TriggerType.Deploy)
                .AbilityAdd(async (triggerInfo) =>
                {
                    await GameSystem.FieldSystem.SetField(new TriggerInfoModel(this,this).SetTargetField(CardField.Vitality, 2));
                    await GameSystem.TransSystem.SummonCard(
                        new TriggerInfoModel(this, GameSystem.InfoSystem.AgainstCardSet[Orientation.My][GameRegion.Deck].CardList
                        .Where(card => card.CardID == 2002001 || card.CardID == 2002002)
                        .ToList())
                        );
                }, Condition.Default)
                .AbilityAppend();
            //���ٻ�ʱЧ��
            AbalityRegister(TriggerTime.When, TriggerType.Summon)
               .AbilityAdd(async (triggerInfo) =>
               {
                   await GameSystem.FieldSystem.SetField(new TriggerInfoModel(this, this).SetTargetField(CardField.Vitality, 2));
               }, Condition.Default)
               .AbilityAppend();
        }
    }
}