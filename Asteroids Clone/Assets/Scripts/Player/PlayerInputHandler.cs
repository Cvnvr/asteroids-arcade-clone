using UnityEngine;

public static class PlayerInputHandler
{
    public static float GetForwardInput()
    {
        return Input.GetAxis("Vertical");
    }

    public static float GetRotationalInput()
    {
        return Input.GetAxis("Horizontal");
    }

    public static bool IsFiringProjectiles()
    {
        return Input.GetButton("Fire1");
    }
}