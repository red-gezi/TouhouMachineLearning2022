using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouhouMachineLearningSummary.GameEnum;
using TouhouMachineLearningSummary.Info;
using TouhouMachineLearningSummary.Model;

namespace TouhouMachineLearningSummary.CardSpace
{
    public class Card20002 : Card
    {
        public override void Init()
        {
            //初始化通用卡牌效果
            base.Init();
            AbalityRegister(TriggerTime.When, TriggerType.Play)
               .AbilityAdd(async (triggerInfo) =>
               {
                   UnityEngine.Debug.Log("开始选择坐标");
                   await GameSystem.SelectSystem.SelectLocation(this, region, territory);
                   UnityEngine.Debug.Log("选择单位完毕");
                   await GameSystem.TransSystem.DeployCard(new TriggerInfoModel(this).SetTargetCard(this));
               })
               .AbilityAppend();
            AbalityRegister(TriggerTime.When, TriggerType.Deploy)
                .AbilityAdd(async (triggerInfo) =>
                {
                    await GameSystem.PointSystem.Gain(new TriggerInfoModel(this).SetTargetCard(this).SetPoint(10));
                    UnityEngine.Debug.Log("增益自身10点");
                }, Condition.Default)
               .AbilityAdd(async (triggerInfo) =>
               {
                   UnityEngine.Debug.Log("开始选择单位");
                   await GameSystem.SelectSystem.SelectUnite(this, AgainstInfo.cardSet[Orientation.My][GameRegion.Battle][CardRank.Copper, CardRank.Silver][CardTag.Fairy].CardList, 1);
                   UnityEngine.Debug.Log("选择单位完毕");
                   await GameSystem.PointSystem.Cure
                   (
                       new TriggerInfoModel(this)
                       .SetTargetCard(AgainstInfo.SelectUnits)
                       .SetBullet(new BulletModel(BulletType.BigBall, BulletColor.Green, BulletTrack.Line))
                   );
                   if (AgainstInfo.SelectUnits.Any())
                   {
                       //AgainstInfo.SelectRegion = Command.RowCommand.GetSingleRowInfoById(AgainstInfo.SelectUnits[0].Location.X);
                       AgainstInfo.SelectRowRank = AgainstInfo.SelectUnits[0].Location.X;
                       AgainstInfo.SelectRank = AgainstInfo.SelectUnits[0].Location.Y;
                   }
                   await GameSystem.TransSystem.DeployCard(new TriggerInfoModel(this).SetTargetCard(AgainstInfo.SelectUnits));
               }, Condition.Default)
               .AbilityAppend();
        }
    }
}