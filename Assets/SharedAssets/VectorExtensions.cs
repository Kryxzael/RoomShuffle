using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;

/// <summary>
/// Holds extension methods for vector related operations
/// </summary>
public static class VectorExtensions
{
    /// <summary>
    /// Clones the vector and changes the x component of the clone, keeping the old y, and z values
    /// </summary>
    /// <param name="v"></param>
    /// <param name="x">new x value</param>
    public static Vector3 SetX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    /// <summary>
    /// Clones the vector and changes the z component of the clone, keeping the old x and y values
    /// </summary>
    /// <param name="v"></param>
    /// <param name="z">new z value</param>
    public static Vector3 SetZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    /// <summary>
    /// Clones the vector and changes the y component of the clone, keeping the old x and z values
    /// </summary>
    /// <param name="v"></param>
    /// <param name="y">new y value</param>
    public static Vector3 SetY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    /// <summary>
    /// Clones the vector and changes the x component of the clone, keeping the old y value
    /// </summary>
    /// <param name="v"></param>
    /// <param name="x">new x value</param>
    public static Vector2 SetX(this Vector2 v, float x)
    {
        return new Vector2(x, v.y);
    }

    /// <summary>
    /// Clones the vector and changes the y component of the clone, keeping the old x value
    /// </summary>
    /// <param name="v"></param>
    /// <param name="y">new y value</param>
    public static Vector2 SetY(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }

    /// <summary>
    /// Converts a degree (0-360) to a vector pointing in that direction
    /// </summary>
    /// <param name="degree">The degree as a number between 0 and 360</param>
    /// <returns></returns>
    public static Vector2 VectorFromDegree(float degree)
    {
        return Quaternion.Euler(0, 0, degree) * Vector2.up;
    }

    /// <summary>
    /// Sets the X position of the transform's coordinates
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveX(this Transform transform, float x)
    {
        transform.position = transform.position.SetX(x);
    }

    /// <summary>
    /// Sets the local X position of the transform's coordinates
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveLocalX(this Transform transform, float x)
    {
        transform.localPosition = transform.localPosition.SetX(x);
    }

    /// <summary>
    /// Translates the X position of the transform's coordinates
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void TranslateX(this Transform transform, float deltaX)
    {
        transform.Translate(deltaX, 0f);
    }

    /// <summary>
    /// Sets the X position of the transform's coordinates with the provided delegate, the delegate passes the current x position
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveX(this Transform transform, Func<float, float> xTransformation)
    {
        transform.position = transform.position.SetX(xTransformation(transform.position.x));
    }

    /// <summary>
    /// Sets the local X position of the transform's coordinates with the provided delegate, the delegate passes the current x position
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveLocalX(this Transform transform, Func<float, float> xTransformation)
    {
        transform.localPosition = transform.localPosition.SetX(xTransformation(transform.localPosition.x));
    }

    /// <summary>
    /// Sets the Y position of the transform's coordinates
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveY(this Transform transform, float y)
    {
        transform.position = transform.position.SetY(y);
    }

    /// <summary>
    /// Sets the local Y position of the transform's coordinates
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveLocalY(this Transform transform, float y)
    {
        transform.localPosition = transform.localPosition.SetY(y);
    }

    /// <summary>
    /// Translates the Y position of the transform's coordinates
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void TranslateY(this Transform transform, float deltaY)
    {
        transform.Translate(0f, deltaY);
    }

    /// <summary>
    /// Sets the Y position of the transform's coordinates with the provided delegate, the delegate passes the current Y position
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveY(this Transform transform, Func<float, float> yTransformation)
    {
        transform.position = transform.position.SetY(yTransformation(transform.position.y));
    }

    /// <summary>
    /// Sets the local Y position of the transform's coordinates with the provided delegate, the delegate passes the current Y position
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveLocalY(this Transform transform, Func<float, float> yTransformation)
    {
        transform.localPosition = transform.localPosition.SetY(yTransformation(transform.localPosition.y));
    }

    /// <summary>
    /// Sets the Z position of the transform's coordinates
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveZ(this Transform transform, float z)
    {
        transform.position = transform.position.SetZ(z);
    }

    /// <summary>
    /// Sets the local Z position of the transform's coordinates
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveLocalZ(this Transform transform, float z)
    {
        transform.localPosition = transform.localPosition.SetZ(z);
    }

    /// <summary>
    /// Translates the Z position of the transform's coordinates
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void TranslateZ(this Transform transform, float deltaZ)
    {
        transform.Translate(0f, 0f, deltaZ);
    }

    /// <summary>
    /// Sets the Z position of the transform's coordinates with the provided delegate, the delegate passes the current z position
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveZ(this Transform transform, Func<float, float> zTransformation)
    {
        transform.position = transform.position.SetZ(zTransformation(transform.position.z));
    }

    /// <summary>
    /// Sets the local Z position of the transform's coordinates with the provided delegate, the delegate passes the current z position
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void MoveLocalZ(this Transform transform, Func<float, float> zTransformation)
    {
        transform.localPosition = transform.localPosition.SetZ(zTransformation(transform.localPosition.z));
    }

    /// <summary>
    /// Sets the rigidbody's horizontal velocity
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="x"></param>
    public static void SetVelocityX(this Rigidbody2D rigidbody, float x)
    {
        rigidbody.velocity = rigidbody.velocity.SetX(x);
    }

    /// <summary>
    /// Sets the rigidbody's horizontal velocity with the provided delegate. The current X velocity is passed as its argument
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="x"></param>
    public static void SetVelocityX(this Rigidbody2D rigidbody, Func<float, float> xTransformation)
    {
        rigidbody.velocity = rigidbody.velocity.SetX(xTransformation(rigidbody.velocity.x));
    }

    /// <summary>
    /// Adds to the rigidbody's horizontal velocity
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="xAcceleration"></param>
    public static void AccelerateVelocityX(this Rigidbody2D rigidbody, float xAcceleration)
    {
        rigidbody.velocity += Vector2.right * xAcceleration;
    }

    /// <summary>
    /// Sets the rigidbody's vertical velocity
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="x"></param>
    public static void SetVelocityY(this Rigidbody2D rigidbody, float y)
    {
        rigidbody.velocity = rigidbody.velocity.SetY(y);
    }

    /// <summary>
    /// Sets the rigidbody's vertical velocity with the provided delegate. The current Y velocity is passed as its argument
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="x"></param>
    public static void SetVelocityY(this Rigidbody2D rigidbody, Func<float, float> yTransformation)
    {
        rigidbody.velocity = rigidbody.velocity.SetY(yTransformation(rigidbody.velocity.y));
    }

    /// <summary>
    /// Adds to the rigidbody's vertical velocity
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="xAcceleration"></param>
    public static void AccelerateVelocityY(this Rigidbody2D rigidbody, float yAcceleration)
    {
        rigidbody.velocity += Vector2.up * yAcceleration;
    }
}
