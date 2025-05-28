﻿using Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BepInEx.Logging;
using ToolBox;
using ToolBox.Extensions;
using UILib;
using UILib.ContextMenu;
using UILib.EventHandlers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Type = System.Type;

#if IPA
using Harmony;
using IllusionPlugin;
#elif BEPINEX
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
#endif
#if KOIKATSU || SUNSHINE
using Expression = ExpressionBone;
using ExtensibleSaveFormat;
using Sideloader.AutoResolver;
#elif AISHOUJO || HONEYSELECT2
using CharaUtils;
using ExtensibleSaveFormat;
#endif

#if AISHOUJO || HONEYSELECT2
using AIChara;
#endif


namespace WorkspaceSupport
{
#if BEPINEX
    [BepInPlugin(GUID, Name, Version)]
#if KOIKATSU || SUNSHINE
    [BepInProcess("CharaStudio")]
#elif AISHOUJO || HONEYSELECT2
    [BepInProcess("StudioNEOV2")]
#endif
    [BepInDependency("com.bepis.bepinex.extendedsave")]
#endif
    public class WorkspaceSupport : GenericPlugin
#if IPA
                            , IEnhancedPlugin
#endif
    {
        #region Constants
        public const string Name = "WorkspaceSupport";
        public const string Version = "1.0.0";
        public const string GUID = "com.alton.illusionplugins.workspace";
        internal const string _ownerId = "WorkspaceSupport";
#if KOIKATSU || AISHOUJO || HONEYSELECT2
        private const int _saveVersion = 0;
        private const string _extSaveKey = "workspace_support";
#endif
        #endregion

#if IPA
        public override string Name { get { return _name; } }
        public override string Version { get { return _version; } }
        public override string[] Filter { get { return new[] { "StudioNEO_32", "StudioNEO_64" }; } }
#endif

        #region Private Types
        // private class HeaderDisplay
        // {
        //     public GameObject gameObject;
        //     public LayoutElement layoutElement;
        //     public RectTransform container;
        //     public Text name;
        //     public InputField inputField;

        //     public bool expanded = true;
        //     public GroupNode<InterpolableGroup> group;
        // }


        #endregion

        #region Private Variables

        internal static new ManualLogSource Logger;
        internal static WorkspaceSupport _self;

        internal static Dictionary<string, string> boneDict = new Dictionary<string, string>();

        private static string _assemblyLocation;
        private bool _loaded = false;

        #endregion

        #region Accessors
        internal static ConfigEntry<KeyboardShortcut> ConfigMainWindowShortcut { get; private set; }
        #endregion


        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            // ConfigMainWindowShortcut = Config.Bind("Config", "Open Timeline UI", new KeyboardShortcut(KeyCode.T, KeyCode.LeftControl));

            _self = this;
            Logger = base.Logger;

            _assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var harmonyInstance = HarmonyExtensions.CreateInstance(GUID);
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            boneDict["Part: Pony"] = "N_Hair_pony";
            boneDict["Part: Twin Tail (L)"] = "N_Hair_twin_L";
            boneDict["Part: Twin Tail (R)"] = "N_Hair_twin_R";
            boneDict["Part: Hairpin (L)"] = "N_Hair_pin_L";
            boneDict["Part: Hairpin (R)"] = "N_Hair_pin_R";
            boneDict["Part: Hat"] = "N_Hitai";
            boneDict["Part: Forehead"] = "N_Head_top";
            boneDict["Part: Head (Center)"] = "N_Head";
            boneDict["Part: Face"] = "N_Face";
            boneDict["Part: Ear (L)"] = "N_Earring_L";
            boneDict["Part: Ear (R)"] = "N_Earring_R";
            boneDict["Part: Glasses"] = "N_Face";
            boneDict["Part: Nose"] = "N_Nose";
            boneDict["Part: Mouth"] = "cf_J_MouthMove";
            boneDict["Part: Neck"] = "N_Neck";
            boneDict["Part: Upper Breasts"] = "N_Chest_f";
            boneDict["Part: Breasts Upper Center"] = "N_Chest_f";
            boneDict["Part: Breast (L)"] = "N_Tikubi_L";
            boneDict["Part: Breast (R)"] = "N_Tikubi_R";
            boneDict["Part: Back (Center)"] = "N_Back";
            boneDict["Part: Back (L)"] = "N_Back_L";
            boneDict["Part: Back (R)"] = "N_Back_R";
            boneDict["Part: Lower Back"] = "N_Waist_b";
            boneDict["Part: Hip (Front)"] = "N_Kokan";
            boneDict["Part: Hip (Back)"] = "N_Ana"; // cf_J_Hips N_Ana
            boneDict["Part: Hip (L)"] = "N_Tikubi_L";
            boneDict["Part: Hip (R)"] = "N_Tikubi_R";
            boneDict["Part: Left Bicep"] = "k_f_shoulderL_00";
            boneDict["Part: Elbow (L)"] = "N_Elbo_L";
            boneDict["Part: Upper Arm (L)"] = "N_Arm_L";
            boneDict["Part: Wrist (L)"] = "N_Wrist_L";
            boneDict["Part: Right Bicep"] = "k_f_shoulderR_00";
            boneDict["Part: Elbow (R)"] = "N_Elbo_R";
            boneDict["Part: Upper Arm (R)"] = "N_Arm_R";
            boneDict["Part: Wrist (R)"] = "N_Wrist_R";
            boneDict["Part: Hand (L)"] = "N_Hand_R";
            boneDict["Part: Index Finger (L)"] = "N_Index_L";
            boneDict["Part: Middle Finger (L)"] = "N_Middle_L";
            boneDict["Part: Ring Finger (L)"] = "N_Ring_L";
            boneDict["Part: Hand (R)"] = "N_Hand_L";
            boneDict["Part: Index Finger (R)"] = "N_Index_R";
            boneDict["Part: Middle Finger (R)"] = "N_Middle_R";
            boneDict["Part: Ring Finger (R)"] = "N_Ring_R";
            boneDict["Part: Thigh (L)"] = "N_Leg_L";
            boneDict["Part: Knee (L)"] = "N_Knee_L";
            boneDict["Part: Ankle (L)"] = "N_Ankle_L";
            boneDict["Part: Left Heel"] = "N_Foot_L";
            boneDict["Part: Thigh (R)"] = "N_Leg_R";
            boneDict["Part: Knee (R)"] = "N_Knee_R";
            boneDict["Part: Lower Belly ①"] = "N_Dan";
            boneDict["Part: Lower Belly ②"] = "N_Dan";
            boneDict["Part: Lower Belly ③"] = "N_Dan";
        }

#if HONEYSELECT
        protected override void LevelLoaded(int level)
        {
            if (level == 3)
                this.Init();
        }
#elif SUNSHINE || HONEYSELECT2 || AISHOUJO
        protected override void LevelLoaded(Scene scene, LoadSceneMode mode)
        {
            base.LevelLoaded(scene, mode);
            if (mode == LoadSceneMode.Single && scene.buildIndex == 2)
                Init();
        }

#elif KOIKATSU
        protected override void LevelLoaded(Scene scene, LoadSceneMode mode)
        {
            base.LevelLoaded(scene, mode);
            if (mode == LoadSceneMode.Single && scene.buildIndex == 1)
                Init();
        }
#endif

