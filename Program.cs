

using EmuladorGBA;
using EmuladorGBA.Business;
using EmuladorGBA.Business.Enum;

const string pathRom = @"C:\Users\paulo\Source\Repos\Phsouza159\EmuladorGBA\Roms\cpu_instrs.gb";

Game game = new Game();
game.Card = new Card();
var card = game.Card;

card.SetPathRom(pathRom);
card.SetTypeRom(TypeRom.PATH);
card.LoadRom();

game.LoadHead();
game.ShowHeadValues();


game.Start();