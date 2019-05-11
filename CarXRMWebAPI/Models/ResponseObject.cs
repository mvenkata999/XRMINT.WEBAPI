using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarXRMWebAPI.Models
{
    public class ResponseObject : IResponseObject
    {
        private int _ErrId;
        private string _ErrDesc = string.Empty;
        private List<object> _ObjectList = null;

        public ResponseObject()
        {
            _ObjectList = new List<object>();
            _ErrId = -1;
            _ErrDesc = string.Empty;
        }

        public int ErrId { get => _ErrId; set => _ErrId = value; }
        public string ErrDesc { get => _ErrDesc; set => _ErrDesc = value; }
        public List<object> ObjectList { get => _ObjectList; set => _ObjectList = value; }
    }
}
