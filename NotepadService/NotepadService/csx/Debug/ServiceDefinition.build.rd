<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="NotepadService" generation="1" functional="0" release="0" Id="65d66e96-fa64-4784-b6de-b10a7ce8ceb6" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="NotepadServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="NotepadServiceRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/NotepadService/NotepadServiceGroup/LB:NotepadServiceRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="NotepadServiceRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/NotepadService/NotepadServiceGroup/MapNotepadServiceRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="NotepadServiceRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/NotepadService/NotepadServiceGroup/MapNotepadServiceRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:NotepadServiceRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/NotepadService/NotepadServiceGroup/NotepadServiceRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapNotepadServiceRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/NotepadService/NotepadServiceGroup/NotepadServiceRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapNotepadServiceRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/NotepadService/NotepadServiceGroup/NotepadServiceRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="NotepadServiceRole" generation="1" functional="0" release="0" software="C:\Users\tromano.REDMOND\Projects\Phone\NotepadService\NotepadService\csx\Debug\roles\NotepadServiceRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;NotepadServiceRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;NotepadServiceRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="NotepadServiceRole.svclog" defaultAmount="[1000,1000,1000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/NotepadService/NotepadServiceGroup/NotepadServiceRoleInstances" />
            <sCSPolicyFaultDomainMoniker name="/NotepadService/NotepadServiceGroup/NotepadServiceRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="NotepadServiceRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="NotepadServiceRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="d714e709-f7f1-45f7-9a39-b5f5e6676af0" ref="Microsoft.RedDog.Contract\ServiceContract\NotepadServiceContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="a23a9fdb-5f9d-4bab-984b-21e451211f32" ref="Microsoft.RedDog.Contract\Interface\NotepadServiceRole:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/NotepadService/NotepadServiceGroup/NotepadServiceRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>