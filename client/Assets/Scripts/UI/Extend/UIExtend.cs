using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonState
{
    Normal,
    Disable,
    Select,
    Pressed,
    Remove,
    Release,
    Off,
    On
}

public static class MeasureDefine
{
    public const string Percent = "%";
    public const string Plus = "+";
    public const string Minus = "-";
}

public static class ExtendMethods
{
    public static void SetActive(this Transform t, bool active)
    {
        if (t == null)
            return;
        t.gameObject.SetActive(active);
    }

    public static void SetActive(this MonoBehaviour m, bool active)
    {
        m.gameObject.SetActive(active);
    }

    public static bool GetActive(this Transform t)
    {
        return t.gameObject.activeInHierarchy;
    }

    public static bool IsNumber(this object o)
    {
        return o is int
               || o is float
               || o is double
               || o is long
               || o is uint
               || o is ulong;
    }

    public static string ToMoneyString(this uint i)
    {
        return i.ToString("#,0");
    }

    public static string ToMoneyString(this int i)
    {
        return i.ToString("#,0");
    }

    public static string ToMoneyString(this long l)
    {
        return l.ToString("#,0");
    }

    public static string ToMoneyString(this ulong l)
    {
        return l.ToString("#,0");
    }

    public static string ToMoneyString(this float f)
    {
        return f.ToString("#,0");
    }

    public static string ToPercentString(this int i)
    {
        return i + MeasureDefine.Percent;
    }

    public static string ToPercentString(this long l)
    {
        return l + MeasureDefine.Percent;
    }

    public static string ToFloatString(this float f, bool mustShowLowerFloat = true)
    {
        if (mustShowLowerFloat)
            return f.ToString("#0.00");
        else return f.ToString("#0.##");
    }

    public static bool Not(this bool v)
    {
        return !v;
    }

    public static string ToPercentString(this float f, bool toInt = false)
    {
        if (toInt)
            return f.ToString("00") + MeasureDefine.Percent;
        else
            return ToFloatString(f) + MeasureDefine.Percent;
    }


    public static string[] ToStrings(this int[] numbers)
    {
        if (numbers[0].IsNumber().Not())
            return new string[0];

        var s = new string[numbers.Length];
        Debug.Log(numbers.Length);
        for (int i = 0; i < numbers.Length; ++i)
            s[i] = numbers[i].ToString();

        return s;
    }

    public static string[] ToStrings(this float[] numbers)
    {
        if (numbers.IsNumber().Not())
            return new string[0];

        var s = new string[numbers.Length];
        for (int i = 0; i < numbers.Length; ++i)
            s[i] = numbers[i].ToString();

        return s;
    }

    public static void SetButtonState(this Transform button, ButtonState s)
    {
        var names = Enum.GetNames(typeof(ButtonState));
        foreach (var name in names)
        {
            var t = button.FindChild(name);
            if (t != null)
                t.SetActive(t.name == s.ToString());
        }
    }

    public static ButtonState GetButtonState(this Transform button)
    {
        var names = Enum.GetNames(typeof(ButtonState));
        foreach (var name in names)
        {
            var t = button.FindChild(name);
            if (t != null && t.GetActive())
                return (ButtonState)Enum.Parse(typeof(ButtonState), t.name);
        }
        return ButtonState.Remove;
    }

    public static void SetDepth(this Transform t, int depth)
    {
        t.GetComponent<UIWidget>().depth = depth;
    }

    public static int GetDepth(this Transform t)
    {
        return t.GetComponent<UIWidget>().depth;
    }

    private static bool buttonClicked = false;
	public static UIEventTrigger SetTrigger(this Transform b, EventDelegate.Callback onClick = null, bool buttonTween = true)
	{
		var t = GetTrigger(b, buttonTween);
		EventDelegate.Set(t.onClick, () =>
		{
			if (onClick != null && buttonClicked == false)
			{
				// 나중에 버튼 사운드 추가시 수정
				//PlayButtonClickSound();

				buttonClicked = true;
				DelayedCalls.Add(() => { buttonClicked = false; }, 0.1f);
				onClick();
			}
		});

		return t;
	}

	public static UIEventTrigger GetTrigger(this Transform b, bool buttonTween = false)
	{
		var t = b.GetComponent<UIEventTrigger>();
		if (t == null)
			t = b.gameObject.AddComponent<UIEventTrigger>();
		if (t.GetComponent<Collider>() == null)
			NGUITools.AddWidgetCollider(b.gameObject);
		if (buttonTween)
		{
			var s = b.GetComponent<UIButtonScale>();
			if (s == null)
			{
				s = b.gameObject.AddComponent<UIButtonScale>();
				s.hover = Vector3.one;
				s.pressed = new Vector3(1.1f, 1.1f, 1.1f);
				s.duration = 0.2f;
			}
		}
		t.GetComponent<Collider>().enabled = true;

		return t;
	}

