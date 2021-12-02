using System;
using System.Collections;
using System.Collections.Generic;
namespace CommandSharp.Commands
{
    public class SyntaxErrorParameters
    {
        public string CommandNamePassed { get; internal set; }
        internal string GetLegend() => $"Legend: '[]: Optional Argument', '<>: Required Argument'";
    }
}