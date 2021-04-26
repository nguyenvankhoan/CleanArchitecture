using CleanArchitecture.Domain.Entites;
using CleanArchitecture.Domain.Enums;
using System;

namespace CleanArchitecture.Application.Models
{
    public class Product: AuditableEntity
    {
        public Guid ProductID { get; set; }
        public string ProductKey { get; set; }
        public string ProductImageUri { get; set; }
        public string ProductName { get; set; }
        public Guid ProductTypeID { get; set; }
        public RecordStatus RecordStatus { get; set; }
    }
}
