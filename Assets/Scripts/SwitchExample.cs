using UnityEngine;

public static class SwitchExample
{
    public enum Directions
    {
        Up,
        Down,
        Right,
        Left
    }

    public enum Orientation
    {
        North,
        South,
        East,
        West
    }

    public static void Main()
    {
        var direction = Directions.Right;
        Debug.Log($"Map view direction is {direction}");

        var orientation = direction switch
        {
            Directions.Up => Orientation.North,
            Directions.Right => Orientation.East,
            Directions.Down => Orientation.South,
            Directions.Left => Orientation.West,
            _ => throw new System.NotImplementedException(),
        };
        Debug.Log($"Cardinal orientation is {orientation}");
    }
}
