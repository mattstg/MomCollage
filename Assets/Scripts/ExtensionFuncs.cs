using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public static class ExtensionFuncs  {

    static System.Random rnd = new System.Random();

    private static readonly Dictionary<System.Type, System.Enum[]> bakedEnumDict = new Dictionary<System.Type, System.Enum[]>();
    public static T[] GetArrOfEnums<T>() where T : struct, System.IConvertible
    {
        //Not sure if micro opt, but bakes the enums in a dict first time retrieval, since we retreieve some of them alot of times
        if (!bakedEnumDict.ContainsKey(typeof(T)))
            bakedEnumDict.Add(typeof(T), System.Enum.GetValues(typeof(T)).Cast<System.Enum>().ToArray<System.Enum>());

        return bakedEnumDict[typeof(T)].Cast<T>().ToArray<T>();
    }

    public static string[] GetStringArrOfEnums<T>() where T : struct, System.IConvertible
    {
        return ConvertArrToString<T>(GetArrOfEnums<T>()).ToArray();
    }

    public static List<string> GetStringListOfEnums<T>() where T : struct, System.IConvertible
    {
        return ConvertArrToString<T>(GetArrOfEnums<T>()).ToList();
    }

    public static string[] ConvertArrToString<T>(T[] arr)
    {
        string[] toRet = new string[arr.Length];
        for (int i = 0; i < arr.Length; i++)
            toRet[i] = arr[i].ToString();
        return toRet;
    }


    public static int Sign(this bool b, bool positiveIsNegative = false)
    {
        if(!positiveIsNegative)
            return (b) ? 1 : -1;
        else
            return (b) ? -1 : 1;

    }

    public static void DeleteAllChildren(this Transform t)
	{
		Transform[] childarr = t.ChildrenArray();
		foreach (Transform child in childarr)
		{
			GameObject.Destroy(child.gameObject);
		}
	}

    public static Bounds GetTotalColiBoundsOfGameObject(this GameObject g)
    {
        Bounds finalBounds;
        Collider2D[] allColis = g.GetComponentsInChildren<Collider2D>(true);
        if (allColis.Length > 1)
        {
            finalBounds = allColis[0].bounds;
        }
        else
        {
            finalBounds = new Bounds();
            Debug.Log("Object has no colliders to get bounds from, if it is a prefab resource, they do not have colliders initialized yet, (Unity thing)");
        }
       

        foreach (Collider2D pcoli in allColis)
        {
            finalBounds.Encapsulate(pcoli.bounds);
        }

        return finalBounds;
    }

    public static Bounds GetTotalSpriteBoundsOfGameObject(this GameObject g)
    {
        Bounds finalBounds;
        SpriteRenderer[] allSr = g.GetComponentsInChildren<SpriteRenderer>(true);
        if (allSr.Length > 1)
        {
            finalBounds = allSr[0].bounds;
        }
        else
        {
            finalBounds = new Bounds();
            Debug.Log("Object has no sprite renderers");
        }


        foreach (SpriteRenderer pcoli in allSr)
        {
            finalBounds.Encapsulate(pcoli.bounds);
        }

        return finalBounds;
    }


    public static T ToEnum<T>(this string s) where T : struct, IConvertible, IComparable, IFormattable
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("String conversion to enum failed, T was not enum type, must be an enum.");
		}
		T toRet;
		if (Enum.TryParse<T>(s, out toRet))
		{
			return toRet;
		}
		else
		{
			System.Type st = typeof(T);
			Debug.LogError($"ERROR, Conversion from string to enum for string:{s} and enumType:{st} failed!! Returning default value for enum");
			return default(T);
		}

	}


    public static List<string> GetStringListOfEnums(this System.Type t)
    {
        if (!t.IsEnum)
        {
            Debug.LogError("Get string list of enums called on not an enum");
            return new List<string>();
        }
        else
            return Enum.GetNames(t).ToList();
    }


    public static Bounds GetBoundsFromPoints(this Vector2[] points)
    {
        Vector2 botLeft = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 topRight = new Vector2(-float.MaxValue, -float.MaxValue);
        

        for(int i = 0; i < points.Length; i++)
        {
            botLeft.x = Mathf.Min(botLeft.x, points[i].x);
            botLeft.y = Mathf.Min(botLeft.y, points[i].y);
            topRight.x = Mathf.Max(topRight.x, points[i].x);
            topRight.y = Mathf.Max(topRight.y, points[i].y);
        }

        return new Bounds((botLeft + topRight) / 2f, topRight - botLeft);
    }

    public static Vector2 GetCenterOfPoints(this List<Vector2> vectors)
    {
        Vector2 center = new Vector2();
        foreach (Vector2 v in vectors)
            center += v;
        center /= vectors.Count;
        return center;
    }

	public static Transform[] ChildrenArray(this Transform t)
	{
		return t.Cast<Transform>().ToArray();
	}

	public static Transform ChildByName(this Transform t, string _name)
	{
		return t.ChildrenArray().FirstOrDefault(tchild => tchild.name == _name);
	}

	public static float SurfaceArea(this Vector2 v)
	{
		return v.x * v.y;
	}

	public static Vector3 V3(this Vector2 v)
	{
		return new Vector3(v.x, v.y,0);
	}


	public static Vector3 FlipSign(this Vector3 v, bool flipX, bool flipY)
	{
		return new Vector3((flipX) ? v.x * -1 : v.x, (flipY) ? v.y * -1 : v.y, v.z);
	}


	public static Vector3 Abs(this Vector3 v)
	{
		return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
	}

	public static Vector2 Mult(this Vector2 v, Vector2 multBy)
	{
		return new Vector2(v.x * multBy.x, v.y * multBy.y);
	}

	public static T[] GetAllCompRecursive<T>(this Transform t) where T : Component
	{
		List<T> toFill = new List<T>();
		_GetAllCompRecursive<T>(t, toFill);
		return toFill.ToArray();
	}

	private static void _GetAllCompRecursive<T>(this Transform t, List<T> allComp) where T : Component
    {
		if (t.childCount > 0)
			foreach (Transform child in t)
				_GetAllCompRecursive<T>(child, allComp);
		T compToAdd = t.GetComponent<T>();
		if(compToAdd)
			allComp.Add(compToAdd);
	}

    public static float Random(this Vector2 v)
    {
        return UnityEngine.Random.Range(v.x, v.y);
    }

    public static int Random(this Vector2Int v)
    {
        return UnityEngine.Random.Range(v.x, v.y + 1);
    }

    public static bool IsBetween(this Vector2 v, float v2)
    {
        return v2 >= v.x && v2 <= v.y;
    }

    public static Vector2 SumTotal(this List<Vector2> toSum)
    {
        Vector2 toRet = Vector2.zero;
        foreach(Vector2 v2 in toSum)
            toRet += v2;
        return toRet;
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        //if (source == null) throw new ArgumentNullException("source");
        //if (action == null) throw new ArgumentNullException("action");

        foreach (T item in source)
            action(item);
    }

    public static bool TrueForAll<T>(this IEnumerable<T> source, Predicate<T> action)
    {
        foreach (T item in source)
            if (!action(item))
                return false;
        return true;
    }

    public delegate G ParsingDelg<T,G>(T elem);
    public static G[] CollectionFromForEach<T,G>(this IEnumerable<T> source, ParsingDelg<T,G> parsingDelg)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (parsingDelg == null) throw new ArgumentNullException("action");

        List<G> toRet = new List<G>();

        foreach (T item in source)
        {
            toRet.Add(parsingDelg(item));
        }
        return toRet.ToArray();
    }

    public delegate bool conditionalDelg<T>(T element);
	//public Predicate( )

	public static List<T> GetAll<T>(this IEnumerable<T> source, Predicate<T> action)
	{
		List<T> toReturn = new List<T>(); 
		foreach(T t in source)
		{
			if (action.Invoke(t))
				toReturn.Add(t);
		}
		return toReturn;
	}

    public static bool IsMaskContainedInThisLayerMask(this int lm, string layerName)
    {
        int n = LayerMask.NameToLayer(layerName);
        return lm == (lm | (1 << n)); //self explainatory
    }

    public static bool IsMaskContainedInThisLayerMask(this int lm, int layer)
    {
        return lm == (lm | (1 << layer)); //see above
    }

    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch(Exception e) 
                {
                    Debug.LogError("GetCopyOfFailed: " + e.ToString());

                } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    public static T AddCopyComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }


    public static T GetRandomElement<T>(this System.Collections.Generic.ICollection<T> iCollec)
    {
        if (iCollec.Count <= 0)
            return default(T);
        return iCollec.ElementAt(UnityEngine.Random.Range(0, iCollec.Count));
    }

    public static int GetIndexOf<T>(this T[] arr, T elem)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].Equals(elem))
            {
                return i;
            }
        }
        return -1;
    }
    public static T GetRandom<T>(this T[] v)
    {
        if (v.Length <= 0)
        {
            Debug.LogError("GetRandom was given an array of length less than 0");
            return default(T);
        }
        return v[UnityEngine.Random.Range(0, v.Length)];
    }

    public static Vector2 GetSpritePivot(this Sprite sprite)
    {
        Bounds bounds = sprite.bounds;
        var pivotX = -bounds.center.x / bounds.extents.x / 2 + 0.5f;
        var pivotY = -bounds.center.y / bounds.extents.y / 2 + 0.5f;

        return new Vector2(pivotX, pivotY);
    }

    public static List<T> RandomizeOrder<T>(this List<T> v)
    {
        var result = v.OrderBy(item => rnd.Next());
        return result.ToList();
    }

   
    /// <summary>
    /// EX) Given a percent range of .5 and a value of 10 will return a value between 5 and 15, Given zero, it is the same value
    /// </summary>
    /// <param name="v"></param>
    /// <param name="withinPercentRange"></param>
    /// <returns></returns>
    public static float RandomizeByPercent(this float v, float withinPercentRange)
    {
        if (withinPercentRange == 0)
            return v;
        return v + UnityEngine.Random.Range(-v * withinPercentRange, v * withinPercentRange);
    }

	public static string[] CollectionToStringArray<T>(this System.Collections.Generic.ICollection<T> v)
	{
		string[] toRet = new string[v.Count];
		int i = 0; //cannot use for, for this situtation
		foreach(T elem in v)
		{
			toRet[i] = elem.ToString();
			i++;
		}
		return toRet;
	}

    

    public static string CollectionToString<T>(this System.Collections.Generic.ICollection<T> v)
    {
        return StringArrayToString(CollectionToStringArray<T>(v));
    }

    public static string StringArrayToString(this string[] v)
    {
        string toRet = "";
        foreach (string elem in v)
        {
            toRet += elem + ",";
        }

        if (string.IsNullOrEmpty(toRet))
            return toRet;
        return toRet.Substring(0, toRet.Length - 1); //remove last ","
    }

}
