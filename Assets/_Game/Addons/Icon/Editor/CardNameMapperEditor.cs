using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardNameMapper))]
public class CardNameMapperEditor : Editor
{
    private string testCardName = "Ace of Diamonds";
    
    public override void OnInspectorGUI()
    {
        CardNameMapper mapper = (CardNameMapper)target;
        
        // Header
        EditorGUILayout.Space(5);
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
        headerStyle.fontSize = 14;
        EditorGUILayout.LabelField("🎴 Card Name Mapper Configuration", headerStyle);
        EditorGUILayout.Space(5);
        
        // Draw default inspector
        DrawDefaultInspector();
        
        EditorGUILayout.Space(10);
        
        // Test section
        EditorGUILayout.LabelField("🧪 Test Conversion", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        testCardName = EditorGUILayout.TextField("Test Card Name:", testCardName);
        if (GUILayout.Button("Test", GUILayout.Width(60)))
        {
            TestConversion(mapper, testCardName);
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // Quick test buttons
        EditorGUILayout.LabelField("Quick Tests:", EditorStyles.miniLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Ace of Diamonds", EditorStyles.miniButton))
        {
            TestConversion(mapper, "Ace of Diamonds");
        }
        if (GUILayout.Button("Two of Hearts", EditorStyles.miniButton))
        {
            TestConversion(mapper, "Two of Hearts");
        }
        if (GUILayout.Button("King of Spades", EditorStyles.miniButton))
        {
            TestConversion(mapper, "King of Spades");
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(10);
        
        // Pattern preview
        EditorGUILayout.LabelField("📋 Pattern Preview", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            $"Pattern: {mapper.spriteNamePattern}\n" +
            $"Lowercase: {mapper.toLowerCase}\n" +
            $"Remove Spaces: {mapper.removeSpaces}\n\n" +
            $"Example Output: '{mapper.ConvertToSpriteName("Ace of Diamonds")}'",
            MessageType.Info
        );
        
        EditorGUILayout.Space(5);
        
        // Validation
        if (mapper.rankMappings.Count == 0)
        {
            EditorGUILayout.HelpBox("⚠️ No rank mappings defined!", MessageType.Warning);
        }
        
        if (mapper.suitMappings.Count == 0)
        {
            EditorGUILayout.HelpBox("⚠️ No suit mappings defined!", MessageType.Warning);
        }
        
        EditorGUILayout.Space(5);
        
        // Stats
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"Ranks: {mapper.rankMappings.Count}", EditorStyles.miniLabel);
        EditorGUILayout.LabelField($"Suits: {mapper.suitMappings.Count}", EditorStyles.miniLabel);
        EditorGUILayout.LabelField($"Custom: {mapper.customMappings.Count}", EditorStyles.miniLabel);
        EditorGUILayout.EndHorizontal();
    }
    
    private void TestConversion(CardNameMapper mapper, string cardName)
    {
        Debug.Log("═══════════════════════════════════════");
        Debug.Log($"🧪 Testing Card Name: '{cardName}'");
        Debug.Log("───────────────────────────────────────");
        
        string mainResult = mapper.ConvertToSpriteName(cardName);
        Debug.Log($"✅ Main Result: '{mainResult}'");
        
        var possibilities = mapper.GetPossibleSpriteNames(cardName);
        Debug.Log($"\n📋 All Possible Names ({possibilities.Count}):");
        for (int i = 0; i < possibilities.Count; i++)
        {
            Debug.Log($"  {i + 1}. '{possibilities[i]}'");
        }
        
        Debug.Log("═══════════════════════════════════════");
    }
}