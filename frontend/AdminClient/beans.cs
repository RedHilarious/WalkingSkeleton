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
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/Beans")]
[System.Xml.Serialization.XmlRootAttribute("Test", Namespace="http://www.example.org/Beans", IsNullable=false)]
public partial class Action : Step {
    
    private int stepIdField;
    
    private string nameField;
    
    private string userNameField;
    
    /// <remarks/>
    public int StepId {
        get {
            return this.stepIdField;
        }
        set {
            this.stepIdField = value;
        }
    }
    
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
    public string UserName {
        get {
            return this.userNameField;
        }
        set {
            this.userNameField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(Action))]
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/Beans")]
public partial class Step {
    
    private int idField;
    
    /// <remarks/>
    public int Id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}
