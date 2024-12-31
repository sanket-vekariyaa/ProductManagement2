using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Model;
using ProductManagement.Provider;
using ProductManagement.Providers;
using System.Reflection;

namespace ProductManagement.Buisness
{
    public class GlobalVariables : IDisposable
    {
        //public User CurrentUser { get; set; }
        public void Dispose() { GC.SuppressFinalize(this); }

        //internal T UpdateTransection<T>(T data)
        //{
        //    if (CurrentUser != null && CurrentUser.Id != 0)
        //    {
        //        PropertyInfo createdByField = typeof(T).GetProperty(nameof(TransectionKeys.CreatedBy));
        //        string createdByValue = Convert.ToString(createdByField.GetValue(data));
        //        if (string.IsNullOrWhiteSpace(createdByValue) || createdByValue == "0")
        //        {
        //            PropertyInfo createdDateField = typeof(T).GetProperty(nameof(TransectionKeys.CreatedDate));
        //            createdByField.SetValue(data, CurrentUser.Id);
        //            createdDateField.SetValue(data, DateTime.Now);
        //        }
        //        else
        //        {
        //            PropertyInfo updatedByField = typeof(T).GetProperty(nameof(TransectionKeys.UpdatedBy));
        //            PropertyInfo updatedDateField = typeof(T).GetProperty(nameof(TransectionKeys.UpdatedDate));
        //            updatedByField.SetValue(data, CurrentUser.Id);
        //            updatedDateField.SetValue(data, DateTime.Now);
        //        }
        //    }
        //    return data;
        //}
        internal static PageData GetRecordsOfPage<T>(PageData pageData, IEnumerable<T> data) { pageData.RecordsCount = data.Count(); pageData.Data = pageData.IsClientSide ? data : data.Skip(pageData.CurrentPage > 0 ? pageData.CurrentPage - (1 * pageData.PageSize) : 0).Take(pageData.PageSize); return pageData; }
        internal static PageData GetDefaultPage<T>(PageData data) { return new PageData { PageSize = data.PageSize != 0 ? data.PageSize : 12, IsClientSide = data.IsClientSide, CurrentPage = data.CurrentPage != 0 ? data.CurrentPage : 0, RecordsCount = data.RecordsCount != 0 ? data.RecordsCount : 0, Filter = data.IsClientSide || data.Filter == null ? DictionaryFromType<T>() : data.Filter }; }
        private static Dictionary<string, object> DictionaryFromType<T>() { Dictionary<string, object> directory = []; foreach (PropertyInfo prop in typeof(T).GetProperties()) { directory.Add(prop.Name.ToCamelCase(), null); } return directory; }

        internal async Task<List<T>> GetDataList<T>(int id = 0) where T : class
        {
            Dictionary<string, object> filter = []; if (id != 0) { filter.Add(nameof(id), id); }
            using DefaultContext defaultContext = new DefaultContext(new Connection());
            return await defaultContext.Set<T>().AsQueryable().AsNoTracking().OrFilter(filter).ToListAsync();
        }
        public async Task<Response> Get<T>(int id) where T : class
        {
            Response apiResponse = new() { Status = (byte)StatusFlags.Success };
            try { apiResponse.Data = await GetDataList<T>(id); }
            catch (Exception ex) { apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<Response> GetPage<T>(PageData pageData) where T : class
        {
            Response apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                pageData = GetDefaultPage<T>(pageData); using DefaultContext defaultContext = new DefaultContext(new Connection());
                apiResponse.Data = GetRecordsOfPage(pageData, await defaultContext.Set<T>().AsQueryable().AsNoTracking().OrFilter(pageData.Filter).ToListAsync());
            }
            catch (Exception ex) { apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
}
