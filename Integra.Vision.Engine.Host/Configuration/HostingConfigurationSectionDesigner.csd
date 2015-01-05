<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4" namespace="Integra.Vision.Engine.Host.Configuration" xmlSchemaNamespace="urn:Integra.Vision.Engine.Host.Configuration" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
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
    <configurationSectionGroup name="EngineConfigurationSectionGroup">
      <configurationSectionProperties>
        <configurationSectionProperty>
          <containedConfigurationSection>
            <configurationSectionMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/HostingConfigurationSection" />
          </containedConfigurationSection>
        </configurationSectionProperty>
      </configurationSectionProperties>
    </configurationSectionGroup>
    <configurationSection name="HostingConfigurationSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="hostingConfigurationSection">
      <elementProperties>
        <elementProperty name="Bootstrap" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="bootstrap" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/ModuleConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="DependenciesConfigurationElement">
      <elementProperties>
        <elementProperty name="Aliases" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="aliases" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/AliasConfigurationElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Registers" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="registers" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/RegisterConfigurationElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="AliasConfigurationElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="RegisterConfigurationElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Alias" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="alias" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="ModuleConfigurationElement">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="ModuleConfigurationElementCollection" collectionType="AddRemoveClearMap" xmlItemName="controllerConfigurationElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods" addItemName="module">
      <attributeProperties>
        <attributeProperty name="EngineBuilderType" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="engineBuilderType" isReadOnly="false">
          <validator>
            <stringValidatorMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/StringValidator" />
          </validator>
          <type>
            <externalTypeMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <itemType>
        <configurationElementMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/ModuleConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="AliasConfigurationElementCollection" collectionType="AddRemoveClearMap" xmlItemName="aliasConfigurationElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods" addItemName="alias">
      <itemType>
        <configurationElementMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/AliasConfigurationElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="RegisterConfigurationElementCollection" collectionType="AddRemoveClearMap" xmlItemName="registerConfigurationElement" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods" addItemName="register">
      <itemType>
        <configurationElementMoniker name="/1fb8e9c2-f02b-4b13-8359-a7adcd0b3dd4/RegisterConfigurationElement" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators>
      <stringValidator name="StringValidator" />
    </validators>
  </propertyValidators>
</configurationSectionModel>