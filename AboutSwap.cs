using AI_YuriPlus.PatcherClasses_Main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AI_YuriPlus {
    static class Swap {
        static byte[] GetIconByteArray() {
            MemoryStream memoryStream = new MemoryStream();
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".Icon_Swap.png");
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        internal static bool CheckIconExist() {
            return (IconGameObject != null);
        }

        internal static GameObject IconGameObject;
        internal static void CreateIcon() {
            Transform transform_SubCategories = Singleton<Manager.HSceneManager>.Instance.HSceneUISet.transform.Find("Canvas").Find("CanvasGroup").Find("Categories").Find("SubCategories");
            transform_SubCategories.Translate(new Vector3(0, -0.08f));
            IconGameObject = UnityEngine.Object.Instantiate(transform_SubCategories.Find("atariLight").gameObject, transform_SubCategories);
            IconGameObject.name = "atariSwap";
            Texture2D NewTexture2D = new Texture2D(0, 0);
            ImageConversion.LoadImage(NewTexture2D, GetIconByteArray());
            Transform transform_Swap = IconGameObject.transform.Find("Light");
            transform_Swap.gameObject.name = "Swap";
            transform_Swap.Find("Icon").GetComponent<Image>().sprite = Sprite.Create(NewTexture2D, new Rect(0, 0, 519, 488), new Vector2(259.5f, 244));
            transform_Swap.Find("BG").Find("Text").GetComponent<Text>().text = "攻受交换";
            transform_Swap.gameObject.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            transform_Swap.gameObject.GetComponent<Button>().onClick.AddListener(OnClick_Swap);
        }

        internal static bool IsModified = false;

        static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        static AIChara.ChaControl[] chaFemales;
        static HScene hScene;
        static Manager.HSceneManager hSceneManager;

        static void OnClick_Swap() {
            hScene = Singleton<HScene>.Instance;
            hSceneManager = Singleton<Manager.HSceneManager>.Instance;
            chaFemales = hScene.GetFemales();
            Modify();
            hScene.StartCoroutine(enumerator(hSceneManager.females[0].ChaControl, hSceneManager.females[1].ChaControl));
            hScene.ctrlFlag.selectAnimationListInfo = hScene.ctrlFlag.nowAnimationInfo;
            hScene.ctrlFlag.nowAnimationInfo = (HScene.AnimationListInfo)(typeof(HScene.AnimationListInfo).GetMethod("MemberwiseClone", bindingFlags).Invoke(hScene.ctrlFlag.nowAnimationInfo, new object[] { }));
            hScene.ctrlFlag.nowAnimationInfo.nameAnimation = hScene.ctrlFlag.nowAnimationInfo.nameAnimation + "_Clone";
        }
        internal static void Modify() {
            bool NeedModify_Patcher_ChangeAnimation = false;
            if (Patcher_ChangeAnimation_HScene.IsModified_cha) {
                Patcher_ChangeAnimation_HScene.ModifyBack();
                NeedModify_Patcher_ChangeAnimation = true;
            } else {
                List<System.Tuple<int, int, MotionIK>> lstMotionIK = (List<System.Tuple<int, int, MotionIK>>)(typeof(HScene).GetField("lstMotionIK", bindingFlags).GetValue(Singleton<HScene>.Instance));
                lstMotionIK.Clear();
            }
            IsModified = !IsModified;
            InternalStaticFuntions.SwapReference<AIProject.Actor>(ref hSceneManager.females[0], ref hSceneManager.females[1]);
            //test();
            InternalStaticFuntions.SwapReference<AIChara.ChaControl>(ref chaFemales[0], ref chaFemales[1]);
            Debug.Log(1);
            Transform[] chaFemalesTrans = (Transform[])(typeof(HScene).GetField("chaFemalesTrans", bindingFlags).GetValue(hScene));
            InternalStaticFuntions.SwapReference<Transform>(ref chaFemalesTrans[0], ref chaFemalesTrans[1]);
            Debug.Log(1.1);
            InternalStaticFuntions.SwapReference<HitObjectCtrl>(ref hScene.ctrlHitObjectFemales[0], ref hScene.ctrlHitObjectFemales[1]);
            hScene.ctrlHitObjectMales[0] = hScene.ctrlHitObjectFemales[1];
            Debug.Log(2);
            hSceneManager.Personality[0] = chaFemales[0].chaFile.parameter.personality;
            FeelHit ctrlFeelHit = (FeelHit)(typeof(HScene).GetField("ctrlFeelHit", bindingFlags).GetValue(hScene));
            ctrlFeelHit.FeelHitInit(hSceneManager.Personality[0]);
            Debug.Log(3);
            InternalStaticFuntions.SwapReference<YureCtrl>(ref hScene.ctrlYures[0], ref hScene.ctrlYures[1]);
            hScene.ctrlYureMale.chaMale = chaFemales[1];
            Debug.Log(4);
            DynamicBoneReferenceCtrl[] ctrlDynamics = (DynamicBoneReferenceCtrl[])(typeof(HScene).GetField("ctrlDynamics", bindingFlags).GetValue(hScene));
            InternalStaticFuntions.SwapReference<DynamicBoneReferenceCtrl>(ref ctrlDynamics[0], ref ctrlDynamics[1]);
            //ctrlDynamics[0].Init(chaFemales[0]);
            //ctrlDynamics[1].Init(chaFemales[1]);
            Debug.Log(5);
            hSceneManager.PersonalPhase[0] = chaFemales[0].fileGameInfo.phase;
            Debug.Log(6);
            hScene.ctrlEyeNeckFemale[0].Init(chaFemales[0], 0);
            hScene.ctrlEyeNeckFemale[0].SetPartner(chaFemales[1].objBodyBone, null, chaFemales[1].objBodyBone);
            hScene.ctrlEyeNeckMale[0].Init(chaFemales[1], 0);
            hScene.ctrlEyeNeckMale[0].SetPartner(chaFemales[0].objBodyBone, null, null);
            hScene.hMotionEyeNeckLesP.Init(chaFemales[1], 1);
            hScene.hMotionEyeNeckLesP.SetPartner(chaFemales[0].objBodyBone);
            Debug.Log(7);
            InternalStaticFuntions.SwapReference<SiruPasteCtrl>(ref hScene.ctrlSiruPastes[0], ref hScene.ctrlSiruPastes[1]);
            Debug.Log(8);
            
            if (NeedModify_Patcher_ChangeAnimation) {
                Patcher_ChangeAnimation_HScene.Modify();
            }
        }

        static int i = 0;
        static void test() {
            Transform[] chaFemalesTrans;
            Debug.Log(i);
            switch (i) {
                case 0:
                    InternalStaticFuntions.SwapReference<AIChara.ChaControl>(ref chaFemales[0], ref chaFemales[1]);
                    break;
                case 1:
                    chaFemalesTrans = (Transform[])(typeof(HScene).GetField("chaFemalesTrans", bindingFlags).GetValue(hScene));
                    InternalStaticFuntions.SwapReference<Transform>(ref chaFemalesTrans[0], ref chaFemalesTrans[1]);
                    break;
                case 2:
                    hSceneManager.Personality[0] = chaFemales[0].chaFile.parameter.personality;
                    FeelHit ctrlFeelHit = (FeelHit)(typeof(HScene).GetField("ctrlFeelHit", bindingFlags).GetValue(hScene));
                    ctrlFeelHit.FeelHitInit(hSceneManager.Personality[0]);
                    break;
                case 3:
                    InternalStaticFuntions.SwapReference<YureCtrl>(ref hScene.ctrlYures[0], ref hScene.ctrlYures[1]);
                    hScene.ctrlYureMale.chaMale = chaFemales[1];
                    break;
                case 4:
                    DynamicBoneReferenceCtrl[] ctrlDynamics = (DynamicBoneReferenceCtrl[])(typeof(HScene).GetField("ctrlDynamics", bindingFlags).GetValue(hScene));
                    InternalStaticFuntions.SwapReference<DynamicBoneReferenceCtrl>(ref ctrlDynamics[0], ref ctrlDynamics[1]);
                    break;
                case 5:
                    hSceneManager.PersonalPhase[0] = chaFemales[0].fileGameInfo.phase;
                    break;
                case 6:
                    hScene.ctrlEyeNeckFemale[0].Init(chaFemales[0], 0);
                    hScene.ctrlEyeNeckFemale[0].SetPartner(chaFemales[1].objBodyBone, null, chaFemales[1].objBodyBone);
                    hScene.ctrlEyeNeckMale[0].Init(chaFemales[1], 0);
                    hScene.ctrlEyeNeckMale[0].SetPartner(chaFemales[0].objBodyBone, null, null);
                    hScene.hMotionEyeNeckLesP.Init(chaFemales[1], 1);
                    hScene.hMotionEyeNeckLesP.SetPartner(chaFemales[0].objBodyBone);
                    break;
                case 7:
                    InternalStaticFuntions.SwapReference<SiruPasteCtrl>(ref hScene.ctrlSiruPastes[0], ref hScene.ctrlSiruPastes[1]);
                    break;
            }
            if (i > 7) {
                i = 0;
            } else {
                i++;
            }
        }

        static IEnumerator enumerator(AIChara.ChaControl chaFemale0, AIChara.ChaControl chaFemale1) {
            yield return hScene.ctrlVoice.Init(hSceneManager.Personality[0], chaFemale0.fileParam.voicePitch, hSceneManager.females[0], 0, 0f, null, -1, false, false);
            hScene.ctrlFlag.voice.voiceTrs[0] = chaFemale0.cmpBoneBody.targetEtc.trfHeadParent;
            hScene.ctrlFlag.voice.voiceTrs[1] = chaFemale1.cmpBoneBody.targetEtc.trfHeadParent;

        }
    }
}
