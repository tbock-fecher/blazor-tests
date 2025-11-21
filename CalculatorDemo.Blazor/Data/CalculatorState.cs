using System;
using System.Collections.Generic;
using System.Globalization;

namespace CalculatorDemo.Blazor.Data
{
    public sealed class CalculatorState
    {
        private readonly List<string> _paper = new();
        private string _display = "0";
        private string _lastVal = string.Empty;
        private string _memVal = string.Empty;
        private string _paperArgs = string.Empty;
        private Operation _lastOper = Operation.None;
        private bool _eraseDisplay = true;

        public string Display => string.IsNullOrEmpty(_display) ? "0" : _display;

        public string MemoryLabel => string.IsNullOrEmpty(_memVal) ? "Memory: [empty]" : $"Memory: {_memVal}";

        public IReadOnlyList<string> Paper => _paper;

        public string? Error { get; private set; }

        public void ProcessKey(char c)
        {
            Error = null;
            if (_eraseDisplay)
            {
                _display = string.Empty;
                _eraseDisplay = false;
            }

            AddToDisplay(c);
        }

        public void ProcessOperation(OperationKey key)
        {
            Error = null;
            switch (key)
            {
                case OperationKey.Negate:
                    _lastOper = Operation.Negate;
                    LastValue = Display;
                    CalcResults();
                    LastValue = Display;
                    _eraseDisplay = true;
                    _lastOper = Operation.None;
                    break;
                case OperationKey.Divide:
                    if (_eraseDisplay)
                    {
                        _lastOper = Operation.Divide;
                        break;
                    }
                    CalcResults();
                    _lastOper = Operation.Divide;
                    LastValue = Display;
                    _eraseDisplay = true;
                    break;
                case OperationKey.Multiply:
                    if (_eraseDisplay)
                    {
                        _lastOper = Operation.Multiply;
                        break;
                    }
                    CalcResults();
                    _lastOper = Operation.Multiply;
                    LastValue = Display;
                    _eraseDisplay = true;
                    break;
                case OperationKey.Subtract:
                    if (_eraseDisplay)
                    {
                        _lastOper = Operation.Subtract;
                        break;
                    }
                    CalcResults();
                    _lastOper = Operation.Subtract;
                    LastValue = Display;
                    _eraseDisplay = true;
                    break;
                case OperationKey.Add:
                    if (_eraseDisplay)
                    {
                        _lastOper = Operation.Add;
                        break;
                    }
                    CalcResults();
                    _lastOper = Operation.Add;
                    LastValue = Display;
                    _eraseDisplay = true;
                    break;
                case OperationKey.Equal:
                    if (_eraseDisplay)
                    {
                        break;
                    }
                    CalcResults();
                    _eraseDisplay = true;
                    _lastOper = Operation.None;
                    LastValue = Display;
                    break;
                case OperationKey.Sqrt:
                    _lastOper = Operation.Sqrt;
                    LastValue = Display;
                    CalcResults();
                    LastValue = Display;
                    _eraseDisplay = true;
                    _lastOper = Operation.None;
                    break;
                case OperationKey.Percent:
                    if (_eraseDisplay)
                    {
                        _lastOper = Operation.Percent;
                        break;
                    }
                    CalcResults();
                    _lastOper = Operation.Percent;
                    LastValue = Display;
                    _eraseDisplay = true;
                    break;
                case OperationKey.OneOver:
                    _lastOper = Operation.OneX;
                    LastValue = Display;
                    CalcResults();
                    LastValue = Display;
                    _eraseDisplay = true;
                    _lastOper = Operation.None;
                    break;
                case OperationKey.ClearAll:
                    _lastOper = Operation.None;
                    _display = LastValue = string.Empty;
                    _paper.Clear();
                    UpdateDisplay();
                    break;
                case OperationKey.ClearEntry:
                    _lastOper = Operation.None;
                    _display = LastValue;
                    UpdateDisplay();
                    break;
                case OperationKey.MemoryClear:
                    Memory = 0.0;
                    break;
                case OperationKey.MemorySave:
                    Memory = Convert.ToDouble(Display);
                    _eraseDisplay = true;
                    break;
                case OperationKey.MemoryRecall:
                    _display = Memory.ToString(CultureInfo.InvariantCulture);
                    UpdateDisplay();
                    _eraseDisplay = false;
                    break;
                case OperationKey.MemoryPlus:
                    Memory += Convert.ToDouble(Display);
                    _eraseDisplay = true;
                    break;
            }
        }

