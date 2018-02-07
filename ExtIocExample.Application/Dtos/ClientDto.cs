using System.Collections.Generic;

namespace ExtIocExample.Application.Dtos
{
    public class ClientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}