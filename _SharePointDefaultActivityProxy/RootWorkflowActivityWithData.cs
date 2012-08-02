using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities;
using System.ComponentModel;
using System.Workflow.ComponentModel;

namespace Microsoft.SharePoint.WorkflowActions
{
    public sealed class RootWorkflowActivityWithData : SequentialWorkflowActivity
    {
        public static readonly DependencyProperty WorkflowFieldsProperty = DependencyProperty.Register("WorkflowFields", typeof(WorkflowDataFieldsCollection), typeof(RootWorkflowActivityWithData), new PropertyMetadata(DependencyPropertyOptions.Metadata | DependencyPropertyOptions.ReadOnly, new Attribute[] { new BrowsableAttribute(false), new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content) }));

        public RootWorkflowActivityWithData()
        {
            base.SetReadOnlyPropertyValue(WorkflowFieldsProperty, new WorkflowDataFieldsCollection(this));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WorkflowDataFieldsCollection WorkflowFields
        {
            get
            {
                return (base.GetValue(WorkflowFieldsProperty) as WorkflowDataFieldsCollection);
            }
        }
    }
}
