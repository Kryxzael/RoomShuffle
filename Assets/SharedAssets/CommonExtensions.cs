using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Static mega-class containing extension methods for other classes
/// </summary>
public static class CommonExtensions
{
    /// <summary>
    /// The default mask the OnGround checks will use if nothing else is specified
    /// </summary>
    public static LayerMask DefaultGroundMask = ~0;

    /// <summary>
    /// Checks if the 2D game object of this monobehaviour is grounded
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static RaycastHit2D OnGround2D(this UnityEngine.MonoBehaviour mb)
    {
        return OnGround2D(mb.gameObject, new string[0]);
    }

    /// <summary>
    /// Checks if the 2D game object of this monobehaviour is grounded
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static RaycastHit2D OnGround2D(this UnityEngine.MonoBehaviour mb, params string[] layers)
    {
        return OnGround2D(mb.gameObject, layers);
    }

    /// <summary>
    /// Checks if a 2D object is grounded
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static RaycastHit2D OnGround2D(this GameObject obj)
    {
        return OnGround2D(obj, new string[0]);
    }

    /// <summary>
    /// Checks if a 2D object is grounded
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static RaycastHit2D OnGround2D(this GameObject obj, params string[] layers)
    {
        const float RAY_DISTANCE = 0.03f;

        Collider2D collider = obj.GetComponent<Collider2D>();

        //If the game object does not have a collider we can assume it isn't grounded as it won't interact with it
        if (collider == null)
        {
            return default;
        }

        //If the collider is a box collider. check get the edge radius and add that to the new Bounds object
        float edgeRadius = 0;
        if (collider is BoxCollider2D)
        {
            edgeRadius = ((BoxCollider2D) collider).edgeRadius;
        }

        //Create bounds object based on collider.bounds, but add the edgeradius
        Bounds colliderBounds = new Bounds(collider.bounds.center,
            new Vector3(collider.bounds.size.x + (edgeRadius * 2), collider.bounds.size.y + (edgeRadius * 2),
                collider.bounds.size.z));
        
            
        //Does a horizontal raycasting under the object's collider to check if it collides with another collider
        if (layers.Any())
        {
            return Physics2D.Raycast(new Vector2(colliderBounds.min.x, colliderBounds.min.y), Vector2.down, RAY_DISTANCE, LayerMask.GetMask(layers));
        }

        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(new Vector2(colliderBounds.min.x, colliderBounds.min.y), Vector2.down, RAY_DISTANCE, DefaultGroundMask))
            return hit;

        if (hit = Physics2D.Raycast(new Vector2(colliderBounds.max.x, colliderBounds.min.y), Vector2.down, RAY_DISTANCE, DefaultGroundMask))
            return hit;

