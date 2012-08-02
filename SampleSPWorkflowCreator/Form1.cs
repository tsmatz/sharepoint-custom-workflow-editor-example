using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint.WorkflowActions;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Net;
using System.Collections;

namespace SampleSPWorkflowCreator
{
    ////////////////////////////
    // WebPart などサーバ側のカスタム処理の場合は、
    // Microsoft.SharePoint.WorkflowActions.dll を直接参照すれば OK です。
    // この場合は、下記の通り assembly の PublicKeyToken を null にします。
    // ここでは、Client 側で Workflow を構築するため、
    // _SharePointDefaultActivityStub プロジェクトを構築して Activity を作成します。
    //
    // なお、SharePoint Designer では
    // %userprofile%\AppData\Roaming\Microsoft\SharePoint Designer\ProxyAssemblyCache\12.0.0.6219
    // に dll の proxy (Microsoft.SharePoint.WorkflowActions.Proxy.dll) を作成して
    // 使用しています. (この proxy dll の PublicKeyToken は null です)
    ////////////////////////////
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ComboBox[] Conditions = new ComboBox[3];
        ComboBox[] Tasks = new ComboBox[3];
        TextBox[] MailAddresses = new TextBox[3];
        TextBox[] FieldNames = new TextBox[3];
        TextBox[] FieldValues = new TextBox[3];
        TextBox[] Approvers = new TextBox[3];

        XmlDocument xomlDoc;
        XmlDocument configDoc;

        private void Form1_Load(object sender, EventArgs e)
        {
            Tasks[0] = Task0;
            Tasks[1] = Task1;
            Tasks[2] = Task2;

            MailAddresses[0] = MailAddress0;
            MailAddresses[1] = MailAddress1;
            MailAddresses[2] = MailAddress2;

            FieldNames[0] = FieldName0;
            FieldNames[1] = FieldName1;
            FieldNames[2] = FieldName2;

            FieldValues[0] = FieldValue0;
            FieldValues[1] = FieldValue1;
            FieldValues[2] = FieldValue2;
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            // Step 0 : Workflow Location などの設定
            Step_SetupObjects();

            // Step 1 : Workflow Config を作成
            Step_CreateWorkflowConfig();

            // Step 2 : XOML を作成
            Step_CreateXoml();

            // Step 3 : Upload !
            Step_UploadFiles();

            // Step 4 : ワークフローのコンパイル
            WebPartPagesSvc.WebPartPagesWebService sv = new WebPartPagesSvc.WebPartPagesWebService();
            sv.Url = string.Format("{0}/{1}", SiteLoc.Text.TrimEnd(new char[] { '/' }), "_vti_bin/WebPartPages.asmx");
            sv.UseDefaultCredentials = true;
            sv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            sv.PreAuthenticate = true;

            sv.ValidateWorkflowMarkupAndCreateSupportObjects(xomlDoc.InnerXml, "", configDoc.InnerXml, "2");
            sv.AssociateWorkflowMarkup(string.Format("Workflows/{0}/{0}.xoml.wfconfig.xml", WorkflowName.Text), "V1.0");

            MessageBox.Show("ワークフローを配置しました");
        }

        private void Step_SetupObjects()
        {
            string taskListGUID = null;
            string workflowLibGUID = null;

            ListsSvc.Lists sv = new ListsSvc.Lists();
            sv.Url = string.Format("{0}/{1}", SiteLoc.Text.TrimEnd(new char[] { '/' }), "_vti_bin/Lists.asmx");
            sv.UseDefaultCredentials = true;
            sv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            sv.PreAuthenticate = true;

            XmlNode listCol = sv.GetListCollection();
            foreach (XmlNode list in listCol)
            {
                // Get Task List ID
                if (list.Attributes["ServerTemplate"].Value == "107")
                    taskListGUID = list.Attributes["ID"].Value;
                // Get No-Code Workflow Location ID
                else if (list.Attributes["ServerTemplate"].Value == "117")
                    workflowLibGUID = list.Attributes["ID"].Value;
            }

            if (string.IsNullOrEmpty(taskListGUID))
                sv.AddList("Tasks", "Tasks", 107);
            if (string.IsNullOrEmpty(workflowLibGUID))
                sv.AddList("Workflows", "Workflows", 117);
        }

