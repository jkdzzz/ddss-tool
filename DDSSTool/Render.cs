using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DDSSTool
{
    public static class Render
    {
        // Token: 0x06000010 RID: 16 RVA: 0x00002400 File Offset: 0x00000600
        public static bool IsOnScreen(Vector3 position)
        {
            return position.y > 0.01f && position.y < (float)Screen.height - 5f && position.z > 0.01f;
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000011 RID: 17 RVA: 0x00002432 File Offset: 0x00000632
        // (set) Token: 0x06000012 RID: 18 RVA: 0x00002439 File Offset: 0x00000639
        public static Color Color
        {
            get
            {
                return GUI.color;
            }
            set
            {
                GUI.color = value;
            }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002441 File Offset: 0x00000641
        public static void DrawLine(Vector2 from, Vector2 to, float thickness, Color color)
        {
            Render.Color = color;
            Render.DrawLine(from, to, thickness);
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002454 File Offset: 0x00000654
        public static void DrawLine(Vector2 from, Vector2 to, float thickness)
        {
            Vector2 normalized = (to - from).normalized;
            float num = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
            GUIUtility.RotateAroundPivot(num, from);
            Render.DrawBox(from, Vector2.right * (from - to).magnitude, thickness, false);
            GUIUtility.RotateAroundPivot(-num, from);
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000024B8 File Offset: 0x000006B8
        public static void CornerBox(Vector2 Head, float Width, float Height, float thickness, Color color, bool outline)
        {
            int num = (int)(Width / 4f);
            int num2 = num;
            if (outline)
            {
                Render.RectFilled(Head.x - Width / 2f - 1f, Head.y - 1f, (float)(num + 2), 3f, Color.black);
                Render.RectFilled(Head.x - Width / 2f - 1f, Head.y - 1f, 3f, (float)(num2 + 2), Color.black);
                Render.RectFilled(Head.x + Width / 2f - (float)num - 1f, Head.y - 1f, (float)(num + 2), 3f, Color.black);
                Render.RectFilled(Head.x + Width / 2f - 1f, Head.y - 1f, 3f, (float)(num2 + 2), Color.black);
                Render.RectFilled(Head.x - Width / 2f - 1f, Head.y + Height - 4f, (float)(num + 2), 3f, Color.black);
                Render.RectFilled(Head.x - Width / 2f - 1f, Head.y + Height - (float)num2 - 4f, 3f, (float)(num2 + 2), Color.black);
                Render.RectFilled(Head.x + Width / 2f - (float)num - 1f, Head.y + Height - 4f, (float)(num + 2), 3f, Color.black);
                Render.RectFilled(Head.x + Width / 2f - 1f, Head.y + Height - (float)num2 - 4f, 3f, (float)(num2 + 3), Color.black);
            }
            Render.RectFilled(Head.x - Width / 2f, Head.y, (float)num, 1f, color);
            Render.RectFilled(Head.x - Width / 2f, Head.y, 1f, (float)num2, color);
            Render.RectFilled(Head.x + Width / 2f - (float)num, Head.y, (float)num, 1f, color);
            Render.RectFilled(Head.x + Width / 2f, Head.y, 1f, (float)num2, color);
            Render.RectFilled(Head.x - Width / 2f, Head.y + Height - 3f, (float)num, 1f, color);
            Render.RectFilled(Head.x - Width / 2f, Head.y + Height - (float)num2 - 3f, 1f, (float)num2, color);
            Render.RectFilled(Head.x + Width / 2f - (float)num, Head.y + Height - 3f, (float)num, 1f, color);
            Render.RectFilled(Head.x + Width / 2f, Head.y + Height - (float)num2 - 3f, 1f, (float)(num2 + 1), color);
        }

        // Token: 0x06000016 RID: 22 RVA: 0x000027C4 File Offset: 0x000009C4
        internal static void RectFilled(float x, float y, float width, float height, Color color)
        {
            if (!Render.drawingTex)
            {
                Render.drawingTex = new Texture2D(1, 1);
            }
            if (color != Render.lastTexColour)
            {
                Render.drawingTex.SetPixel(0, 0, color);
                Render.drawingTex.Apply();
                Render.lastTexColour = color;
            }
            GUI.DrawTexture(new Rect(x, y, width, height), Render.drawingTex);
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002829 File Offset: 0x00000A29
        public static void DrawBox(Vector2 position, Vector2 size, float thickness, Color color, bool centered = true)
        {
            Render.Color = color;
            Render.DrawBox(position, size, thickness, centered);
        }

        // Token: 0x06000018 RID: 24 RVA: 0x0000283C File Offset: 0x00000A3C
        public static void DrawBox(Vector2 position, Vector2 size, float thickness, bool centered = true)
        {
            if (centered)
            {
                position -= size / 2f;
            }
            GUI.DrawTexture(new Rect(position.x, position.y, size.x, thickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(position.x, position.y, thickness, size.y), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(position.x + size.x, position.y, thickness, size.y), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(position.x, position.y + size.y, size.x + thickness, thickness), Texture2D.whiteTexture);
        }

        // Token: 0x06000019 RID: 25 RVA: 0x000028F6 File Offset: 0x00000AF6
        public static void DrawCross(Vector2 position, Vector2 size, float thickness, Color color)
        {
            Render.Color = color;
            Render.DrawCross(position, size, thickness);
        }

        // Token: 0x0600001A RID: 26 RVA: 0x00002908 File Offset: 0x00000B08
        public static void DrawCross(Vector2 position, Vector2 size, float thickness)
        {
            GUI.DrawTexture(new Rect(position.x - size.x / 2f, position.y, size.x, thickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(position.x, position.y - size.y / 2f, thickness, size.y), Texture2D.whiteTexture);
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00002973 File Offset: 0x00000B73
        public static void DrawDot(Vector2 position, Color color)
        {
            Render.Color = color;
            Render.DrawDot(position);
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00002981 File Offset: 0x00000B81
        public static void DrawDot(Vector2 position)
        {
            Render.DrawBox(position - Vector2.one, Vector2.one * 2f, 1f, true);
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000029A8 File Offset: 0x00000BA8
        public static void DrawString(Vector2 pos, string text, Color color, bool center = true, int size = 12, FontStyle fontStyle = FontStyle.Bold, int depth = 1)
        {
            Render.__style.fontSize = size;
            Render.__style.richText = true;
            Render.__style.normal.textColor = color;
            Render.__style.fontStyle = fontStyle;
            Render.__outlineStyle.fontSize = size;
            Render.__outlineStyle.richText = true;
            Render.__outlineStyle.normal.textColor = new Color(0f, 0f, 0f, 1f);
            Render.__outlineStyle.fontStyle = fontStyle;
            GUIContent guicontent = new GUIContent(text);
            GUIContent guicontent2 = new GUIContent(text);
            if (center)
            {
                pos.x -= Render.__style.CalcSize(guicontent).x / 2f;
            }
            switch (depth)
            {
                case 0:
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), guicontent, Render.__style);
                    return;
                case 1:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), guicontent2, Render.__outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), guicontent, Render.__style);
                    return;
                case 2:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), guicontent2, Render.__outlineStyle);
                    GUI.Label(new Rect(pos.x - 1f, pos.y - 1f, 300f, 25f), guicontent2, Render.__outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), guicontent, Render.__style);
                    return;
                case 3:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), guicontent2, Render.__outlineStyle);
                    GUI.Label(new Rect(pos.x - 1f, pos.y - 1f, 300f, 25f), guicontent2, Render.__outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y - 1f, 300f, 25f), guicontent2, Render.__outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y + 1f, 300f, 25f), guicontent2, Render.__outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), guicontent, Render.__style);
                    return;
                default:
                    return;
            }
        }

        // Token: 0x0600001E RID: 30 RVA: 0x00002C6A File Offset: 0x00000E6A
        public static void DrawCircle(Vector2 position, float radius, int numSides, bool centered = true, float thickness = 1f)
        {
            Render.DrawCircle(position, radius, numSides, Color.white, centered, thickness);
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00002C7C File Offset: 0x00000E7C
        public static void DrawCircle(Vector2 position, float radius, int numSides, Color color, bool centered = true, float thickness = 1f)
        {
            Render.RingArray ringArray;
            if (Render.ringDict.ContainsKey(numSides))
            {
                ringArray = Render.ringDict[numSides];
            }
            else
            {
                ringArray = (Render.ringDict[numSides] = new Render.RingArray(numSides));
            }
            Vector2 vector = centered ? position : (position + Vector2.one * radius);
            for (int i = 0; i < numSides - 1; i++)
            {
                Render.DrawLine(vector + ringArray.Positions[i] * radius, vector + ringArray.Positions[i + 1] * radius, thickness, color);
            }
            Render.DrawLine(vector + ringArray.Positions[0] * radius, vector + ringArray.Positions[ringArray.Positions.Length - 1] * radius, thickness, color);
        }

        // Token: 0x0400000D RID: 13
        private static Dictionary<int, Render.RingArray> ringDict = new Dictionary<int, Render.RingArray>();

        // Token: 0x0400000E RID: 14
        private static GUIStyle __style = new GUIStyle();

        // Token: 0x0400000F RID: 15
        private static GUIStyle __outlineStyle = new GUIStyle();

        // Token: 0x04000010 RID: 16
        private static Texture2D drawingTex;

        // Token: 0x04000011 RID: 17
        private static Color lastTexColour;

        // Token: 0x02000009 RID: 9
        private class RingArray
        {
            // Token: 0x17000002 RID: 2
            // (get) Token: 0x06000022 RID: 34 RVA: 0x00002D83 File Offset: 0x00000F83
            // (set) Token: 0x06000023 RID: 35 RVA: 0x00002D8B File Offset: 0x00000F8B
            public Vector2[] Positions { get; private set; }

            // Token: 0x06000024 RID: 36 RVA: 0x00002D94 File Offset: 0x00000F94
            public RingArray(int numSegments)
            {
                this.Positions = new Vector2[numSegments];
                float num = 360f / (float)numSegments;
                for (int i = 0; i < numSegments; i++)
                {
                    float num2 = 0.017453292f * num * (float)i;
                    this.Positions[i] = new Vector2(Mathf.Sin(num2), Mathf.Cos(num2));
                }
            }
        }
    }
}
