using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var NList = new[]  {
                new Foo(){ Id = 1, ParentId = 0, Title = "root" },
                new Foo(){ Id = 2, ParentId = 1, Title = "child 1" },
                new Foo(){ Id = 3, ParentId = 1, Title = "child 2" },
                new Foo(){ Id = 4, ParentId = 1, Title = "child 3" },
                new Foo(){ Id = 5, ParentId = 4, Title = "grandchild 1" },
            };

            var query = NList.AsHierarchy(z => z.Id, z => z.ParentId);
        }
    }

    public class Foo
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
    }

    public class Node<T>
    {
        public T Entity { get; set; }
        public IEnumerable<Node<T>> ChildNodes { get; set; }
        public int Depth { get; set; }
    }

    public static class LinqExtensions
    {
        private static IEnumerable<Node<TEntity>> CreateHierarchy<TEntity, TProperty>(IEnumerable<TEntity> allItems, TEntity parentItem, Func<TEntity, TProperty> idProperty, Func<TEntity, TProperty> parentIdProperty, int depth) where TEntity : class
        {
            IEnumerable<TEntity> childs;

            if (parentItem == null)
                childs = allItems.Where(i => parentIdProperty(i).Equals(default(TProperty)));
            else
                childs = allItems.Where(i => parentIdProperty(i).Equals(idProperty(parentItem)));

            if (childs.Count() > 0)
            {
                depth++;

                foreach (var item in childs)
                    yield return new Node<TEntity>()
                    {
                        Entity = item,
                        ChildNodes = CreateHierarchy<TEntity, TProperty>(allItems, item, idProperty, parentIdProperty, depth),
                        Depth = depth
                    };
            }
        }

        public static IEnumerable<Node<TEntity>> AsHierarchy<TEntity, TProperty>(this IEnumerable<TEntity> allItems, Func<TEntity, TProperty> idProperty, Func<TEntity, TProperty> parentIdProperty) where TEntity : class
        {
            return CreateHierarchy(allItems, default(TEntity), idProperty, parentIdProperty, 0);
        }
    }
}