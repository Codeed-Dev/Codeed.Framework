namespace Codeed.Framework.Domain.ChangeTracker
{
    public class CollectionChangeTracker<T>
    {
        private readonly ICollection<T> _addedItems = new List<T>();
        private readonly ICollection<T> _modifiedItems = new List<T>();
        private readonly ICollection<T> _removedItems = new List<T>();

        public IEnumerable<T> Added => _addedItems;

        public IEnumerable<T> Modified => _modifiedItems;

        public IEnumerable<T> Removed => _removedItems;

        public void Add(T item)
        {
            _addedItems.Add(item);
        }

        public void Modify(T item)
        {
            _modifiedItems.Add(item);
        }

        public void Remove(T item)
        {
            _removedItems.Add(item);
        }
    }
}