        protected override void Update()
        {
            if (_loaded == false)
                return;
        }

        private void PostLateUpdate()
        {
        }
        #endregion

        #region Public Methods

        #endregion

        #region Private Methods
        private void Init()
        {
            UIUtility.Init();
            _loaded = true;
        }
        #endregion

        #region Patches
        internal static TreeNodeObject FindRoot(TreeNodeObject node)
        {
            if (node == null)
            {
                return null;
            }

            if (node.parent == null)
            {
                return node; // 루트 도달
            }

            return FindRoot(node.parent); // 부모로 재귀
        }
        
        [HarmonyPatch(typeof(WorkspaceCtrl), nameof(WorkspaceCtrl.OnClickDelete))]
        internal static class WorkspaceCtrl_OnClickDelete_Patches
        {
            private static bool Prefix()
            {
                return true;
            }
        }

        [HarmonyPatch(typeof(WorkspaceCtrl), nameof(WorkspaceCtrl.OnSelectMultiple))]
        private static class WorkspaceCtrl_OnSelectMultiple_Patches
        {
            private static bool Prefix(object __instance)
            {
                if (Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectNodes.Length > 1)
                {
                    ObjectCtrlInfo objectCtrlInfo = null;

                    TreeNodeObject _base_node = Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectNodes[0];
                    TreeNodeObject _root = FindRoot(_base_node);

                    if (Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(_root, out objectCtrlInfo))
                    {
                        string value;
                        if (boneDict.TryGetValue(_base_node.m_TextName.text, out value))
                        {
                            _base_node.enableCopy = true;
                        }
                    } 
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(WorkspaceCtrl), nameof(WorkspaceCtrl.OnClickCopy))]
        private static class WorkspaceCtrl_OnClickCopy_Patches
        {
            private static void Postfix(object __instance)
            {
                ObjectCtrlInfo objectCtrlInfo = null;
                string value;

                TreeNodeObject _base_node = Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectNodes[0];
                TreeNodeObject _root = FindRoot(_base_node);

                if (Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(_root, out objectCtrlInfo))
                {
                    OCIChar _baseOciChar = objectCtrlInfo as OCIChar;
                    ChaControl charConctrol = _baseOciChar.charInfo;

                    //charConctrol.cmpBoneBody.targetAccessory.acs_Neck 

                    // this.targetAccessory.acs_Hair_pony = findAssist.GetTransformFromName("N_Hair_pony");

                    if (boneDict.TryGetValue(_base_node.m_TextName.text, out value))
                    {
                        string _value = value;
                        FindAssist findAssist = new FindAssist();
                        findAssist.Initialize(charConctrol.gameObject.transform);
                        Transform bone = findAssist.GetTransformFromName(_value);

                        foreach (TreeNodeObject node in Singleton<Studio.Studio>.Instance.treeNodeCtrl.selectNodes.Skip(0))
                        {
                            if (Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(node, out objectCtrlInfo))
                            {
                                objectCtrlInfo.guideObject.transformTarget.position = bone.position;
                            }
                        }

                        Singleton<Studio.Studio>.Instance.treeNodeCtrl.RemoveNode();
                    }
                }
            }
        }

#if KOIKATSU
        [HarmonyPatch(typeof(ShortcutKeyCtrl), "Update")]
        private static class ShortcutKeyCtrl_Update_Patches
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> instructionList = instructions.ToList();
                for (int i = 0; i < instructionList.Count; i++)
                {
                    CodeInstruction instruction = instructionList[i];
                    if (i != 0 && instruction.opcode == OpCodes.Call && instructionList[i - 1].opcode == OpCodes.Ldc_I4_S && (sbyte)instructionList[i - 1].operand == 99)
                        yield return new CodeInstruction(OpCodes.Call, typeof(ShortcutKeyCtrl_Update_Patches).GetMethod(nameof(PreventKeyIfCtrl), BindingFlags.NonPublic | BindingFlags.Static));
                    else
                        yield return instruction;
                }
            }

            private static bool PreventKeyIfCtrl(KeyCode key)
            {
                return Input.GetKey(KeyCode.LeftControl) == false && Input.GetKeyDown(key);
            }
        }
#endif
        #endregion
    }
}
