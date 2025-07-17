using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using OrderProcessingBackEnd.Models;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Transactions;
namespace OrderProcessingBackEnd.Models
{
    [Table("Transactions")]
    public class Transactions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }
        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }   //define below 'Payment method' in the form of enum
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be a non-negative value.")]
        public decimal AmountPaid { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        [Required]
        public string TransactionStatus { get; set; }  //create enum for this 

        [Required]
        [ForeignKey("Order")]
        public int OrderID { get; set; }

        [JsonIgnore]
        public virtual Orders Order { get; set; }
        //one order can have multiple transactions 
    }
    public enum PaymentMethod
    {
        CreditCard,
        PayPal,
        Bank,
        Cash
    }
    public enum TransactionStatus
    {
        Success,
        Failed,
        Pending
    }
}
