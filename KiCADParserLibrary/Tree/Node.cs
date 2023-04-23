using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KiCADParserLibrary.Tree;

/// <summary>
/// Tree node used for parsing.
/// </summary>
public class Node
{
   #region Local Props
   /// <summary>
   /// Parent Node of this child.
   /// </summary>
   public Node? Parent { get; set; }

   /// <summary>
   /// All children of this node.
   /// </summary>
   public List<Node>? Children { get; set; }

   /// <summary>
   /// DEPRECIATED
   /// </summary>
   public List<NodeProperty>? Properties { get; set; }

   /// <summary>
   /// String representing the type specifier of this node.
   /// </summary>
   public string Type { get; set; } = string.Empty;
   public string Key { get; set; } = string.Empty;
   public string Value { get; set; } = string.Empty;

   /// <summary>
   /// The raw string that contains all the <see cref="Props"/> data.
   /// </summary>
   public string Data { get; set; } = string.Empty;

   /// <summary>
   /// Local properties that are "in-line" with this current node.
   /// </summary>
   public List<string> Props { get; set; } = null!;
   public Regex Reg { get; set; } = new Regex("\"([^\"]*)\"", RegexOptions.Multiline);
   #endregion

   #region Methods
   /// <inheritdoc/>
   public override string ToString()
   {
      return $"Node {Data}";
   }

   /// <summary>
   /// Searches for the node with the 
   /// </summary>
   /// <param name="searchValue"></param>
   /// <returns></returns>
   public Node? GetNode(string searchValue)
   {
      if (Type.Trim() == searchValue)
      {
         return this;
      }
      if (Children is null)
         return null;
      foreach (var node in Children)
      {
         if (node.GetNode(searchValue) is Node foundNode)
         {
            return foundNode;
         }
      }
      return null;
   }

   public List<Node> GetNodes(string nodeLink)
   {
      List<Node> foundNode = new();
      Node? parent = this;
      string id = nodeLink;
      if (nodeLink.Contains('>'))
      {
         var spl = nodeLink.Split('>');
         id = spl[^1];
         parent = SearchRecursive(spl[..^1]);
      }
      if (parent is null || parent?.Children?.Count == 0)
      {
         return new();
      }
      foreach (var item in parent.Children!)
      {
         if (item.Type == id || item.Type.StartsWith(id))
         {
            foundNode.Add(item);
         }
      }
      return foundNode;
   }

   /// <summary>
   /// DEPRECIATED
   /// </summary>
   public NodeProperty? GetProperty(string key) => Properties?.FirstOrDefault(p => p.Key == key);

   /// <summary>
   /// Searches children only for the matching node sequence.
   /// </summary>
   /// <param name="prop">The type ID of the child node.</param>
   /// <returns>The found <see cref="Node"/> otherwise <see langword="null"/>.</returns>
   public Node? SearchChildren(string prop)
   {
      if (Children is null)
         return null;
      foreach (var child in Children)
      {
         if (child.Value == prop)
         {
            return child;
         }
      }
      return null;
   }

   /// <summary>
   /// Searches all local nodes recursively for the matching node sequence.
   /// <para/>
   /// Delimited with ">" characters.
   /// </summary>
   /// <param name="nodeLink">The '>' delimited list of type IDs.</param>
   /// <returns></returns>
   public Node? Search(string nodeLink)
   {
      if (Type == nodeLink)
      {
         return this;
      }
      if (nodeLink.Contains('>'))
      {
         var spl = nodeLink.Split('>');
         return SearchRecursive(spl);
      }
      else
      {
         return SearchRecursive(new[] { nodeLink });
      }
   }

   /// <summary>
   /// DEPRECIATED
   /// </summary>
   private Node? SearchRecursive(string[] linkList)
   {
      if (linkList.Length == 0)
         return this;
      if (Children != null)
      {
         foreach (var ch in Children)
         {
            if (ch.Type == linkList[0] || ch.Type.StartsWith(linkList[0]))
            {
               return ch.SearchRecursive(linkList[1..]);
            }
         }
      }
      return null;
   }

   /// <summary>
   /// Not sure ATM.
   /// <para/>
   /// Probably going to be replaced entirely...
   /// </summary>
   /// <returns></returns>
   public string Write()
   {
      StringBuilder builder = new StringBuilder();
      builder.Append('(');
      if (string.IsNullOrEmpty(Value) && Properties is null && Children is null)
      {
         builder.Append(Data);
      }
      else
      {
         if (Type == "pin")
         {
            builder.Append(Data);
         }
         else
         {
            builder.Append(Type);
            builder.Append(' ');
            if (!string.IsNullOrEmpty(Value) && Properties is null && Children is null)
            {
               builder.Append($" {Value}");
            }
            else if (!string.IsNullOrEmpty(Value) && Children != null)
            {
               if (Type == "pin")
               {
                  builder.Append(Data.Replace("pin ", ""));
                  builder.Append(' ');
               }
               else if (Value == "hide")
               {
                  builder.Append(Value);
                  builder.Append(' ');
               }
               else
               {
                  builder.Append($" \"{Value}\" ");
               }
            }
         }
         if (Properties != null)
         {
            builder.Append(' ');
            foreach (var prop in Properties)
            {
               builder.Append($"\"{prop.Key}\" \"{prop.Value}\" ");
            }
         }
         if (Children != null)
         {
            foreach (var node in Children)
            {
               builder.Append(node.Write());
            }
            builder.Append(' ');
         }
      }
      builder.Append(')');
      return builder.ToString();
   }

   /// <summary>
   /// Creates a simple copy of this node.
   /// </summary>
   /// <returns>The copy of this node.</returns>
   public Node ShallowCopy()
   {
      return new Node
      {
         Children = Children,
         Type = Type,
         Data = Data,
         Key = Key,
         Value = Value,
         Parent = Parent,
         Properties = Properties,
         Props = Props
      };
   }

   /// <summary>
   /// Updates the property data.
   /// </summary>
   public void UpdateData()
   {
      if (Props is null) return;
      StringBuilder builder = new();
      //builder.Append(Type);
      foreach (var prop in Props)
      {
         builder.Append(" \"");
         builder.Append(prop);
         builder.Append('"');
      }
      Data = builder.ToString();
   }

   //public void UpdateData()
   //{
   //   if (Props is null) return;
   //   StringBuilder builder = new();
   //   if (Data.Contains('"'))
   //   {
   //      var results = Reg.Matches(Data);
   //      for (int i = 0; i < results.Count; i++)
   //      {
   //         results.repl
   //      }
   //   }
   //   else
   //   {
   //      foreach (var prop in Props)
   //      {
   //         builder.Append(' ');
   //         builder.Append(prop);
   //      }
   //   }
   //}
   #endregion

   #region Full Props

   #endregion
}
