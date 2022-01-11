﻿#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TouhouMachineLearningSummary.Model;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TouhouMachineLearningSummary.Other
{
    public class EditorExtension : MonoBehaviour
    {
        [MenuItem("Tools/打开服务端", false, 1)]
        static void StartServer() => Process.Start(@"OtherSolution\Server\bin\Debug\net6.0\Server.exe");
        [MenuItem("Tools/打开游戏客户端", false, 2)]
        static void StartClient() => Process.Start(@"Pc\TouhouMachineLearning2022.exe");
        [MenuItem("Tools/打开数据表格", false, 51)]
        static void OpenXls() => Process.Start(@"Assets\Resources\GameData\GameData.xlsx");
        [MenuItem("Tools/打开表格数据实时同步工具", false, 52)]
        static void UpdateXls() => Process.Start(@"OtherSolution\xls检测更新\bin\Debug\net461\xls检测更新.exe");
        [MenuItem("Tools/发布当前卡牌版本", false, 53)]
        static void UpdateCardSpace()
        {
            var gameCardAssembly = new DirectoryInfo(@"Library\ScriptAssemblies").GetFiles("GameCard*.dll").FirstOrDefault();
            var singleCardFile = new FileInfo(@"Assets\Resources\GameData\CardData-Single.json");
            var multiCardFile = new FileInfo(@"Assets\Resources\GameData\CardData-Multi.json");
            if (gameCardAssembly != null && singleCardFile != null && multiCardFile != null)
            {
                CardConfig cardConfig = new CardConfig(DateTime.Today.ToString("yyy_MM_dd"), gameCardAssembly, singleCardFile, multiCardFile);
                _ = Command.NetCommand.UploadCardConfigsAsync(cardConfig);
            }
            else
            {
                UnityEngine.Debug.LogError("检索不到卡牌dll文件");
            }
        }
        [MenuItem("Tools/打包素材", priority = 101)]
        static void BuildAssetBundles() => BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);
        [MenuItem("Tools/载入场景", priority = 102)]
        static void LoadAssetBundles()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "sceneasset");
            //加载场景Bundle
            AssetBundle.LoadFromFile(path);
            SceneManager.LoadScene("test");
        }
    }
}
#endif