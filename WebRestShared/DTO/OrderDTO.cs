using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRestShared.DTO
{
    public partial class OrderDTO
    {
        public string OrdersId { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public string OrderCustomerId { get; set; } = null!;

        public string OrderCrtdId { get; set; } = null!;

        public DateTime OrderCrtdDt { get; set; }

        public string OrderUpdtId { get; set; } = null!;

        public DateTime OrderUpdtDt { get; set; }

    }
}
