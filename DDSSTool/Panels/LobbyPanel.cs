using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using Il2CppGameManagement;
using Il2CppPlayer;
using Il2CppPlayer.Lobby;
using Il2CppProps.Easel;
using Il2CppUMUI;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using static System.Net.Mime.MediaTypeNames;
using Text = UnityEngine.UI.Text;

namespace DDSSTool.Panels
{
    public class LobbyPanel : UniverseLib.UI.Panels.PanelBase
    {
        public LobbyPanel(UIBase owner) : base(owner) { }
        public override string Name => "Lobby Info [WIP]";
        public override int MinWidth => 50;
        public override int MinHeight => 70;
        public override Vector2 DefaultAnchorMin => new Vector2(0.1f, 0.25f);
        public override Vector2 DefaultAnchorMax => new Vector2(0.3f, 0.75f);
        public override bool CanDragAndResize => true;
        private int index = 0;

        public static List<PlayerController> players = new List<PlayerController>();
        public static List<LobbyPlayer> lobbyPlayers = new List<LobbyPlayer>();
        private List<ButtonRef> buttonRefs = new List<ButtonRef>();

        public static Toggle keepOnSelf;
        public static Toggle spamChat;

        protected override void ConstructPanelContent()
        {
            GameObject keepTeleport = UIFactory.CreateToggle(ContentRoot, "keepTpToggle", out keepOnSelf, out Text keepTpText);
            UIFactory.SetLayoutElement(keepTeleport, minHeight: 25, flexibleWidth: 9999);
            keepOnSelf.isOn = false;
            keepTpText.text = "Keep Players on Self";

            ButtonRef teleportAllToSelf = UIFactory.CreateButton(ContentRoot, "allToSelfBtn", "Teleport all to self");
            UIFactory.SetLayoutElement(teleportAllToSelf.GameObject, minHeight: 25, flexibleWidth: 9999);
            teleportAllToSelf.OnClick = tpAllSelf;

            ButtonRef getPlayersBtn = UIFactory.CreateButton(ContentRoot, "getPlayersBtn", "Load Players [ TP ]");
            UIFactory.SetLayoutElement(getPlayersBtn.GameObject, minHeight: 25, flexibleWidth: 9999);
            getPlayersBtn.OnClick = loadAllPlayers;

            ButtonRef getPlayersBtnF = UIFactory.CreateButton(ContentRoot, "getPlayersBtnF", "Load Players [ FIRE ]");
            UIFactory.SetLayoutElement(getPlayersBtnF.GameObject, minHeight: 25, flexibleWidth: 9999);
            getPlayersBtnF.OnClick = loadAllPlayersFire;

            GameObject spamChatToggle = UIFactory.CreateToggle(ContentRoot, "chatToggle", out spamChat, out Text chatText);
            UIFactory.SetLayoutElement(spamChatToggle, minHeight: 25, flexibleWidth: 9999);
            spamChat.isOn = false;
            chatText.text = "Spam Chat";

            this.SetActive(true);
        }

        public static void tpAllSelf()
        {
            players = new List<PlayerController>(UnityEngine.Object.FindObjectsOfType<PlayerController>());
            foreach (var player in players)
            {
                if (!player.isLocalPlayer)
                {
                    player.CmdMovePlayer(ToolMain.localPlayer.transform.position);
                }
            }
        }

        private void loadAllPlayers()
        {
            if (buttonRefs.Count != 0)
            {
                foreach (var button in buttonRefs)
                {
                    button.GameObject.SetActive(false);
                }
            }

            buttonRefs.Clear();
            index = 0;

            players = new List<PlayerController>(UnityEngine.Object.FindObjectsOfType<PlayerController>());

            foreach (var player in players)
            {
                ButtonRef playerTpBtn = UIFactory.CreateButton(ContentRoot, "playerTpBtn", player.lobbyPlayer.steamUsername);
                playerTpBtn.GameObject.SetActive(false);
                buttonRefs.Add(playerTpBtn);
            }

            foreach (ButtonRef button in buttonRefs)
            {
                UIFactory.SetLayoutElement(button.GameObject, minHeight: 25, flexibleWidth: 9999);
                button.GameObject.SetActive(true);

                int capturedIndex = index;

                button.OnClick = () => {
                    teleportToPlayer(capturedIndex);
                };

                index++;
            }
        }

        private void loadAllPlayersFire()
        {
            if (buttonRefs.Count != 0)
            {
                foreach (var button in buttonRefs)
                {
                    button.GameObject.SetActive(false);
                }
            }

            buttonRefs.Clear();
            index = 0;

            lobbyPlayers = new List<LobbyPlayer>(UnityEngine.Object.FindObjectsOfType<LobbyPlayer>());

            foreach (var lobbyPlayer in lobbyPlayers)
            {
                ButtonRef playerTpBtn = UIFactory.CreateButton(ContentRoot, "playerTpBtn", lobbyPlayer.steamUsername);
                playerTpBtn.GameObject.SetActive(false);
                buttonRefs.Add(playerTpBtn);
            }

            foreach (ButtonRef button in buttonRefs)
            {
                UIFactory.SetLayoutElement(button.GameObject, minHeight: 25, flexibleWidth: 9999);
                button.GameObject.SetActive(true);

                int capturedIndex = index;

                button.OnClick = () => {
                    firePlayer(capturedIndex);
                };

                index++;
            }
        }

        private void teleportToPlayer(int i)
        {
            var players = UnityEngine.Object.FindObjectsOfType<PlayerController>();
            ToolMain.localPlayer.MovePlayer(players[i].transform.position);
        }

        private void firePlayer(int i)
        {
            var lobbyPlayers = UnityEngine.Object.FindObjectsOfType<LobbyPlayer>();
            foreach (var manager in UnityEngine.Object.FindObjectsOfType<LobbyManager>())
            {
                manager.CmdSendChatMessage(lobbyPlayers[i].netIdentity, "hacked by " + lobbyPlayers[i].steamUsername, "69:420");
            }
        }

        public static void spamChatFnc()
        {
            var lobbyPlayers = UnityEngine.Object.FindObjectsOfType<LobbyPlayer>();
            foreach (var manager in UnityEngine.Object.FindObjectsOfType<LobbyManager>())
            {
                foreach (var lobbyPlayer in lobbyPlayers)
                {
                    manager.CmdSendChatMessage(lobbyPlayer.netIdentity, "hacked by " + lobbyPlayer.steamUsername, "69:420");
                }
            }
        }
    }
}
