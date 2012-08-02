using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using _SPObjectStub;
using System.Collections;

namespace Microsoft.SharePoint.WorkflowActions
{
    [DesignerSerializer(typeof(ActivityTypeCodeDomSerializer), typeof(TypeCodeDomSerializer)), DesignerSerializer(typeof(DependencyObjectCodeDomSerializer), typeof(CodeDomSerializer)), DesignerSerializer(typeof(WorkflowMarkupSerializer), typeof(WorkflowMarkupSerializer)), DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer)), DesignerSerializer(typeof(ActivityCodeDomSerializer), typeof(CodeDomSerializer))]
    public sealed class EmailActivity : Activity
    {
        public static DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(EmailActivity));
        public static DependencyProperty BCCProperty = DependencyProperty.Register("BCC", typeof(ArrayList), typeof(EmailActivity));
        public static DependencyProperty BodyProperty = DependencyProperty.Register("Body", typeof(string), typeof(EmailActivity));
        public static DependencyProperty CCProperty = DependencyProperty.Register("CC", typeof(ArrayList), typeof(EmailActivity));
        public static DependencyProperty SubjectProperty = DependencyProperty.Register("Subject", typeof(string), typeof(EmailActivity));
        public static DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(ArrayList), typeof(EmailActivity));

        public EmailActivity()
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

        public ArrayList BCC
        {
            get
            {
                return (ArrayList)base.GetValue(BCCProperty);
            }
            set
            {
                base.SetValue(BCCProperty, value);
            }
        }

        public string Body
        {
            get
            {
                return (string)base.GetValue(BodyProperty);
            }
            set
            {
                base.SetValue(BodyProperty, value);
            }
        }

        public ArrayList CC
        {
            get
            {
                return (ArrayList)base.GetValue(CCProperty);
            }
            set
            {
                base.SetValue(CCProperty, value);
            }
        }

        public string Subject
        {
            get
            {
                return (string)base.GetValue(SubjectProperty);
            }
            set
            {
                base.SetValue(SubjectProperty, value);
            }
        }

        public ArrayList To
        {
            get
            {
                return (ArrayList)base.GetValue(ToProperty);
            }
            set
            {
                base.SetValue(ToProperty, value);
            }
        }
    }
}
