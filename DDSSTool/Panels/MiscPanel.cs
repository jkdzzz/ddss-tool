using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using Il2CppGameManagement;
using Il2CppPlayer;
using Il2CppProps.FireAlarm;
using Il2CppProps.TrashBin;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace DDSSTool.Panels
{
    public class MiscPanel : UniverseLib.UI.Panels.PanelBase
    {
        public MiscPanel(UIBase owner) : base(owner) { }
        public override string Name => "Misc Settings";
        public override int MinWidth => 50;
        public override int MinHeight => 70;
        public override Vector2 DefaultAnchorMin => new Vector2(0.1f, 0.25f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.3f, 0.5f);
        public override bool CanDragAndResize => true;

        public static Toggle cosmeticUnlocker;
        public static Toggle changeSpeed;
        public static Slider movementSpeed;
        public static Toggle spinBot;
        public static Toggle thirdPerson;
        private Text speedSliderText;

        protected override void ConstructPanelContent()
        {
            ButtonRef dupeSelfBtn = UIFactory.CreateButton(ContentRoot, "dupeBtn", "Dupe Self");
            UIFactory.SetLayoutElement(dupeSelfBtn.GameObject, minHeight: 25, flexibleWidth: 30);
            dupeSelfBtn.OnClick = duplicateSelf;

            GameObject cameraToggle = UIFactory.CreateToggle(ContentRoot, "cameraToggle", out thirdPerson, out Text cameraText);
            UIFactory.SetLayoutElement(cameraToggle.gameObject, minHeight: 25, minWidth: 9999);
            thirdPerson.isOn = false;
            cameraText.text = "Third Person";

            GameObject spinBotObj = UIFactory.CreateToggle(ContentRoot, "spinBotObj", out spinBot, out Text spinBotText);
            UIFactory.SetLayoutElement(spinBotObj, minHeight: 25, flexibleWidth: 9999);
            spinBot.isOn = false;
            spinBotText.text = "Spinbot";

            ButtonRef testBtn = UIFactory.CreateButton(ContentRoot, "testBtn", "Test");
            UIFactory.SetLayoutElement(testBtn.GameObject, minHeight: 25, flexibleWidth: 30);
            testBtn.OnClick = testFunction;

            GameObject cosmeticUnlockObj = UIFactory.CreateToggle(ContentRoot, "Cosmetic Unlocker", out cosmeticUnlocker, out Text cosmeticText);
            UIFactory.SetLayoutElement(cosmeticUnlockObj, minHeight: 25, flexibleWidth: 9999);
            cosmeticUnlocker.isOn = true;
            cosmeticText.text = "Cosmetic Unlocker";

            ButtonRef forceManager = UIFactory.CreateButton(ContentRoot, "startGame", "Force Manager");
            UIFactory.SetLayoutElement(forceManager.GameObject, minHeight: 25, flexibleWidth: 9999);
            forceManager.OnClick = forceManagerClicked;

            GameObject speedToggle = UIFactory.CreateToggle(ContentRoot, "Speedhack", out changeSpeed, out Text speedText);
            UIFactory.SetLayoutElement(speedToggle, minHeight: 25, flexibleWidth: 9999);
            changeSpeed.isOn = false;
            speedText.text = "Speedhack";
            
            
            GameObject speedSlider = UIFactory.CreateSlider(ContentRoot, "speedSlider", out movementSpeed);
            movementSpeed.wholeNumbers = true;
            movementSpeed.value = 2f;
            movementSpeed.minValue = 2f;
            movementSpeed.maxValue = 10f;

            speedSliderText = UIFactory.CreateLabel(ContentRoot, "speedText", "Movement Speed: " + Mathf.Round(movementSpeed.value).ToString());
            movementSpeed.onValueChanged.AddListener(moveSpeedChanged);
            UIFactory.SetLayoutElement(speedSliderText.gameObject, minHeight: 25, minWidth: 9999);
            UIFactory.SetLayoutElement(speedSlider, minHeight: 25, flexibleWidth: 30);

            this.SetActive(true);
        }

        private void duplicateSelf()
        {

        }

        private void testFunction()
        {
            foreach (var bin in UnityEngine.Object.FindObjectsOfType<TrashBin>())
            {
                bin.EnableFireAlarm(true);
                bin.isOnFire = true;
                bin.RpcEnableFire(ToolMain.localPlayer.netIdentity, true);
            }
        }

        private void forceManagerClicked()
        {
            try
            {

                foreach (var manager in UnityEngine.Object.FindObjectsOfType<LobbyManager>())
                {
                    manager.CmdForceManagerRole(ToolMain.localLobbyPlayer.netIdentity);
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        private void moveSpeedChanged(float value)
        {
            speedSliderText.text = "Movement Speed: " + Mathf.Round(value).ToString();
        }
    }
}
