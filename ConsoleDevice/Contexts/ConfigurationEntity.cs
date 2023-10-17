using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDevice.Contexts
{
    internal class ConfigurationEntity
    {
        [Key]
        public string DeviceId { get; set; } = "EF:12:AC:17:B4";
        public string ConnectionString { get; set; } = null!;

        public string? DeviceType { get; set; }

        public string? Location { get; set; }

    }
}
