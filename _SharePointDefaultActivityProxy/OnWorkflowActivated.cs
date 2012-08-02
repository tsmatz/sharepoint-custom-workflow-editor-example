using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Collections;
using System.Workflow.Activities;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using _SPObjectStub;

namespace Microsoft.SharePoint.WorkflowActions
{
    [DesignerSerializer(typeof(ActivityTypeCodeDomSerializer), typeof(TypeCodeDomSerializer)), DesignerSerializer(typeof(WorkflowMarkupSerializer), typeof(WorkflowMarkupSerializer)), DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer)), DesignerSerializer(typeof(ActivityCodeDomSerializer), typeof(CodeDomSerializer)), DesignerSerializer(typeof(DependencyObjectCodeDomSerializer), typeof(CodeDomSerializer))]
    public sealed class OnWorkflowActivated : HandleExternalEventActivity
    {
        public static DependencyProperty WorkflowIdProperty = DependencyProperty.Register("WorkflowId", typeof(Guid), typeof(OnWorkflowActivated));
        public static DependencyProperty WorkflowPropertiesProperty = DependencyProperty.Register("WorkflowProperties", typeof(SPWorkflowActivationProperties), typeof(OnWorkflowActivated));

        public OnWorkflowActivated()
        {
        }

        //public override void ApplyValues(IDictionary dictionary1)
        //{
        //    object obj2 = dictionary1[0xf317a50];
        //    if (obj2 is Type)
        //    {
        //        this.InterfaceType = (Type)obj2;
        //    }
        //    obj2 = dictionary1[0x1b5b0ffc];
        //    if (obj2 is string)
        //    {
        //        this.EventName = (string)obj2;
        //    }
        //    obj2 = dictionary1[-2138022877];
        //    if (obj2 is CorrelationToken)
        //    {
        //        this.CorrelationToken = (CorrelationToken)obj2;
        //    }
        //    obj2 = dictionary1[0x3bd1c9b];
        //    if (obj2 is string)
        //    {
        //        this.Name = (string)obj2;
        //    }
        //    obj2 = dictionary1[0x2683e9ec];
        //    if (obj2 is bool)
        //    {
        //        this.Enabled = (bool)obj2;
        //    }
        //    obj2 = dictionary1[-1461350978];
        //    if (obj2 is string)
        //    {
        //        this.Description = (string)obj2;
        //    }
        //}

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), TypeConverter(typeof(GuidConverter))]
        public Guid WorkflowId
        {
            get
            {
                return (Guid)base.GetValue(WorkflowIdProperty);
            }
            set
            {
                base.SetValue(WorkflowIdProperty, value);
            }
        }

        public SPWorkflowActivationProperties WorkflowProperties
        {
            get
            {
                return (SPWorkflowActivationProperties)base.GetValue(WorkflowPropertiesProperty);
            }
            set
            {
                base.SetValue(WorkflowPropertiesProperty, value);
            }
        }
    }
}
