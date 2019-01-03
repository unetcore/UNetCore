using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 分页列表拓展
/// </summary>
public static class PageLinqExtensions
{
    public static PagedList<T> ToPagedList<T>
        (
            this IQueryable<T> allItems,
            int pageIndex,
            int pageSize
        )
    {
        if (pageIndex < 1)
            pageIndex = 1;
        var itemIndex = (pageIndex - 1) * pageSize;
        var totalItemCount = allItems.Count();
        while (totalItemCount <= itemIndex && pageIndex > 1)
        {
            itemIndex = (--pageIndex - 1) * pageSize;
        }
        var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
        return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
    }
    public static PagedList<T> ToPagedList<T>(this IEnumerable<T> allItems, int pageIndex, int pageSize)
    {
        return allItems.AsQueryable().ToPagedList(pageIndex, pageSize);
    }

    public static PagedList<object> ToPagedListObject<T>(this PagedList<T> allItems, int pageIndex, int pageSize, int totalItemCount)
    {
        List<object> list = new List<object>();

        foreach (var item in allItems)
        {
            object obj = item as object;
            list.Add(obj);
        }

        var data = new PagedList<object>(list, pageIndex, pageSize, totalItemCount);
        return data;
    }
}