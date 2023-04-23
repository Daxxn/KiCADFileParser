using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.Tree;

/// <summary>
/// Options to use while parsing.
/// </summary>
public class TreeBuilderOptions
{
    public char OpenDelimiter { get; set; } = '(';
    public char CloseDelimiter { get; set; } = ')';
    public char PropertyDelimiter { get; set; } = '"';
    public char[] ExclusionChars { get; set; } = new[] { '\n', '\r', '\t' };
    public char[] TrimChars { get; set; } = new[] { ' ', '"' };
}
