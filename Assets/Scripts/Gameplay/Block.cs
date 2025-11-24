using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay
{
    public class Block : MonoBehaviour, IPointerClickHandler
    {

        public delegate void BlockClicked(Block block);
        public event BlockClicked OnBlockClicked;
        public Vector2Int GridPosition { get; private set; }
        [SerializeField] private SpriteRenderer spriteRenderer;
        public int BlockType { get; private set; }

        
        public void Initialize(Vector2Int position,int blockType,  Sprite sprite = null )
        {
            GridPosition = position;
            spriteRenderer.sprite = sprite;
            BlockType = blockType;
            transform.position = (Vector2)GridPosition;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            OnBlockClicked?.Invoke(this);
        }

        public void SetGridPosition(Vector2Int position)
        {
            GridPosition = position;
        }
    }
}