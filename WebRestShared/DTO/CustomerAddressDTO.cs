using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebRestShared.DTO
{
    public partial class CustomerAddressDTO
    {
        public string CustomerAddressId { get; set; } = null!;

        public string CustomerAddressCustomerId { get; set; } = null!;

        public string CustomerAddressAddressId { get; set; } = null!;

        public string CustomerAddressAddressTypeID { get; set; } = null!;

        public int CustomerAddressActvInd { get; set; }

        public int CustomerAddressDefaultInd { get; set; }

        public string CustomerAddressCrtdId { get; set; } = null!;

        public DateTime CustomerAddressCrtdDt { get; set; }

        public string CustomerAddressUpdtId { get; set; } = null!;

        public DateTime CustomerAddressUpdtDt { get; set; }

    }
}
