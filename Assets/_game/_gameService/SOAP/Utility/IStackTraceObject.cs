using System.Collections.Generic;
//public class SOASettings : ScriptableObject
//{
//    public string serviceName;
//    public string serviceUrl;
//    public bool isEnabled = true;
//    public void Initialize(string name, string url)
//    {
//        serviceName = name;
//        serviceUrl = url;
//    }
//    public void ToggleService(bool enable)
//    {
//        isEnabled = enable;
//    }
//}
public interface IStackTraceObject
{
    List<StackTraceEntry> StackTraces { get; }

    void AddStackTrace();
    void AddStackTrace(object value);
}