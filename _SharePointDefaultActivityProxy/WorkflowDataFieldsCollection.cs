using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Workflow.ComponentModel;

namespace Microsoft.SharePoint.WorkflowActions
{
    public sealed class WorkflowDataFieldsCollection : KeyedCollection<string, WorkflowDataField>
    {
        private Activity ownerActivity = null;

        public WorkflowDataFieldsCollection(Activity ownerActivity)
        {
            if (ownerActivity == null)
            {
                throw new ArgumentNullException("ownerActivity");
            }
            this.ownerActivity = ownerActivity;
        }

        public WorkflowDataField GetItem(string key)
        {
            return base[key];
        }

        protected override string GetKeyForItem(WorkflowDataField item)
        {
            return item.Name;
        }
    }
}
