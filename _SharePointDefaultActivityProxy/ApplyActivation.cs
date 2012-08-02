using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using _SPObjectStub;

namespace Microsoft.SharePoint.WorkflowActions
{
    [DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer)), DesignerSerializer(typeof(DependencyObjectCodeDomSerializer), typeof(CodeDomSerializer)), DesignerSerializer(typeof(WorkflowMarkupSerializer), typeof(WorkflowMarkupSerializer)), DesignerSerializer(typeof(ActivityCodeDomSerializer), typeof(CodeDomSerializer)), DesignerSerializer(typeof(ActivityTypeCodeDomSerializer), typeof(TypeCodeDomSerializer))]
    public sealed class ApplyActivation : CallExternalMethodActivity
    {
        public static DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(ApplyActivation));
        public static DependencyProperty __WorkflowPropertiesProperty = DependencyProperty.Register("__WorkflowProperties", typeof(SPWorkflowActivationProperties), typeof(ApplyActivation));

        public ApplyActivation()
        {
        }

        public WorkflowContext __Context
        {
            get
            {
                return (WorkflowContext)base.GetValue(__ContextProperty);
            }
            set
            {
                base.SetValue(__ContextProperty, value);
            }
        }

        public SPWorkflowActivationProperties __WorkflowProperties
        {
            get
            {
                return (SPWorkflowActivationProperties)base.GetValue(__WorkflowPropertiesProperty);
            }
            set
            {
                base.SetValue(__WorkflowPropertiesProperty, value);
            }
        }
    }
}
