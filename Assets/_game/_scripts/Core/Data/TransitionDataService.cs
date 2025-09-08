public interface ITransitionDataService
{
    void SetData(object data);
    T GetData<T>() where T : class;
    void Clear();
}
public class TransitionDataService : ITransitionDataService
{
    private object transitionData;

    public void SetData(object data)
    {
        transitionData = data;
    }

    public T GetData<T>() where T : class
    {
        return transitionData as T;
    }

    public void Clear()
    {
        transitionData = null;
    }
}