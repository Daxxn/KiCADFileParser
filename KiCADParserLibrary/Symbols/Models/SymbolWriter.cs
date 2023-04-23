using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KiCADParserLibrary.Symbols.Models;

public class SymbolWriter
{
   #region Local Props
   public Regex ParensCleanup { get; } = new("(?<=\\))\\s(?=\\))");
   #endregion

   #region Constructors
   public SymbolWriter() { }
   #endregion

   #region Methods
   public void Write(string path, Library library)
   {
      library.SyncTree();

      // Figure out how to write all the data
      // back to the file without bricking it.

      string result = library.RawTree.Write();
      result = result.Replace("  ", " ");

      result = ParensCleanup.Replace(result, "");

      FileStreamOptions options = new()
      {
         Access = FileAccess.Write,
         Mode = FileMode.Create,
      };
      using StreamWriter writer = new(path, options);
      writer.Write(result);
   }
   #endregion

   #region Full Props

   #endregion
}
