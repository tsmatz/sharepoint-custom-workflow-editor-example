using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _SPObjectStub;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.ComponentModel;

namespace Microsoft.SharePoint.WorkflowActions
{
    [DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer)), DesignerSerializer(typeof(ActivityTypeCodeDomSerializer), typeof(TypeCodeDomSerializer)), DesignerSerializer(typeof(ActivityCodeDomSerializer), typeof(CodeDomSerializer)), DesignerSerializer(typeof(DependencyObjectCodeDomSerializer), typeof(CodeDomSerializer)), DesignerSerializer(typeof(WorkflowMarkupSerializer), typeof(WorkflowMarkupSerializer))]
    public sealed class SetFieldActivity : Activity
    {
        public static DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(SetFieldActivity));
        public static DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(SetFieldActivity));
        public static DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(SetFieldActivity));
        public static DependencyProperty FieldNameProperty = DependencyProperty.Register("FieldName", typeof(string), typeof(SetFieldActivity));
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(SetFieldActivity));

        public SetFieldActivity()
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

        public string __ListId
        {
            get
            {
                return (string)base.GetValue(__ListIdProperty);
            }
            set
            {
                base.SetValue(__ListIdProperty, value);
            }
        }

        [TypeConverter(typeof(Int32Converter))]
        public int __ListItem
        {
            get
            {
                return (int)base.GetValue(__ListItemProperty);
            }
            set
            {
                base.SetValue(__ListItemProperty, value);
            }
        }

        public string FieldName
        {
            get
            {
                return (string)base.GetValue(FieldNameProperty);
            }
            set
            {
                base.SetValue(FieldNameProperty, value);
            }
        }

        public object Value
        {
            get
            {
                return base.GetValue(ValueProperty);
            }
            set
            {
                base.SetValue(ValueProperty, value);
            }
        }
    }
}
