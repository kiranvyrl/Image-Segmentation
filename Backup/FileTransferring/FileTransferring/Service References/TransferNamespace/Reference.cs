﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileTransferring.TransferNamespace {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FilesTransfer", Namespace="http://schemas.datacontract.org/2004/07/FileTransferring")]
    [System.SerializableAttribute()]
    public partial class FilesTransfer : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private FileTransferring.TransferNamespace.Action ActionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<FileTransferring.TransferNamespace.Transfer> FilesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public FileTransferring.TransferNamespace.Action Action {
            get {
                return this.ActionField;
            }
            set {
                if ((this.ActionField.Equals(value) != true)) {
                    this.ActionField = value;
                    this.RaisePropertyChanged("Action");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<FileTransferring.TransferNamespace.Transfer> Files {
            get {
                return this.FilesField;
            }
            set {
                if ((object.ReferenceEquals(this.FilesField, value) != true)) {
                    this.FilesField = value;
                    this.RaisePropertyChanged("Files");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Action", Namespace="http://schemas.datacontract.org/2004/07/FileTransferring")]
    public enum Action : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invite = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ok = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Busy = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Cancel = 3,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Transfer", Namespace="http://schemas.datacontract.org/2004/07/FileTransferring")]
    [System.SerializableAttribute()]
    public partial class Transfer : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CatalogField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FullPathField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long LengthField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Catalog {
            get {
                return this.CatalogField;
            }
            set {
                if ((object.ReferenceEquals(this.CatalogField, value) != true)) {
                    this.CatalogField = value;
                    this.RaisePropertyChanged("Catalog");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FullPath {
            get {
                return this.FullPathField;
            }
            set {
                if ((object.ReferenceEquals(this.FullPathField, value) != true)) {
                    this.FullPathField = value;
                    this.RaisePropertyChanged("FullPath");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Length {
            get {
                return this.LengthField;
            }
            set {
                if ((this.LengthField.Equals(value) != true)) {
                    this.LengthField = value;
                    this.RaisePropertyChanged("Length");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TransferNamespace.ITransfer", SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface ITransfer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITransfer/SendQuery", ReplyAction="http://tempuri.org/ITransfer/SendQueryResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FileTransferring.TransferNamespace.FilesTransfer))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FileTransferring.TransferNamespace.Action))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<FileTransferring.TransferNamespace.Transfer>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FileTransferring.TransferNamespace.Transfer))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Collections.Generic.List<object>))]
        System.Collections.Generic.List<object> SendQuery(string ip, FileTransferring.TransferNamespace.FilesTransfer transfer, int sport);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface ITransferChannel : FileTransferring.TransferNamespace.ITransfer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class TransferClient : System.ServiceModel.ClientBase<FileTransferring.TransferNamespace.ITransfer>, FileTransferring.TransferNamespace.ITransfer {
        
        public TransferClient() {
        }
        
        public TransferClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TransferClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TransferClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TransferClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<object> SendQuery(string ip, FileTransferring.TransferNamespace.FilesTransfer transfer, int sport) {
            return base.Channel.SendQuery(ip, transfer, sport);
        }
    }
}
