// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.32989
//    <NameSpace>CommunicationLib</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>False</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net20</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>True</GenerateXMLAttributes><OrderXMLAttrib>True</OrderXMLAttrib><EnableEncoding>False</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>True</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace CommunicationLib
{
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.Collections.Generic;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.example.org/Beans")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.example.org/Beans", IsNullable = false)]
    public partial class FinalStep : Step
    {

        private string nameField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/Beans")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.example.org/Beans", IsNullable = true)]
    public partial class Step
    {

        private int idField;

        private List<Step> nextStepsField;

        public Step()
        {
            this.nextStepsField = new List<Step>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("nextSteps", Order = 1)]
        public List<Step> nextSteps
        {
            get
            {
                return this.nextStepsField;
            }
            set
            {
                this.nextStepsField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.example.org/Beans")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.example.org/Beans", IsNullable = false)]
    public partial class StartStep : Step
    {

        private int userIdField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int UserId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.example.org/Beans")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.example.org/Beans", IsNullable = false)]
    public partial class Action : Step
    {

        private string nameField;

        private int userIdField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int UserId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.example.org/Beans")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.example.org/Beans", IsNullable = false)]
    public partial class User
    {

        private int idField;

        private string nameField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.example.org/Beans")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.example.org/Beans", IsNullable = false)]
    public partial class Workflow
    {

        private int idField;

        private List<Step> stepField;

        private List<Item> itemField;

        public Workflow()
        {
            this.itemField = new List<Item>();
            this.stepField = new List<Step>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Step", Order = 1)]
        public List<Step> Step
        {
            get
            {
                return this.stepField;
            }
            set
            {
                this.stepField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Item", Order = 2)]
        public List<Item> Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/Beans")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.example.org/Beans", IsNullable = true)]
    public partial class Item
    {

        private int idField;

        private int workflowIdField;

        private List<MetaEntry> metadataField;

        private bool finishedField;

        public Item()
        {
            this.metadataField = new List<MetaEntry>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int WorkflowId
        {
            get
            {
                return this.workflowIdField;
            }
            set
            {
                this.workflowIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Metadata", Order = 2)]
        public List<MetaEntry> Metadata
        {
            get
            {
                return this.metadataField;
            }
            set
            {
                this.metadataField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public bool finished
        {
            get
            {
                return this.finishedField;
            }
            set
            {
                this.finishedField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/Beans")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.example.org/Beans", IsNullable = true)]
    public partial class MetaEntry
    {

        private string keyField;

        private string valueField;

        private string groupIdField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string groupId
        {
            get
            {
                return this.groupIdField;
            }
            set
            {
                this.groupIdField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/Beans")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.example.org/Beans", IsNullable = false)]
    public enum MetaState
    {

        /// <remarks/>
        INACTIVE,

        /// <remarks/>
        OPEN,

        /// <remarks/>
        BUSY,

        /// <remarks/>
        DONE,
    }
}
