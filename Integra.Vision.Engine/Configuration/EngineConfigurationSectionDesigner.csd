<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="3c10ef24-3d75-4d8e-99ee-94cd5f8607ec" namespace="Integra.Vision.Engine.Configuration" xmlSchemaNamespace="urn:Integra.Vision.Engine.Configuration" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSectionGroup name="EngineConfigurationSectionGroup" accessModifier="Internal">
      <configurationSectionProperties>
        <configurationSectionProperty>
          <containedConfigurationSection>
            <configurationSectionMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/EngineConfigurationSection" />
          </containedConfigurationSection>
        </configurationSectionProperty>
      </configurationSectionProperties>
    </configurationSectionGroup>
    <configurationSection name="EngineConfigurationSection" accessModifier="Internal" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="engineConfigurationSection">
      <elementProperties>
        <elementProperty name="Listener" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="listener" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/ListenerConfigurationElement" />
          </type>
        </elementProperty>
        <elementProperty name="Database" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="database" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/DatabaseConfigurationElement" />
          </type>
        </elementProperty>
        <elementProperty name="Operation" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="operation" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/OperationConfigurationElement" />
          </type>
        </elementProperty>
        <elementProperty name="Concurrency" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="concurrency" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/ConcurrencyConfigurationElement" />
          </type>
        </elementProperty>
        <elementProperty name="BufferManagement" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="bufferManagement" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/BufferManagmentConfigurationElement" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="EndpointConfigurationElementCollection" accessModifier="Internal" collectionType="AddRemoveClearMap" xmlItemName="endpointConfigurationElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods" addItemName="endpoint">
      <itemType>
        <configurationElementMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/EndpointConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="EndpointConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Address" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="address" isReadOnly="true">
          <validator>
            <callbackValidatorMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/AddressValidator" />
          </validator>
          <type>
            <externalTypeMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="ListenerConfigurationElement" accessModifier="Internal">
      <elementProperties>
        <elementProperty name="Endpoints" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="endpoints" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/EndpointConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="DatabaseConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="ConnectionString" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="connectionString" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="OperationConfigurationElement" accessModifier="Internal">
      <elementProperties>
        <elementProperty name="Actions" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="actions" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/CommandConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Dispatcher" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="dispatcher" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/FilterConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="FilterConfigurationElementCollection" accessModifier="Internal" collectionType="AddRemoveClearMap" xmlItemName="filterConfigurationElementCollection" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods" addItemName="filter">
      <itemType>
        <configurationElementMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/FilterConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="FilterConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="ConcurrencyConfigurationElement" accessModifier="Internal">
      <attributeProperties>
        <attributeProperty name="Level" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="level" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="CommandConfigurationElementCollection" accessModifier="Internal" hasCustomChildElements="true" collectionType="AddRemoveClearMap" xmlItemName="commandConfigurationElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementCollectionMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/CommandConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="CommandConfigurationElement" accessModifier="Internal" collectionType="AddRemoveClearMap" xmlItemName="filterConfigurationElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods" addItemName="filter">
      <attributeProperties>
        <attributeProperty name="CommandType" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="commandType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <itemType>
        <configurationElementMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/FilterConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="BufferManagmentConfigurationElement">
      <attributeProperties>
        <attributeProperty name="MaxBufferPoolSize" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="maxBufferPoolSize" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/Int64" />
          </type>
        </attributeProperty>
        <attributeProperty name="MaxBufferSize" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="maxBufferSize" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/3c10ef24-3d75-4d8e-99ee-94cd5f8607ec/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators>
      <callbackValidator name="AddressValidator" callback="ValidateAddress" />
    </validators>
  </propertyValidators>
</configurationSectionModel>