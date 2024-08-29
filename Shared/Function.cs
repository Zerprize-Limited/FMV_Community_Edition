using Microsoft.AspNetCore.Components;
using System.Xml;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FMV_Standard.Shared
{
    public class Function
    {
        public ElementReference elementReference;
        private string _fnStyle;
        private string _FunctionType;
        private Dictionary<string, string[]> _variables = new Dictionary<string, string[]>();
        public Function(XmlNode fn)
        {
            this.IDNr = fn.SelectSingleNode("IDNr")!.InnerText;
            var _x = fn.SelectSingleNode("@x")?.Value ?? "50";
            if (_x == "") _x = "50";
            this.x = double.Parse(_x, CultureInfo.InvariantCulture);
            var _y = fn.SelectSingleNode("@y")?.Value ?? "50";
            if (_y == "") _y = "50";
            this.y = double.Parse(_y, CultureInfo.InvariantCulture);
            _fnStyle = fn.SelectSingleNode("@fnStyle")?.Value ?? "0";
            _FunctionType = fn.SelectSingleNode("FunctionType")?.InnerText ?? "2";
            this.IDName = fn.SelectSingleNode("IDName")?.InnerText ?? "";
            this.orphans = int.Parse(fn.SelectSingleNode("@orphans")?.Value ?? "0");
            this.isInput = fn.SelectSingleNode("@isInput")?.Value ?? "false";
            this.fnColorStyle = fn.SelectSingleNode("@style")?.Value ?? "";
            this.fnColorValue = fn.SelectSingleNode("@color")?.Value ?? "";
            options = $"{fnStyle}:{FunctionType}:";
            this.profileFn = fn.SelectSingleNode("@profileFn")?.Value ?? "";
            this.profileI = fn.SelectSingleNode("@profileI")?.Value ?? "";
            this.profileP = fn.SelectSingleNode("@profileP")?.Value ?? "";
            this.profileR = fn.SelectSingleNode("@profileR")?.Value ?? "";
            this.profileC = fn.SelectSingleNode("@profileC")?.Value ?? "";
            this.profileT = fn.SelectSingleNode("@profileT")?.Value ?? "";
            this.metadata = new List<string[]>();
            if (fn.SelectSingleNode("metadata") is not null)
            {
                foreach (XmlElement key in fn.SelectSingleNode("metadata")!)
                {
                    this.metadata.Add(new string[] { key.Name, key.InnerText, key.Attributes["active"]?.Value ?? "", key.Attributes["equation"]?.Value ?? "", key.InnerText });
                }
            }
            this.Description = fn.SelectSingleNode("Description")?.InnerText ?? "";
        }
        public List<string[]> metadata { get; set; }
        public string options { get; set; }
        public string IDNr { get; set; }
        public int orphans { get; set; }
        public string isInput { get; set; }
        public string fnColorStyle { get; set; }
        public string fnColorValue { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public string fmiType { get; set; } = "";
        public string fmiHighlight { get; set; } = "";
        public List<string> activeI { get; set; } = new List<string>();
        public List<string> activeP { get; set; } = new List<string>();
        public List<string> activeR { get; set; } = new List<string>();
        public List<string> activeC { get; set; } = new List<string>();
        public List<string> activeT { get; set; } = new List<string>();
        public List<string> totalI { get; set; } = new List<string>();
        public List<string> totalP { get; set; } = new List<string>();
        public List<string> totalR { get; set; } = new List<string>();
        public List<string> totalC { get; set; } = new List<string>();
        public List<string> totalT { get; set; } = new List<string>();
        public bool wasActive { get; set; } = false;
        public string profileFn { get; set; }
        public string profileI { get; set; }
        public string profileP { get; set; }
        public string profileR { get; set; }
        public string profileC { get; set; }
        public string profileT { get; set; }
        public double startX { get; set; } = 0;
        public double startY { get; set; } = 0;
        public Dictionary<string, double> Kvalues { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, string> Cvalues { get; set; } = new Dictionary<string, string>();
        public string toolTip { get; set; } = "";
        public bool dragFn { get; set; } = false;
        public string fnClass { get; set; } = "fn-point";
        public string fnStyle
        {
            get { return _fnStyle; }
            set
            {
                _fnStyle = value;
                options = $"{_fnStyle}:{_FunctionType}:";
            }
        }
        public string FunctionType
        {
            get { return _FunctionType; }
            set
            {
                _FunctionType = value;
                options = $"{_fnStyle}:{_FunctionType}:";
            }
        }
        public string IDName { get; set; }
        public string Description { get; set; }
        public List<string> ReturnTextLines(int length)
        {
            var textLines = new List<string>();
            string[] textWords = IDName.Split(" ");
            int tL = 0;
            textLines.Add("");
            int lL = length;
            foreach (string tWord in textWords)
            {
                var doTWord = tWord;
                if (doTWord.StartsWith(Environment.NewLine))
                {
                    textLines.Add("");
                    tL++;
                    lL = length;
                    doTWord = tWord.Replace(Environment.NewLine, "");
                }
                if (doTWord.Contains(Environment.NewLine) && !doTWord.EndsWith(Environment.NewLine))
                {
                    if (lL < length)
                    {
                        textLines[tL] += " ";
                    }
                    string[] splitLines = doTWord.Split(Environment.NewLine);
                    int sLi = 0;
                    foreach (string splitWord in splitLines)
                    {
                        doTWord = splitWord;
                        if (sLi < splitLines.Length - 1)
                        {
                            textLines[tL] += splitWord;
                            textLines.Add("");
                            tL++;
                            lL = length;
                        }
                        sLi++;
                    }
                }
                if (doTWord.Length <= lL)
                {
                    if (lL < length)
                    {
                        textLines[tL] += " ";
                    }
                    textLines[tL] += doTWord;
                    lL -= doTWord.Length;
                    if (doTWord.EndsWith(Environment.NewLine))
                    {
                        textLines.Add("");
                        tL++;
                        lL = length;
                    }
                }
                else if (doTWord.Length > length)
                {
                    string tempWord = doTWord;
                    while (tempWord.Length > 0)
                    {
                        if (tL > 0 || tempWord.Length > length + 1) 
                        {
                            textLines.Add("");
                            tL++;
                        }
                        lL = length;
                        string addWord = tempWord.Substring(0, Math.Min(length + 1, tempWord.Length));
                        textLines[tL] += addWord;
                        lL -= addWord.Length;
                        tempWord = tempWord.Substring(addWord.Length);
                    }
                }
                else
                {
                    textLines.Add("");
                    tL++;
                    textLines[tL] += doTWord;
                    lL = length - doTWord.Length;
                }
            }
            return textLines;
        }
        public bool isActive()
        {
            if (activeI.Count == 0)
            {
                return false;
            }
            else
            {
                switch (profileI)
                {
                    case "": if (activeI.Count < totalI.Count) return false; break;
                    case "All": if (activeI.Count < totalI.Count) return false; break;
                    case "Any": if (totalI.Count > 0 && activeI.Count == 0) return false; break;
                }
                switch (profileP)
                {
                    case "": if (activeP.Count < totalP.Count) return false; break;
                    case "All": if (activeP.Count < totalP.Count) return false; break;
                    case "Any": if (totalP.Count > 0 && activeP.Count == 0) return false; break;
                }
                switch (profileR)
                {
                    case "": if (activeR.Count < totalR.Count) return false; break;
                    case "All": if (activeR.Count < totalR.Count) return false; break;
                    case "Any": if (totalR.Count > 0 && activeR.Count == 0) return false; break;
                }
                switch (profileT)
                {
                    case "": if (activeT.Count < totalT.Count) return false; break;
                    case "All": if (activeT.Count < totalT.Count) return false; break;
                    case "Any": if (totalT.Count > 0 && activeT.Count == 0) return false; break;
                }
                switch (profileC)
                {
                    case "": if (activeC.Count < totalC.Count) return false; break;
                    case "All": if (activeC.Count < totalC.Count) return false; break;
                    case "Any": if (totalC.Count > 0 && activeC.Count == 0) return false; break;
                }
                return true;
            }
        }
        public void resetAspects()
        {
            activeI.Clear();
            activeP.Clear();
            activeR.Clear();
            activeC.Clear();
            activeT.Clear();
            totalI.Clear();
            totalP.Clear();
            totalR.Clear();
            totalC.Clear();
            totalT.Clear();
            fmiHighlight = "";
            wasActive = false;
        }
        public Dictionary<string, string[]> getVariables()
        {
            return _variables;
        }
        public void newVariable(string key, string[] value)
        {
            _variables[key] = value;
        }
        public string[]? getVariable(string key)
        {
            if (_variables.ContainsKey(key))
            {
                //Console.WriteLine($"getVariable: {key} | {_variables[key][0]} | {_variables[key][1]}");
                return _variables[key];
            }
            else
            {
                return null;
            }
        }
        public void resetVariables()
        {
            _variables = new Dictionary<string, string[]>();
        }
        public void removeVariable(string key)
        {
            foreach (var s in _variables.Where(kv => kv.Key.StartsWith(key)).ToList())
            {
                _variables.Remove(s.Key);
                //Console.WriteLine($"Removed: {s.Key}");
            }
        }
        public void resetKvalues()
        {
            Kvalues = new Dictionary<string, double>();
            Cvalues = new Dictionary<string, string>();
        }
        public string returnTrue()
        {
            return "1";
        }
    }
}