using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models.Shapes;
public struct Circle : IShape
{
   #region Local Props
   public Point Start { get; set; } = new();
   public Point End { get; set; } = new();
   public double Stroke { get; set; } = 0;
   public Layer Layer { get; set; } = Layer.None;
   #endregion

   #region Constructors
   public Circle() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props
   public double Radius => Start.X - End.X;
   public double Diameter => Radius * 2;
   #endregion
}
