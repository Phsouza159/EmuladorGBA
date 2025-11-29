

using EmuladorGBA;
using EmuladorGBA.Business;
using EmuladorGBA.Business.Enum;

const string pathRom = @"C:\\Users\\paulo\\Downloads\\Pokemon - Silver Version (USA, Europe) (SGB Enhanced) (GB Compatible).gbc";

Game game = new Game();
game.Card = new Card();
var card = game.Card;

card.SetPathRom(pathRom);
card.SetTypeRom(TypeRom.PATH);
card.LoadRom();

game.LoadHead();
game.ShowHeadValues();


game.Start();