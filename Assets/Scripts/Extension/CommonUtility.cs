using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEditor;
using System.Reflection;
using Lean.Pool;
using System.Text;
using System.IO.Compression;
using System.Globalization;

public static class CommonUtility
{
    public static float fixedDeltaTime => Time.fixedDeltaTime * Time.timeScale;

    public readonly static YieldInstruction fixedUpdate = new WaitForFixedUpdate();
    public readonly static YieldInstruction endOfFrame = new WaitForEndOfFrame();

    static Dictionary<float, YieldInstruction> yieldSecDic = new Dictionary<float, YieldInstruction>();

    public static YieldInstruction GetYieldSec(float sec)
    {
        if (!yieldSecDic.ContainsKey(sec))
            yieldSecDic.Add(sec, new WaitForSeconds(sec));
        return yieldSecDic[sec];
    }

    public static void ForEach<T>(this T[] array, Action<T> callback)
    {
        foreach (T element in array)
        {
            callback(element);
        }
    }

    public static T Find<T>(this T[] array, Func<T, bool> callback)
    {
        foreach (T data in array)
        {
            if ((bool)callback?.Invoke(data))
                return data;
        }
        return default;
    }

    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        System.Random random = new System.Random();
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
    public static IEnumerator AnimateImage(this Image image, List<Sprite> targetSprites, float delay, bool isInfinite = false, Action onComplete = null)
    {
        do
        {
            for (int i = 0; i < targetSprites.Count; i++)
            {
                var targetSprite = targetSprites[i];
                image.SetSprite(targetSprite);
                yield return GetYieldSec(delay);
            }
        }
        while (isInfinite);
        onComplete?.Invoke();
    }
    public static void InterpolateImageAlpha(this Image targetImage, float targetAlpha, float t)
        => targetImage.color = targetImage.color.CopyColor(a: Mathf.Lerp(targetImage.color.a, targetAlpha, t));

    public static bool IsNullOrEmpty(this string target)
        => string.IsNullOrEmpty(target);

    public static T LoadResource<T>(this string path)
        where T : Object
        => Resources.Load<T>($"{Path.GetDirectoryName(path)}/{Path.GetFileNameWithoutExtension(path)}");

    public static Vector2 Copy(this Vector2 v, float? x = null, float? y = null)
        => new Vector2(x ?? v.x, y ?? v.y);

