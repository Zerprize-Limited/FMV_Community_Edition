using System.Globalization;

namespace FMV_Standard.Shared
{
    public class Coupling
    {
        private List<string>? _displayText;
        private string _label;
        private double _labelDx;
        private double _labelDy;
        public int displayTextCount;
        public Coupling(string Name, double labelDx, double labelDy, string directionX, string directionY, string notGroup, string outputFn, string label, string toFn, string toType,
            double drawTox, double drawToy, double drawFromx, double drawFromy)
        {
            this.Name = Name;
            _labelDx = labelDx;
            _labelDy = labelDy;
            this.directionX = directionX;
            this.directionY = directionY;
            this.notGroup = notGroup;
            this.outputFn = outputFn;
            this.toFn = toFn;
            _label = label;
            this.toType = toType;
            this.drawTox = drawTox;
            this.drawToy = drawToy;
            this.drawFromx = drawFromx;
            this.drawFromy = drawFromy;
        }
        public string Name { get; set; }
        public double labelDx
        {
            get { return _labelDx; }
            set
            {
                _labelDx = value;
                resetLabel();
            }
        }
        public double labelDy
        {
            get { return _labelDy; }
            set
            {
                _labelDy = value;
            }
        }
        public string directionX { get; set; }
        public string directionY { get; set; }
        public string notGroup { get; set; }
        public string outputFn { get; set; }
        public string toFn { get; set; }
        public string label
        {
            get { return _label; }
            set
            {
                _label = value;
                resetLabel();
            }
        }
        public bool dragAspect { get; set; } = false;
        public string aspectClass { get; set; } = "fn-point";
        public double labelX { get; set; }
        public double labelY { get; set; }
        public double startLabelX { get; set; } = 0;
        public double startLabelY { get; set; } = 0;
        public double Twidth { get; set; }
        public string toType { get; set; }
        public double drawTox { get; set; }
        public double drawToy { get; set; }
        public double drawFromx { get; set; }
        public double drawFromy { get; set; }
        public double drawAx { get; set; }
        public double drawAy { get; set; }
        public double drawBx { get; set; }
        public double drawBy { get; set; }
        public double drawIntx { get; set; }
        public double drawInty { get; set; }
        public bool wasActive { get; set; } = false;
        public bool activated { get; set; } = false;
        public string fmiHighlight { get; set; } = "";
        public List<string> ReturnTextLines(int length)
        {
            var textLines = new List<string>();
            string[] textWords = _label.Split(" ");
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
                        textLines.Add("");
                        tL++;
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
            _displayText = textLines;
            displayTextCount = _displayText.Count;
            return textLines;
        }
        public string curve
        {
            get
            {
                string[] _curve = new string[10]
                {
            drawTox.ToString("0.##", CultureInfo.InvariantCulture),
            drawToy.ToString("0.##", CultureInfo.InvariantCulture),
            drawFromx.ToString("0.##", CultureInfo.InvariantCulture),
            drawFromy.ToString("0.##", CultureInfo.InvariantCulture),
            drawAx.ToString("0.##", CultureInfo.InvariantCulture),
            drawAy.ToString("0.##", CultureInfo.InvariantCulture),
            drawBx.ToString("0.##", CultureInfo.InvariantCulture),
            drawBy.ToString("0.##", CultureInfo.InvariantCulture),
            drawIntx.ToString("0.##", CultureInfo.InvariantCulture),
            drawInty.ToString("0.##", CultureInfo.InvariantCulture)
                };
                return string.Join("|", _curve);
            }
        }
        public string curve2
        {
            get
            {
                string[] _curve2 = new string[13] {
            "M", drawFromx.ToString("0.##", CultureInfo.InvariantCulture), drawFromy.ToString("0.##", CultureInfo.InvariantCulture),
            "Q", drawAx.ToString("0.##", CultureInfo.InvariantCulture), drawAy.ToString("0.##", CultureInfo.InvariantCulture), drawIntx.ToString("0.##", CultureInfo.InvariantCulture), drawInty.ToString("0.##", CultureInfo.InvariantCulture),
            "Q", drawBx.ToString("0.##", CultureInfo.InvariantCulture), drawBy.ToString("0.##", CultureInfo.InvariantCulture), drawTox.ToString("0.##", CultureInfo.InvariantCulture), drawToy.ToString("0.##", CultureInfo.InvariantCulture)
            };
                return string.Join(" ", _curve2);
            }
        }
        public void resetLabel()
        {
            if (_displayText != null)
            {
                if (directionX == "from")
                {
                    Twidth = _displayText.Max(x => x.Length) * 4 + 4;
                    labelX = labelDx * labelDx * drawFromx + 2 * labelDx * (1 - labelDx) * drawAx + (1 - labelDx) * (1 - labelDx) * drawIntx;
                    labelY = labelDx * labelDx * drawFromy + 2 * labelDx * (1 - labelDx) * drawAy + (1 - labelDx) * (1 - labelDx) * drawInty;
                    //    - (2.5 + _displayText.Count * 4);
                }
                else
                {
                    Twidth = _displayText.Max(x => x.Length) * 4 + 4;
                    labelX = (1 + labelDx) * (1 + labelDx) * drawIntx + 2 * (1 + labelDx) * -labelDx * drawBx + -labelDx * -labelDx * drawTox;
                    labelY = (1 + labelDx) * (1 + labelDx) * drawInty + 2 * (1 + labelDx) * -labelDx * drawBy + -labelDx * -labelDx * drawToy;
                    //    -(2.5 + _displayText.Count * 4);
                }
            }
        }
        public string reDrawLines(double fnX, double fnY, bool isOutput, double interX = 0, double interY = 0)
        {
            //this function gets all the line coordinates
            //draws two quadratic Bezier curves to connect an Output with another Aspect
            //draws from {drawFrom} to {drawTo} through intermediate point {drawInt}
            //{drawA} and {drawB} are control points to create the curve. For a smooth curve {drawA}, {drawB} and {drawInt} must all be on the same line
            if (isOutput)
            {
                drawTox = fnX;
                drawToy = fnY;
                if (interX != 0 || interY != 0)
                {
                    drawFromx = interX;
                    drawFromy = interY;
                }
            }
            else
            {
                drawFromx = fnX;
                drawFromy = fnY;
            }
            double deltaX = drawTox - drawFromx;
            double deltaY = drawToy - drawFromy;
            if (toType == "I")
            { //does a fn1 Output connect with a fn2 Input
                if (deltaX < 0)
                {
                    drawAx = drawFromx + deltaX * 0.25;
                    drawAy = drawFromy;
                    drawBx = drawFromx + deltaX * 0.75;
                    drawBy = drawToy;
                }
                else
                {
                    if (Math.Abs(deltaY) > 120)
                    {
                        drawAx = drawFromx - 20;
                        drawAy = drawFromy + deltaY * 0.375;
                        drawBx = drawTox + 20;
                        drawBy = drawFromy + deltaY * 0.625;
                    }
                    else
                    {
                        drawAx = drawFromx - 20;
                        drawAy = drawFromy + deltaY * 0.5;
                        drawBx = drawTox + 20;
                        drawBy = drawFromy + deltaY * 0.5;
                    }
                }
                drawIntx = drawFromx + deltaX * 0.5;
                drawInty = drawFromy + deltaY * 0.5;
            }
            else if (toType == "T")
            { //does a fn1 output connect with a fn2 Time
                if (deltaX < 0)
                {
                    if (deltaY < 0)
                    {
                        drawAx = drawFromx + deltaX * 0.25;
                        drawAy = drawFromy + deltaY * 0.5;
                        drawBx = drawFromx + deltaX * 0.75;
                        drawBy = drawToy;
                        drawIntx = drawFromx + deltaX * 0.5;
                        drawInty = drawFromy + deltaY * 0.75;
                    }
                    else
                    {
                        drawAx = drawFromx + deltaX * 0.5;
                        drawAy = drawFromy;
                        drawBx = drawFromx + deltaX * 0.75;
                        drawBy = drawToy;
                        drawIntx = drawFromx + deltaX * 0.625;
                        drawInty = drawFromy + deltaY * 0.5;
                    }
                }
                else
                {
                    if (deltaY < 0)
                    {
                        if (Math.Abs(deltaY) > 120)
                        {
                            drawAx = drawFromx - 20;
                            drawAy = drawFromy + deltaY * 0.125;
                            drawBx = drawTox + 20;
                            drawBy = drawFromy + deltaY * 0.375;
                            drawIntx = drawFromx + deltaX * 0.5;
                            drawInty = drawFromy + deltaY * 0.25;
                        }
                        else
                        {
                            drawAx = drawFromx - 20;
                            drawAy = drawFromy + deltaY * 0.25;
                            drawBx = drawTox + 20;
                            drawBy = drawFromy + deltaY * 0.25;
                            drawIntx = drawFromx + deltaX * 0.5;
                            drawInty = drawFromy + deltaY * 0.25;
                        }
                    }
                    else
                    {
                        drawAx = drawFromx;
                        drawAy = drawFromy - 20;
                        drawBx = drawTox + 20;
                        drawBy = drawFromy - 20;
                        drawIntx = drawFromx + deltaX * 0.25;
                        drawInty = drawFromy - 20;
                    }
                }
            }
            else if (toType == "C")
            { //does a fn1 output connect with a fn2 Control
                if (deltaX < 0)
                {
                    if (deltaY < -20)
                    {
                        drawAx = drawFromx;
                        drawAy = drawFromy + deltaY * 0.5;
                        drawBx = drawFromx + deltaX * 0.5;
                        drawBy = drawToy;
                        drawIntx = drawFromx + deltaX * 0.25;
                        drawInty = drawFromy + deltaY * 0.75;
                    }
                    else
                    {
                        drawAx = drawFromx;
                        drawAy = drawFromy - 20;
                        drawBx = drawTox + 20;
                        drawBy = drawFromy - 20;
                        drawIntx = drawFromx + deltaX * 0.25;
                        drawInty = drawFromy - 20;
                    }
                }
                else
                {
                    drawAx = drawFromx + deltaX * 0.5 + 20;
                    drawAy = drawFromy;
                    drawBx = drawTox + 20;
                    drawBy = drawFromy + deltaY * 0.5;
                    drawIntx = drawFromx + deltaX * 0.75 + 20;
                    drawInty = drawFromy + deltaY * 0.25;
                }
            }
            else if (toType == "P")
            { //does a fn1 output connect with a fn2 Precondition
                if (deltaX < 0)
                {
                    if (deltaY < 0)
                    {
                        drawAx = drawFromx + deltaX * 0.5;
                        drawAy = drawFromy;
                        drawBx = drawFromx + deltaX * 0.75;
                        drawBy = drawToy;
                        drawIntx = drawFromx + deltaX * 0.625;
                        drawInty = drawFromy + deltaY * 0.5;
                    }
                    else
                    {
                        drawAx = drawFromx + deltaX * 0.25;
                        drawAy = drawFromy + deltaY * 0.5;
                        drawBx = drawFromx + deltaX * 0.75;
                        drawBy = drawToy;
                        drawIntx = drawFromx + deltaX * 0.5;
                        drawInty = drawFromy + deltaY * 0.75;
                    }
                }
                else
                {
                    if (deltaY < 0)
                    {
                        drawAx = drawFromx;
                        drawAy = drawFromy + 20;
                        drawBx = drawTox + 20;
                        drawBy = drawFromy + 20;
                        drawIntx = drawFromx + deltaX * 0.25;
                        drawInty = drawFromy + 20;
                    }
                    else
                    {
                        if (Math.Abs(deltaY) > 120)
                        {
                            drawAx = drawFromx - 20;
                            drawAy = drawFromy + deltaY * 0.125;
                            drawBx = drawTox + 20;
                            drawBy = drawFromy + deltaY * 0.375;
                            drawIntx = drawFromx + deltaX * 0.5;
                            drawInty = drawFromy + deltaY * 0.25;
                        }
                        else
                        {
                            drawAx = drawFromx - 20;
                            drawAy = drawFromy + deltaY * 0.25;
                            drawBx = drawTox + 20;
                            drawBy = drawFromy + deltaY * 0.25;
                            drawIntx = drawFromx + deltaX * 0.5;
                            drawInty = drawFromy + deltaY * 0.25;
                        }
                    }
                }
            }
            else if (toType == "R")
            { //does a fn1 output connect with a fn2 Resource
                if (deltaX < 0)
                {
                    if (deltaY < 20)
                    {
                        drawAx = drawFromx;
                        drawAy = drawFromy + 20;
                        drawBx = drawTox + 20;
                        drawBy = drawFromy + 20;
                        drawIntx = drawFromx + deltaX * 0.25;
                        drawInty = drawFromy + 20;
                    }
                    else
                    {
                        drawAx = drawFromx;
                        drawAy = drawFromy + deltaY * 0.5;
                        drawBx = drawFromx + deltaX * 0.5;
                        drawBy = drawToy;
                        drawIntx = drawFromx + deltaX * 0.25;
                        drawInty = drawFromy + deltaY * 0.75;
                    }
                }
                else
                {
                    drawAx = drawFromx + deltaX * 0.5 + 20;
                    drawAy = drawFromy;
                    drawBx = drawTox + 20;
                    drawBy = drawFromy + deltaY * 0.5;
                    drawIntx = drawFromx + deltaX * 0.75 + 20;
                    drawInty = drawFromy + deltaY * 0.25;
                }
            }

            string[] curve2 = new string[13] {
            "M", drawFromx.ToString("0.##", CultureInfo.InvariantCulture), drawFromy.ToString("0.##", CultureInfo.InvariantCulture),
            "Q", drawAx.ToString("0.##", CultureInfo.InvariantCulture), drawAy.ToString("0.##", CultureInfo.InvariantCulture), drawIntx.ToString("0.##", CultureInfo.InvariantCulture), drawInty.ToString("0.##", CultureInfo.InvariantCulture),
            "Q", drawBx.ToString("0.##", CultureInfo.InvariantCulture), drawBy.ToString("0.##", CultureInfo.InvariantCulture), drawTox.ToString("0.##", CultureInfo.InvariantCulture), drawToy.ToString("0.##", CultureInfo.InvariantCulture)
            };
            resetLabel();
            return string.Join(" ", curve2);
        }
    }
}