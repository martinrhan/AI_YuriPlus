using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace AI_YuriPlus.PatcherClasses_ClothAndAccessory {
    static class Patcher_LateUpdate_HScene {
        static internal bool IsDuring = false;

        [HarmonyPatch(typeof(HScene), "LateUpdate")]
        [HarmonyPrefix]
        static bool Prefix() {
            IsDuring = true;
            return true;
        }

        [HarmonyPatch(typeof(HScene), "LateUpdate")]
        [HarmonyPostfix]
        static void Postfix() {
            IsDuring = false;
        }
    }

    static class Patcher_SetClothesState_AIChara {
        [HarmonyPatch(typeof(AIChara.ChaControl), nameof(AIChara.ChaControl.SetClothesState))]
        [HarmonyPrefix]
        static bool Prefix() {
            if (Patcher_LateUpdate_HScene.IsDuring) {
                return false;
            }
            return true;
        }
    }

    static class Patcher_SetMaleSelectBtn_HSceneSpriteChaChoice {
        [HarmonyPatch(typeof(HSceneSpriteChaChoice),nameof(HSceneSpriteChaChoice.SetMaleSelectBtn))]
        [HarmonyPrefix]
        static bool Prefix(ref bool setVal) {
            setVal = false;
            return true;
        }
    }

    static class ClothAndAccessory {
        static internal void InitialApply() {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            ConfigScene.HSystem hdata = Manager.Config.HData;
            Manager.HSceneManager hSceneManager = Singleton<Manager.HSceneManager>.Instance;
            foreach (int clothesKind in new List<int> { 0, 1, 2, 3, 5, 6 }) {
                if (hSceneManager.Player.ChaControl.IsClothesStateKind(clothesKind)) {
                    byte state = 0;
                    if (!hdata.Cloth) {
                        state = 2;
                    }
                    hSceneManager.Player.ChaControl.SetClothesState(clothesKind, state, true);
                }
                hSceneManager.Player.ChaControl.SetAccessoryStateAll(hdata.Accessory);
                hSceneManager.Player.ChaControl.SetClothesState(7, (!hdata.Shoes) ? Convert.ToByte(2) : Convert.ToByte(0), true);
            }
            //Debug.Log("a");
            AIChara.ChaControl[] females_Cloth = new AIChara.ChaControl[2];
            females_Cloth[0] = hSceneManager.females[0].ChaControl;
            females_Cloth[1] = hSceneManager.females[1].ChaControl;
            typeof(HSceneSpriteClothCondition).GetField("females", bindingFlags).SetValue(Singleton<HSceneSprite>.Instance.objCloth, females_Cloth);
            Singleton<HSceneSprite>.Instance.objCloth.SetClothCharacter(false);
            //Debug.Log("b");
            AIChara.ChaControl[] females_Accessory = new AIChara.ChaControl[2];
            females_Accessory[0] = hSceneManager.females[0].ChaControl;
            females_Accessory[1] = hSceneManager.females[1].ChaControl;
            typeof(HSceneSpriteAccessoryCondition).GetField("females", bindingFlags).SetValue(Singleton<HSceneSprite>.Instance.objAccessory, females_Accessory);
            typeof(HSceneSpriteAccessoryCondition).GetField("Males", bindingFlags).SetValue(Singleton<HSceneSprite>.Instance.objAccessory, new AIChara.ChaControl[2]);
            Singleton<HSceneSprite>.Instance.objAccessory.SetAccessoryCharacter(false);
            //Debug.Log("c");
            AIChara.ChaControl[] females_ClothCard = new AIChara.ChaControl[2];
            females_ClothCard[0] = hSceneManager.females[0].ChaControl;
            females_ClothCard[1] = hSceneManager.females[1].ChaControl;
            typeof(HSceneSpriteCoordinatesCard).GetField("femailes", bindingFlags).SetValue(Singleton<HSceneSprite>.Instance.objClothCard, females_ClothCard);
            //the programmer fucking spelled it wrong
        }
    }
}
