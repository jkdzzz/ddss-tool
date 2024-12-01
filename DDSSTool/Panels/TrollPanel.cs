using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using Il2CppGameManagement;
using Il2CppGameManagement.StateMachine;
using Il2CppMirror;
using Il2CppObjects.Scripts;
using Il2CppPlayer;
using Il2CppPlayer.Lobby;
using Il2CppPlayer.PlayerEffects;
using Il2CppProps.ServerRack;
using Il2CppProps.VendingMachine;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace DDSSTool.Panels
{
    public class TrollPanel : UniverseLib.UI.Panels.PanelBase
    {
        public TrollPanel(UIBase owner) : base(owner) { }
        public override string Name => "Troll Menu";
        public override int MinWidth => 50;
        public override int MinHeight => 70;
        public override Vector2 DefaultAnchorMin => new Vector2(0.1f, 0.25f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.3f, 0.75f);
        public override bool CanDragAndResize => true;

        public static Toggle spankSpamToggle;
        public static Toggle assistantSpamToggle;
        public static Toggle tpToVoid;
        public static Toggle tpLocal;
        public static Toggle slackerEveryoneToggle;

        private GameObject tpLocalToggle;

        protected override void ConstructPanelContent()
        {
            GameObject slackerToggle = UIFactory.CreateToggle(ContentRoot, "slackerToggle", out slackerEveryoneToggle, out Text slackerText);
            UIFactory.SetLayoutElement(slackerToggle, minHeight: 25, minWidth: 30);
            slackerEveryoneToggle.isOn = false;
            slackerText.text = "Everyone Slacker Spam";

            GameObject tpVoidToggle = UIFactory.CreateToggle(ContentRoot, "voidToggle", out tpToVoid, out Text voidText);
            UIFactory.SetLayoutElement(tpVoidToggle, minHeight: 25, flexibleWidth: 30);
            tpToVoid.onValueChanged.AddListener(tpVoidChanged);
            tpToVoid.isOn = false;
            voidText.text = "Tp To Void";

            tpLocalToggle = UIFactory.CreateToggle(ContentRoot, "localToggle", out tpLocal, out Text localText);
            UIFactory.SetLayoutElement(tpLocalToggle, minHeight: 25, flexibleWidth: 30);
            tpLocalToggle.SetActive(false);
            tpLocal.isOn = true;
            localText.text = "Tp Self";

            GameObject spankToggle = UIFactory.CreateToggle(ContentRoot, "spankToggle", out spankSpamToggle, out Text spankText);
            UIFactory.SetLayoutElement(spankToggle, minHeight: 25, flexibleWidth: 30);
            spankSpamToggle.isOn = false;
            spankText.text = "Spam Spank All (VERY LOUD)";

            GameObject assistantToggle = UIFactory.CreateToggle(ContentRoot, "assistantToggle", out assistantSpamToggle, out Text assistantText);
            UIFactory.SetLayoutElement(assistantToggle, minHeight: 25, flexibleWidth: 30);
            assistantSpamToggle.isOn = false;
            assistantText.text = "Spam Everyone Assistant (VERY LOUD)";

            ButtonRef slackerEveryone = UIFactory.CreateButton(ContentRoot, "slackerBtn", "Slacker Everyone");
            UIFactory.SetLayoutElement(slackerEveryone.GameObject, minHeight: 25, flexibleWidth: 30);
            slackerEveryone.OnClick = setSlacker;

            ButtonRef startMeeting = UIFactory.CreateButton(ContentRoot, "startMettingBtn", "Start Meeting");
            UIFactory.SetLayoutElement(startMeeting.GameObject, minHeight: 25, flexibleWidth: 30);
            startMeeting.OnClick = startMeetingClick;

            ButtonRef everyoneDrunk = UIFactory.CreateButton(ContentRoot, "everyoneDrunkBtn", "Everyone Drunk");
            UIFactory.SetLayoutElement(everyoneDrunk.GameObject, minHeight: 25, flexibleWidth: 30);
            everyoneDrunk.OnClick = everyoneDrunkClicked;

            ButtonRef everyoneSpeed = UIFactory.CreateButton(ContentRoot, "everyoneSpeedBtn", "Everyone Speedboost");
            UIFactory.SetLayoutElement(everyoneSpeed.GameObject, minHeight: 25, flexibleWidth: 30);
            everyoneSpeed.OnClick = everyoneSpeedClicked;

            ButtonRef vendRandom = UIFactory.CreateButton(ContentRoot, "vendRandomBtn", "Random Vending Machine Item");
            UIFactory.SetLayoutElement(vendRandom.GameObject, minHeight: 25, flexibleWidth: 30);
            vendRandom.OnClick = vendRandomClicked;

            ButtonRef spankAll = UIFactory.CreateButton(ContentRoot, "spankAllBtn", "Spank All");
            UIFactory.SetLayoutElement(spankAll.GameObject, minHeight: 25, flexibleWidth: 30);
            spankAll.OnClick = spankAllPlayers;

            ButtonRef everyoneAssistant = UIFactory.CreateButton(ContentRoot, "everyoneAssistantBtn", "Everyone Assistant");
            UIFactory.SetLayoutElement(everyoneAssistant.GameObject, minHeight: 25, flexibleWidth: 30);
            everyoneAssistant.OnClick = makeEveryoneAssistant;

            ButtonRef selfAssistant = UIFactory.CreateButton(ContentRoot, "selfAssistantBtn", "Self Assistant");
            UIFactory.SetLayoutElement(selfAssistant.GameObject, minHeight: 25, flexibleWidth: 30);
            selfAssistant.OnClick = makeSelfAssistant;

            ButtonRef stickyNoteEveryone = UIFactory.CreateButton(ContentRoot, "stickyNoteBtn", "Sticky Note Everyone");
            UIFactory.SetLayoutElement(stickyNoteEveryone.GameObject, minHeight: 25, flexibleWidth: 30);
            stickyNoteEveryone.OnClick = stickyAll;

            ButtonRef jellyAllBtn = UIFactory.CreateButton(ContentRoot, "jellyAllBtn", "Jelly Everyone");
            UIFactory.SetLayoutElement(jellyAllBtn.GameObject, minHeight: 25, flexibleWidth: 30);
            jellyAllBtn.OnClick = jellyAll;

            ButtonRef banAllBtn = UIFactory.CreateButton(ContentRoot, "banAllBtn", "Ban All");
            UIFactory.SetLayoutElement(banAllBtn.GameObject, minHeight: 25, flexibleWidth: 30);
            banAllBtn.OnClick = banAll;

            ButtonRef serverOutageBtn = UIFactory.CreateButton(ContentRoot, "serverOutageBtn", "Server Outage [WIP]");
            UIFactory.SetLayoutElement(serverOutageBtn.GameObject, minHeight: 25, flexibleWidth: 30);
            serverOutageBtn.OnClick = serverOutage;

            ButtonRef lockDoors = UIFactory.CreateButton(ContentRoot, "lockDoorsBtn", "Lock Doors [WIP]");
            UIFactory.SetLayoutElement(lockDoors.GameObject, minHeight: 25, flexibleWidth: 30);
            lockDoors.OnClick = lockAllDoors;

            this.SetActive(true);
        }

        public static void setSlacker()
        {
            foreach (var player in UnityEngine.Object.FindObjectsOfType<PlayerController>())
            {
                player.lobbyPlayer.RpcFirePlayer(true, true, player.lobbyPlayer.playerRole);
                //foreach (var manager in UnityEngine.Object.FindObjectsOfType<GameManager>())
                //{
                //    manager.RpcDisplayNewRoles(ToolMain.localPlayer.netIdentity, player.netIdentity);
                //}
            }
        }

        private void startMeetingClick()
        {
            foreach (var test in UnityEngine.Object.FindObjectsOfType<GameManager>())
            {
                test.ServerCallMeeting();
            }
        }

        private void tpVoidChanged(bool active)
        {
            tpLocalToggle.SetActive(active);
        }

        private void banAll()
        {
            LobbyManager manager = UnityEngine.Object.FindObjectOfType<LobbyManager>();
            foreach (LobbyPlayer lobbyPlayer in UnityEngine.Object.FindObjectsOfType<LobbyPlayer>())
            {
                manager.BlackListPlayer(lobbyPlayer);
            }
        }

        private void serverOutage()
        {
            foreach (ServerController controller in UnityEngine.Object.FindObjectsOfType<ServerController>())
            {
                controller.CmdSetConnectionEnabled(controller.netIdentity, false);
                controller.RpcSetConnectionEnabled(controller.netIdentity, false);
            }
        }

        private void jellyAll()
        {
            foreach (WorkStationController workStationController in UnityEngine.Object.FindObjectsOfType<WorkStationController>())
            {
                workStationController.EnableJelly(ToolMain.localPlayer.netIdentity, true);
            }
        }

        private void everyoneDrunkClicked()
        {
            foreach (PlayerEffectController playerEffectController in UnityEngine.Object.FindObjectsOfType<PlayerEffectController>())
            {
                playerEffectController.GetComponent<PlayerController>();
                playerEffectController.CmdSetEffect(2, 1000f);
            }
        }

        private void everyoneSpeedClicked()
        {
            foreach (PlayerEffectController playerEffectController in UnityEngine.Object.FindObjectsOfType<PlayerEffectController>())
            {
                playerEffectController.GetComponent<PlayerController>();
                playerEffectController.CmdSetEffect(1, 1000f);
            }
        }

        private void vendRandomClicked()
        {
            VendingMachine vendingMachine = UnityEngine.Object.FindObjectOfType<VendingMachine>();
            for (int i = 0; i < 8; i++)
            {
                vendingMachine.CmdSpawnItem(ToolMain.localPlayer.netIdentity, i % 3);
            }
        }

        private void stickyAll()
        {
            foreach (StickyNoteController stickyNoteController in UnityEngine.Object.FindObjectsOfType<StickyNoteController>())
            {
                stickyNoteController.contentText.text = "Greetings from reniaz <3";
                foreach (PlayerController player in UnityEngine.Object.FindObjectsOfType<PlayerController>())
                {
                    if (player.isLocalPlayer)
                        stickyNoteController.RpcSetText("Greetings from reniaz <3");
                }
                stickyNoteController.UpdateCollectible();
            }
        }

        private void lockAllDoors()
        {
            foreach (DoorController doorController in UnityEngine.Object.FindObjectsOfType<DoorController>())
            {
                foreach (PlayerController player in UnityEngine.Object.FindObjectsOfType<PlayerController>())
                {
                    doorController.CmdSetLockState(player.netIdentity, true);
                }
            }
        }

        public static void makeEveryoneAssistant()
        {
            foreach (PlayerController playerController in UnityEngine.Object.FindObjectsOfType<PlayerController>())
            {
                playerController.lobbyPlayer.CmdSetSubRole(Il2CppPlayer.Lobby.SubRole.Assistant);
            }
        }

        private void makeSelfAssistant()
        {
            ToolMain.localLobbyPlayer.CmdSetSubRole(Il2CppPlayer.Lobby.SubRole.Assistant);
        }

        public static void spankAllPlayers()
        {
            foreach (PlayerController playerController in UnityEngine.Object.FindObjectsOfType<PlayerController>())
            {
                playerController.CmdSpank(playerController.lobbyPlayer.playerController);
            }
        }
    }
}