	public static void RemoveTrigger(this Transform b, bool removeCollider = true)
	{
		var uiEventTrigger = b.GetComponent<UIEventTrigger>();
		if (uiEventTrigger != null)
			GameObject.Destroy(uiEventTrigger);

		if (removeCollider)
		{
			var collider = b.GetComponent<Collider>();
			if (collider != null)
				GameObject.Destroy(collider);
		}

		var buttonScale = b.GetComponent<UIButtonScale>();
		if (buttonScale != null)
			GameObject.Destroy(buttonScale);
	}

	public static void SetButtonTrigger(this Transform b, ButtonState s, EventDelegate.Callback onClick, bool buttonTween = true)
	{
		Transform targetT = b.FindChild(s.ToString());
		SetTrigger(targetT, onClick, buttonTween);
	}

	public static UIEventTrigger GetButtonTrigger(this Transform b, ButtonState s)
	{
		Transform targetT = b.FindChild(s.ToString());
		return GetTrigger(targetT);
	}

	public static UILabel GetLabel(this Transform t)
	{
		return t.GetComponent<UILabel>();
	}

	public static UILabel SetText(this Transform t, string text)
	{
		var label = GetLabel(t);
		label.text = text;
		return label;
	}

	public static UILabel SetTextWithShadow(this Transform t, string text)
	{
		var label = SetText(t, text);
		t.Find("shadow").SetText(text);
		return label;
	}

	public static string GetText(this Transform t)
	{
		return t.GetLabel().text;
	}

	public static UILabel SetChildText(this Transform t, string text)
	{
		return t.FindChild("label").SetText(text);
	}

	public static void SetSprite(this Transform t, string spriteName)
	{
		t.GetComponent<UISprite>().spriteName = spriteName;
	}

	public static void SetPixelPerfect(this Transform t)
	{
		t.GetComponent<UIWidget>().MakePixelPerfect();
	}

	public static void SetColor(this Transform t, Color color)
	{
		t.GetComponent<UIWidget>().color = color;
	}

    public static Color GetColor(this Transform t)
    {
        return t.GetComponent<UIWidget>().color;
    }

    public static void SetTexture(this Transform t, Texture tex)
	{
		var textureUI = t.GetComponent<UITexture>();
		if (textureUI != null)
			textureUI.mainTexture = tex;
	}
	public static bool IsNullOrEmpty(this string s)
	{
		if (s == "none" || s == "None" || s == "NONE")
			s = "";
		return string.IsNullOrEmpty(s);
	}

	public static bool IsNullOrEmpty(this System.Array array)
    {
        if (array == null || array.Length < 1)
            return true;

        return false;
    }

    public static bool IsInRange(this System.Array array, int index)
    {
        if (array.IsNullOrEmpty())
            return false;

        return index >= 0 && index < array.Length;
    }
    
    public static void Clear(this System.Array array)
    {
        System.Array.Clear(array, 0, array.Length);
    }

    public static bool IsEmpty(this System.Collections.IList list)
    {
        return list.Count < 1;
    }

    public static bool IsIncludeFlag(this Enum enumFlags, Enum checkFlag)
    {
        Type enumType = enumFlags.GetType();
        if (enumType != checkFlag.GetType())
            return false;

        int flag = (int)Enum.Parse(enumType, checkFlag.ToString());
        int flags = (int)Enum.Parse(enumType, enumFlags.ToString());

        return ((flags & flag) == flag);
    }

	public static void GridReposition(this Transform t)
	{
		t.GetComponent<UIGrid>().Reposition();
	}

	public static void GridRepositionNextFrame(this Transform t, MonoBehaviour mono)
	{
		mono.StartCoroutine(gridReposition(t));
	}

	private static IEnumerator gridReposition(Transform t)
	{
		yield return new WaitForEndOfFrame();
		t.GetComponent<UIGrid>().Reposition();
	}

	public static void StopAndStartCoroutine(this MonoBehaviour owner, IEnumerator coroutine, ref IEnumerator keep)
	{
		if (keep != null)
			owner.StopCoroutine(keep);

		keep = coroutine;
		owner.StartCoroutine(keep);
	}
}