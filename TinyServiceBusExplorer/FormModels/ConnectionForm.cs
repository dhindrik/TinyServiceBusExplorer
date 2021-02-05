using System;
using System.ComponentModel.DataAnnotations;

namespace TinyServiceBusExplorer.FormModels
{
    public record ConnectionForm
    {
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
        public bool Save { get; set; }
    }
}
