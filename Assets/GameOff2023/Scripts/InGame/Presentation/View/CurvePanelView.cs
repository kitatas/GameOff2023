using UniEx;
using UnityEngine;

namespace GameOff2023.InGame.Presentation.View
{
    public sealed class CurvePanelView : PanelView
    {
        [SerializeField] private CurveType curveType = default;

        private bool _isCurving = false;

        public override void ExecAction(PlayerView player)
        {
            if (_isCurving)
            {
                return;
            }

            if (currentPosition.GetSqrLength(player.currentPosition) < StageConfig.JUDGE_DISTANCE)
            {
                player.SetPosition(transform.position);
                player.direction = curveType.ToNextDirection(player.direction);

                _isCurving = true;
                this.Delay(1.0f, () => _isCurving = false);
            }
        }
    }
}