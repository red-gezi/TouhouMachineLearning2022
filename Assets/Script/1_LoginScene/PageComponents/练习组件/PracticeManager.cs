using UnityEngine;
using TouhouMachineLearningSummary.GameEnum;
namespace TouhouMachineLearningSummary.Manager
{
    public class PracticeManager : MonoBehaviour
    {
        public void SelectLeader(int leader) => Info.PageCompnentInfo.SelectLeader = (PracticeLeader)leader;
        public void SelectFirstHandMode(int firstHangMode) => Info.PageCompnentInfo.SelectFirstHandMode = firstHangMode;
    }
}