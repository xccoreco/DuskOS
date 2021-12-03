/*
* PROJECT:         Dusk Operating System Development
* CONTENT          CommandSharp/Commands/SyntaxErrorParameters.cs
* PROGRAMMERS:     
*                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
*
*/

namespace CommandSharp.Commands
{
    public class SyntaxErrorParameters
    {
        public string CommandNamePassed { get; internal set; }
        internal string GetLegend() => $"Legend: '[]: Optional Argument', '<>: Required Argument'";
    }
}