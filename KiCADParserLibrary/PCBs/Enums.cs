using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Enums;
public enum LayerType
{
   Signal,
   User
}

public enum PadTech
{
   SMD,
   PTH,
   MCH
}

public enum PadType
{
   Rect,
   RoundRect,
   Circle,
   Poly
}

public enum GraphicsType
{
   Default,
   Dot,
   Dash,
   DotDash,
   Solid
}