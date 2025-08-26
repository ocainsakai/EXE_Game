using UnityEngine;

public class ListWithButtonAttribute : PropertyAttribute
{
    public string buttonLabel;
    public ListWithButtonAttribute(string buttonLabel = "Add Default")
    {
        this.buttonLabel = buttonLabel;
    }
}
