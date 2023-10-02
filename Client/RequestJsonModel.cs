using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class RequestJsonModel
    {
        public int Id { get; set; } = -1;
        public string Password { get; set; } = "";

        public RequestJsonModel(int id, string password)
        {
            Id = id;
            Password = password;
        }
    }
}
