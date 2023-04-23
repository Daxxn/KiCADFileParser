using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.PCBs.Models;
public class Footprint
{
   #region Local Props
   public string LibraryRef { get; set; } = string.Empty;
   public string Reference { get; set; } = string.Empty;
   public string Value { get; set; } = string.Empty;
   public string? Description { get; set; }
   public string? PartNumber { get; set; }
   public List<Pad> Pads { get; set; } = new();
   #endregion

   #region Constructors
   public Footprint() { }
   #endregion

   #region Methods

   #endregion

   #region Full Props

   #endregion
}