        return default;
    }

    /// <summary>
    /// Checks if the 3D game object of this monobehaviour is grounded
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static bool OnGround(this UnityEngine.MonoBehaviour mb)
    {
        return OnGround(mb.gameObject, new string[0]);
    }

    /// <summary>
    /// Checks if the 3D game object of this monobehaviour is grounded
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static bool OnGround(this UnityEngine.MonoBehaviour mb, params string[] layers)
    {
        return OnGround(mb.gameObject, layers);
    }

    /// <summary>
    /// Checks if the 3D game object is grounded
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static bool OnGround(this GameObject obj)
    {
        return OnGround(obj, new string[0]);
    }

    /// <summary>
    /// Checks if the 3D game object is grounded
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static bool OnGround(this GameObject obj, params string[] layers)
    {
        //Warning: Does not work for multiple colliders!
        Collider collider = obj.GetComponent<Collider>();

        if (collider == null)
        {
            return false;
        }

        Vector3[] pnts = new[]
        {
            new Vector3(collider.bounds.min.x, collider.bounds.min.y + 0.01f, collider.bounds.min.z),
            new Vector3(collider.bounds.min.x, collider.bounds.min.y + 0.01f, collider.bounds.max.z),
            new Vector3(collider.bounds.max.x, collider.bounds.min.y + 0.01f, collider.bounds.min.z),
            new Vector3(collider.bounds.max.x, collider.bounds.min.y + 0.01f, collider.bounds.max.z),
            new Vector3(collider.bounds.center.x, collider.bounds.min.y + 0.01f, collider.bounds.center.z)
        };


        if (layers.Any())
        {
            return pnts.Any(i => Physics.RaycastAll(new Ray(i, Vector3.down), 0.1f, LayerMask.GetMask(layers)).Any());
        }
        else
        {
            return pnts.Any(i => Physics.RaycastAll(new Ray(i, Vector3.down), 0.1f, DefaultGroundMask).Any());
        }
    }

    /// <summary>
    /// Is this game object the player
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsPlayer(this GameObject obj)
    {
        return obj.CompareTag("Player");
    }

    /// <summary>
    /// Accelerates a rigidbody
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="direction">Direction to accelerate towards</param>
    /// <param name="speed">Speed to apply</param>
    /// <param name="maxX">Maximum x speed</param>
    /// <param name="maxZ">Maximum z speed</param>
    public static void MoveInDirection(this Rigidbody rigidbody, Vector3 direction, float speed)
    {
        rigidbody.velocity = (direction * speed) + (Vector3.up * rigidbody.velocity.y);
    }

    /// <summary>
    /// Is this game object the player
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsPlayer(this MonoBehaviour scr)
    {
        return IsPlayer(scr.gameObject);
    }

    /// <summary>
    /// Gets the player
    /// </summary>
    /// <param name="scr"></param>
    /// <returns></returns>
    public static GameObject GetPlayer(this MonoBehaviour scr)
    {
        return GetPlayer();
    }

    /// <summary>
    /// Gets the player
    /// </summary>
    /// <returns></returns>
    public static GameObject GetPlayer()
    {
        return GameObject.FindWithTag("Player");
    }

    public static float RealAngleBetween(this Vector2 v1, Vector2 v2)
    {
        return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * 180 / Mathf.PI - 90;
    }

    /// <summary>
    /// Sets the roation of a transform in euler angles
    /// </summary>
    /// <param name="transform">Transform</param>
    /// <param name="v">Angles to set</param>
    public static void SetEuler(this Transform transform, Vector3 v)
    {
        transform.rotation = Quaternion.Euler(v);
    }

    /// <summary>
    /// Sets the roation of a transform in euler angles
    /// </summary>
    /// <param name="transform">Transform</param>
    /// <param name="x">X-angle</param>
    /// <param name="y">Y-angle</param>
    /// <param name="z">Z-angle</param>
    public static void SetEuler(this Transform transform, float x, float y, float z)
    {
        transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
    }

    /// <summary>
    /// Sets the roation of a transform in euler angles
    /// </summary>
    /// <param name="transform">Transform</param>
    /// <param name="z">Angle</param>
    public static void SetEuler2D(this Transform transform, float z)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
    }

    /// <summary>
    /// Gets the rotation of the transform as euler angles
    /// </summary>
    /// <param name="transform">Transform</param>
    /// <returns></returns>
    public static Vector3 GetEuler(this Transform transform)
    {
        return transform.rotation.eulerAngles;
    }

    /// <summary>
    /// Gets the rotation of the transform as euler angles
    /// </summary>
    /// <param name="transform">Transform</param>
    /// <returns></returns>
    public static float GetEuler2D(this Transform transform)
    {
        return GetEuler(transform).z;
    }

    /// <summary>
    /// Reverses a 1 dimensional direction
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static Direction1D Reverse(this Direction1D dir)
    {
        switch (dir)
        {
            case Direction1D.Left:
                return Direction1D.Right;
            case Direction1D.Right:
                return Direction1D.Left;
            default:
                return Direction1D.None;
        }
    }

    /// <summary>
    /// Gets the gamemanager
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static GameObject GetGameManager(this MonoBehaviour mb)
    {
        return GetGameManager();
    }

    /// <summary>
    /// Gets the gamemanager
    /// </summary>
    /// <returns></returns>
    public static GameObject GetGameManager()
    {
        return GameObject.FindWithTag("GameController");
    }

    /// <summary>
    /// Gets the camera of the scene
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public static Camera GetCamera(this MonoBehaviour mb)
    {
        return GetCamera();
    }

    /// <summary>
    /// Gets the camera of the scene
    /// </summary>
    /// <returns></returns>
    public static Camera GetCamera()
    {
        return Camera.allCameras.FirstOrDefault();
    }

    /// <summary>
    /// Gets the direct children of this game object
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static IEnumerable<GameObject> GetChildren(this Transform trans)
    {
        return Enumerable.Range(0, trans.childCount).Select(i => trans.GetChild(i).gameObject);
    }

    /// <summary>
    /// Gets all the children of this game object (recursive search)
    /// </summary>
    /// <param name="trans">The transform of the gameobject to get the children of</param>
    /// <returns></returns>
    public static IEnumerable<GameObject> GetAllChildren(this Transform trans)
    {
        List<GameObject> _ = new List<GameObject>();

        foreach (GameObject i in GetChildren(trans))
        {
            _.Add(i);
            _.AddRange(GetAllChildren(i.transform));
        }

        return _;
    }

    /// <summary>
    /// Checks if a bitflag(s) is/are set on an enum value
    /// </summary>
    /// <param name="enu">Enum value in question</param>
    /// <param name="value">Flag to check</param>
    /// <returns></returns>
    public static bool HasBitFlag(this System.Enum enu, System.Enum value)
    {
        return (System.Convert.ToInt32(enu) & System.Convert.ToInt32(value)) == System.Convert.ToInt32(value);
    }

    /// <summary>
    /// Sets (a) bitflag(s) in an enum value to be active
    /// </summary>
    /// <param name="enu">Enum value in question</param>
    /// <param name="value">Flag(s) to set</param>
    /// <returns></returns>
    public static int SetBitFlag(this System.Enum enu, System.Enum value)
    {
        return System.Convert.ToInt32(enu) | System.Convert.ToInt32(value);    
    }

    /// <summary>
    /// Sets (a) bitflag(s) in an enum value to be inactive
    /// </summary>
    /// <param name="enu">Enum value in question</param>
    /// <param name="value">Flag(s) to unset</param>
    /// <returns></returns>
    public static int UnsetBitFlag(this System.Enum enu, System.Enum value)
    {
        return System.Convert.ToInt32(enu) & ~System.Convert.ToInt32(value);
    }

    /// <summary>
    /// Toggles the state of (a) bitflag(s)
    /// </summary>
    /// <param name="enu">Enum value in question</param>
    /// <param name="value">Flag(s) to toggle</param>
    /// <returns></returns>
    public static int ToggleBitFlag(this System.Enum enu, System.Enum value)
    {
        return System.Convert.ToInt32(enu) ^ System.Convert.ToInt32(value);
    }

    /// <summary>
    /// Rotates the rigidbody so that it faces the direction it is heading
    /// </summary>
    /// <param name="rb">Rigidbody to rotate</param>
    public static void RotateToForward(this Rigidbody2D rb)
    {
        if (rb.velocity != Vector2.zero)
        {
            Vector2 dir = rb.velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rb.transform.Rotate(90);
        }
    }

    /// <summary>
    /// Returns the current enumerable as a readonly list
    /// </summary>
    /// <typeparam name="T">Implicit generic</typeparam>
    /// <param name="enumerable">Enumerable to convert</param>
    /// <returns></returns>
    public static ReadOnlyList<T> AsReadOnly<T>(this IEnumerable<T> enumerable)
    {
        return new ReadOnlyList<T>(enumerable);
    }

    #region Transform 2D extensions
    /// <summary>
    /// The position of the transform in 2D world space
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static Vector2 Position2D(this Transform trans)
    {
        return trans.position;
    }

    /// <summary>
    /// The z-euler of the rotation of the transform in world space
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static float Rotation2D(this Transform trans)
    {
        return trans.rotation.eulerAngles.z;
    }

    /// <summary>
    /// The global scale of the object in 2D
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static Vector2 LossyScale2D(this Transform trans)
    {
        return trans.lossyScale;
    }

    /// <summary>
    /// Position of the transform in 2D relative to parent transform
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static Vector2 LocalPosition2D(this Transform trans)
    {
        return trans.localPosition;
    }

    /// <summary>
    /// The z-euler of the rotation of the transform relative to the parent tranform's rotation
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static float LocalRotation2D(this Transform trans)
    {
        return trans.GetEuler2D();
    }

    /// <summary>
    /// Rotates the transform around the z-euler only
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="z"></param>
    public static void Rotate(this Transform trans, float z)
    {
        trans.Rotate(0, 0, z);
    }

    /// <summary>
    /// Moves the transform by x along the x axis, and y along the y axis
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static void Translate(this Transform trans, float x, float y)
    {
        trans.Translate(x, y, 0);
    }

    #endregion

    #region Component shorthands

    /// <summary>
    /// Gets the AudioSource component of this game object that has a specific audio clip
    /// </summary>
    /// <param name="clip">Clip in question</param>
    /// <returns></returns>
    public static AudioSource GetAudioSource(this GameObject gameObject, AudioClip clip)
    {
        return gameObject.GetComponents<AudioSource>().Where(i => i.clip == clip).SingleOrDefault();
    }

    /// <summary>
    /// Gets the AudioSource component of this game object that has a specific audio clip
    /// </summary>
    /// <param name="clipName">Name of clip in question</param>
    /// <returns></returns>
    public static AudioSource GetAudioSource(this GameObject gameObject, string clipName)
    {
        return gameObject.GetComponents<AudioSource>().Where(i => i.clip.name == clipName).SingleOrDefault();
    }
    #endregion

    /// <summary>
    /// Executes a block of code if the gameobject has a component
    /// </summary>
    /// <typeparam name="T">Component to check existance of</typeparam>
    /// <param name="go"></param>
    /// <param name="code">Code to run</param>
    /// <returns></returns>
    public static bool ExecuteIfHasComponent<T>(this GameObject go, System.Action<T> code) where T : Component
    {
        if (go.GetComponent<T>() != null)
        {
            code(go.GetComponent<T>());
            return true;
        }

        return false;
    }

    /// <summary>
    /// Executes a block of code if the gameobject has a component
    /// </summary>
    /// <typeparam name="T">Component to check existance of</typeparam>
    /// <param name="comp"></param>
    /// <param name="code">Code to run</param>
    /// <returns></returns>
    public static bool ExecuteIfHasComponent<T>(this Component comp, System.Action<T> code) where T : Component
    {
        return ExecuteIfHasComponent<T>(comp.gameObject, code);
    }

    /// <summary>
    /// Executes a block of code on every element in an enumerable
    /// </summary>
    /// <typeparam name="T">Generic value T</typeparam>
    /// <param name="values">Values to ForEach over</param>
    /// <param name="action">Code to run</param>
    public static void ForEach<T>(this IEnumerable<T> values, System.Action<T> action)
    {
        foreach (T i in values)
        {
            action(i);
        }
    }

    private class DummyComponent : MonoBehaviour
    {
        private void Start()
        {
            Destroy(this);
        }
    }

}