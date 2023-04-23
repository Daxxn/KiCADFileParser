using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models;
public struct Size
{
   #region Local Props
   public double Width { get; set; } = -1;
   public double Height { get; set; } = -1;
   #endregion

   #region Constructors
   public Size() { }
   public Size(double width, double height)
   {
      Width = width;
      Height = height;
   }
   #endregion

   #region Methods
   public static Size Parse(IEnumerable<string> data)
   {
      Size newPoint = new();
      if (data.Count() == 2)
      {
         var d = data.ToArray();
         if (double.TryParse(d[0], out double width))
         {
            newPoint.Width = width;
         }
         if (double.TryParse(d[1], out double height))
         {
            newPoint.Width = height;
         }
      }
      return newPoint;
   }
   #endregion

   #region Full Props

   #endregion
}
