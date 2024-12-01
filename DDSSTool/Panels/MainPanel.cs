using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.Input;
using UniverseLib.UI;

namespace DDSSTool.Panels
{
    public class MainPanel : UniverseLib.UI.Panels.PanelBase
    {
        public MainPanel(UIBase owner) : base(owner) { }
        public override string Name => "DDSS Tool";
        public override int MinWidth => 100;
        public override int MinHeight => 200;
        public override Vector2 DefaultAnchorMin => new Vector2(0.1f, 0.25f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.3f, 0.45f);
        public override bool CanDragAndResize => true;

        public static Toggle showEspPanel;
        public static Toggle showMiscPanel;
        public static Toggle showTrollPanel;
        public static Toggle showDocumentPanel;
        public static Toggle showLobbyPanel;

        protected override void ConstructPanelContent()
        {
            GameObject espPanelObj = UIFactory.CreateToggle(ContentRoot, "ESP Settings", out showEspPanel, out Text espText);
            UIFactory.SetLayoutElement(espPanelObj, minHeight: 25, flexibleWidth: 9999);
            showEspPanel.isOn = true;
            espText.text = "ESP Settings";
            showEspPanel.onValueChanged.AddListener(OnShowEspChanged);

            GameObject cosmeticToggleObj = UIFactory.CreateToggle(ContentRoot, "Misc Settings", out showMiscPanel, out Text cosmeticText);
            UIFactory.SetLayoutElement(cosmeticToggleObj, minHeight: 25, flexibleWidth: 9999);
            showMiscPanel.isOn = true;
            cosmeticText.text = "Misc Settings";
            showMiscPanel.onValueChanged.AddListener(OnShowMiscChanged);

            GameObject trollPanelObj = UIFactory.CreateToggle(ContentRoot, "Troll Menu", out showTrollPanel, out Text trollText);
            UIFactory.SetLayoutElement(trollPanelObj, minHeight: 25, flexibleWidth: 9999);
            showTrollPanel.isOn = true;
            trollText.text = "Troll Menu";
            showTrollPanel.onValueChanged.AddListener(OnShowTrollChanged);

            GameObject documentPanelObj = UIFactory.CreateToggle(ContentRoot, "Document Sender", out showDocumentPanel, out Text documentText);
            UIFactory.SetLayoutElement(documentPanelObj, minHeight: 25, flexibleWidth: 9999);
            showDocumentPanel.isOn = true;
            documentText.text = "Document Sender";
            showDocumentPanel.onValueChanged.AddListener(OnShowDocumentChanged);

            GameObject lobbyPanelObj = UIFactory.CreateToggle(ContentRoot, "Lobby Info [WIP]", out showLobbyPanel, out Text lobbyText);
            UIFactory.SetLayoutElement(lobbyPanelObj, minHeight: 25, flexibleWidth: 9999);
            showLobbyPanel.isOn = true;
            lobbyText.text = "Lobby Info [WIP]";
            showLobbyPanel.onValueChanged.AddListener(OnShowLobbyChanged);
        }

        private void OnShowDocumentChanged(bool active)
        {
            ToolMain.documentPanel.SetActive(active);
        }

        private void OnShowEspChanged(bool active)
        {
            ToolMain.espPanel.SetActive(active);
        }

        private void OnShowMiscChanged(bool active)
        {
            ToolMain.miscPanel.SetActive(active);
        }

        private void OnShowTrollChanged(bool active)
        {
            ToolMain.trollPanel.SetActive(active);
        }

        private void OnShowLobbyChanged(bool active)
        {
            ToolMain.lobbyPanel.SetActive(active);
        }
    }
}
