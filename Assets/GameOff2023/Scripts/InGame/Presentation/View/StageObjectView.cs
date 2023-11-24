using MagicTween;
using UniEx;
using UnityEngine;

namespace GameOff2023.InGame.Presentation.View
{
    public abstract class StageObjectView : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer spriteRenderer = default;

        public Vector3 currentPosition => transform.position;

        public int currentXToInt => Mathf.RoundToInt(currentPosition.x);
        public int currentYToInt => Mathf.RoundToInt(currentPosition.y);

        private void Awake()
        {
            spriteRenderer.SetColorA(0.0f);
            transform.localScale = Vector3.one * StageObjectConfig.HIDE_SCALE_RATE;
        }

        public void SetPosition(Vector3 position, float duration = 0.0f)
        {
            if (duration.IsZero())
            {
                transform.position = position;
            }
            else
            {
                transform
                    .TweenPosition(position, duration)
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject);
            }
        }

        public virtual Tween Show(float duration)
        {
            return Sequence.Create()
                .Append(spriteRenderer
                    .TweenColorAlpha(1.0f, duration))
                .Join(transform
                    .TweenLocalScale(Vector3.one * StageObjectConfig.SHOW_SCALE_RATE, duration))
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public virtual Tween Hide(float duration)
        {
            return Sequence.Create()
                .Append(spriteRenderer
                    .TweenColorAlpha(0.0f, duration))
                .Join(transform
                    .TweenLocalScale(Vector3.one * StageObjectConfig.HIDE_SCALE_RATE, duration))
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }
    }
}