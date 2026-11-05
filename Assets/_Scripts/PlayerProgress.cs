using UnityEngine;

public static class PlayerProgress
{
    public static bool HasHealth { get; private set; }
    public static float Health { get; private set; }

    public static void SaveHealth(float value)
    {
        Health = value;
        HasHealth = true;
    }

    public static void Clear()
    {
        HasHealth = false;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetOnPlay()
    {
        HasHealth = false;
    }
}