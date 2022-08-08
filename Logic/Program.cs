// See https://aka.ms/new-console-template for more information
using Main;
using Model;

Console.WriteLine("Hello, World!");

var game = new ModelBuilder().LoadGame("./TileConnections/originalTiles.txt", 2);

