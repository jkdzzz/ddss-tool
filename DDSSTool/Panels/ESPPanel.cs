using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;

namespace DDSSTool.Panels
{
    public class ESPPanel : UniverseLib.UI.Panels.PanelBase
    {
        public ESPPanel(UIBase owner) : base(owner) { }
        public override string Name => "ESP Settings";
        public override int MinWidth => 50;
        public override int MinHeight => 70;
        public override Vector2 DefaultAnchorMin => new Vector2(0.1f, 0.25f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.3f, 0.45f);
        public override bool CanDragAndResize => true;
        public static Toggle playerEsp;
        public static Toggle playerChams;
        public static Toggle cornerBox;
        public static Toggle snapline;
        public static Toggle centerSnap;

        private GameObject cornerBoxToggleObj;
        private GameObject snaplineToggleObj;
        private GameObject centerSnapToggleObj;

        protected override void ConstructPanelContent()
        {
            GameObject playerEspToggleObj = UIFactory.CreateToggle(ContentRoot, "Player ESP", out playerEsp, out Text playerEspText);
            UIFactory.SetLayoutElement(playerEspToggleObj, minHeight: 25, flexibleWidth: 9999);
            playerEsp.onValueChanged.AddListener(showPlayerOptions);
            playerEsp.isOn = true;
            playerEspText.text = "Player Esp";

            GameObject playerChamsToggleObj = UIFactory.CreateToggle(ContentRoot, "Chams", out playerChams, out Text playerChamsText);
            UIFactory.SetLayoutElement(playerChamsToggleObj, minHeight: 25, flexibleWidth: 9999);
            playerChams.isOn = false;
            playerChamsText.text = "Chams";

            cornerBoxToggleObj = UIFactory.CreateToggle(ContentRoot, "Corner Box", out cornerBox, out Text cornerBoxText);
            UIFactory.SetLayoutElement(cornerBoxToggleObj, minHeight: 25, flexibleWidth: 9999);
            cornerBoxToggleObj.SetActive(true);
            cornerBox.isOn = false;
            cornerBoxText.text = "Corner Box";

            snaplineToggleObj = UIFactory.CreateToggle(ContentRoot, "Snapline", out snapline, out Text snaplineText);
            UIFactory.SetLayoutElement(snaplineToggleObj, minHeight: 25, flexibleWidth: 9999);
            snapline.onValueChanged.AddListener(showLineOptions);
            snaplineToggleObj.SetActive(true);
            snapline.isOn = false;
            snaplineText.text = "Snapline";

            centerSnapToggleObj = UIFactory.CreateToggle(ContentRoot, "Center Line", out centerSnap, out Text centerLineText);
            UIFactory.SetLayoutElement(centerSnapToggleObj, minHeight: 25, flexibleWidth: 9999);
            centerSnapToggleObj.SetActive(false);
            centerSnap.isOn = false;
            centerLineText.text = "Center Line";

            this.SetActive(true);
        }

        private void showPlayerOptions(bool active)
        {
            cornerBoxToggleObj.SetActive(active);
            snaplineToggleObj.SetActive(active);
        }

        private void showLineOptions(bool active)
        {
            centerSnapToggleObj.SetActive(active);
        }
    }
}