        private void Step_CreateWorkflowConfig()
        {
            string docListGUID = GetListGUIDFromName(SiteLoc.Text, AssocDocLib.Text);
            string taskListGUID, workflowLibGUID;
            GetWorkflowMetadata(SiteLoc.Text, out taskListGUID, out workflowLibGUID);

            // Root
            configDoc = new XmlDocument();
            XmlElement elemRoot = configDoc.CreateElement("WorkflowConfig");
            configDoc.AppendChild(elemRoot);

            // Template 要素
            XmlElement elemTemplate = configDoc.CreateElement("Template");
            XmlAttribute attBaseID = configDoc.CreateAttribute("BaseID");
            attBaseID.Value = "{" + Guid.NewGuid().ToString() + "}";
            elemTemplate.Attributes.Append(attBaseID);
            XmlAttribute attDocLibID = configDoc.CreateAttribute("DocLibID");
            attDocLibID.Value = workflowLibGUID;
            elemTemplate.Attributes.Append(attDocLibID);
            XmlAttribute attXomlHref = configDoc.CreateAttribute("XomlHref");
            attXomlHref.Value = string.Format("Workflows/{0}/{0}.xoml", WorkflowName.Text);
            elemTemplate.Attributes.Append(attXomlHref);
            XmlAttribute attXomlVersion = configDoc.CreateAttribute("XomlVersion");
            attXomlVersion.Value = "V1.0";
            elemTemplate.Attributes.Append(attXomlVersion);
            elemRoot.AppendChild(elemTemplate);

            // Association 要素
            XmlElement elemAssociation = configDoc.CreateElement("Association");
            XmlAttribute attListID = configDoc.CreateAttribute("ListID");
            attListID.Value = docListGUID;
            elemAssociation.Attributes.Append(attListID);
            XmlAttribute attTaskListID = configDoc.CreateAttribute("TaskListID");
            attTaskListID.Value = taskListGUID;
            elemAssociation.Attributes.Append(attTaskListID);
            XmlAttribute attStartOnCreate = configDoc.CreateAttribute("StartOnCreate");
            attStartOnCreate.Value = "true";
            elemAssociation.Attributes.Append(attStartOnCreate);
            elemRoot.AppendChild(elemAssociation);

            // ContentTypes 要素
            XmlElement elemContentTypes = configDoc.CreateElement("ContentTypes");
            elemRoot.AppendChild(elemContentTypes);

            // Initiation 要素
            XmlElement elemInitiation = configDoc.CreateElement("Initiation");
            XmlAttribute attURL = configDoc.CreateAttribute("URL");
            attURL.Value = "None";    // もしある場合は, .aspx を作成して相対パスを記載する
            elemInitiation.Attributes.Append(attURL);
            {
                // Fields 要素
                XmlElement elemFields = configDoc.CreateElement("Fields");
                elemInitiation.AppendChild(elemFields);

                // Parameters 要素
                XmlElement elemParameters = configDoc.CreateElement("Parameters");
                elemInitiation.AppendChild(elemParameters);
            }
            elemRoot.AppendChild(elemInitiation);
        }

