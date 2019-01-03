using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 分页列表接口
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagedList<T> : List<T>, IPagedList<T>
{
    /// <summary>
    /// 使用要分页的所有数据项、当前页索引和每页显示的记录数初始化PagedList对象
    /// </summary>
    /// <param name="allItems">要分页的所有数据项</param>
    /// <param name="pageIndex">当前页索引</param>
    /// <param name="pageSize">每页显示的记录数</param>
    public PagedList(IEnumerable<T> allItems, int pageIndex, int pageSize)
    {
        PageSize = pageSize;
        var items = allItems as IList<T> ?? allItems.ToList();
        TotalItemCount = items.Count();
        CurrentPageIndex = pageIndex;
        AddRange(items.Skip(StartRecordIndex - 1).Take(pageSize));
    }
    /// <summary>
    /// 使用当前页数据项、当前页索引、每页显示记录数和要分页的总记录数初始化PagedList对象
    /// </summary>
    /// <param name="currentPageItems">总前页数据项</param>
    /// <param name="pageIndex">当前页索引</param>
    /// <param name="pageSize">每页显示记录数</param>
    /// <param name="totalItemCount">要分页的总记录数</param>
    public PagedList(IEnumerable<T> currentPageItems, int pageIndex, int pageSize, int totalItemCount)
    {
        AddRange(currentPageItems);
        TotalItemCount = totalItemCount;
        CurrentPageIndex = pageIndex;
        PageSize = pageSize;
    }
    /// <summary>
    /// 使用要分页的所有数据项、当前页索引和每页显示的记录数初始化PagedList对象
    /// </summary>
    /// <param name="allItems">要分页的所有数据项</param>
    /// <param name="pageIndex">当前页索引</param>
    /// <param name="pageSize">每页显示的记录数</param>
    public PagedList(IQueryable<T> allItems, int pageIndex, int pageSize)
    {
        int startIndex = (pageIndex - 1) * pageSize;
        AddRange(allItems.Skip(startIndex).Take(pageSize));
        TotalItemCount = allItems.Count();
        CurrentPageIndex = pageIndex;
        PageSize = pageSize;
    }
    /// <summary>
    /// 使用当前页数据项、当前页索引、每页显示记录数和要分页的总记录数初始化PagedList对象
    /// </summary>
    /// <param name="currentPageItems">总前页数据项</param>
    /// <param name="pageIndex">当前页索引</param>
    /// <param name="pageSize">每页显示记录数</param>
    /// <param name="totalItemCount">要分页的总记录数</param>
    public PagedList(IQueryable<T> currentPageItems, int pageIndex, int pageSize, int totalItemCount)
    {
        AddRange(currentPageItems);
        TotalItemCount = totalItemCount;
        CurrentPageIndex = pageIndex;
        PageSize = pageSize;
    }

    /// <summary>
    /// 当前分页索引
    /// </summary>
    public int CurrentPageIndex { get; set; }
    /// <summary>
    /// 每页行数
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// 总行数
    /// </summary>
    public int TotalItemCount { get; set; }
    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPageCount { get { return (int)Math.Ceiling(TotalItemCount / (double)PageSize); } }
    /// <summary>
    /// 开始索引
    /// </summary>
    public int StartRecordIndex { get { return (CurrentPageIndex - 1) * PageSize + 1; } }
    /// <summary>
    /// 结束索引
    /// </summary>
    public int EndRecordIndex { get { return TotalItemCount > CurrentPageIndex * PageSize ? CurrentPageIndex * PageSize : TotalItemCount; } }
}
