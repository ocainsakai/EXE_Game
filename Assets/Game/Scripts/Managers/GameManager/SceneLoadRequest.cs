using System;

public class SceneLoadRequest
{
    public string SceneName { get; private set; }
    public object Data { get; private set; }
    public Action OnLoaded { get; private set; }
    public Action OnClosed { get; private set; }

    private readonly SceneLoader loader;

    public SceneLoadRequest(string sceneName, SceneLoader loader)
    {
        SceneName = sceneName;
        this.loader = loader;
    }

    public SceneLoadRequest WithData(object data)
    {
        Data = data;
        return this;
    }

    public SceneLoadRequest WithOnLoaded(Action onLoaded)
    {
        OnLoaded = onLoaded;
        return this;
    }

    public SceneLoadRequest WithOnClosed(Action onClosed)
    {
        OnClosed = onClosed;
        return this;
    }

    public void Execute()
    {
        loader.LoadScene(SceneName, Data, OnLoaded);
    }
}
