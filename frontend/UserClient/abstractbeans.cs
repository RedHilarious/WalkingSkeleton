﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:2.0.50727.5485
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// Dieser Quellcode wurde automatisch generiert von xsd, Version=2.0.50727.3038.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.example.org/Beans")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.example.org/Beans", IsNullable=false)]
public partial class AbstractFinalStep : AbstractStep {
    
    private string nameField;
    
    /// <remarks/>
    public string Name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/Beans")]
public partial class AbstractStep {
    
    private int idField;
    
    private AbstractStep[] nextStepsField;
    
    /// <remarks/>
    public int Id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("nextSteps")]
    public AbstractStep[] nextSteps {
        get {
            return this.nextStepsField;
        }
        set {
            this.nextStepsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/Beans")]
public partial class AbstractMap {
    
    private int[] keyField;
    
    private AbstractMetaState[] valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("key")]
    public int[] key {
        get {
            return this.keyField;
        }
        set {
            this.keyField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("value")]
    public AbstractMetaState[] value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/Beans")]
public enum AbstractMetaState {
    
    /// <remarks/>
    INACTIVE,
    
    /// <remarks/>
    OPEN,
    
    /// <remarks/>
    BUSY,
    
    /// <remarks/>
    DONE,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/Beans")]
public partial class AbstractItem {
    
    private int idField;
    
    private int workflowIdField;
    
    private AbstractMap metadataField;
    
    private bool finishedField;
    
    /// <remarks/>
    public int Id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
    
    /// <remarks/>
    public int WorkflowId {
        get {
            return this.workflowIdField;
        }
        set {
            this.workflowIdField = value;
        }
    }
    
    /// <remarks/>
    public AbstractMap Metadata {
        get {
            return this.metadataField;
        }
        set {
            this.metadataField = value;
        }
    }
    
    /// <remarks/>
    public bool finished {
        get {
            return this.finishedField;
        }
        set {
            this.finishedField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.example.org/Beans")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.example.org/Beans", IsNullable=false)]
public partial class AbstractStartStep : AbstractStep {
    
    private int userIdField;
    
    /// <remarks/>
    public int UserId {
        get {
            return this.userIdField;
        }
        set {
            this.userIdField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.example.org/Beans")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.example.org/Beans", IsNullable=false)]
public partial class AbstractAction : AbstractStep {
    
    private string nameField;
    
    private int userIdField;
    
    /// <remarks/>
    public string Name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    public int UserId {
        get {
            return this.userIdField;
        }
        set {
            this.userIdField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.example.org/Beans")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.example.org/Beans", IsNullable=false)]
public partial class AbstractWorkflow {
    
    private int idField;
    
    private AbstractStep[] stepField;
    
    private AbstractItem[] itemField;
    
    /// <remarks/>
    public int Id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Step")]
    public AbstractStep[] Step {
        get {
            return this.stepField;
        }
        set {
            this.stepField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Item")]
    public AbstractItem[] Item {
        get {
            return this.itemField;
        }
        set {
            this.itemField = value;
        }
    }
}
