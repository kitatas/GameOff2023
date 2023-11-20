using UniEx;
using UnityEngine;

namespace GameOff2023.InGame.Presentation.View
{
    public sealed class CurvePanelView : PanelView
    {
        [SerializeField] private Direction direction1 = default;
        [SerializeField] private Direction direction2 = default;

        private bool _isCurving = false;

        public override void ExecAction(PlayerView player)
        {
            if (_isCurving)
            {
                return;
            }

            if (currentPosition.GetSqrLength(player.currentPosition) < StageConfig.JUDGE_DISTANCE)
            {
                if (player.direction.IsEnter(direction1))
                {
                    player.direction = direction2;
                    player.SetPosition(transform.position);
                }
                else if (player.direction.IsEnter(direction2))
                {
                    player.direction = direction1;
                    player.SetPosition(transform.position);
                }
                else
                {
                    player.SetDead();
                    return;
                }

                _isCurving = true;
                this.Delay(1.0f, () => _isCurving = false);
            }
        }
    }
}