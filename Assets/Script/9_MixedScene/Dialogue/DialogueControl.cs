using TouhouMachineLearningSummary.Command;
using TouhouMachineLearningSummary.Info;
using UnityEngine;
namespace TouhouMachineLearningSummary.Control
{
    /// <summary>
    /// 剧情演出控制器
    /// </summary>
    public class DialogueControl : MonoBehaviour
    {
        public async void ShowNextText()
        {
            if (!DialogueInfo.SelectMode)
            {
                DialogueInfo.IsShowNextText = true;
            }
        }
        public async void ShowLastText()
        {
            DialogueInfo.instance.selectUi.SetActive(false);
            DialogueInfo.CurrentPoint = Mathf.Max(0, DialogueInfo.CurrentPoint - 2);
            await DialogueCommand.RunNextOperations();
        }
        public async void SetBranch(int index)
        {
            DialogueInfo.SelectBranch = index;
            DialogueInfo.instance.selectUi.SetActive(false);
            DialogueInfo.CurrentPoint++;
            DialogueInfo.IsSelectOver= true;
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(100, 100, 100, 100), "播放剧情"))
            {
                DialogueCommand.Play("0", 0);
            }
            //if (GUI.Button(new Rect(200, 100, 100, 100), "上一句话"))
            //{
            //    ShowLastText();
            //}
        }
    }
}