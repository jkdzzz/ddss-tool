using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDSSTool.Panels;
using Il2Cpp;
using Il2CppPlayer.Scripts;
using Il2CppUI.Tabs.CustomizeTab;
using MelonLoader;
using UnityEngine;
using UniverseLib.UI;
using HarmonyLib;
using Il2CppMirror;
using Il2CppPlayer;
using Il2CppGameManagement;
using Il2CppPlayer.Lobby;
using Unity.VisualScripting;
using Unity.Mathematics.Geometry;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DDSSTool
{
    public class ToolMain : MelonMod
    {
        public static UIBase UiBase { get; private set; }
        
        public static GUIStyle style;
        
        public static bool isInitialized = false;

        public static MainPanel mainPanel;
        public static ESPPanel espPanel;
        public static MiscPanel miscPanel;
        public static TrollPanel trollPanel;
        public static DocumentPanel documentPanel;
        public static LobbyPanel lobbyPanel;
        
        public static PlayerController localPlayer;
        public static LobbyPlayer localLobbyPlayer;

        public static Camera newCam;

        private Vector3 defaultCameraOffset = new Vector3(0, 2, -5); // Default offset (before zoom)
        private float rotationSpeed = 2.0f;
        private float verticalRotationLimit = 25.0f;
        private float verticalRotation = 0.0f;
        private float horizontalRotation = 0.0f;

        // Zoom variables
        private float currentZoom = 5.0f;
        private float minZoom = 2.0f;
        private float maxZoom = 10.0f;
        private float zoomSpeed = 2.0f;

        private HarmonyLib.Harmony _cameraPatch;

        public override void OnLateInitializeMelon()
        {
            LoggerInstance.Msg("Thanks for using my menu! <3");
            ToolEntry();
        }

        public override void OnInitializeMelon()
        {
            _cameraPatch = new HarmonyLib.Harmony("zain.tool.cameraPatch");
            if (MiscPanel.thirdPerson.isOn)
            {
                patchCamera();
            }
        }

        private void patchCamera()
        {
            var originalMethod = typeof(CameraController).GetMethod("Update");
            var prefixMethod = typeof(CameraPatch).GetMethod("Prefix");
            _cameraPatch.Patch(originalMethod, prefix: new HarmonyMethod(prefixMethod));
        }

        public override void OnUpdate()
        {
            try
            {
                foreach (PlayerController player in UnityEngine.Object.FindObjectsOfType<PlayerController>())
                {
                    if (player.isLocalPlayer)
                        localPlayer = player;
                }
            } 
            catch (Exception e)
            {

            }

            try
            {
                foreach (LobbyPlayer player in UnityEngine.Object.FindObjectsOfType<LobbyPlayer>())
                {
                    if (player.isLocalPlayer)
                        localLobbyPlayer = player;
                }
            }
            catch (Exception e)
            {

            }

            if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.F8))
            {
                UiBase.Enabled = !UiBase.Enabled;
            }

            if (MiscPanel.cosmeticUnlocker.isOn)
            {
                try
                {
                    foreach (CustomizationItem customizationItem in UnityEngine.Object.FindObjectsOfType<CustomizationItem>())
                    {
                        customizationItem.price = 0;
                    }
                    foreach (ColorItem colorItem in UnityEngine.Object.FindObjectsOfType<ColorItem>())
                    {
                        colorItem.price = 0;
                    }
                    foreach (ProductItem productItem in UnityEngine.Object.FindObjectsOfType<ProductItem>())
                    {
                        productItem.Order();
                    }
                    foreach (CustomizeTab customizeTab in UnityEngine.Object.FindObjectsOfType<CustomizeTab>())
                    {
                        customizeTab.UpdateCustomization();
                        customizeTab.Update();
                        customizeTab.UpdateTab();
                    }
                }
                catch (Exception ex)
                {

                }
            }

            if (TrollPanel.spankSpamToggle.isOn)
            {
                try
                {
                    TrollPanel.spankAllPlayers();
                }
                catch (Exception e)
                {
                    TrollPanel.spankSpamToggle.isOn = false;
                }
            }

            if (TrollPanel.assistantSpamToggle.isOn)
            {
                try
                {
                    TrollPanel.makeEveryoneAssistant();
                }
                catch (Exception e)
                {
                    TrollPanel.assistantSpamToggle.isOn = false;
                }
            }

            if (MiscPanel.changeSpeed.isOn)
            {
                try
                {
                    ToolMain.localPlayer.moveSpeed = Mathf.Round(MiscPanel.movementSpeed.value);
                }
                catch (Exception e)
                {
                    MiscPanel.changeSpeed.isOn = false;
                }
            } 
            else
            {
                try
                {
                    ToolMain.localPlayer.moveSpeed = 2.2f;
                }
                catch
                {

                }
            }

            if (MiscPanel.spinBot.isOn)
            {
                try
                {
                }
                catch
                {
                    MiscPanel.spinBot.isOn = false;
                }
            }

            if (LobbyPanel.keepOnSelf.isOn)
            {
                try
                {
                    LobbyPanel.tpAllSelf();
                }
                catch
                {
                    LobbyPanel.keepOnSelf.isOn = false;
                }
            }

            if (TrollPanel.tpToVoid.isOn)
            {
                try
                {
                    foreach (var player in UnityEngine.Object.FindObjectsOfType<PlayerController>())
                    {
                        if (!player.isLocalPlayer || TrollPanel.tpLocal.isOn)
                        {
                            player.CmdMovePlayer(new Vector3(999, 0, 999));
                        }
                    }
                }
                catch
                {
                    TrollPanel.tpToVoid.isOn = false;
                }
            }

            if (LobbyPanel.spamChat.isOn)
            {
                try
                {
                    LobbyPanel.spamChatFnc();
                }
                catch
                {
                    LobbyPanel.spamChat.isOn = false;
                }
            }

            if (TrollPanel.slackerEveryoneToggle.isOn)
            {
                try
                {
                    TrollPanel.setSlacker();
                }
                catch
                {
                    TrollPanel.slackerEveryoneToggle.isOn = false;
                }
            }
        }

        public override void OnLateUpdate()
        {
            if (MiscPanel.thirdPerson.isOn)
            {
                try
                {
                    UpdateCameraPosition();
                    HandleCameraControls();
                }
                catch
                {
                    MiscPanel.thirdPerson.isOn = false;
                }
            }
        }

        private void RotatePlayerWithCamera()
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            localPlayer.transform.rotation = Quaternion.LookRotation(cameraForward);
        }

        private void HandleCameraControls()
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            // Horizontal rotation (yaw)
            horizontalRotation += mouseX * rotationSpeed;

            // Vertical rotation (pitch)
            verticalRotation -= mouseY * rotationSpeed;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

            // Zoom control
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            localPlayer.transform.rotation = Camera.main.transform.rotation;
        }

        private void UpdateCameraPosition()
        {

            // Start with the default offset
            Vector3 offset = defaultCameraOffset.normalized * currentZoom;

            // Apply rotation around the player
            Quaternion horizontalRotationQuaternion = Quaternion.Euler(0, horizontalRotation, 0);
            Quaternion verticalRotationQuaternion = Quaternion.Euler(verticalRotation, 0, 0);

            // Combine rotations
            Quaternion finalRotation = horizontalRotationQuaternion * verticalRotationQuaternion;

            // Rotate the offset
            Vector3 rotatedOffset = finalRotation * offset;

            // Calculate the new camera position relative to the player
            Vector3 desiredPosition = localPlayer.transform.position + rotatedOffset;

            // Smoothly interpolate the camera position
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, desiredPosition, Time.deltaTime * 5);

            // Make the camera look at the player
            Camera.main.transform.LookAt(localPlayer.transform.position + Vector3.up * 1.5f);
        }

        void ToolEntry()
        {
            float startupDelay = 2f;
            UniverseLib.Config.UniverseLibConfig config = new UniverseLib.Config.UniverseLibConfig()
            {
                Disable_EventSystem_Override = false, // or null
                Force_Unlock_Mouse = true, // or null
            };

            UniverseLib.Universe.Init(startupDelay, Universe_OnInitialized, LogHandler, config);
        }

        void Universe_OnInitialized()
        {
            UiBase = UniversalUI.RegisterUI("zain.tool", UiUpdate);
            UiBase.Enabled = true;

            mainPanel = new MainPanel(UiBase);
            espPanel = new ESPPanel(UiBase);
            miscPanel = new MiscPanel(UiBase);
            trollPanel = new TrollPanel(UiBase);
            documentPanel = new DocumentPanel(UiBase);
            lobbyPanel = new LobbyPanel(UiBase);

            espPanel.SetActive(true);
            miscPanel.SetActive(true);
            trollPanel.SetActive(true);
            documentPanel.SetActive(true);
            lobbyPanel.SetActive(true);

            isInitialized = true;
            LoggerInstance.Msg("UI fully initialized! No more errors should be appearing!\nEnjoy the game! <3");

            //ThirdPersonCamera cam = newCam.AddComponent<ThirdPersonCamera>();
            //GameObject CameraController = new GameObject();
            //CameraController.AddComponent<CameraSwitcher>();
        }

        public override void OnGUI()
        {
            if (!isInitialized) return;
            if (!UiBase.Enabled && Event.current.type != EventType.Repaint) return;

            string watermark = "DDSS Tool | v2.0.0";
            if (!UiBase.Enabled)
                watermark += " | press DELETE or F8";

            Color darkBackground = new Color(23f / 255f, 23f / 255f, 23f / 255f, 1f);

            GUI.backgroundColor = darkBackground;
            GUI.contentColor = Color.white;

            style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontStyle = FontStyle.Bold;

            GUI.color = Color.white;

            Render.DrawString(new Vector2(10f, 5f), watermark, GUI.color, false);

            if (ESPPanel.playerEsp.isOn)
            {
                ESP.playerESP();
            }
        }

        void UiUpdate()
        {

        }

        void LogHandler(string message, LogType type)
        {
            // ...
        }


        [HarmonyPatch(typeof(CameraController), "Update")]
        public class CameraPatch
        {
            public static bool Prefix(ref bool __runOriginal)
            {
                if (MiscPanel.thirdPerson.isOn)
                {
                    __runOriginal = false;
                    return false;
                }

                __runOriginal = true;
                return true;
            }
        }
    }
}