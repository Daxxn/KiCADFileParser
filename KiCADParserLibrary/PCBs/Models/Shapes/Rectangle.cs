using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models.Shapes;
public class Rectangle : IShape
{
    #region Local Props
    public Point Start { get; set; }
    public Point End { get; set; }
    public double Stroke { get; set; }
    public string? Fill { get; set; }
    public Layer Layer { get; set; } = Layer.None;
    #endregion

    #region Constructors
    public Rectangle() { }
    #endregion

    #region Methods

    #endregion

    #region Full Props

    #endregion
}
