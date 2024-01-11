using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Play.Models;

namespace Play.Dto.Response
{
    #nullable disable
    public class OwnerRes
    {
        public string Status{get;set;}
        public string Message{get;set;}
        public Owner Owner {get;set;}

    }
    public class OwnerRess
    {
        public string Status{get;set;}
        public string Message{get;set;}
        public IEnumerable<Owner> Owner {get;set;}

    }
    public class OwnerResd
    {
        public string Status{get;set;}
        public string Message{get;set;}
        public Owner Owner {get;set;}

    }
    
}