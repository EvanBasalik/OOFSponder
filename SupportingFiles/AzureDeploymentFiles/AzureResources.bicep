param components_OOFSponderInsights_name string = 'OOFSponderInsightsBicep'
param components_OOFSponderLogAnalytics_name string = 'OOFSponderLogAnalyticsBicep'

resource components_OOFSponderInsights_name_resource 'microsoft.insights/components@2020-02-02' = {
  name: components_OOFSponderInsights_name
  location: 'westus'
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Redfield'
    Request_Source: 'IbizaAIExtension'
    SamplingPercentage: json('100')
    RetentionInDays: 90
    WorkspaceResourceId: logAnalyticsWorkspace.id
    IngestionMode: 'LogAnalytics'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: components_OOFSponderLogAnalytics_name
  location: 'westus'
  sku: {
    name: 'PerGB2018'
  }
  properties: {
    retentionInDays: 90
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}
