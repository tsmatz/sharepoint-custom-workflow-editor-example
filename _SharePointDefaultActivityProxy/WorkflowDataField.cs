using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SharePoint.WorkflowActions
{
    public sealed class WorkflowDataField
    {
        private string name;
        private string type;

        public WorkflowDataField()
        {
        }

        public WorkflowDataField(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}