        private void CalcResults()
        {
            if (_lastOper == Operation.None)
            {
                return;
            }

            var d = Calc(_lastOper);
            _display = d.ToString(CultureInfo.InvariantCulture);
            UpdateDisplay();
        }

        private double Calc(Operation lastOper)
        {
            var result = 0.0;
            try
            {
                switch (lastOper)
                {
                    case Operation.Divide:
                        AddArguments(LastValue + " / " + Display);
                        result = Convert.ToDouble(LastValue) / Convert.ToDouble(Display);
                        CheckResult(result);
                        AddResult(result.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.Add:
                        AddArguments(LastValue + " + " + Display);
                        result = Convert.ToDouble(LastValue) + Convert.ToDouble(Display);
                        CheckResult(result);
                        AddResult(result.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.Multiply:
                        AddArguments(LastValue + " * " + Display);
                        result = Convert.ToDouble(LastValue) * Convert.ToDouble(Display);
                        CheckResult(result);
                        AddResult(result.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.Percent:
                        AddArguments(LastValue + " % " + Display);
                        result = (Convert.ToDouble(LastValue) * Convert.ToDouble(Display)) / 100.0;
                        CheckResult(result);
                        AddResult(result.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.Subtract:
                        AddArguments(LastValue + " - " + Display);
                        result = Convert.ToDouble(LastValue) - Convert.ToDouble(Display);
                        CheckResult(result);
                        AddResult(result.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.Sqrt:
                        AddArguments("Sqrt( " + LastValue + " )");
                        result = Math.Sqrt(Convert.ToDouble(LastValue));
                        CheckResult(result);
                        AddResult(result.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.OneX:
                        AddArguments("1 / " + LastValue);
                        result = 1.0 / Convert.ToDouble(LastValue);
                        CheckResult(result);
                        AddResult(result.ToString(CultureInfo.InvariantCulture));
                        break;
                    case Operation.Negate:
                        result = Convert.ToDouble(LastValue) * -1.0;
                        break;
                }
            }
            catch
            {
                result = 0.0;
                AddResult("Error");
                Error = "Operation cannot be performed.";
            }

            return result;
        }

        private void AddArguments(string args)
        {
            _paperArgs = args;
        }

        private void AddResult(string result)
        {
            _paper.Add(_paperArgs + " = " + result);
        }

        private void CheckResult(double value)
        {
            if (double.IsNegativeInfinity(value) || double.IsPositiveInfinity(value) || double.IsNaN(value))
            {
                throw new InvalidOperationException("Illegal value");
            }
        }

        private void AddToDisplay(char c)
        {
            if (c == '.')
            {
                if (_display.IndexOf('.', StringComparison.Ordinal) >= 0)
                {
                    return;
                }
                _display += c;
            }
            else if (c >= '0' && c <= '9')
            {
                _display += c;
            }
            else if (c == '\b')
            {
                if (_display.Length <= 1)
                {
                    _display = string.Empty;
                }
                else
                {
                    _display = _display.Remove(_display.Length - 1, 1);
                }
            }

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (string.IsNullOrEmpty(_display))
            {
                _display = "0";
            }
        }

        private double Memory
        {
            get => string.IsNullOrEmpty(_memVal) ? 0.0 : Convert.ToDouble(_memVal);
            set => _memVal = value.ToString(CultureInfo.InvariantCulture);
        }

        private string LastValue
        {
            get => string.IsNullOrEmpty(_lastVal) ? "0" : _lastVal;
            set => _lastVal = value;
        }

        public enum OperationKey
        {
            Negate,
            Divide,
            Multiply,
            Subtract,
            Add,
            Equal,
            Sqrt,
            Percent,
            OneOver,
            ClearAll,
            ClearEntry,
            MemoryClear,
            MemorySave,
            MemoryRecall,
            MemoryPlus
        }

        private enum Operation
        {
            None,
            Divide,
            Multiply,
            Subtract,
            Add,
            Percent,
            Sqrt,
            OneX,
            Negate
        }
    }
}
