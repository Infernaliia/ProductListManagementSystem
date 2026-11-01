using System.Collections.Generic;

namespace ProductListManagementSystem
{
    public class GenericRepository<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(items);
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        public int Count()
        {
            return items.Count;
        }
    }
}
