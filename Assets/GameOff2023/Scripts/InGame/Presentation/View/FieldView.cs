using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MagicTween;
using UnityEngine;

namespace GameOff2023.InGame.Presentation.View
{
    public sealed class FieldView : MonoBehaviour
    {
        [SerializeField] private CellView cellView = default;
        [SerializeField] private WallView wallView = default;
        [SerializeField] private GoalView goalView = default;
        [SerializeField] private PlayerView playerView = default;

        private List<CellView> _fields;
        private List<WallView> _walls;

        public List<CellView> notFixedCells =>
            _fields
                .Where(x => x.cellType != CellType.Fixed)
                .ToList();

        public void Init()
        {
            _fields = new List<CellView>();
            _walls = new List<WallView>();
        }

        public async UniTask BuildAsync(float duration, CancellationToken token)
        {
            for (int x = 1; x <= StageConfig.X; x++)
            {
                for (int y = 1; y <= StageConfig.Y; y++)
                {
                    var cell = Instantiate(cellView, transform);
                    cell.SetPosition(new Vector3(x, y, 0.0f));
                    cell.Show(duration);
                    _fields.Add(cell);
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token);
        }

        public void BuildField(Data.Entity.CellEntity cellEntity)
        {
            var cell = _fields.Find(x => x.currentPosition == cellEntity.position);
            if (cell == null)
            {
                throw new Exception();
            }

            cell.SetType(CellType.Fixed);

            StageObjectView stageObjectView;
            switch (cellEntity.type)
            {
                case ObjectType.Player:
                    playerView.SetStartPosition(cellEntity.position);
                    stageObjectView = playerView;
                    break;
                case ObjectType.Goal:
                    stageObjectView = goalView;
                    break;
                case ObjectType.Wall:
                    var wall = Instantiate(wallView, transform);
                    _walls.Add(wall);
                    stageObjectView = wall;
                    break;
                default:
                    throw new Exception();
            }

            stageObjectView.SetPosition(cellEntity.position);
            stageObjectView.Show(StageObjectConfig.SHOW_TIME);
        }

        public void Hide(float duration)
        {
            foreach (var field in _fields)
            {
                field.Hide(duration)
                    .OnComplete(() => Destroy(field.gameObject));
            }

            foreach (var wall in _walls)
            {
                wall.Hide(duration)
                    .OnComplete(() => Destroy(wall.gameObject));
            }

            goalView.Hide(duration);
            playerView.Hide(duration);
        }
    }
}