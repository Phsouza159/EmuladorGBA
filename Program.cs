

using EmuladorGBA;

const string pathRom = @"C:\_git\EmuladorGBA\Roms\tetris.gb";
try
{
    Game game = new();
    game.LoadRomFromPath(pathRom);
    game.Start();
}
catch (Exception ex)
{
    Console.WriteLine(string.Empty);
    Console.WriteLine(ex.Message);
}