        private void Step_CreateXoml()
        {
            // Root アクティビティ
            RootWorkflowActivityWithData rootActivity = new RootWorkflowActivityWithData();
            rootActivity.Name = "ROOT";
            rootActivity.WorkflowFields.Add(new WorkflowDataField("__list", "System.String"));
            rootActivity.WorkflowFields.Add(new WorkflowDataField("__item", "System.Int32"));
            rootActivity.WorkflowFields.Add(new WorkflowDataField("__context", "Microsoft.SharePoint.WorkflowActions.WorkflowContext"));
            rootActivity.WorkflowFields.Add(new WorkflowDataField("__initParams", "Microsoft.SharePoint.Workflow.SPWorkflowActivationProperties"));
            rootActivity.WorkflowFields.Add(new WorkflowDataField("__workflowId", "System.Guid"));

            // OnWorkflowActivated アクティビティ
            OnWorkflowActivated onWorkflowActivatedActivity = new OnWorkflowActivated();
            CorrelationToken onWorkflowActivatedColToken = new CorrelationToken("refObject");
            onWorkflowActivatedColToken.OwnerActivityName = "ROOT";
            onWorkflowActivatedActivity.CorrelationToken = onWorkflowActivatedColToken;
            ActivityBind onWorkflowActivatedBind = new ActivityBind();
            onWorkflowActivatedBind.Name = "ROOT";
            onWorkflowActivatedBind.Path = "__initParams";
            onWorkflowActivatedActivity.SetBinding(OnWorkflowActivated.WorkflowPropertiesProperty, onWorkflowActivatedBind);
            rootActivity.Activities.Add(onWorkflowActivatedActivity);

            // ApplyActivation アクティビティ
            ApplyActivation applyActivationActivity = new ApplyActivation();
            ActivityBind applyActivationBindContextProp = new ActivityBind();
            applyActivationBindContextProp.Name = "ROOT";
            applyActivationBindContextProp.Path = "__context";
            applyActivationActivity.SetBinding(ApplyActivation.__ContextProperty, applyActivationBindContextProp);
            ActivityBind applyActivationBindWorkflowProp = new ActivityBind();
            applyActivationBindWorkflowProp.Name = "ROOT";
            applyActivationBindWorkflowProp.Path = "__initParams";
            applyActivationActivity.SetBinding(ApplyActivation.__WorkflowPropertiesProperty, applyActivationBindWorkflowProp);
            rootActivity.Activities.Add(applyActivationActivity);

            // Sequence アクティビティ (ステップ 1)
            SequenceActivity step1Activity = new SequenceActivity();
            step1Activity.Description = "ステップ 1";
            for (int i = 0; i < 3; i++)
            {
                // SetField アクティビティ
                if (Tasks[i].SelectedItem == "Field Update")
                {
                    SetFieldActivity setFieldActivity = new SetFieldActivity();
                    string docListGUID = GetListGUIDFromName(SiteLoc.Text, AssocDocLib.Text);
                    setFieldActivity.FieldName = GetFieldInternalName(SiteLoc.Text, docListGUID, FieldNames[i].Text);
                    setFieldActivity.Value = FieldValues[i].Text;

                    ActivityBind contextPropertyBind = new ActivityBind();
                    contextPropertyBind.Name = "ROOT";
                    contextPropertyBind.Path = "__context";
                    setFieldActivity.SetBinding(SetFieldActivity.__ContextProperty, contextPropertyBind);

                    ActivityBind listIdBind = new ActivityBind();
                    listIdBind.Name = "ROOT";
                    listIdBind.Path = "__list";
                    setFieldActivity.SetBinding(SetFieldActivity.__ListIdProperty, listIdBind);

                    ActivityBind listItemBind = new ActivityBind();
                    listItemBind.Name = "ROOT";
                    listItemBind.Path = "__item";
                    setFieldActivity.SetBinding(SetFieldActivity.__ListItemProperty, listItemBind);

                    step1Activity.Activities.Add(setFieldActivity);
                }
                // EMail アクティビティ
                else if (Tasks[i].SelectedItem == "Send Mail")
                {
                    EmailActivity eMailActivity = new EmailActivity();

                    ArrayList toArray = new ArrayList();
                    toArray.Add(MailAddresses[i].Text);
                    eMailActivity.To = toArray;
                    eMailActivity.CC = null;
                    eMailActivity.BCC = null;

                    eMailActivity.Subject = "Custom Workflow Test";
                    eMailActivity.Body = "Hello, SharePoint !";

                    ActivityBind contextPropertyBind = new ActivityBind();
                    contextPropertyBind.Name = "ROOT";
                    contextPropertyBind.Path = "__context";
                    eMailActivity.SetBinding(EmailActivity.__ContextProperty, contextPropertyBind);

                    step1Activity.Activities.Add(eMailActivity);
                }
            }
            rootActivity.Activities.Add(step1Activity);

            // XmlDocument に保存
            MemoryStream xomlMem = new MemoryStream();
            XmlTextWriter xomlWriter = new XmlTextWriter(xomlMem, Encoding.UTF8);
            WorkflowMarkupSerializer xomlSerializer = new WorkflowMarkupSerializer();
            xomlSerializer.Serialize(xomlWriter, rootActivity);
            xomlDoc = new XmlDocument();
            xomlMem.Position = 3; // Attention! : 先頭の BOM (Byte Order Mark) は SharePoint でおかしな動きになる !
            xomlDoc.Load(xomlMem);
            xomlWriter.Close();
            xomlMem.Close();

            // コードも含めたコンパイル (サーバ側) に備え x:Class を追加
            XmlAttribute classAttr = xomlDoc.CreateAttribute("Class", @"http://schemas.microsoft.com/winfx/2006/xaml");
            classAttr.Value = "Microsoft.SharePoint.Workflow.ROOT";
            xomlDoc.ChildNodes[0].Attributes.Append(classAttr);

            // if you use Microsoft.SharePoint.WorkflowActions.dll,
            // this code is needed. (Above notation)
            // xomlDoc.ChildNodes[0].Attributes["xmlns:ns0"].Value = @"clr-namespace:Microsoft.SharePoint.WorkflowActions;Assembly=Microsoft.SharePoint.WorkflowActions, Version=12.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        private void Step_UploadFiles()
        {
            byte[] buf = new byte[1024];

            // Workflow Folder の作成
            string folderUrl = string.Format("{0}/Workflows/{1}", SiteLoc.Text.TrimEnd(new char[] { '/' }), WorkflowName.Text);
            WebRequest folderRequest = WebRequest.Create(folderUrl);
            folderRequest.UseDefaultCredentials = true;
            folderRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            folderRequest.PreAuthenticate = true;
            folderRequest.Method = "MKCOL";
            folderRequest.GetResponse().Close();

            // Workflow Config のアップロード
            string configUrl = string.Format("{0}/Workflows/{1}/{1}.xoml.wfconfig.xml", SiteLoc.Text.TrimEnd(new char[] { '/' }), WorkflowName.Text);
            WebRequest configRequest = WebRequest.Create(configUrl);
            configRequest.UseDefaultCredentials = true;
            configRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            configRequest.PreAuthenticate = true;
            configRequest.Method = "PUT";

            MemoryStream configMem = new MemoryStream();
            configDoc.Save(configMem);
            using (Stream reqStream = configRequest.GetRequestStream())
            {
                configMem.Seek(0, SeekOrigin.Begin);
                for (int byteCount = configMem.Read(buf, 0, buf.Length); byteCount > 0; byteCount = configMem.Read(buf, 0, buf.Length))
                {
                    reqStream.Write(buf, 0, byteCount);
                }
            }
            configMem.Close();
            configRequest.GetResponse().Close();

            // Xoml のアップロード
            string xomlUrl = string.Format("{0}/Workflows/{1}/{1}.xoml", SiteLoc.Text.TrimEnd(new char[] { '/' }), WorkflowName.Text);
            WebRequest xomlRequest = WebRequest.Create(xomlUrl);
            xomlRequest.UseDefaultCredentials = true;
            xomlRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            xomlRequest.PreAuthenticate = true;
            xomlRequest.Method = "PUT";

            MemoryStream xomlMem = new MemoryStream();
            xomlDoc.Save(xomlMem);
            using (Stream reqStream = xomlRequest.GetRequestStream())
            {
                xomlMem.Seek(0, SeekOrigin.Begin);
                for (int byteCount = xomlMem.Read(buf, 0, buf.Length); byteCount > 0; byteCount = xomlMem.Read(buf, 0, buf.Length))
                {
                    reqStream.Write(buf, 0, byteCount);
                }
            }
            xomlMem.Close();
            xomlRequest.GetResponse().Close();
        }

        #region Helper Functions

        // フィールド名称からフィールドの内部名を取得する
        private string GetFieldInternalName(string pSiteLocation, string pDocListGUID, string pDisplayName)
        {
            string fieldName = null;

            ListsSvc.Lists sv = new ListsSvc.Lists();
            sv.Url = string.Format("{0}/{1}", pSiteLocation.TrimEnd(new char[] { '/' }), "_vti_bin/Lists.asmx");
            sv.UseDefaultCredentials = true;
            sv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            sv.PreAuthenticate = true;
            XmlNode listNode = sv.GetList(pDocListGUID);
            XmlElement fieldsElem = listNode["Fields"];
            foreach (XmlElement field in fieldsElem)
            {
                if (field.Attributes["DisplayName"].Value == pDisplayName)
                    fieldName = field.Attributes["Name"].Value;
            }
            return fieldName;
        }

        // デフォルトのタスクリストIDなど、
        // ワークフロー作成に必要なデータを収集する
        private void GetWorkflowMetadata(string pSiteLocation, out string pTaskListGUID, out string pWorkflowLibGUID)
        {
            pTaskListGUID = null;
            pWorkflowLibGUID = null;

            ListsSvc.Lists sv = new ListsSvc.Lists();
            sv.Url = string.Format("{0}/{1}", pSiteLocation.TrimEnd(new char[] { '/' }), "_vti_bin/Lists.asmx");
            sv.UseDefaultCredentials = true;
            sv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            sv.PreAuthenticate = true;
            XmlNode listCol = sv.GetListCollection();
            foreach (XmlNode list in listCol)
            {
                // Get Task List ID
                if(list.Attributes["ServerTemplate"].Value == "107")
                    pTaskListGUID = list.Attributes["ID"].Value;
                // Get No-Code Workflow Location ID
                else if (list.Attributes["ServerTemplate"].Value == "117")
                    pWorkflowLibGUID = list.Attributes["ID"].Value;
            }
        }

        // List Web サービスから List の Guid を取得する
        private string GetListGUIDFromName(string siteLocation, string pListName)
        {
            ListsSvc.Lists sv = new ListsSvc.Lists();
            sv.Url = string.Format("{0}/{1}", siteLocation.TrimEnd(new char[] { '/' }), "_vti_bin/Lists.asmx");
            sv.UseDefaultCredentials = true;
            sv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            sv.PreAuthenticate = true;
            XmlNode list = sv.GetListAndView(pListName, "");
            return list.ChildNodes[0].Attributes["Name"].Value;
        }

        #endregion Helper Functions
    }
}
