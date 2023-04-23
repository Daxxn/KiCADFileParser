using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.Tree;

/// <summary>
/// Builds the S-Expression tree structure from the parsed file.
/// </summary>
public class TreeBuilder
{
   #region Local Props
   public TreeBuilderOptions Options { get; set; }
   public Node? RootNode { get; set; }

   #endregion

   #region Constructors
   public TreeBuilder() => Options = new();
   public TreeBuilder(TreeBuilderOptions options) => Options = options;
   #endregion

   #region Methods
   public Node ParseFile(string path)
   {
      string data = File.ReadAllText(path);

      data = CleanNewlines(data);

      RootNode = new();
      (RootNode.Type, int end) = GetFileType(data);
      Node currentNode = RootNode;
      data = data.Remove(0, end);
      bool openQuotes = false;
      foreach (var ch in data)
      {
         if (ch == Options.OpenDelimiter && !openQuotes)
         {
            Node newNode = new()
            {
               Parent = currentNode
            };
            currentNode.Children ??= new();
            currentNode.Children.Add(newNode);
            currentNode = newNode;
         }
         else if (ch == Options.CloseDelimiter && !openQuotes)
         {
            if (currentNode.Parent is null)
            {
               break;
            }
            ParseProps(currentNode);
            currentNode = currentNode.Parent;
         }
         else
         {
            if (!Options.ExclusionChars.Contains(ch))
            {
               if (ch == '"')
               {
                  openQuotes = !openQuotes;
               }
               if (currentNode.Data.Length != 0)
               {
                  if (currentNode.Data[^1] == ' ' && ch == ' ')
                  {
                     continue;
                  }
               }
               currentNode.Data += ch;
            }
         }
      }
      return RootNode;
   }

   private string CleanNewlines(string data)
   {
      return data.Replace("\r\n", "").Replace("\t", "");
   }

   private (string fileType, int index) GetFileType(string data)
   {
      var end = data.IndexOf(' ');
      return (data[1..end], end);
   }

   private void ParseProps(Node node)
   {
      if (node.Data.Contains('"'))
      {
         (var quoted, string unQuoted) = ParseQuotes(node.Data);
         var dataSplit = unQuoted.Split(' ', StringSplitOptions.RemoveEmptyEntries);
         if (dataSplit.Length == 0)
         {
            throw new Exception("Unknown node. No type can be found.");
         }
         node.Type = dataSplit[0];
         node.Props = new();
         node.Props.AddRange(dataSplit[1..]);
         node.Props.AddRange(quoted);
      }
      else
      {
         var dataSplit = node.Data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
         if (dataSplit.Length == 0)
         {
            throw new Exception("Unknown node. No type can be found.");
         }
         node.Type = dataSplit[0].Trim();
         if (dataSplit.Length > 1)
         {
            node.Props = new(dataSplit[1..]);
         }
      }
   }

   private (List<string> quoted, string unQuoted) ParseQuotes(string data)
   {
      string unQuoted = "";
      bool inQuotes = false;
      string temp = "";
      List<string> result = new();
      for (int i = 0; i < data.Length; i++)
      {
         if (data[i] == '"')
         {
            inQuotes = !inQuotes;
            continue;
         }
         if (inQuotes)
         {
            temp += data[i];
         }
         else
         {
            if (temp.Length > 0)
            {
               result.Add(temp);
               temp = "";
            }
            else
            {
               unQuoted += data[i];
            }
         }
      }
      if (temp.Length > 0)
      {
         result.Add(temp);
      }
      return (result, unQuoted);
   }

   // OLD
   //private void ParseProps(Node node)
   //{
   //   var dataSplit = node.Data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
   //   if (dataSplit.Length == 1)
   //   {
   //      node.Type = dataSplit[0].Trim();
   //      return;
   //   }
   //   else if (dataSplit.Length == 2)
   //   {
   //      node.Type = dataSplit[0].Trim();
   //      node.Value = dataSplit[1].Trim(Options.TrimChars);
   //      return;
   //   }
   //   else if (dataSplit.Length == 4 && !dataSplit[2].StartsWith('"'))
   //   {
   //      node.Type = dataSplit[0].Trim();
   //      node.Value = node.Data[dataSplit[0].Length..].Trim();
   //   }
   //   else
   //   {
   //      node.Type = dataSplit[0].Trim();
   //      if (node.Data.Contains('"'))
   //      {
   //         string propStr = node.Data[node.Data.IndexOf('"')..node.Data.LastIndexOf('"')];
   //         propStr = propStr.Trim('"');
   //         string[] props = propStr.Split("\" \"", StringSplitOptions.RemoveEmptyEntries);
   //         for (int i = 0; i < props.Length; i += 2)
   //         {
   //            if (i + 1 > props.Length)
   //               continue;
   //            node.Properties ??= new();
   //            node.Properties.Add(new()
   //            {
   //               Key = props[i].Trim(Options.TrimChars),
   //               Value = i + 1 > props.Length - 1 ? "" : props[i + 1].Trim(Options.TrimChars),
   //            });
   //         }
   //      }
   //   }
   //}
   #endregion

   #region Full Props

   #endregion
}
