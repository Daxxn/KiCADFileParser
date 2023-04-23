using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models.Shapes;
public interface IShape
{
   Point Start { get; set; }
   Point End { get; set; }
   double Stroke { get; set; }
   Layer Layer { get; set; }
}