    public static Vector3 Copy(this Vector3 v, float? x = null, float? y = null, float? z = null)
        => new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);

    public static void SetRateOverTime(this ParticleSystem.EmissionModule emission, float rate)
        => emission.rateOverTime = rate;

    public static void SetMaxParticleCount(this ParticleSystem.MainModule mainModule, int count)
        => mainModule.maxParticles = count;

    public static bool PermissionRange(this Vector3 v, Vector3 target, float range)
        => Mathf.Abs(target.x - v.x) <= range &&
           Mathf.Abs(target.y - v.y) <= range &&
           Mathf.Abs(target.z - v.z) <= range;

    public static Color CopyColor(this Color color, float? r = null, float? g = null, float? b = null, float? a = null)
        => new Color(r ?? color.r, g ?? color.g, b ?? color.b, a ?? color.a);

    public static void SetSprite(this Image image, Sprite sprite)
        => image.sprite = sprite;

    public static void SetText(this Text text, string target)
        => text.text = target;
    public static void SetColor(this Image image, Color32 color)
        => image.color = color;

    public static void SetColor(this Text text, Color32 color)
        => text.color = color;

    public static Vector2Int ToVector2Int(this Vector2 targetVec, Func<float, int> converter)
        => new Vector2Int((int)converter?.Invoke(targetVec.x), (int)converter?.Invoke(targetVec.y));
    public static int ToClamp(this int target, int min, int max)
        => Mathf.Clamp(target, min, max);
    public static Vector3 MultiplyVector(this Vector3 target, Vector3 targetVec)
        => new Vector3(target.x * targetVec.x, target.y * targetVec.y, target.z * targetVec.z);
    public static Vector3 DevideVector(this Vector3 target, Vector3 targetVec)
        => new Vector3(target.x / targetVec.x, target.y / targetVec.y, target.z / targetVec.z);
    public static Vector2 DevideVector(this Vector2 target, Vector2 targetVec)
        => new Vector2(target.x / targetVec.x, target.y / targetVec.y);

    public static void ChangeLayersRecursively(this Transform target, string name)
    {
        target.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in target)
        {
            ChangeLayersRecursively(child, name);
        }
    }


    public static string ToExpString(this int target)
        => target.ToString("#,##0");

    // ; => newLine
    public static string ParseEscapeSequence(this string target)
    {
        target = target.Replace(";", Environment.NewLine);
        return target;
    }
    public static void CopyToClipboard(this string str)
    {
        GUIUtility.systemCopyBuffer = str;
    }

    public static byte[] ToGZip(this string str)
    {
        byte[] data = Encoding.UTF8.GetBytes(str);

        GZipStream hgs;
        byte[] cmpData;

        using (MemoryStream cmpStream = new MemoryStream())
        using (hgs = new GZipStream(cmpStream, CompressionMode.Compress))
        {
            hgs.Write(data, 0, data.Length);
            hgs.Close();
            cmpData = cmpStream.ToArray();
        }
        return cmpData;
    }

    public static string GetFullPath(this Transform trn)
    {
        string path = "/" + trn.name;
        while (trn.transform.parent != null)
        {
            trn = trn.parent;
            path = "/" + trn.name + path;
        }
        return path;
    }

    public static T ToEnum<T>(this string target)
        => (T)Enum.Parse(typeof(T), target);

    public static Color32 GetProductTagColorByValue(int value)
    {
        if (value >= 0 && value < 50)
        {
            // E15041
            return new Color32(0xE1, 0x50, 0x41, 0xFF);
        }
        else if (value >= 0 && value < 70)
        {
            // 8241E1
            return new Color32(0x82, 0x41, 0xE1, 0xFF);
        }
        else if (value >= 70 && value < 85)
        {
            // FFBB42
            return new Color32(0xFF, 0xBB, 0x42, 0xFF);
        }
        else if (value >= 85 && value < 100)
        {
            // 41E1C5
            return new Color32(0x41, 0xE1, 0xC5, 0xFF);
        }
        else
        {
            return new Color32(0xE1, 0x50, 0x41, 0xFF);
        }
    }

    public static void EnumForEach<T>(Action<T> callback)
    {
        var elements = Enum.GetValues(typeof(T));
        foreach (var element in elements)
        {
            callback?.Invoke((T)element);
        }
    }

    public static void SetSprite(this SpriteRenderer spriteRenderer, Sprite sprite)
        => spriteRenderer.sprite = sprite;


    public static Transform GetRootDirectChild(this RectTransform rectTransform)
        => rectTransform.GetChild(0);

    public static int ParseToGlobalInt(this string target)
        => int.Parse(target, CultureInfo.InvariantCulture);

    public static float ParseToGlobalFloat(this string target)
        => float.Parse(target, CultureInfo.InvariantCulture);

    public static Vector2 SetX(this Vector2 vector2, float x)
    {
        vector2.x = x;
        return vector2;
    }

    public static Vector2 SetY(this Vector2 vector2, float y)
    {
        vector2.y = y;
        return vector2;
    }

    public static float Clamp01Value(this float value)
        => Mathf.Clamp01(value);

    public static List<BehaviourState> BehavioursToList(this S3BehaviourDiceData behaviourDiceData)
    {
        var list = new List<BehaviourState>();
        list.Add(behaviourDiceData.diceFace_1);
        list.Add(behaviourDiceData.diceFace_2);
        list.Add(behaviourDiceData.diceFace_3);
        list.Add(behaviourDiceData.diceFace_4);
        list.Add(behaviourDiceData.diceFace_5);
        list.Add(behaviourDiceData.diceFace_6);
        return list;
    }

    public static List<int> ActingPowerToList(this S3ActingPowerDiceData actingPowerDiceData)
    {
        var list = new List<int>();
        list.Add(actingPowerDiceData.diceFace_1);
        list.Add(actingPowerDiceData.diceFace_2);
        list.Add(actingPowerDiceData.diceFace_3);
        list.Add(actingPowerDiceData.diceFace_4);
        list.Add(actingPowerDiceData.diceFace_5);
        list.Add(actingPowerDiceData.diceFace_6);
        return list;
    }
}
