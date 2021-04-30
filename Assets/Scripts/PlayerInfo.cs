using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class PlayerInfo
{
    static Color player1color = new Color(1f, 0f, 0f);
    static Color player2color = new Color(59f / 255f, 109f / 255f, 1f);
    static Color player3color = new Color(231f / 255f, 231f / 255f, 42f / 255f);
    static Color player4color = new Color(33f / 255f, 238f / 255f, 59f / 255f);

    public static Color GetColor(int player)
    {
        if (player == 1) return player1color;
        if (player == 2) return player2color;
        if (player == 3) return player3color;
        if (player == 4) return player4color;
        return Color.magenta;
    }
}
//64*35 = small map