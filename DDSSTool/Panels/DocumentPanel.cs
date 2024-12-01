using System;
using Il2Cpp;
using Il2CppGameManagement;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppObjects.Scripts;
using Il2CppPlayer;
using Il2CppPlayer.PlayerEffects;
using Il2CppProps;
using Il2CppProps.ServerRack;
using Il2CppProps.TrashBin;
using Il2CppProps.VendingMachine;
using Il2CppSystem;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace DDSSTool.Panels
{
    public class DocumentPanel : UniverseLib.UI.Panels.PanelBase
    {
        public DocumentPanel(UIBase owner) : base(owner) { }
        public override string Name => "Document Sender";
        public override int MinWidth => 50;
        public override int MinHeight => 70;
        public override Vector2 DefaultAnchorMin => new Vector2(0.1f, 0.25f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.45f, 0.6f);
        public override bool CanDragAndResize => true;

        private string documentName = "";
        private string documentText = "";
        private int documentAmount = 1;

        protected override void ConstructPanelContent()
        {
            Text infoText = UIFactory.CreateLabel(ContentRoot, "infoText", "Gives everyone a document. {player}, {role}, and \\n are valid to use in the text field.");
            UIFactory.SetLayoutElement(infoText.gameObject, minHeight: 25, flexibleWidth: 30);

            Text nameText = UIFactory.CreateLabel(ContentRoot, "nameText", "Document Name");
            UIFactory.SetLayoutElement(nameText.gameObject, minHeight: 25, flexibleWidth: 10);
            InputFieldRef documentNameInput = UIFactory.CreateInputField(ContentRoot, "documentNameInput", "// Custom Name");
            UIFactory.SetLayoutElement(documentNameInput.GameObject, minHeight: 25, flexibleWidth: 10);
            documentNameInput.OnValueChanged += documentNameChanged;

            Text customTextText = UIFactory.CreateLabel(ContentRoot, "customTextText", "Custom Text");
            UIFactory.SetLayoutElement(customTextText.gameObject, minHeight: 25, flexibleWidth: 10);
            InputFieldRef documentTextInput = UIFactory.CreateInputField(ContentRoot, "documentTextInput", "// Custom Document Text");
            UIFactory.SetLayoutElement(documentTextInput.GameObject, minHeight: 45, flexibleWidth: 15);
            documentTextInput.OnValueChanged += documentTextChanged;

            Text amountText = UIFactory.CreateLabel(ContentRoot, "amountText", "Amount of Documents: ");
            UIFactory.SetLayoutElement(amountText.gameObject, minHeight: 25, flexibleWidth: 30);
            InputFieldRef documentAmountInput = UIFactory.CreateInputField(ContentRoot, "documentAmountInput", "1");
            UIFactory.SetLayoutElement(documentAmountInput.GameObject, minHeight: 25, flexibleWidth: 15);
            documentAmountInput.OnValueChanged += documentAmountChanged;

            ButtonRef sendDocumentsBtn = UIFactory.CreateButton(ContentRoot, "sendDocumentsBtn", "Send Documents");
            UIFactory.SetLayoutElement(sendDocumentsBtn.GameObject, minHeight: 25, flexibleWidth: 15);
            sendDocumentsBtn.OnClick = sendDocumentClicked;

            ButtonRef crashGameBtn = UIFactory.CreateButton(ContentRoot, "crashGameBtn", "Crash Lobby");
            UIFactory.SetLayoutElement(crashGameBtn.GameObject, minHeight: 25, flexibleWidth: 15);
            crashGameBtn.OnClick = crashLobbyClicked;

            this.SetActive(true);
        }

        private void crashLobbyClicked()
        {
            this.sendDocuments("lobby_crasher_by_reniaz", "lobby crashed with DDSS Tool by reniaz\nPublished on unknowncheats.me\n<3 <3 <3", 99999);
        }

        private void sendDocumentClicked()
        {
            this.sendDocuments(this.documentName, this.documentText, this.documentAmount);
        }

        private void sendDocuments(string documentName, string documentText, int documentAmount)
        {
            GameManager gameManager = UnityEngine.Object.FindObjectsOfType<GameManager>()[0];
            Il2CppArrayBase<Binder> il2CppArrayBase = UnityEngine.Object.FindObjectsOfType<Binder>();
            Il2CppArrayBase<PlayerController> il2CppArrayBase2 = UnityEngine.Object.FindObjectsOfType<PlayerController>();
            for (int i = 0; i < documentAmount; i++)
            {
                foreach (PlayerController playerController in il2CppArrayBase2)
                {
                    string text = documentText.Replace("{player}", playerController.lobbyPlayer.Networkusername).Replace("{role}", playerController.lobbyPlayer.NetworkplayerRole.ToString());
                    il2CppArrayBase[0].CmdGrabDocument(documentName, text, playerController.lobbyPlayer.playerController);
                }
            }
        }

        private void documentTextChanged(string text)
        {
            try
            {
                this.documentText = text;
            }
            catch (System.Exception e)
            {
                this.documentText = "https://www.unknowncheats.me/forum/unity/673690-dale-dawson-stationery-supplies-tool.html";
            }
        }
        private void documentAmountChanged(string text)
        {
            try
            {
                this.documentAmount = int.Parse(text);
            }
            catch (System.Exception e)
            {
                this.documentAmount = 0;
            }
        }

        private void documentNameChanged(string text)
        {
            try
            {
                this.documentName = text;
            }
            catch (System.Exception e)
            {
                this.documentName = "haha";
            }
        }
    }
}
