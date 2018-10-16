// States indicate which "iteration" of the game loop the game is in.
// Typically, game goes StartGame -> CardSelect -> Gameplay -> CardSelect -> Gameplay -> ... -> Gameplay -> EndGame
namespace GameStates {
    public enum GameState {
        StartGame,
        CardSelect,
        Gameplay,
        EndGame
    };
}
