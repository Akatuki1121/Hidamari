using UnityEngine;

public static class GameResultData
{
    public static float last_grow_time { get; private set; }
    public static float last_height { get; private set; }

    public static void SetResult(float p_grow_time, float p_height)
    {
        last_grow_time = p_grow_time;
        last_height = p_height;
    }
}
