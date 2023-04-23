using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KiCADParserLibrary.Tree;

using MVVMLibrary;

namespace KiCADParserLibrary.Symbols.VModels;

/// <summary>
/// Representation of a KiCAD schematic symbol library
/// </summary>
public class Library : Model
{
	private string _name = null!;
   private string _generator = null!;
   private ObservableCollection<Symbol> _symbols = new();
	private Node _root = null!;
	private Node _generatorNode = null;

	public override string ToString() => $"Symbol Library {Name} - Gen: {Generator} Symbol Count: {Symbols.Count}";

	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged();
		}
	}

	public string Generator
	{
		get => _generator;
		set
		{
			_generator = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<Symbol> Symbols
	{
		get => _symbols;
		set
		{
			_symbols = value;
			OnPropertyChanged();
		}
	}

	public Node RootNode
	{
		get => _root;
		set
		{
			_root = value;
			OnPropertyChanged();
		}
	}

	public Node GeneratorNode
	{
		get => _generatorNode;
		set
		{
			_generatorNode = value;
			OnPropertyChanged();
		}
	}
}
