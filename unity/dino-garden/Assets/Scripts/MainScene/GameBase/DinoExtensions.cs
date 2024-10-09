using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace DinoExtensions
{
    public static class IntExtensions
    {
        public static bool IsGreaterThan(this int i, int value)
        {
            return i > value;
        }

        public static string FormatMoney(this long i)
        {
            return i.ToString("N0");
        }

        public static string FormatMoney(this int i)
        {
            return i.ToString("N0");
        }

        public static string FormatMoney(this double i)
        {
            return i.ToString("N0");
        }

        public static string FormatMoney(this float i)
        {
            return i.ToString("N0");
        }

        static string FormatMoneyKMB(this int i)
        {
            if (i >= 100000000)
            {
                return (i / 1000000D).ToString("0.#M");
            }
            if (i >= 1000000)
            {
                return (i / 1000000D).ToString("0.##M");
            }
            if (i >= 100000)
            {
                return (i / 1000D).ToString("0.#k");
            }
            if (i >= 10000)
            {
                return (i / 1000D).ToString("0.##k");
            }

            return i.ToString("#,0");
        }

        static string FormatMoneyKMB(this double i)
        {
            if (i >= 100000000)
            {
                return (i / 1000000D).ToString("0.#M");
            }
            if (i >= 1000000)
            {
                return (i / 1000000D).ToString("0.##M");
            }
            if (i >= 100000)
            {
                return (i / 1000D).ToString("0.#k");
            }
            if (i >= 10000)
            {
                return (i / 1000D).ToString("0.##k");
            }

            return i.ToString("#,0");
        }
    }

    public static class ImageExtensions
    {
        public static void SetAlpha(this Image image, float value)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }
    }

    public static class TransformExtensions
    {
        public static void SetPositionX(this Transform transfrom, float value)
        {
            var position = transfrom.position;
            position.x = value;
            transfrom.position = position;
        }

        public static void SetPositionY(this Transform transfrom, float value)
        {
            var position = transfrom.position;
            position.y = value;
            transfrom.position = position;
        }

        public static void SetPositionZ(this Transform transfrom, float value)
        {
            var position = transfrom.position;
            position.z = value;
            transfrom.position = position;
        }

        public static void SetLocalPositionX(this Transform transfrom, float value)
        {
            var position = transfrom.localPosition;
            position.x = value;
            transfrom.localPosition = position;
        }

        public static void SetLocalPositionY(this Transform transfrom, float value)
        {
            var position = transfrom.localPosition;
            position.y = value;
            transfrom.localPosition = position;
        }

        public static void SetLocalPositionZ(this Transform transfrom, float value)
        {
            var position = transfrom.localPosition;
            position.z = value;
            transfrom.localPosition = position;
        }

        public static void SetLocalScaleX(this Transform transfrom, float value)
        {
            var scale = transfrom.localScale;
            scale.x = value;
            transfrom.localScale = scale;
        }

        public static void SetLocalScaleY(this Transform transfrom, float value)
        {
            var scale = transfrom.localScale;
            scale.y = value;
            transfrom.localScale = scale;
        }

        public static void SetLocalScaleZ(this Transform transfrom, float value)
        {
            var scale = transfrom.localScale;
            scale.z = value;
            transfrom.localScale = scale;
        }
    }

    public static class GameObjectExtensions
    {
        public static void ForceRebuildLayoutImmediate(this GameObject gameObject)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
        }

        public static void SetObjectAlpha(this GameObject gameObject, float alpha)
        {
            var canvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = alpha;
        }

        public static void DestroyAllChild(this GameObject gameObject)
        {
            foreach (Transform child in gameObject.transform)
            {
                UnityEngine.Object.Destroy(child.gameObject, 0);
            }
        }
    }

    public static class StringExtensions
    {
        public static int ToInt(this string str, int defaultVl = 0)
        {
            bool success = int.TryParse(str, out int x);
            if (success)
            {
                return x;
            }
            else
            {
                return defaultVl;
            }
        }

        public static float ToFloat(this string str, float defaultVl = 0.0f)
        {
            bool success = float.TryParse(str, out float x);
            if (success)
            {
                return x;
            }
            else
            {
                return defaultVl;
            }
        }
    }
}