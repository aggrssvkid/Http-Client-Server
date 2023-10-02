using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Моделька для ответа, на всякий случай.
 */

namespace Server
{
    public class ResponseJsonModel
    {
        public string Answer { get; set; } = "";
        public ResponseJsonModel(string answer) => Answer = answer;
    }
}
