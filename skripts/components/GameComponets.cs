
public interface GameComponet { }

public sealed class GameComponets
{
    private static GameComponets instance = null;
    private static readonly object padlock = new();

    private readonly System.Collections.Generic.Dictionary<string, GameComponet> data;

    private GameComponets()
    {
        data = new()
        {
            { typeof(UserPreferences).FullName, new UserPreferences() }
        };
    }

    public static GameComponets Instance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameComponets tmp = new();
                        instance = tmp;
                    }
                }
            }
            return instance;
        }
    }

    public T Get<T>() where T : GameComponet
    {
        var className = typeof(T).FullName;
        var response = data[className];
        if (response != null && response is T r)
        {
            return r;
        }
        throw new System.ArgumentException("Unknown component " + typeof(T).FullName);
    }
}