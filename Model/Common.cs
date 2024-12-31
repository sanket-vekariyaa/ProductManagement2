
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Model
{
    public struct Response
    {
        public byte Status { get; set; }
        public string Message { get; set; }
        public string DetailedError { get; set; }
        public object Data { get; set; }
    }
    public struct PageData
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int RecordsCount { get; set; }
        public bool IsClientSide { get; set; }
        public object Data { get; set; }
        public Dictionary<string, object> Filter { get; set; }
    }

    public class AuthModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string TenantCode { get; set; }
    }
    public abstract class TransectionKeys
    {
        [Key] public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
    }
    public enum StatusFlags : byte
    {
        Success = 1,
        Failed = 2,
        AlreadyExists = 3,
        DependencyExists = 4,
        NotPermitted = 5
    }
}
