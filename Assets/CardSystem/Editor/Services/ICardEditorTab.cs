namespace CardSystem
{
    public interface ICardEditorTab
    {
        string TabName { get; }
        void DrawGUI();
    }
}