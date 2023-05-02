using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace PowerControls;
public class Mask
{
    private string _mask;

    private Dictionary<char, string> _definitions = new();

    private char _placeholder;

    private char _caret;

    public string MaskString
    {
        get => _mask;
        set => _mask = value;
    }

    public ImmutableDictionary<char, string> Definitions => _definitions.ToImmutableDictionary();

    public char Placeholder
    {
        get => _placeholder;
        set => _placeholder = value;
    }

    public char Caret
    {
        get => _caret;
        set => _caret = value;
    }

    public Mask(string mask, Dictionary<string, string> definitions, char placeholder, char caret)
    {
        _mask = mask;
        _definitions = definitions.SelectMany(d => d.Key.Select(ch => new KeyValuePair<char, string>(ch, d.Value))).Distinct().Where(k => k.Key != ' ' && k.Key != '\\').ToDictionary(k => k.Key, v => v.Value);
        _placeholder = placeholder;
        Caret = caret;
    }

    public Mask(string mask, Dictionary<string, string> definitions, string placeholder) : this(mask, definitions, placeholder[0], '_') { }

    public Mask(string mask, char placeholder) : this(mask, new Dictionary<string, string>() { { "09#", new(@"\d") }, { "a", new(@"[A-Za-z]") }, { "*", new(@"[A-Za-z0-9]") } }, placeholder, '_') { }

    public Mask(string mask, string placeholder) : this(mask, placeholder[0]) { }

    public Mask(string mask) : this(mask, ' ') { }

    public void AddDef(string name, string reg) => name.Select(ch => ch).ToList().ForEach(ch => Definitions.Add(ch, new(reg)));

    public void RemoveDef(string names) => Definitions.RemoveRange(names);

    public (string, string) GetText(string text, bool setCaret = false)
    {
        if (MaskString == "")
        {
            return (text, text);
        }

        bool skip = false;
        string result = string.Join("", MaskString.Select(ch =>
        {
            if (ch == '\\')
            {
                skip = true;
                return "";
            }

            string res;

            if (Definitions.ContainsKey(ch) && !skip)
                res = Placeholder.ToString();
            else
                res = ch.ToString();

            skip = false;
            return res;
        }));

        var mask = Regex.Replace(MaskString, @"\\.", " ");

        var maskParts = mask.Where(ch => Definitions.ContainsKey(ch)).Select(ch => new Regex(Definitions[ch])).ToList();
        var maskIndex = mask.Select((ch, i) => Definitions.ContainsKey(ch) ? i : -1).Where(i => i > -1).ToList();

        if (text.Length > maskParts.Count)
            text = text[..maskParts.Count];

        for (int i = 0; i <= text.Length; i++)
        {
            if (i == text.Length)
            {
                if (text.Length < maskParts.Count && setCaret)
                {
                    result = result[..maskIndex[i]] + Caret + result[(maskIndex[i] + 1)..];
                }
                break;
            }
            if (!maskParts[i].IsMatch(text[i].ToString()))
            {
                text = text[..i];
                return (result, text);
            }
            result = result[..maskIndex[i]] + text[i] + result[(maskIndex[i] + 1)..];
        }

        return (result, text);
    }

    public static explicit operator Mask(string str) => new(str);
}
