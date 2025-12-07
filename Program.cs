

using EmuladorGBA;


string pathRom = string.Empty;

if (args.Length > 0)
{
    pathRom = args[0].ToString();
}
else
{
    pathRom = @"C:\_git\EmuladorGBA\Roms\cpu_instrs.gb";
}

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
