using AI_YuriPlus.PatcherClasses_Main;
using BepInEx;
using BepInEx.Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AI_YuriPlus {
    [BepInPlugin("AI_YuriPlus", "Yuri Plus Plugin", "1.5")]
    class Main : BaseUnityPlugin {
        internal const bool IsDebugMode = false;
        static internal Main Instance;
        void Start() {
            GameObject MainGameObject = new GameObject("AI_YuriPlus");
            Instance = this;
            DontDestroyOnLoad(MainGameObject);
            HarmonyWrapper.PatchAll(Assembly.GetExecutingAssembly());
            if (IsDebugMode) {
                UnityGameObjectVisualizer.Initializer.Start();
                Logger.LogMessage("Unity GameObject Visualizer started");
            }
            InitializeConfig();
        }
        internal BepInEx.Configuration.ConfigDefinition Config_DisableMotionLimit = new BepInEx.Configuration.ConfigDefinition("设置", "解除动作限制", "有些动作需要达成某些需求才能使用，激活此项可以在没达成需求的情况下也能使用那些动作。");
        internal BepInEx.Configuration.ConfigDefinition Config_DisableBodyShapeLock = new BepInEx.Configuration.ConfigDefinition("设置", "解除身高锁定", "每个人物都有一个身高值，区间0-1，默认0.5，系统会强制会把主角的身高强制设置为0.75，激活此项可以让系统在H场景中不强制设定身高。");
        void InitializeConfig() {
            Config.Bind<bool>(Config_DisableMotionLimit, true);
            Config.Bind<bool>(Config_DisableBodyShapeLock, false);
        }
    }

    internal static class ConfigValues {
        static internal bool DisableMotionLimit {
            get {
                return (bool)Main.Instance.Config[Main.Instance.Config_DisableMotionLimit].BoxedValue;
            }
        }
        static internal bool DisableBodyShapeChange {
            get {
                return (bool)Main.Instance.Config[Main.Instance.Config_DisableBodyShapeLock].BoxedValue;
            }
        }
    }
    internal static class InternalStaticFuntions {
        internal static void SwapReference<T>(ref T a, ref T b) {
            T medium = a;
            a = b;
            b = medium;
        }

        internal static void ModifyEverythingBack() {
            if (Patcher_ChangeAnimation_HScene.IsModified_cha) {
                Patcher_ChangeAnimation_HScene.ModifyBack();
            }
            if (Swap.IsModified) {
                Swap.Modify();
            }
            foreach (HMotionEyeNeckMale thing in Singleton<HScene>.Instance.ctrlEyeNeckMale) {
                if (thing != null) {
                    thing.NowEndADV = true;
                }
            }
        }
    }

    internal class Tester : MonoBehaviour {
        internal static Tester instance;
        internal static void Initialize() {
            instance = GameObject.Find("AI_YuriPlus").AddComponent<Tester>();
        }
        internal float a = 0;
        internal float b = 0;
    }
}
