using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KiCADParserLibrary.Tree;

namespace KiCADParserLibrary.Symbols.VModels;

/// <summary>
/// Writes the <see cref="Symbol"/> <see cref="Library"/> back to the file.
/// </summary>
public class SymbolWriter
{
   #region Local Props
   private static readonly string[] SymbolKeywords = new string[]
   {
      "yes",
      "left",
      "hide",
      "input",
      "line",
      "bidirectional",
      "output",
      "default",
      "none",
      "right",
      "open_collector",
      "open_emitter",
      "no_connect",
      "tri_state",
      "passive",
      "free",
      "unspecified",
      "power_in",
      "power_out",
      "non_logic",
      "edge_clock_high",
      "output_low",
      "clock_low",
      "input_low",
      "inverted_clock",
      "clock",
      "inverted",
      "dash_dot_dot",
      "dash_dot",
      "dot",
      "dash",
      "solid",
      "background",
      "color",
      "outline",
      "top",
      "bottom",
      "private",
      "bold",
      "italic",
      "kicad_symbol_editor",
      "alternate"
   };

   private static string[] StringTypes = new string[]
   {
      "property",
      "symbol",
      "name",
      "number",
      "alternate",
      "symbol",
      "text",
      "text_box"
   };
   #endregion

   #region Constructors
   /// <inheritdoc/>
   public SymbolWriter() { }
   #endregion

   #region Methods
   public void Write(Library library, string path)
   {
      StringBuilder sb = new();
      library.Generator = "kicad_symbol_editor_PartsHolde_Edit";
      //library.RootNode.Search("generator");
      WriteNode(sb, library.RootNode);

      var result = sb.ToString();

      using (StreamWriter writer = new(path))
      {
         writer.Write(result);
      }
   }

   private void WriteNode(StringBuilder builder, Node node)
   {
      UpdateData(node);
      builder.Append('(');
      bool appendHide = false;
      if (!node.Data.StartsWith(node.Type))
      {
         builder.Append(node.Type);
      }
      if (node.Data.EndsWith("hide") && node.Children != null)
      {
         appendHide = true;
      }
      else
      {
         builder.Append(node.Data);
         builder.Append(' ');
      }
      if (node != null)
      {
         if (node.Children != null)
         {
            foreach (var n in node.Children)
            {
               WriteNode(builder, n);
            }
         }
      }
      if (appendHide)
      {
         builder.Append(" hide");
      }
      builder.Append(')');
   }

   private void WriteChildren(StringBuilder builder, Node node)
   {
      node.UpdateData();
      builder.Append('(');
      builder.Append(node.Type);
      builder.Append(node.Data);
      if (node != null)
      {
         foreach (var n in node.Children)
         {
            WriteNode(builder, n);
         }
      }
      builder.Append(')');
   }

   private void UpdateData(Node node)
   {
      if (node.Props is null) return;
      StringBuilder builder = new();
      foreach (var prop in node.Props)
      {
         var p = prop.Trim();
         if (SymbolKeywords.Contains(p))
         {
            builder.Append(' ');
            builder.Append(p);
         }
         else
         {
            if (p.Any(n => char.IsLetter(n)))
            {
               builder.Append(" \"");
               builder.Append(p);
               builder.Append('"');
            }
            else
            {
               if (StringTypes.Contains(node.Type))
               {
                  builder.Append(" \"");
                  builder.Append(p);
                  builder.Append('"');
               }
               else
               {
                  builder.Append(' ');
                  builder.Append(p);
               }
            }
         }
      }
      if (node.Props.Count <= 1 && node.Data.Contains("\"\""))
      {
         builder.Append(" \"\" ");
      }
      node.Data = builder.ToString();
   }
   #endregion

   #region Full Props

   #endregion
}
