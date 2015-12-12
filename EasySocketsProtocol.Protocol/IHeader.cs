using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Protocol
{
    public interface IHeader
    {
        string CallBackID { get; set; }
    }
}
