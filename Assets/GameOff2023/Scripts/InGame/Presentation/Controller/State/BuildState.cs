using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2023.InGame.Domain.UseCase;
using GameOff2023.InGame.Presentation.View;

namespace GameOff2023.InGame.Presentation.Controller
{
    public sealed class BuildState : BaseState
    {
        private readonly StageUseCase _stageUseCase;
        private readonly GoalView _goalView;
        private readonly PlayerView _playerView;
        private readonly StageView _stageView;

        public BuildState(StageUseCase stageUseCase, GoalView goalView, PlayerView playerView, StageView stageView)
        {
            _stageUseCase = stageUseCase;
            _goalView = goalView;
            _playerView = playerView;
            _stageView = stageView;
        }

        public override GameState state => GameState.Build;

        public override async UniTask InitAsync(CancellationToken token)
        {
            _stageView.Init();
            await UniTask.Yield(token);
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _stageView.BuildBaseAsync(token);

            var stageData = _stageUseCase.GetStageData();
            _stageView.BuildField(stageData.cells, _playerView, _goalView);
            _stageView.BuildPanel(stageData.panels);

            return GameState.SetUp;
        }
    }
}