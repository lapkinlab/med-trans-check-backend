using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Models.MechanicNotes
{
    public class MechanicNotePatchInfo
    {
        public Guid Id { get; set; }
        public IReadOnlyList<bool> Params { get; set; }
        public string MechanicName { get; set; }
        public Permission? Permission { get; set; }
        
        public MechanicNotePatchInfo(Guid id, IEnumerable<bool> paramsArr = null, string mechanicName = null, 
            Permission? permission = null)
        {
            Id = id;
            Params = paramsArr?.ToArray();
            MechanicName = mechanicName;
            Permission = permission;
        }
    }
}