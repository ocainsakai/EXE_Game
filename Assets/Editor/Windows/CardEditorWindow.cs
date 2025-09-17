using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CardSystem
{
    public class CardEditorWindow : EditorWindow
    {
        private bool multitab;
        private Vector2 sidebarScroll;
        private Vector2 contentScroll;
        private float sidebarWidth = 150f;            // có thể resize
        private const float splitterWidth = 4f;       // kích thước vùng draggable
        private bool isResizing = false;              // trạng thái drag
        private readonly float minSidebar = 100f;
        private readonly float maxSidebar = 400f;
        // Trạng thái tab nào đang mở
        private Dictionary<CardEditorTabs, bool> openedTabs = new Dictionary<CardEditorTabs, bool>();

        // Các tab instance
        private CardManagerTab managerTab = new CardManagerTab();
        private CardGeneratorTab generatorTab = new CardGeneratorTab();
        private CardOverviewTab overviewTab = new CardOverviewTab();
        private PokerHandTestTab pokerTestTab = new PokerHandTestTab();

        [MenuItem("Tools/Card Editor")]
        public static void ShowWindow()
        {
            GetWindow<CardEditorWindow>("Card Editor");
        }

        private void OnEnable()
        {
            // Khởi tạo trạng thái đóng hết
            CloseAll();
            openedTabs[CardEditorTabs.Manager] = true;
            multitab = false;
        }

        private void CloseAll()
        {
            foreach (CardEditorTabs tab in System.Enum.GetValues(typeof(CardEditorTabs)))
            {
                if (!openedTabs.ContainsKey(tab))
                    openedTabs.Add(tab, false);
            }
        }
        private void HandleSplitterEvents()
        {
            var e = Event.current;
            // vị trí splitter world-space
            float totalPadding = 8f;
            Rect splitterRect = new Rect(totalPadding + sidebarWidth, totalPadding, splitterWidth, position.height - totalPadding * 2f);

            if (e.type == EventType.MouseDown && splitterRect.Contains(e.mousePosition) && e.button == 0)
            {
                isResizing = true;
                e.Use();
            }

            if (isResizing)
            {
                if (e.type == EventType.MouseDrag)
                {
                    // cập nhật sidebarWidth theo chuột
                    float newWidth = e.mousePosition.x - totalPadding;
                    sidebarWidth = Mathf.Clamp(newWidth, minSidebar, maxSidebar);
                    Repaint();
                    e.Use();
                }
                else if (e.type == EventType.MouseUp)
                {
                    isResizing = false;
                    e.Use();
                    // lưu nếu muốn: EditorPrefs.SetFloat("CardEditor.SidebarWidth", sidebarWidth);
                }
            }
        }

        private void OnGUI()
        {
            HandleSplitterEvents();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(4);
            EditorGUILayout.BeginVertical();

            EditorGUI.BeginChangeCheck();
            bool newMulti = EditorGUILayout.ToggleLeft("Multi Tab", multitab, GUILayout.Width(140));
            if (EditorGUI.EndChangeCheck())
            {
                multitab = newMulti;

                if (!multitab)
                {
                    // Nếu vừa tắt multi, đảm bảo chỉ 1 tab mở: giữ cái đầu tiên còn true, đóng lại các cái khác
                    CardEditorTabs? first = null;
                    foreach (var kvp in openedTabs)
                    {
                        if (kvp.Value)
                        {
                            first = kvp.Key;
                            break;
                        }
                    }

                    if (first == null)
                    {
                        openedTabs[CardEditorTabs.Manager] = true;
                        foreach (var k in new List<CardEditorTabs>(openedTabs.Keys))
                            if (k != CardEditorTabs.Manager) openedTabs[k] = false;
                    }
                    else
                    {
                        foreach (var k in new List<CardEditorTabs>(openedTabs.Keys))
                            openedTabs[k] = (k == first.Value);
                    }
                }
            }

            EditorGUILayout.Space(6);

            // === SIDEBAR (danh sách tab) ===
            sidebarScroll = EditorGUILayout.BeginScrollView(sidebarScroll, GUILayout.Width(150), GUILayout.Height(300));
            foreach (CardEditorTabs tab in System.Enum.GetValues(typeof(CardEditorTabs)))
            {
                bool current = openedTabs.ContainsKey(tab) && openedTabs[tab];

                EditorGUI.BeginChangeCheck();
                bool toggled = GUILayout.Toggle(current, tab.ToString(), "Button");
                if (EditorGUI.EndChangeCheck())
                {
                    // Nếu đang ở single-mode và người dùng cố click lại tab đang active để tắt,
                    // thì ta ngăn hành động đó (không cho tắt).
                    if (!multitab && current && !toggled)
                    {
                        toggled = true;
                    }

                    if (!toggled)
                    {
                        // Người dùng muốn đóng tab -> chỉ cho phép nếu có >=2 tab đang mở
                        int openCount = openedTabs.Count(kv => kv.Value);
                        if (openCount <= 1)
                        {
                            // Giữ nguyên, không đóng tab (silent ignore)
                            toggled = true;
                        }
                        else
                        {
                            openedTabs[tab] = false;
                        }
                    }
                    else
                    {
                        // Người dùng muốn mở tab
                        if (!openedTabs.ContainsKey(tab)) openedTabs[tab] = true;

                        if (!multitab)
                        {
                            // đóng tất cả tab khác, chỉ mở tab này
                            var keys = new List<CardEditorTabs>(openedTabs.Keys);
                            foreach (var k in keys)
                                openedTabs[k] = (k == tab);
                        }
                        else
                        {
                            openedTabs[tab] = true;
                        }
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();

            // === CONTENT AREA (các tab đã mở) ===
            contentScroll = EditorGUILayout.BeginScrollView(contentScroll);

            // dùng ToList() để an toàn nếu bạn thay đổi openedTabs trong vòng lặp
            foreach (var kvp in openedTabs.ToList())
            {
                if (!kvp.Value) continue;

                EditorGUILayout.BeginVertical("box");

                // header
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(kvp.Key.ToString(), EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();

                bool closeClicked = false;
                // Chỉ hiển thị nút close nếu multitab == true
                if (multitab)
                {
                    if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(18)))
                    {
                        // trước khi đóng, kiểm tra xem có >=2 tab đang mở
                        int openCount = openedTabs.Count(k => k.Value);
                        if (openCount > 1)
                        {
                            closeClicked = true;
                        }
                        else
                        {
                            // nếu chỉ còn 1 tab thì ignore (không đóng)
                            // (nếu muốn, có thể hiển thị tooltip hoặc dialog)
                            closeClicked = false;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal(); // ALWAYS đóng horizontal ngay sau vẽ header

                if (closeClicked)
                {
                    openedTabs[kvp.Key] = false;

                    EditorGUILayout.EndVertical(); // đóng vertical tương ứng với BeginVertical("box")
                    EditorGUILayout.Space();
                    continue;
                }

                // Nếu không đóng, vẽ nội dung tab bình thường
                switch (kvp.Key)
                {
                    case CardEditorTabs.Manager:
                        managerTab.DrawGUI();
                        break;
                    case CardEditorTabs.Generator:
                        generatorTab.DrawGUI();
                        break;
                    case CardEditorTabs.Overview:
                        overviewTab.DrawGUI();
                        break;
                    case CardEditorTabs.PokerTest:
                        pokerTestTab.DrawGUI();
                        break;
                    default:
                        GUILayout.Label("No implementation for this tab.");
                        break;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }

}
