using UnityEngine;

namespace Base.Buildings
{
    public class Buildings : MonoBehaviour
    {
        public Vector2Int Size = Vector2Int.one;
        private Item _item;
        private SpriteRenderer _sprite;
    
        private SpriteRenderer _buildingSpriteRenderer;

        private void Awake()
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            if(_item)
                _sprite.sprite = _item.icon;
            
            _buildingSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void RefreshItem(Item item)
        {
            if (!item) return;
        
            _item = item;
            _sprite.sprite = _item.icon;

            if (item.BuildingPrefab)
            {
                Size = item.BuildingPrefab.GetComponent<Buildings>().Size;
            }
            else Size = Vector2Int.one;
        }

        public void SetAvailableColor(bool isAvailable)
        {
            _buildingSpriteRenderer.color = isAvailable ? Color.green : Color.red;
        }
    
        public void SetDefaultColor()
        {
            _buildingSpriteRenderer.color = Color.white;
        }
    }
}
