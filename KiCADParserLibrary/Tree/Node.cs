using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KiCADParserLibrary.Tree;

/// <summary>
/// Tree node used for parsing.
/// </summary>
public class Node
{
   #region Local Props
   public Node? Parent { get; set; }
   public List<Node>? Children { get; set; }
   public List<NodeProperty>? Properties { get; set; }
   public string Type { get; set; } = string.Empty;
   public string Key { get; set; } = string.Empty;
   public string Value { get; set; } = string.Empty;
   public string Data { get; set; } = string.Empty;
   public List<string>? Props { get; set; }
   #endregion

   #region Methods
   public override string ToString()
   {
      return $"Node {Data}";
   }

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
      if (nodeLink.Contains(">"))
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

   public NodeProperty? GetProperty(string key) => Properties?.FirstOrDefault(p => p.Key == key);

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
   /// Delimited by ">"
   /// </summary>
   /// <param name="nodeLink"></param>
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
   #endregion

   #region Full Props

   #endregion
}
