using OrderProcessingBackEnd.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrderProcessingBackEnd.Repository;

namespace OrderProcessingBackEnd.DTO
{
    public enum orderStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }

    public class OrdersDTO
    {
        [Required]
        public int OrderID { get; set; }
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be a non-negative value.")]  //rnage starting from 0 to max value
        public decimal TotalAmount { get; set; }
        [Required]
       // [MaxLength(50)]   // to show enus as drop down remove this max length 
        public orderStatus OrderStatus { get; set; }  //use actual Enum from Model class  in DTO  to show drop down in respnse body of swagger 
        public int CustomerID { get; set; }
        //This is only for display, not mapped to DB

    }
    //directly add Enum for drop down in response body 
   
}
