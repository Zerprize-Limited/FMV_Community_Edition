using FMV_Standard.Shared;
using Microsoft.AspNetCore.Components;
using System.Data;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace FMV_Standard.Shared
{
    public class StateContainer
    {
        private string _selectedFn = "-1";
        private string _selectedLabel = "";
        public bool showProperties = false;
        public bool showAspects = false;
        public bool showFMIProfile = false;
        public bool showMetadata = false;
        public bool showKeys = false;
        public string lastKey = "";
        public int cycleFMI = -1;
        public Function? selectedFunction;
        public Coupling? selectedCoupling;
        public DataSet metadataDS = new DataSet();
        public Dictionary<string, string> metaDataKeys = new Dictionary<string, string>();
        public bool metaIsDirty = false;
        public string fnClass { get; set; } = "fn-hover";
        public string selectedFn
        {
            get
            {
                return _selectedFn;
            }
            set
            {
                if (selectedFunction is not null)
                {
                    if (metaIsDirty)
                    {
                        var fnMD = projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/metadata");
                        if (fnMD is not null)
                        {
                            fnMD.RemoveAll();
                            foreach (var keyValue in selectedFunction.metadata)
                            {
                                XmlNode addMeta = projectData_Undo[0].CreateElement(keyValue[0]);
                                addMeta.InnerText = keyValue[1];
                                XmlAttribute metaCheck = projectData_Undo[0].CreateAttribute("active");
                                metaCheck.Value = keyValue[2];
                                addMeta.Attributes!.Append(metaCheck);
                                XmlAttribute metaEquation = projectData_Undo[0].CreateAttribute("equation");
                                metaEquation.Value = keyValue[3];
                                addMeta.Attributes.Append(metaEquation);
                                fnMD.AppendChild(addMeta);
                            }
                        }
                        metaIsDirty = false;
                    }
                }
                _selectedFn = value;
                if (selectedMulti.Count > 0)
                {
                    foreach (var fnIDNr in selectedMulti)
                    {
                        var fn = functionList.Find(x => x.IDNr == fnIDNr);
                        if (fn is not null)
                        {
                            fn.dragFn = false;
                        }
                    }
                }
                selectedMulti = new List<string>();
                if (functionList is not null)
                {
                    selectedFunction = functionList.Find(x => x.IDNr == _selectedFn);
                }
                else
                {
                    selectedFunction = null;
                }
                if (_selectedFn != "-1" && selectedFunction is not null)
                {
                    selectedMulti.Add(value);
                }
                else
                {
                    isDisabled = true;
                    fnName = "";
                }
                lastKey = "";
                showVariables = "";
            }
        }
        public string selectedLabel
        {
            get
            {
                return _selectedLabel;
            }
            set
            {
                _selectedLabel = value;
                if (couplingList is not null)
                {
                    selectedCoupling = couplingList.Find(x => x.Name == _selectedLabel);
                }
                else
                {
                    selectedCoupling = null;
                }
                if (_selectedLabel != "" && selectedCoupling is not null)
                {
                    selectedCoupling!.aspectClass = "fn-hover";
                }
            }
        }
        public string touchAction { get; set; } = "auto";
        public int newFnStyle { get; set; } = 0;
        public int spreadsheetVisible { get; set; } = 0;
        public string aspectLabelsDisplay { get; set; } = "";
        public double tempZoomF { get; set; } = 1;
        public double tempZoomA { get; set; } = 1;
        public double scaleZoom { get; set; } = 1.5;
        public double canvasWidth { get; set; }
        public double canvasHeight { get; set; }
        public double viewWidth { get; set; } = 0;
        public double viewHeight { get; set; } = 0;
        public XmlDocument[] projectData_Undo { get; set; } = new XmlDocument[700];
        public readonly int undoLength = 700;
        private readonly int moveBack = 200;
        public string[] selectedFn_Undo { get; set; } = new string[700];
        public int undoIndex { get; set; } = 0;
        public List<Coupling> couplingList { get; set; } = new List<Coupling>();
        public List<Function> functionList { get; set; } = new List<Function>();
        public string fnName { get; set; } = "";
        public bool isDisabled { get; set; } = true;
        public List<Aspect> aspectsList { get; set; } = new List<Aspect>();
        public List<Aspect> outputsList { get; set; } = new List<Aspect>();
        public bool showModal { get; set; } = false;
        public bool showColorPicker { get; set; } = false;
        public bool showFMIPopup { get; set; } = false;
        public bool showPDFreport { get; set; } = false;
        public bool showFMIDisplaySetup { get; set; } = false;
        public string fmiMessage { get; set; } = "";
        public string fileName { get; set; } = "FMV_new.xfmv";
        public bool fileLoaded { get; set; } = false;
        public string inputDropStatus { get; set; } = "file-input-zone-drop";
        public string debugOutput { get; set; } = "";
        public bool isBackup { get; set; } = false;
        public int cycleDelay { get; set; } = 750;
        public string showVariables { get; set; } = "";
        public double selectX { get; set; } = 0;
        public double selectY { get; set; } = 0;
        public double selectWidth { get; set; } = 0;
        public double selectHeight { get; set; } = 0;
        public List<string> selectedMulti { get; set; } = new List<string>();
        public bool isSelecting { get; set; } = false;
        public double startX { get; set; } = 0;
        public double startY { get; set; } = 0;
        public Dictionary<string, string> displaySetup = new Dictionary<string, string>();
        public Dictionary<string, double> displayValue = new Dictionary<string, double>();
        public bool hideFMIhighlight { get; set; } = false;
        public bool hideFMIdisplaytext { get; set; } = false;
        public void defaultFnLabel()
        {
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/IDName")!.InnerText = _selectedFn;
            selectedFunction!.IDName = _selectedFn;
            fnName = _selectedFn;
        }
        public void sFnIsInput(string isInput)
        {
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/@isInput")!.InnerText = isInput;
            selectedFunction!.isInput = isInput;
        }
        public void sFnOrphans(int orphans)
        {
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/@orphans")!.InnerText = orphans.ToString();
            selectedFunction!.orphans = orphans;
        }
        public void sFnFunctionType(string FunctionType)
        {
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/FunctionType")!.InnerText = FunctionType;
            selectedFunction!.FunctionType = FunctionType;
        }
        public void deleteFn()
        {
            if (aspectsList.FindAll(x => x.FunctionIDNr == _selectedFn).Count == 0 && projectData_Undo[0].SelectNodes($"//FM//Outputs/Output[FunctionIDNr=\"{_selectedFn}\"]")?.Count == 0)
            {
                updateUndo();
                projectData_Undo[0].SelectSingleNode("//FM/Functions")!.RemoveChild(projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]")!);
                functionList.RemoveAll(x => x.IDNr == _selectedFn);
                couplingList.RemoveAll(x => x.outputFn == _selectedFn || x.toFn == _selectedFn);
                XmlNodeList oldChildren = projectData_Undo[0].SelectNodes($"//FM/Aspects/Aspect[@outputFn='{_selectedFn}' or @toFn='{_selectedFn}']")!;
                foreach (XmlNode oldChild in oldChildren)
                {
                    projectData_Undo[0].SelectSingleNode("//FM/Aspects")!.RemoveChild(oldChild);
                }
                _selectedFn = "-1";
            }
        }
        public void sFnIDName(string IDName)
        {
            updateUndo();
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/IDName")!.InnerText = IDName;
            selectedFunction!.IDName = IDName;
        }
        public void sFnDesc(string FnDesc)
        {
            updateUndo();
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/Description")!.InnerText = FnDesc;
            selectedFunction!.Description = FnDesc;
        }
        public void updateFnXY(double x, double y, string? sFn = null)
        {
            if (sFn == null)
            {
                sFn = _selectedFn;
            }
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={sFn}]/@x")!.InnerText = x.ToString("0.##", CultureInfo.InvariantCulture);
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={sFn}]/@y")!.InnerText = y.ToString("0.##", CultureInfo.InvariantCulture);
        }
        public void updateAspectXY(double x, double y, string directionX, string directionY)
        {
            projectData_Undo[0].SelectSingleNode($"//FM/Aspects/Aspect[Name=\"{selectedLabel}\"]/@x")!.InnerText = x.ToString("0.##", CultureInfo.InvariantCulture);
            projectData_Undo[0].SelectSingleNode($"//FM/Aspects/Aspect[Name=\"{selectedLabel}\"]/@y")!.InnerText = y.ToString("0.##", CultureInfo.InvariantCulture);
            projectData_Undo[0].SelectSingleNode($"//FM/Aspects/Aspect[Name=\"{selectedLabel}\"]/@directionX")!.InnerText = directionX;
            projectData_Undo[0].SelectSingleNode($"//FM/Aspects/Aspect[Name=\"{selectedLabel}\"]/@directionY")!.InnerText = directionY;
        }
        public void updateDisplaySetup(string index, string attr, string value)
        {
            if (projectData_Undo[0].SelectSingleNode($"//FM/Functions/@{attr}") == null)
            {
                projectData_Undo[0].SelectSingleNode("//FM/Functions")!.Attributes!.Append(projectData_Undo[0].CreateAttribute(attr));
            }
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/@{attr}")!.Value = value;
            displaySetup[attr] = value;
            double dValue = double.NaN;
            double.TryParse(value, CultureInfo.InvariantCulture, out dValue);
            if (displaySetup[attr] == "")
            {
                displayValue[attr] = double.NaN;
            }
            else
            {
                displayValue[attr] = dValue;
            }
            checkDisplayValues(index);
        }
        public void checkDisplayValues(string index)
        {
            var minA = Math.Min(displayValue[$"colour_{index}4"], displayValue[$"colour_{index}3"]);
            minA = Math.Max(minA, displayValue[$"colour_{index}2"]);
            minA = Math.Max(minA, displayValue[$"colour_{index}1"]);
            if (minA is double.NaN) { minA = 0; }

            var maxA = Math.Max(displayValue[$"colour_{index}0"], displayValue[$"colour_{index}1"]);
            maxA = Math.Max(maxA, displayValue[$"colour_{index}2"]);
            maxA = Math.Max(maxA, displayValue[$"colour_{index}3"]);
            if (maxA is double.NaN) { maxA = 1; }

            if (displayValue[$"colour_{index}0"] is double.NaN) { displayValue[$"colour_{index}0"] = minA - 1.0; }
            if (displayValue[$"colour_{index}4"] is double.NaN) { displayValue[$"colour_{index}4"] = maxA * 1.0; }
            if (displayValue[$"colour_{index}2"] is double.NaN) { displayValue[$"colour_{index}2"] = (displayValue[$"colour_{index}0"] + displayValue[$"colour_{index}4"]) * 0.5; }
            if (displayValue[$"colour_{index}1"] is double.NaN) { displayValue[$"colour_{index}1"] = (displayValue[$"colour_{index}0"] + displayValue[$"colour_{index}2"]) * 0.5; }
            if (displayValue[$"colour_{index}3"] is double.NaN) { displayValue[$"colour_{index}3"] = (displayValue[$"colour_{index}2"] + displayValue[$"colour_{index}4"]) * 0.5; }
        }
        public void initialDisplaySetup(string attr, string value)
        {
            if (projectData_Undo[0].SelectSingleNode($"//FM/Functions/@{attr}") == null)
            {
                projectData_Undo[0].SelectSingleNode("//FM/Functions")!.Attributes!.Append(projectData_Undo[0].CreateAttribute(attr));
                projectData_Undo[0].SelectSingleNode($"//FM/Functions/@{attr}")!.Value = value;
            }
            double dValue = double.NaN;
            if (projectData_Undo[0].SelectSingleNode($"//FM/Functions/@{attr}")!.Value is not null)
            {
                displaySetup[attr] = projectData_Undo[0].SelectSingleNode($"//FM/Functions/@{attr}")!.Value!;
                double.TryParse(displaySetup[attr], CultureInfo.InvariantCulture, out dValue);
            }
            else
            {
                displaySetup[attr] = "";
            }
            if (displaySetup[attr] == "")
            {
                displayValue[attr] = double.NaN;
            }
            else
            {
                displayValue[attr] = dValue;
            }
        }
        public void updateKvalue(Function fn, string attr, string value)
        {
            if (projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={fn.IDNr}]/@{attr}") == null)
            {
                projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={fn.IDNr}]")!.Attributes!.Append(projectData_Undo[0].CreateAttribute(attr));
            }
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={fn.IDNr}]/@{attr}")!.Value = value;
            double tryValue = double.NaN;
            double.TryParse(value, CultureInfo.InvariantCulture, out tryValue);
            fn.Kvalues[attr] = tryValue;
        }
        public void updateColor(string selectedColor)
        {
            updateUndo();
            var setStyle = "custom";
            if (projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/@style") == null)
            {
                XmlAttribute fnColorStyle = projectData_Undo[0].CreateAttribute("style");
                projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]")!.Attributes!.Append(fnColorStyle);
            }
            if (projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/@color") == null)
            {
                XmlAttribute fnColorValue = projectData_Undo[0].CreateAttribute("color");
                projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]")!.Attributes!.Append(fnColorValue);
            }
            if (selectedColor == "")
            {
                setStyle = "";
            }
            else
            {
                selectedColor = uint.Parse(selectedColor.Replace("#", ""), NumberStyles.HexNumber).ToString();
            }
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/@style")!.InnerText = setStyle;
            selectedFunction!.fnColorStyle = setStyle;
            projectData_Undo[0].SelectSingleNode($"//FM/Functions/Function[IDNr={_selectedFn}]/@color")!.InnerText = selectedColor;
            selectedFunction!.fnColorValue = selectedColor;
        }
        public void updateUndo()
        {
            if (projectData_Undo[0] is not null)
            {
                if (undoIndex > undoLength - 5)
                {
                    Array.Copy(projectData_Undo, moveBack + 1, projectData_Undo, 1, undoIndex - moveBack);
                    Array.Copy(selectedFn_Undo, moveBack + 1, selectedFn_Undo, 1, undoIndex - moveBack);
                    undoIndex -= moveBack;
                }
                undoIndex += 1;
                projectData_Undo[undoIndex] = (XmlDocument)projectData_Undo[0].Clone();
                selectedFn_Undo[undoIndex] = _selectedFn;
                projectData_Undo[undoIndex + 1] = new XmlDocument();
                projectData_Undo[undoIndex + 2] = new XmlDocument();
            }
        }
        public void reSetAspect(string[] dictArray, double xO, double yO, double aX, double aY, int aFW)
        {
            var aName = string.Join("|", dictArray);
            bool appendNew = false;
            double labelDx = 0;
            double labelDy = 0;
            string directionX = "from";
            string directionY = "to";
            string notGroup = "true";
            XmlNode? aspectI = projectData_Undo[0].SelectSingleNode($"//FM/Aspects/Aspect[Name=\"{aName}\"]");
            if (aspectI == null)
            {
                aspectI = projectData_Undo[0].CreateElement("Aspect");
                XmlAttribute aspectIx = projectData_Undo[0].CreateAttribute("x");
                aspectIx.Value = "0";
                aspectI.Attributes!.Append(aspectIx);
                XmlAttribute aspectIy = projectData_Undo[0].CreateAttribute("y");
                aspectIy.Value = "0";
                aspectI.Attributes.Append(aspectIy);
                XmlAttribute aspectIdx = projectData_Undo[0].CreateAttribute("directionX");
                aspectIdx.Value = "from";
                aspectI.Attributes.Append(aspectIdx);
                XmlAttribute aspectIdy = projectData_Undo[0].CreateAttribute("directionY");
                aspectIdy.Value = "to";
                aspectI.Attributes.Append(aspectIdy);
                XmlAttribute aspectIof = projectData_Undo[0].CreateAttribute("outputFn");
                aspectIof.Value = dictArray[0];
                aspectI.Attributes.Append(aspectIof);
                XmlAttribute aspectItf = projectData_Undo[0].CreateAttribute("toFn");
                aspectItf.Value = dictArray[2];
                aspectI.Attributes.Append(aspectItf);
                XmlNode aspectIname = projectData_Undo[0].CreateElement("Name");
                aspectIname.InnerText = aName;
                aspectI.AppendChild(aspectIname);
                appendNew = true;
            }
            else
            {
                labelDx = 0;
                labelDy = 0;
                double.TryParse(aspectI.SelectSingleNode("@x")?.InnerText ?? "0", CultureInfo.InvariantCulture, out labelDx);
                double.TryParse(aspectI.SelectSingleNode("@y")?.InnerText ?? "0", CultureInfo.InvariantCulture, out labelDy);
                directionX = aspectI.SelectSingleNode("@directionX")?.InnerText ?? "from";
                directionY = aspectI.SelectSingleNode("@directionY")?.InnerText ?? "to";
                if (aspectI.SelectSingleNode("Curve") != null)
                {
                    aspectI.RemoveChild(aspectI.SelectSingleNode("Curve")!);
                }
                if (aspectI.SelectSingleNode("Curve2") != null)
                {
                    aspectI.RemoveChild(aspectI.SelectSingleNode("Curve2")!);
                }
                if (aspectI.SelectSingleNode("@notGroup") != null)
                {
                    notGroup = aspectI.SelectSingleNode("@notGroup")?.InnerText ?? "true";
                }
            }
            XmlNode aspectIcurve = projectData_Undo[0].CreateElement("Curve");
            XmlNode aspectIcurve2 = projectData_Undo[0].CreateElement("Curve2");
            Coupling addCoupling = new(aName, labelDx, labelDy, directionX, directionY, notGroup, dictArray[0], dictArray[1], dictArray[2], dictArray[3],
                functionList.Find(x => x.IDNr == dictArray[0])!.x + xO, functionList.Find(x => x.IDNr == dictArray[0])!.y + yO, 0, 0);
            couplingList.Add(addCoupling);
            addCoupling.ReturnTextLines(aFW);
            aspectIcurve2.InnerText = addCoupling.reDrawLines(functionList.Find(x => x.IDNr == dictArray[2])!.x + aX,
                functionList.Find(x => x.IDNr == dictArray[2])!.y + aY, false);
            aspectIcurve.InnerText = addCoupling.curve;
            aspectI.AppendChild(aspectIcurve);
            aspectI.AppendChild(aspectIcurve2);
            if (appendNew) projectData_Undo[0].SelectSingleNode("//FM/Aspects")!.AppendChild(aspectI);
        }
        public void CreateMetadataTable(XmlNode fn)
        {
            if (!metadataDS.Tables.Contains("Metadata"))
            {
                DataTable metadataTable = new DataTable("Metadata");
                metadataTable.Columns.Add(new DataColumn("Cycle", typeof(int)));
                DataColumn colIdNr = new DataColumn("IDNr", typeof(int));
                metadataTable.Columns.Add(colIdNr);
                //metadataTable.PrimaryKey = new DataColumn[] { colIdNr };
                metadataTable.Columns.Add(new DataColumn("IDName", typeof(string)));
                metadataDS.Tables.Add(metadataTable);
            }
            foreach (XmlElement key in fn.SelectSingleNode("metadata")!)
            {
                if (!metadataDS.Tables["Metadata"]!.Columns.Contains(key.Name))
                {
                    metaDataKeys.Add(key.Name, "");
                    metadataDS.Tables["Metadata"]!.Columns.Add(new DataColumn(key.Name, typeof(string)));
                    metadataDS.Tables["Metadata"]!.Columns.Add(new DataColumn($"{key.Name}-active", typeof(string)));
                    metadataDS.Tables["Metadata"]!.Columns.Add(new DataColumn($"{key.Name}-equation", typeof(string)));
                }
            }
            var row = metadataDS.Tables["Metadata"]!.NewRow();
            row["Cycle"] = -1;
            row["IDNr"] = fn.SelectSingleNode("IDNr")!.InnerText;
            row["IDName"] = fn.SelectSingleNode("IDName")!.InnerText;
            foreach (XmlElement key in fn.SelectSingleNode("metadata")!)
            {
                row[key.Name] = key.InnerText;
                row[$"{key.Name}-active"] = key.Attributes["active"]?.Value ?? "";
                row[$"{key.Name}-equation"] = key.Attributes["equation"]?.Value ?? "";
            }
            metadataDS.Tables["Metadata"]!.Rows.Add(row);
        }
        public void beginMultiSelect()
        {
            isSelecting = false;
            if (_selectedFn != "-1" && selectedFunction?.IDName == "")
            {
                defaultFnLabel();
            }
            if (selectedCoupling is not null)
            {
                selectedCoupling.aspectClass = "fn-point";
            }
            selectedFn = "-1";
            selectedLabel = "";
            touchAction = "auto";
        }
        public void addMetadataResults(Function fnBG)
        {
            DataRow row = metadataDS.Tables["Metadata"]!.NewRow();
            foreach (DataColumn col in metadataDS.Tables["Metadata"]!.Columns)
            { //this section adds a new row to the Metadata table for the results of the evaluated Function (for the current Cycle)
                switch (col.ColumnName)
                {
                    case "Cycle": row[col] = cycleFMI; break;
                    case "IDNr": row[col] = fnBG.IDNr; break;
                    case "IDName": row[col] = fnBG.IDName; break;
                    default:
                        {
                            var evalRow = metadataDS.Tables["Metadata"]!.Select($"IDNr = '{fnBG.IDNr}' and Cycle = -1")[0];
                            if (evalRow is not null)
                            {
                                row[col] = evalRow[col];
                            }
                            break;
                        }
                }
            }
            metadataDS.Tables["Metadata"]!.Rows.Add(row);
        }
    }
}