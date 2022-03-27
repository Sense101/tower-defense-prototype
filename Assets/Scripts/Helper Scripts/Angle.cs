using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()

/// <summary>
/// helper class for 2d - stores an angle in degrees between 0 and 360
/// </summary>
[System.Serializable]
public class Angle
{
    // cached conversions
    private static readonly Dictionary<float, Vector2> cachedAngleToVectors = new Dictionary<float, Vector2>();
    private static readonly Dictionary<Vector2, float> cachedVectorToDegrees = new Dictionary<Vector2, float>();

    /// <summary>
    /// degrees of the angle between 0 and 360
    /// </summary>
    public float degrees;

    public Angle(float degrees)
    {
        this.degrees = ClampDegrees(degrees);
    }


    public Angle(Quaternion quaternion)
    {
        degrees = ClampDegrees(Mathf.RoundToInt(quaternion.eulerAngles.z));
    }

    public Angle(Vector2 vector)
    {
        degrees = ClampDegrees(VectorToDegrees(vector));
    }




    /// <summary>
    /// Rotates the angle and returns itself
    /// </summary>
    /// <param name="rotDegrees">The degrees to rotate by</param>
    /// <returns>The angle object</returns>
    public Angle Rotate(float rotDegrees)
    {
        degrees = ClampDegrees(degrees + rotDegrees);
        //Debug.Log("new angle before clamp: " + (degrees + rotDegrees) + ". After clamp: " + degrees);
        return this;
    }

    public Angle Clone()
    {
        return new Angle(degrees);
    }

    /*
     Returning the angle as another type
     */

    /// <summary>
    /// returns the angle as a Vector3
    /// </summary>
    public Vector3 AsVector3()
    {
        return new Vector3(0, 0, degrees);
    }

    /// <summary>
    /// returns the angle as a quaternion
    /// </summary>
    public Quaternion AsQuaternion()
    {
        return Quaternion.Euler(0, 0, degrees);
    }

    /// <summary>
    /// converts the angle to a vector and returns it
    /// </summary>
    public Vector2 ToVector()
    {
        return AngleToVector(degrees);
    }


    /*
     ---------------------------------------------
    STATIC FUNCTIONS
     ---------------------------------------------
     */

    /// <summary>
    /// converts any integer to the degree of an angle
    /// </summary>
    /// <returns>The degrees of the angle between 0 and 360</returns>
    public static float ClampDegrees(float degrees)
    {
        while (degrees < 0)
        {
            degrees += 360;
        }
        return degrees % 360;
    }


    /// <summary>
    /// converts an angle to a vector direction
    /// </summary>
    public static Vector2 AngleToVector(Angle angle)
    {
        return AngleToVector(angle.degrees);
    }
    /// <summary>
    /// converts an angle to a vector direction
    /// </summary>
    public static Vector2 AngleToVector(float degrees)
    {
        if (cachedAngleToVectors.TryGetValue(degrees, out Vector2 degreeVector))
        {
            return degreeVector;
        }

        float radians = (degrees + 90) * Mathf.Deg2Rad;
        degreeVector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        cachedAngleToVectors.Add(degrees, degreeVector);

        return degreeVector;

    }

    /// <summary>
    /// converts a vector direction to an angle
    /// </summary>
    public static Angle VectorToAngle(Vector2 vector)
    {
        return new Angle(VectorToDegrees(vector));
    }

    /// <summary>
    /// converts a vector direction to degrees
    /// </summary>
    public static float VectorToDegrees(Vector2 vector)
    {
        if (cachedVectorToDegrees.TryGetValue(vector, out float degrees))
        {
            return degrees;
        }
        degrees = ClampDegrees(Mathf.RoundToInt(Vector2.SignedAngle(Vector2.up, vector)));
        cachedVectorToDegrees.Add(vector, degrees);

        return degrees;
    }

    public static Angle Towards(Vector2 pos, Vector2 targetPos)
    {
        var targetOffset = targetPos - pos;
        int degrees = Mathf.RoundToInt(Mathf.Atan2(targetOffset.y, targetOffset.x) * Mathf.Rad2Deg - 90);

        return new Angle(degrees);
    }

    public static Angle Between(Angle angle1, Angle angle2)
    {
        var difference = angle2.degrees - angle1.degrees;
        float newDegrees = angle1.degrees + (difference / 2);

        return new Angle(newDegrees);
    }
    public static Angle Between(float degrees1, float degrees2)
    {
        float difference = degrees2 - degrees1;
        float newDegrees = degrees1 + (difference / 2);

        return new Angle(newDegrees);
    }


    /*
    ------------------------------------------
    OPERATORS
    ------------------------------------------
     */
    public static Angle operator +(Angle lhs, Angle rhs)
    {
        return new Angle(lhs.degrees + rhs.degrees);
    }
    public static Angle operator -(Angle lhs, Angle rhs)
    {
        return new Angle(lhs.degrees - rhs.degrees);
    }

    public static bool operator ==(Angle lhs, Angle rhs)
    {
        return lhs.degrees == rhs.degrees;
    }
    public static bool operator !=(Angle lhs, Angle rhs)
    {
        return lhs.degrees != rhs.degrees;
    }

}

#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
