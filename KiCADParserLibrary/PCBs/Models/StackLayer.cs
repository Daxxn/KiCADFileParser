using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KiCADParserLibrary.PCBs.Enums;

namespace KiCADParserLibrary.PCBs.Models;
public class StackLayer
{
   #region Local Props
   public string Name { get; set; } = string.Empty;
   public string Material { get; set; } = string.Empty;
   public double Thickness { get; set; }
   public double DielectricConstant { get; set; }
   public double LossTangent { get; set; }
   public string Type { get; set; } = string.Empty;
   #endregion

   #region Constructors
   public StackLayer() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
