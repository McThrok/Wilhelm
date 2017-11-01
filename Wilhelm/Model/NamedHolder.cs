using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Frontend.Model
{
    public abstract class NamedHolder : Holder
    {

        private string _name;
        private string _description;
        private bool _archivized;

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnChanged(nameof(Description));
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnChanged(nameof(Name));
            }
        }
        public bool Archivized
        {
            get
            {
                return _archivized;
            }
            set
            {
                _archivized = value;
                OnChanged(nameof(Archivized));
            }
        }
        public NamedHolder() : base() { }
        public NamedHolder(int id, string name) : base(id)
        {
            Name = name;
        }
    }
}
