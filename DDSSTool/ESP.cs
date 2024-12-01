using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DDSSTool.Panels;
using Il2CppPlayer;
using Il2CppPlayer.Lobby;
using UnityEngine;

namespace DDSSTool
{
    public class ESP
    {
        private static bool gotOriginalMaterial = false;
        private static Shader originalShader = null;

        public static List<string> playerRoles = new List<string>()
        {
            "None",
            "Specialist",
            "Slacker",
            "Manager",
            "Janitor",
            "Saboteur"
        };

        public static void playerESP()
        {
            try
            {
                foreach (var player in UnityEngine.Object.FindObjectsOfType<PlayerController>())
                {
                    if (player.isLocalPlayer) continue;
                    Vector3 playerPos = player.transform.position;
                    Vector3 w2s = Camera.main.WorldToScreenPoint(playerPos);
                    Vector3 headPos = Camera.main.WorldToScreenPoint(player.headBone.transform.position);
                    Renderer[] renderers = player.GetComponentsInChildren<Renderer>();

                    float distance = Vector3.Distance(ToolMain.localPlayer.transform.position, playerPos);
                    Color roleColor = (int)player.lobbyPlayer.playerRole != 2 ? Color.green : Color.red;

                    if (w2s.z > 0f)
                    {
                        GUI.color = roleColor;
                        Render.DrawString(new Vector2(w2s.x, (float)Screen.height - w2s.y), player.lobbyPlayer.username + " [" + Mathf.Round(distance).ToString() + "m]", Color.white, true, 14);
                        GUI.color = roleColor;
                        Render.DrawString(new Vector2(w2s.x, (float)Screen.height - w2s.y + 15f), playerRoles[(int)player.lobbyPlayer.playerRole], roleColor, true, 14);
                        float height = Mathf.Abs(headPos.y - w2s.y);
                        
                        if (ESPPanel.cornerBox.isOn) {
                            GUI.color = roleColor;
                            Render.CornerBox(new Vector2(headPos.x, (float)Screen.height - headPos.y - 40f), height / 2f, height + 40f, 2f, roleColor, true);
                        }

                        if (ESPPanel.snapline.isOn) {
                            GUI.color = roleColor;
                            if (ESPPanel.centerSnap.isOn) {
                                Render.DrawLine(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(w2s.x, Screen.height - w2s.y), 1f, roleColor);
                            }
                            else {
                                Render.DrawLine(new Vector2(Screen.width / 2, Screen.height), new Vector2(w2s.x, Screen.height - w2s.y), 1f, roleColor);
                            }
                        }

                        foreach (Renderer renderer in renderers)
                        {
                            foreach (var material in renderer.materials)
                            {
                                if (ESPPanel.playerChams.isOn)
                                {
                                    if (!gotOriginalMaterial)
                                    {
                                        originalShader = material.shader;
                                        gotOriginalMaterial = true;
                                    }

                                    material.shader = null;
                                    material.SetColor("_ColorVisible", Color.green);
                                    material.SetColor("_ColorBehin", Color.yellow);
                                } 
                                else
                                {
                                    if (gotOriginalMaterial)
                                    {
                                        material.shader = originalShader;
                                    }
                                }
                            }
                        }

                        if (gotOriginalMaterial && !ESPPanel.playerChams.isOn)
                        {
                            gotOriginalMaterial = false;
                        }
                    }
                }
            }
            catch
            {
                return;
            }

            try
            {
                foreach (var player in UnityEngine.Object.FindObjectsOfType<PlayerController>())
                {
                    if (player.isLocalPlayer) continue;
                    Renderer renderer = player.gameObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        if (renderer.material.shader != null)
                        {
                            renderer.material.shader = null;
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
    }
}
